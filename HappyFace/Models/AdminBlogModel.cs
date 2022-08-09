using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using PodBeeMedia.Helpers;

namespace PodBeeMedia.Models
{
    public class AdminBlogModel
    {
        public List<BlogModel> data { get; set; }
        public MetaModel meta { get; set; }
    }
    public class AdminBlogItemModel
    {
        public int id { get; set; }
        public string icon { get; set; } = "fa fa-folder icon-lg text-warning";
        public string type { get; set; } = "root";
        public string text { get; set; }
        public bool children { get; set; }
    }
    public class AdminParentBlogListModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Lang { get; set; }
        public int SharedID { get; set; }
    }
    public class AdminParentBlogResultModel
    {
        public bool Status { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public List<AdminParentBlogListModel> Categories { get; set; } = new List<AdminParentBlogListModel>();
    }
    public class AdminBlogViewModel
    {
        public List<CategoryModel> Categories { get; set; } = AdminCategoryMethods.GetCategories("Blog", LanguageHelper.CurrentLanguage).ToList();
    }
    public class AdminBlogDetailModel
    {
        public BlogModel Blog { get; set; }
        public AdminBlogViewModel Categories { get; set; } = new AdminBlogViewModel();
        public List<LanguageModel> Languages { get; set; }
        public List<BlogModel> Blogs { get; set; }
    }
    public class AdminBlogMethods
    {
        #region Get Methods
        public static BlogModel GetBlog(Blogs _Blog)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                BlogModel Blog = new BlogModel();
                try
                {
                    foreach (var prop in _Blog.GetType().GetProperties())
                    {
                        foreach (var _prop in Blog.GetType().GetProperties())
                        {
                            if (prop.Name == _prop.Name)
                            {
                                _prop.SetValue(Blog, prop.GetValue(_Blog));
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                return Blog;
            }
        }
        public static BlogModel GetBlog(int id)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                Blogs _Blog = db.Blogs.FirstOrDefault(b => b.ID == id);
                return GetBlog(_Blog);
            }
        }
        public static List<BlogModel> GetBlogs(int count = 0)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                List<BlogModel> model = new List<BlogModel>();
                List<Blogs> blogs = db.Blogs.Where(b => b.Lang == LanguageHelper.CurrentLanguage).ToList();
                foreach (var blog in blogs)
                {
                    model.Add(GetBlog(blog));
                }
                if (count > 0)
                {
                    model = model.Take(count).ToList();
                }
                return model;
            }
        }
        public static List<BlogModel> GetBlogs(BlogModel mainblog)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                List<BlogModel> model = new List<BlogModel>();
                List<Blogs> blogs = db.Blogs.Where(b => b.Lang != LanguageHelper.DefaultLanguage && b.SharedID == mainblog.SharedID).ToList();
                foreach (var blog in blogs)
                {
                    model.Add(GetBlog(blog));
                }
                return model;
            }
        }
        public static List<BlogModel> GetBlogs(string type, string lang)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                List<BlogModel> model = new List<BlogModel>();
                List<Blogs> blogs = db.Blogs.Where(b => b.Lang == lang).ToList();
                foreach (var blog in blogs)
                {
                    model.Add(GetBlog(blog));
                }
                return model;
            }
        }
        public static AdminBlogDetailModel GetBlogDetail(int id)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                BlogModel blog = GetBlog(id);
                return new AdminBlogDetailModel()
                {
                    Blog = blog,
                    Languages = AdminLanguageMethods.GetLanguages(),
                    Blogs = GetBlogs(blog)
                };
            }
        }
        #endregion
        #region Check Methods
        public static AdminParentPostResultModel CheckParentPost(int id)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                AdminParentPostResultModel model = new AdminParentPostResultModel();
                List<Categories> categories = db.Categories.Where(c => c.SharedID == id).ToList();
                List<Languages> languages = db.Languages.Where(l => l.Status).ToList();
                if (categories.Count == languages.Count)
                {
                    model.Status = true;
                    foreach (var Post in categories)
                    {
                        model.Categories.Add(new AdminParentPostListModel()
                        {
                            ID = Post.ID,
                            Title = Post.Title,
                            Lang = Post.Lang,
                            SharedID = Post.SharedID
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
        public static AdminResultModel SetCategory(int sharedid, int categoryid)
        {
            AdminResultModel model = new AdminResultModel();
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                model.Url = "/blogs";
                try
                {
                    List<Blogs> blogs = db.Blogs.Where(b => b.SharedID == sharedid).ToList();
                    List<Categories> categories = db.Categories.Where(c => c.ID == categoryid).ToList();
                    foreach (var blog in blogs)
                    {
                        bool hasLanguage = categories.Any(c => c.Lang == blog.Lang);
                        blog.CategoryID = hasLanguage ? categories.First(c => c.Lang == blog.Lang).ID : categories.First(c => c.Lang == LanguageHelper.DefaultLanguage).ID;
                    }
                    db.SubmitChanges();
                    model.Message = LanguageHelper.GetValue("SuccessMoveBlog");
                    model.Status = true;
                }
                catch (Exception)
                {
                    model.Message = LanguageHelper.GetValue("ErrorMoveBlog");
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
                    Blogs Blog = db.Blogs.FirstOrDefault(b => b.ID == id);
                    switch (column)
                    {
                        case "Status":
                            foreach (var blog in db.Blogs.Where(b => b.SharedID == Blog.SharedID).ToList())
                            {
                                blog.Status = value == "true" ? false : true;
                            }
                            break;
                        default:
                            break;
                    }
                    db.SubmitChanges();
                    return AdminMethods.GetResult("SuccessUpdateBlog", true, "/blogs");
                }
                catch (Exception)
                {
                    return new AdminResultModel()
                    {
                        Message = LanguageHelper.GetValue("ErrorUpdateBlog")
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
                    Blogs defaultblog = db.Blogs.FirstOrDefault(b => b.SharedID == sharedid && b.Lang == LanguageHelper.DefaultLanguage);
                    List<Blogs> langblogs = db.Blogs.Where(b => b.SharedID == sharedid && b.Lang != LanguageHelper.DefaultLanguage).ToList();
                    BlogModel dblog = GetBlog(defaultblog);
                    dblog.ID = 0;
                    dblog.SharedID = 0;
                    if (db.Blogs.Any(p => p.Url == dblog.Url))
                    {
                        int count = db.Blogs.Where(p => p.Url.Contains(dblog.Url)).Count();
                        if (count == 1)
                        {
                            dblog.Title = dblog.Title + " - Kopya";
                            dblog.Url = dblog.Url + "-kopya";
                        }
                        else
                        {
                            dblog.Title = dblog.Title + " - Kopya " + (count - 1);
                            dblog.Url = dblog.Url + "-kopya-" + (count - 1);
                        }
                    }
                    model = Add(dblog, null, null);
                    if (model.Status)
                    {
                        Blogs blog = db.Blogs.FirstOrDefault(p => p.Url == dblog.Url);
                        foreach (var langblog in langblogs)
                        {
                            BlogModel lblog = GetBlog(langblog);
                            lblog.ID = 0;
                            lblog.SharedID = blog.SharedID;
                            if (db.Blogs.Any(p => p.Url == lblog.Url))
                            {
                                int lcount = db.Blogs.Where(b => b.Url.Contains(lblog.Url)).Count();
                                if (lcount == 1)
                                {
                                    lblog.Title = lblog.Title + " - Kopya";
                                    lblog.Url = lblog.Url + "-kopya";
                                }
                                else
                                {
                                    lblog.Title = lblog.Title + " - Kopya " + (lcount - 1);
                                    lblog.Url = lblog.Url + "-kopya-" + (lcount - 1);
                                }
                            }
                            model = Add(lblog, null, null);
                        }
                    }
                    return AdminMethods.GetResult("SuccessCopyBlog", true, "/blogs");
                }
                catch (Exception)
                {
                    return AdminMethods.GetResult("ErrorCopyBlog", false, "/blogs");
                    throw;
                }
            }
        }
        public static AdminResultModel CopyAll(int[] sharedids)
        {
            AdminResultModel model = new AdminResultModel();
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                model.Url = "/blogs";
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
        public static AdminResultModel Add(BlogModel Blog, HttpPostedFileBase thumbnail, HttpPostedFileBase banner, bool update = false)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                try
                {
                    Blogs _Blog = !update ? new Blogs() : db.Blogs.FirstOrDefault(b => b.ID == Blog.ID);
                    foreach (var prop in Blog.GetType().GetProperties())
                    {
                        foreach (var _prop in _Blog.GetType().GetProperties())
                        {
                            if (prop.Name == _prop.Name)
                            {
                                if (prop.Name != "Banner" && prop.Name != "Thumbnail")
                                {
                                    _prop.SetValue(_Blog, prop.GetValue(Blog));
                                }
                            }
                        }
                    }
                    if (Blog.Title == null || string.IsNullOrEmpty(Blog.Title.Trim()))
                    {
                        return AdminMethods.GetResult("ErrorTitleEmpty", false, "/blogs", "Title");
                    }
                    else if (Blog.Url == null || string.IsNullOrEmpty(Blog.Url.Trim()))
                    {
                        return AdminMethods.GetResult("ErrorTitleEmpty", false, "/blogs", "Url");
                    }
                    else if (db.Posts.Any(p => p.Url == Blog.Url && p.ID != Blog.ID))
                    {
                        return AdminMethods.GetResult("ErrorSameUrl", false, "/blogs", "Url");
                    }
                    if (Blog.CategoryID == 0)
                    {
                        return AdminMethods.GetResult("ErrorCategoryEmpty", false, "/blogs", "CategoryID");
                    }
                    else
                    {
                        CategoryModel _category = AdminCategoryMethods.GetCategoryDetail(Blog.CategoryID).Category;
                        _Blog.CategoryID = _category.SharedID;
                        if (_Blog.Lang == LanguageHelper.CurrentLanguage)
                        {
                            _Blog.Category = _category.Title;
                        }
                        else
                        {
                            CategoryModel _lcategory = AdminCategoryMethods.GetCategory(_category.SharedID, _Blog.Lang);
                            _Blog.Category = _lcategory.Title;
                        }
                    }
                    if (thumbnail != null && !string.IsNullOrEmpty(thumbnail.FileName))
                    {
                        string _name = _Blog.Title;
                        string _path = GlobalHelper.GetFilePath("/Content/img/blog/", _name, thumbnail);
                        thumbnail.SaveAs(_path);
                        _Blog.Thumbnail = "/Content/img/blog/" + GlobalHelper.CreateUrl(_name) + Path.GetExtension(thumbnail.FileName);
                    }
                    if (banner != null && !string.IsNullOrEmpty(banner.FileName))
                    {
                        string _name = banner.FileName;
                        string _path = GlobalHelper.GetFilePath("/Content/img/blog/", _name, banner);
                        banner.SaveAs(_path);
                        _Blog.Banner = "/Content/img/blog/" + GlobalHelper.CreateUrl(_name) + Path.GetExtension(_name);
                    }
                    if (!string.IsNullOrEmpty(_Blog.Description))
                    {
                        _Blog.Description = GlobalHelper.ClearStyle(_Blog.Description);
                    }
                    if (!update)
                    {
                        _Blog.CreateDate = DateTime.Now;
                        db.Blogs.InsertOnSubmit(_Blog);
                    }
                    db.SubmitChanges();
                    if (!update)
                    {
                        Blogs _this = db.Blogs.FirstOrDefault(b => b.Url == Blog.Url);
                        int _returnID = _this.ID;
                        if (_this.SharedID == 0)
                        {
                            _this.SharedID = _this.ID;
                            db.SubmitChanges();
                        }
                        else
                        {
                            _returnID = db.Blogs.FirstOrDefault(b => b.SharedID == _this.SharedID && b.Lang == LanguageHelper.DefaultLanguage).ID;
                        }
                        return AdminMethods.GetResult("SuccessAddBlog", true, "/edit-blog?id=" + _returnID);
                    }
                    return AdminMethods.GetResult("SuccessUpdateBlog", true, "/blogs");
                }
                catch (Exception)
                {
                    return AdminMethods.GetResult(!update ? "ErrorAddBlog" : "ErrorUpdateBlog", false, "/blogs");
                    throw;
                }
            }
        }
        #endregion
        #region Delete
        public static AdminResultModel Delete(int sharedid)
        {
            AdminResultModel model = new AdminResultModel();
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                model.Url = "/blogs";
                try
                {
                    /* Tüm dillerde ana kategori silme */
                    IEnumerable<Blogs> blogs = db.Blogs.Where(b => b.SharedID == sharedid).ToList();
                    db.Blogs.DeleteAllOnSubmit(blogs);
                    db.SubmitChanges();
                    model.Message = LanguageHelper.GetValue("SuccessDeleteBlog");
                    model.Status = true;
                }
                catch (Exception)
                {
                    model.Message = LanguageHelper.GetValue("ErrorDeleteBlog");
                    throw;
                }
            }
            return model;
        }
        #endregion
    }
}