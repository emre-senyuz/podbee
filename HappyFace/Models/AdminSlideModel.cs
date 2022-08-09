using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using PodBeeMedia.Helpers;

namespace PodBeeMedia.Models
{
    public class AdminSlideModel
    {
        public List<SlideModel> data { get; set; }
        public MetaModel meta { get; set; }
    }
    public class AdminSlideViewModel
    {
        public List<LanguageModel> Languages { get; set; } = AdminLanguageMethods.GetLanguages().ToList();
    }
    public class AdminSlideDetailModel
    {
        public SlideModel Slide { get; set; }
        public List<LanguageModel> Languages { get; set; }
    }
    public class AdminSlideMethods
    {
        #region Get Methods
        public static SlideModel GetSlide(Slides _Slide)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                SlideModel Slide = new SlideModel();
                try
                {
                    foreach (var prop in _Slide.GetType().GetProperties())
                    {
                        foreach (var _prop in Slide.GetType().GetProperties())
                        {
                            if (prop.Name == _prop.Name)
                            {
                                _prop.SetValue(Slide, prop.GetValue(_Slide));
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                return Slide;
            }
        }
        public static SlideModel GetSlide(int id)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                Slides _Slide = db.Slides.FirstOrDefault(s => s.ID == id);
                return GetSlide(_Slide);
            }
        }
        public static List<SlideModel> GetSlides()
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                List<SlideModel> model = new List<SlideModel>();
                List<Slides> slides = db.Slides.Where(s => s.Lang == LanguageHelper.CurrentLanguage).ToList();
                foreach (var slide in slides)
                {
                    model.Add(GetSlide(slide));
                }
                return model;
            }
        }
        public static AdminSlideDetailModel GetSlideDetail(int id)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                AdminSlideDetailModel slide = new AdminSlideDetailModel()
                {
                    Slide = GetSlide(id),
                    Languages = AdminLanguageMethods.GetLanguages()
                };
                return slide;
            }
        }
        #endregion
        #region Column Update
        public static AdminResultModel Update(int id, string column, string value)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                try
                {
                    switch (column)
                    {
                        case "Status":
                            foreach (var slide in db.Slides.Where(s => s.ID == id).ToList())
                            {
                                slide.Status = value == "true" ? false : true;
                            }
                            break;
                        default:
                            break;
                    }
                    db.SubmitChanges();
                    return AdminMethods.GetResult("SuccessUpdateSlide", true, "/slides");
                }
                catch (Exception)
                {
                    return new AdminResultModel()
                    {
                        Message = LanguageHelper.GetValue("ErrorUpdateSlide")
                    };
                    throw;
                }
            }
        }
        #endregion
        #region Insert & Update
        public static AdminResultModel Copy(int id)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                try
                {
                    AdminResultModel model = new AdminResultModel();
                    Slides defaultslide = db.Slides.FirstOrDefault(s => s.ID == id && s.Lang == LanguageHelper.DefaultLanguage);
                    SlideModel dslide = GetSlide(defaultslide);
                    dslide.ID = 0;
                    if (db.Slides.Any(p => p.Url == dslide.Url))
                    {
                        int count = db.Slides.Where(s => s.Url.Contains(dslide.Url)).Count();
                        if (count == 1)
                        {
                            dslide.Title = dslide.Title + " - Kopya";
                            dslide.Url = dslide.Url + "-kopya";
                        }
                        else
                        {
                            dslide.Title = dslide.Title + " - Kopya " + (count - 1);
                            dslide.Url = dslide.Url + "-kopya-" + (count - 1);
                        }
                    }
                    model = Add(dslide, null);
                    return AdminMethods.GetResult("SuccessCopySlide", true, "/slides");
                }
                catch (Exception)
                {
                    return AdminMethods.GetResult("ErrorCopySlide", false, "/slides");
                    throw;
                }
            }
        }
        public static AdminResultModel Add(SlideModel Slide, HttpPostedFileBase banner, bool update = false)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                try
                {
                    Slides _Slide = !update ? new Slides() : db.Slides.FirstOrDefault(s => s.ID == Slide.ID);
                    foreach (var prop in Slide.GetType().GetProperties())
                    {
                        foreach (var _prop in _Slide.GetType().GetProperties())
                        {
                            if (prop.Name == _prop.Name)
                            {
                                if (prop.Name != "Banner")
                                {
                                    _prop.SetValue(_Slide, prop.GetValue(Slide));
                                }
                            }
                        }
                    }
                    if (Slide.Title == null || string.IsNullOrEmpty(Slide.Title.Trim()))
                    {
                        return AdminMethods.GetResult("ErrorTitleEmpty", false, "/slides", "Title");
                    }
                    if (banner == null && string.IsNullOrEmpty(_Slide.Banner))
                    {
                        return AdminMethods.GetResult("ErrorImageEmpty", false, "/slides", "Banner");
                    }
                    else if (banner != null)
                    {
                        string _name = banner.FileName;
                        string _path = GlobalHelper.GetFilePath("/Content/img/slide/", _name, banner);
                        banner.SaveAs(_path);
                        _Slide.Banner = "/Content/img/slide/" + GlobalHelper.CreateUrl(_name) + Path.GetExtension(_name);
                    }
                    if (!update)
                    {
                        db.Slides.InsertOnSubmit(_Slide);
                    }
                    db.SubmitChanges();
                    if (!update)
                    {
                        Slides _this = db.Slides.FirstOrDefault(s => s.Url == Slide.Url);
                        int _returnID = _this.ID;
                        return AdminMethods.GetResult("SuccessAddSlide", true, "/edit-slide?id=" + _returnID);
                    }
                    return AdminMethods.GetResult("SuccessUpdateSlide", true, "/slides");
                }
                catch (Exception)
                {
                    return AdminMethods.GetResult(!update ? "ErrorAddSlide" : "ErrorUpdateSlide", false, "/slides");
                    throw;
                }
            }
        }
        #endregion
        #region Delete
        public static AdminResultModel Delete(int id)
        {
            AdminResultModel model = new AdminResultModel();
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                model.Url = "/slides";
                try
                {
                    IEnumerable<Slides> slides = db.Slides.Where(s => s.ID == id).ToList();
                    db.Slides.DeleteAllOnSubmit(slides);
                    db.SubmitChanges();
                    model.Message = LanguageHelper.GetValue("SuccessDeleteSlide");
                    model.Status = true;
                }
                catch (Exception)
                {
                    model.Message = LanguageHelper.GetValue("ErrorDeleteSlide");
                    throw;
                }
            }
            return model;
        }
        #endregion
    }
}