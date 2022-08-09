using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using PodBeeMedia.Helpers;

namespace PodBeeMedia.Models
{
    public class AdminSocialModel
    {
        public List<SocialModel> data { get; set; }
        public MetaModel meta { get; set; }
    }
    public class AdminSocialMethods
    {
        #region Get Methods
        public static SocialModel GetSocial(Socials _Social)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                SocialModel Social = new SocialModel();
                try
                {
                    foreach (var prop in _Social.GetType().GetProperties())
                    {
                        foreach (var _prop in Social.GetType().GetProperties())
                        {
                            if (prop.Name == _prop.Name)
                            {
                                _prop.SetValue(Social, prop.GetValue(_Social));
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                return Social;
            }
        }
        public static SocialModel GetSocial(int id)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                Socials _Social = db.Socials.FirstOrDefault(s => s.ID == id);
                return GetSocial(_Social);
            }
        }
        public static List<SocialModel> GetSocials()
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                List<SocialModel> model = new List<SocialModel>();
                List<Socials> socials = db.Socials.ToList();
                foreach (var social in socials)
                {
                    model.Add(GetSocial(social));
                }
                return model;
            }
        }
        public static SocialModel GetSocialDetail(int id)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                return GetSocial(id);
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
                    Socials Social = db.Socials.FirstOrDefault(s => s.ID == id);
                    switch (column)
                    {
                        case "Status":
                            foreach (var social in db.Socials.Where(s => s.ID == Social.ID).ToList())
                            {
                                social.Status = value == "true" ? false : true;
                            }
                            break;
                        default:
                            break;
                    }
                    db.SubmitChanges();
                    return AdminMethods.GetResult("SuccessUpdateSocial", true, "/socials");
                }
                catch (Exception)
                {
                    return new AdminResultModel()
                    {
                        Message = LanguageHelper.GetValue("ErrorUpdateSocial")
                    };
                    throw;
                }
            }
        }
        #endregion
        #region Insert & Update
        public static AdminResultModel Add(SocialModel Social, HttpPostedFileBase icon, bool update = false)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                try
                {
                    Socials _Social = !update ? new Socials() : db.Socials.FirstOrDefault(s => s.ID == Social.ID);
                    foreach (var prop in Social.GetType().GetProperties())
                    {
                        foreach (var _prop in _Social.GetType().GetProperties())
                        {
                            if (prop.Name == _prop.Name)
                            {
                                if (prop.Name != "Icon")
                                {
                                    _prop.SetValue(_Social, prop.GetValue(Social));
                                }
                            }
                        }
                    }
                    if (Social.Name == null || string.IsNullOrEmpty(Social.Name.Trim()))
                    {
                        return AdminMethods.GetResult("ErrorNameEmpty", false, "/socials", "Name");
                    }
                    else if (Social.Url == null || string.IsNullOrEmpty(Social.Url.Trim()))
                    {
                        return AdminMethods.GetResult("ErrorUrlEmpty", false, "/socials", "Url");
                    }
                    if (icon != null && !string.IsNullOrEmpty(icon.FileName))
                    {
                        string _name = _Social.Name;
                        string _path = GlobalHelper.GetFilePath("/Content/img/social/", _name, icon);
                        icon.SaveAs(_path);
                        _Social.Icon = "/Content/img/social/" + GlobalHelper.CreateUrl(_name) + Path.GetExtension(icon.FileName);
                    }
                    if (!update)
                    {
                        db.Socials.InsertOnSubmit(_Social);
                    }
                    db.SubmitChanges();
                    if (!update)
                    {
                        Socials _this = db.Socials.FirstOrDefault(s => s.Url == Social.Url);
                        int _returnID = _this.ID;
                        db.SubmitChanges();
                        return AdminMethods.GetResult("SuccessAddSocial", true, "/edit-social?id=" + _returnID);
                    }
                    return AdminMethods.GetResult("SuccessUpdateSocial", true, "/socials");
                }
                catch (Exception)
                {
                    return AdminMethods.GetResult(!update ? "ErrorAddSocial" : "ErrorUpdateSocial", false, "/socials");
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
                model.Url = "/socials";
                try
                {
                    IEnumerable<Socials> socials = db.Socials.Where(s => s.ID == id).ToList();
                    db.Socials.DeleteAllOnSubmit(socials);
                    db.SubmitChanges();
                    model.Message = LanguageHelper.GetValue("SuccessDeleteSocial");
                    model.Status = true;
                }
                catch (Exception)
                {
                    model.Message = LanguageHelper.GetValue("ErrorDeleteSocial");
                    throw;
                }
            }
            return model;
        }
        #endregion
    }
}