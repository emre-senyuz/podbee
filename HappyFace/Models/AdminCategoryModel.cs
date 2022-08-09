using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using PodBeeMedia.Helpers;

namespace PodBeeMedia.Models
{
    public class AdminCategoryModel
    {
        public List<CategoryModel> data { get; set; }
        public MetaModel meta { get; set; }
    }
    public class AdminCategoryItemModel
    {
        public int id { get; set; }
        public string icon { get; set; } = "fa fa-folder icon-lg text-warning";
        public string type { get; set; } = "root";
        public string text { get; set; }
        public bool children { get; set; }
    }
    public class AdminParentCategoryListModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Lang { get; set; }
        public int SharedID { get; set; }
    }
    public class AdminParentCategoryResultModel
    {
        public bool Status { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public List<AdminParentCategoryListModel> Categories { get; set; } = new List<AdminParentCategoryListModel>();
    }
    public class AdminCategoryViewModel
    {
        public string[] Types { get; set; } = { "Podcast", "Blog" };
    }
    public class AdminCategoryDetailModel
    {
        public CategoryModel Category { get; set; }
        public AdminCategoryViewModel Types { get; set; } = new AdminCategoryViewModel();
        public List<AdminParentCategoryListModel> Parents { get; set; }
        public List<LanguageModel> Languages { get; set; }
        public List<CategoryModel> Categories { get; set; }
    }
    public class AdminCategoryMethods
    {
        #region Get Methods
        public static CategoryModel GetCategory(Categories _category)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                CategoryModel category = new CategoryModel();
                try
                {
                    foreach (var prop in _category.GetType().GetProperties())
                    {
                        foreach (var _prop in category.GetType().GetProperties())
                        {
                            if (prop.Name == _prop.Name)
                            {
                                _prop.SetValue(category, prop.GetValue(_category));
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                return category;
            }
        }
        public static CategoryModel GetCategory(int id)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                Categories _category = db.Categories.FirstOrDefault(c => c.ID == id);
                return GetCategory(_category);
            }
        }
        public static CategoryModel GetCategory(int sharedid, string lang)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                Categories _category = db.Categories.FirstOrDefault(c => c.SharedID == sharedid && c.Lang == lang);
                return GetCategory(_category);
            }
        }
        public static List<CategoryModel> GetCategories(int parent)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                List<CategoryModel> model = new List<CategoryModel>();
                List<Categories> categories = db.Categories.Where(c => c.Lang == LanguageHelper.DefaultLanguage && c.ParentID == parent).ToList();
                foreach (var category in categories)
                {
                    model.Add(GetCategory(category));
                }
                return model;
            }
        }
        public static List<CategoryModel> GetCategories(CategoryModel maincat)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                List<CategoryModel> model = new List<CategoryModel>();
                List<Categories> categories = db.Categories.Where(c => c.Lang != LanguageHelper.DefaultLanguage && c.SharedID == maincat.SharedID).ToList();
                foreach (var category in categories)
                {
                    model.Add(GetCategory(category));
                }
                return model;
            }
        }
        public static List<CategoryModel> GetCategories(string type, string lang)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                List<CategoryModel> model = new List<CategoryModel>();
                List<Categories> languages = db.Categories.Where(c => c.Type == type && c.Lang == lang).ToList();
                foreach (var category in languages)
                {
                    model.Add(GetCategory(category));
                }
                return model;
            }
        }
        public static List<AdminParentCategoryListModel> GetParentCategory(string type)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                List<AdminParentCategoryListModel> model = new List<AdminParentCategoryListModel>();
                List<Categories> categories = db.Categories.Where(c => c.Type == type).ToList();
                foreach (var category in categories)
                {
                    model.Add(new AdminParentCategoryListModel()
                    {
                        ID = category.ID,
                        Title = category.Title,
                        Lang = category.Lang,
                        SharedID = category.SharedID
                    });
                }
                return model;
            }
        }
        public static AdminCategoryDetailModel GetCategoryDetail(int id)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                CategoryModel category = GetCategory(id);
                return new AdminCategoryDetailModel()
                {
                    Category = category,
                    Parents = GetParentCategory(category.Type),
                    Languages = AdminLanguageMethods.GetLanguages(),
                    Categories = GetCategories(category)
                };
            }
        }
        #endregion
        #region Check Methods
        public static AdminParentCategoryResultModel CheckParentCategory(int id)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                AdminParentCategoryResultModel model = new AdminParentCategoryResultModel();
                List<Categories> categories = db.Categories.Where(c => c.SharedID == id).ToList();
                List<Languages> languages = db.Languages.Where(l => l.Status).ToList();
                if (categories.Count == languages.Count)
                {
                    model.Status = true;
                    foreach (var category in categories)
                    {
                        model.Categories.Add(new AdminParentCategoryListModel()
                        {
                            ID = category.ID,
                            Title = category.Title,
                            Lang = category.Lang,
                            SharedID = category.SharedID
                        });
                    }
                }
                else
                {
                    model.Message = LanguageHelper.GetValue("ErrorParentNeedAllLanguages");
                }
                return model;
            }
        }
        #endregion
        #region Set Methods
        public static void SetType(int sharedid, string type, bool self = true, string lang = "")
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                List<Categories> cats = db.Categories.Where(c => self ? c.SharedID == sharedid : c.ParentID == sharedid).ToList();
                if (!string.IsNullOrEmpty(lang))
                {
                    cats = cats.Where(c => c.Lang == lang).ToList();
                }
                foreach (var cat in cats)
                {
                    if (cat.Parent)
                    {
                        SetType(sharedid, type, false, lang);
                    }
                    cat.Type = type;
                    db.SubmitChanges();
                }
            }
        }
        public static AdminResultModel SetParentCat(int sharedid, int parentid, bool delete = false)
        {
            AdminResultModel model = new AdminResultModel();
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                model.Url = "/categories";
                try
                {
                    List<Categories> cats = db.Categories.Where(c => delete ? c.ParentID == sharedid : c.SharedID == sharedid).ToList();
                    foreach (var cat in cats)
                    {
                        if (!delete)
                        {
                            List<Categories> parents = db.Categories.Where(c => c.SharedID == cat.ParentID).ToList();
                            if (!db.Categories.Any(c => c.SharedID != sharedid && c.ParentID == cat.ParentID))
                            {
                                foreach (var parent in parents)
                                {
                                    parent.Parent = false;
                                }
                            }
                        }
                        cat.ParentID = parentid;
                    }
                    List<Categories> _parents = db.Categories.Where(c => c.SharedID == parentid).ToList();
                    foreach (var parent in _parents)
                    {
                        parent.Parent = true;
                    }
                    db.SubmitChanges();
                    if (_parents.Count > 0)
                    {
                        foreach (var cat in cats)
                        {
                            SetType(cat.SharedID, _parents.FirstOrDefault().Type);
                        }
                    }
                    model.Message = LanguageHelper.GetValue(delete ? "SuccessDeleteCategory" : "SuccessMoveCategory");
                    model.Status = true;
                }
                catch (Exception)
                {
                    model.Message = LanguageHelper.GetValue(delete ? "ErrorDeleteCategory" : "ErrorMoveCategory");
                    throw;
                }
            }
            return model;
        }
        #endregion
        #region Column Update
        public static AdminResultModel Update(int id, string column, string value)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                try
                {
                    Categories category = db.Categories.FirstOrDefault(l => l.ID == id);
                    switch (column)
                    {
                        case "ShowMenu":
                            foreach (var cat in db.Categories.Where(c => c.SharedID == category.SharedID).ToList())
                            {
                                cat.ShowMenu = value == "true" ? false : true;
                            }
                            break;
                        case "Status":
                            foreach (var cat in db.Categories.Where(c => c.SharedID == category.SharedID).ToList())
                            {
                                cat.Status = value == "true" ? false : true;
                            }
                            break;
                        default:
                            break;
                    }
                    db.SubmitChanges();
                    return AdminMethods.GetResult("SuccessUpdateCategory", true, "/categories");
                }
                catch (Exception)
                {
                    return new AdminResultModel()
                    {
                        Message = LanguageHelper.GetValue("ErrorUpdateCategory")
                    };
                    throw;
                }
            }
        }
        #endregion
        #region Insert & Update
        public static AdminResultModel Copy(int sharedid)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                try
                {
                    AdminResultModel model = new AdminResultModel();
                    Categories defaultcat = db.Categories.FirstOrDefault(c => c.SharedID == sharedid && c.Lang == LanguageHelper.DefaultLanguage);
                    List<Categories> langcats = db.Categories.Where(c => c.SharedID == sharedid && c.Lang != LanguageHelper.DefaultLanguage).ToList();
                    CategoryModel dcat = GetCategory(defaultcat);
                    dcat.ID = 0;
                    dcat.SharedID = 0;
                    if (db.Categories.Any(c => c.Url == dcat.Url))
                    {
                        int count = db.Categories.Where(c => c.Url.Contains(dcat.Url)).Count();
                        if (count == 1)
                        {
                            dcat.Title = dcat.Title + " - Kopya";
                            dcat.Url = dcat.Url + "-kopya";
                        }
                        else
                        {
                            dcat.Title = dcat.Title + " - Kopya " + (count - 1);
                            dcat.Url = dcat.Url + "-kopya-" + (count - 1);
                        }
                    }
                    dcat.Parent = false;
                    model = Add(dcat, null, null);
                    if (model.Status)
                    {
                        Categories cat = db.Categories.FirstOrDefault(c => c.Url == dcat.Url);
                        foreach (var langcat in langcats)
                        {
                            CategoryModel lcat = GetCategory(langcat);
                            lcat.ID = 0;
                            lcat.SharedID = cat.SharedID;
                            if (db.Categories.Any(c => c.Url == lcat.Url))
                            {
                                int lcount = db.Categories.Where(c => c.Url.Contains(lcat.Url)).Count();
                                if (lcount == 1)
                                {
                                    lcat.Title = lcat.Title + " - Kopya";
                                    lcat.Url = lcat.Url + "-kopya";
                                }
                                else
                                {
                                    lcat.Title = lcat.Title + " - Kopya " + (lcount - 1);
                                    lcat.Url = lcat.Url + "-kopya-" + (lcount - 1);
                                }
                            }
                            lcat.Parent = false;
                            model = Add(lcat, null, null);
                        }
                    }
                    return AdminMethods.GetResult("SuccessCopyCategory", true, "/categories");
                }
                catch (Exception)
                {
                    return AdminMethods.GetResult("ErrorCopyCategory", false, "/categories");
                    throw;
                }
            }
        }
        public static AdminResultModel CopyAll(int[] sharedids)
        {
            AdminResultModel model = new AdminResultModel();
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                model.Url = "/categories";
                try
                {

                }
                catch (Exception)
                {

                    throw;
                }
            }
            return model;
        }
        public static AdminResultModel Add(CategoryModel category, HttpPostedFileBase thumbnail, HttpPostedFileBase banner, bool update = false)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                try
                {
                    Categories _category = !update ? new Categories() : db.Categories.FirstOrDefault(c => c.ID == category.ID);
                    if (update)
                    {
                        Categories _old = db.Categories.FirstOrDefault(c => c.ID == category.ID);
                        if (!db.Categories.Any(c => c.ParentID == _old.ParentID && c.SharedID != _old.SharedID))
                        {
                            Categories _oldparent = db.Categories.FirstOrDefault(c => c.SharedID == _old.ParentID && c.Lang == _old.Lang);
                            if(_oldparent != null) _oldparent.Parent = false;
                        }
                    }
                    foreach (var prop in category.GetType().GetProperties())
                    {
                        foreach (var _prop in _category.GetType().GetProperties())
                        {
                            if (prop.Name == _prop.Name)
                            {
                                if (prop.Name != "Banner" && prop.Name != "Thumbnail")
                                {
                                    _prop.SetValue(_category, prop.GetValue(category));
                                }
                            }
                        }
                    }
                    if (category.Title == null || string.IsNullOrEmpty(category.Title.Trim()))
                    {
                        return AdminMethods.GetResult("ErrorTitleEmpty", false, "/categories", "Title");
                    }
                    else if (category.Url == null || string.IsNullOrEmpty(category.Url.Trim()))
                    {
                        return AdminMethods.GetResult("ErrorTitleEmpty", false, "/categories", "Url");
                    }
                    else if (db.Categories.Any(c => c.Url == category.Url && c.ID != category.ID))
                    {
                        return AdminMethods.GetResult("ErrorSameUrl", false, "/categories", "Url");
                    }
                    else if (category.Type == null || string.IsNullOrEmpty(category.Type.Trim()) || category.Type == "0")
                    {
                        return AdminMethods.GetResult("ErrorTypeEmpty", false, "/categories", "Type");
                    }
                    if (thumbnail != null && !string.IsNullOrEmpty(thumbnail.FileName))
                    {
                        string _name = _category.Title;
                        string _path = GlobalHelper.GetFilePath("/Content/img/category/", _name, thumbnail);
                        thumbnail.SaveAs(_path);
                        _category.Thumbnail = "/Content/img/category/" + GlobalHelper.CreateUrl(_name) + Path.GetExtension(thumbnail.FileName);
                    }
                    if (banner != null && !string.IsNullOrEmpty(banner.FileName))
                    {
                        string _name = banner.FileName;
                        string _path = GlobalHelper.GetFilePath("/Content/img/category/", _name, banner);
                        banner.SaveAs(_path);
                        _category.Banner = "/Content/img/category/" + GlobalHelper.CreateUrl(_name) + Path.GetExtension(_name);
                    }
                    if (!update)
                    {
                        db.Categories.InsertOnSubmit(_category);
                    }
                    if (category.ParentID > 0)
                    {
                        Categories _parent = db.Categories.FirstOrDefault(c => c.SharedID == category.ParentID && c.Lang == category.Lang);
                        _parent.Parent = true;
                    }
                    if (update)
                    {
                        if (category.Parent)
                        {
                            SetType(category.SharedID, category.Type, false, category.Lang);
                        }
                    }
                    db.SubmitChanges();
                    if (!update)
                    {
                        Categories _this = db.Categories.FirstOrDefault(c => c.Url == category.Url);
                        int _returnID = _this.ID;
                        if (_this.SharedID == 0)
                        {
                            _this.SharedID = _this.ID;
                            db.SubmitChanges();
                        }
                        else
                        {
                            _returnID = db.Categories.FirstOrDefault(c => c.SharedID == _this.SharedID && c.Lang == LanguageHelper.DefaultLanguage).ID;
                        }
                        return AdminMethods.GetResult("SuccessAddCategory", true, "/edit-category?id=" + _returnID);
                    }
                    return AdminMethods.GetResult("SuccessUpdateCategory", true, "/categories");
                }
                catch (Exception)
                {
                    return AdminMethods.GetResult(!update ? "ErrorAddCategory" : "ErrorUpdateCategory", false, "/categories");
                    throw;
                }
            }
        }
        #endregion
        #region Delete
        public static AdminResultModel DeleteSub(int id)
        {
            AdminResultModel model = new AdminResultModel();
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                model.Url = "/categories";
                try
                {
                    Categories subdefault = db.Categories.FirstOrDefault(c => c.ParentID == id && c.Lang == LanguageHelper.DefaultLanguage);
                    if (subdefault != null && subdefault.Parent)
                    {
                        if (db.Categories.Any(c => c.ParentID == subdefault.ID))
                        {
                            DeleteSub(subdefault.ID);
                        }
                    }
                    foreach (var _parent in db.Categories.Where(c => c.SharedID == id).ToList())
                    {
                        _parent.Parent = false;
                    }
                    IEnumerable<Categories> subs = db.Categories.Where(c => c.ParentID == id).ToList();
                    db.Categories.DeleteAllOnSubmit(subs);
                    db.SubmitChanges();
                    model.Message = LanguageHelper.GetValue("SuccessDeleteCategory");
                    model.Status = true;
                }
                catch (Exception)
                {
                    model.Message = LanguageHelper.GetValue("ErrorDeleteCategory");
                    throw;
                }
            }
            return model;
        }
        public static AdminResultModel Delete(int id, bool isparent, bool withchild)
        {
            AdminResultModel model = new AdminResultModel();
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                model.Url = "/categories";
                try
                {
                    Categories _category = db.Categories.FirstOrDefault(c => c.ID == id);
                    /* Tüm dillerde alt kategori silme */
                    if (isparent)
                    {
                        if (withchild)
                        {
                            DeleteSub(_category.SharedID);
                        }
                        else
                        {
                            SetParentCat(_category.SharedID, _category.ParentID, true);
                        }
                    }
                    if (_category.ParentID > 0)
                    {
                        foreach (var cat in db.Categories.Where(c => c.SharedID == _category.ParentID).ToList())
                        {
                            if (!db.Categories.Any(c => c.ParentID == cat.SharedID && c.SharedID != _category.SharedID && c.Lang == cat.Lang))
                            {
                                cat.Parent = false;
                            }
                        }
                    }
                    /* Tüm dillerde ana kategori silme */
                    IEnumerable<Categories> categories = db.Categories.Where(c => c.SharedID == _category.SharedID).ToList();
                    db.Categories.DeleteAllOnSubmit(categories);
                    db.SubmitChanges();
                    model.Message = LanguageHelper.GetValue("SuccessDeleteCategory");
                    model.Status = true;
                }
                catch (Exception)
                {
                    model.Message = LanguageHelper.GetValue("ErrorDeleteCategory");
                    throw;
                }
            }
            return model;
        }
        #endregion
    }
}