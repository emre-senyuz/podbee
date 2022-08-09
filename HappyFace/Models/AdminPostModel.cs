using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PodBeeMedia.Helpers;

namespace PodBeeMedia.Models
{
    public class AdminPostModel
    {
        public List<PostModel> data { get; set; }
        public MetaModel meta { get; set; }
    }
    public class AdminLatestPostsModel
    {
        public List<AdminLatestPostModel> data { get; set; }
        public MetaModel meta { get; set; }
    }
    public class AdminPostItemModel
    {
        public int id { get; set; }
        public string icon { get; set; } = "fa fa-folder icon-lg text-warning";
        public string type { get; set; } = "root";
        public string text { get; set; }
        public bool children { get; set; }
    }
    public class AdminParentPostListModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Lang { get; set; }
        public int SharedID { get; set; }
    }
    public class AdminParentPostResultModel
    {
        public bool Status { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public List<AdminParentPostListModel> Categories { get; set; } = new List<AdminParentPostListModel>();
    }
    public class AdminPostViewModel
    {
        public List<CategoryModel> Categories { get; set; } = AdminCategoryMethods.GetCategories("Podcast", LanguageHelper.CurrentLanguage).ToList();
    }
    public class AdminPostDetailModel
    {
        public PostModel Post { get; set; }
        public AdminPostViewModel Categories { get; set; } = new AdminPostViewModel();
        public List<LanguageModel> Languages { get; set; }
        public List<PostModel> Posts { get; set; }
    }
    public class AlternateCategoryModel
    {
        public int ID { get; set; }
        public int PostID { get; set; }
        public int CategoryID { get; set; }
    }
    public class AdminWeeklyBeesModel
    {
        public int ID { get; set; }
        [AllowHtml]
        public string IFrame { get; set; }
    }
    public class AdminLatestPostModel
    {
        public int ID { get; set; }
        public string Url { get; set; }
        public string Thumbnail { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime UpdateDate { get; set; } = DateTime.Now;
        public string Lang { get; set; }
        [AllowHtml]
        public string IFrame { get; set; }
    }
    public class AdminPostMethods
    {
        #region Get Methods
        public static PostModel GetPost(Posts _Post)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                PostModel Post = new PostModel();
                try
                {
                    foreach (var prop in _Post.GetType().GetProperties())
                    {
                        foreach (var _prop in Post.GetType().GetProperties())
                        {
                            if (prop.Name == _prop.Name)
                            {
                                _prop.SetValue(Post, prop.GetValue(_Post));
                            }
                        }
                    }
                    List<AlternateCategories> alternates = db.AlternateCategories.Where(p => p.PostID == Post.ID && p.CategoryID != Post.CategoryID).ToList();
                    List<string> _alternates = new List<string>();
                    foreach (var alternate in alternates)
                    {
                        _alternates.Add(alternate.CategoryID.ToString());
                    }
                    Post.Alternates = string.Join(",", _alternates);
                }
                catch (Exception)
                {
                    throw;
                }
                return Post;
            }
        }
        public static PostModel GetPost(int id)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                Posts _Post = db.Posts.FirstOrDefault(p => p.ID == id);
                return GetPost(_Post);
            }
        }
        public static List<PostModel> GetPosts(int count = 0)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                List<PostModel> model = new List<PostModel>();
                List<Posts> posts = db.Posts.Where(p => p.Lang == LanguageHelper.CurrentLanguage).OrderBy(p => p.Carousel).ToList();
                foreach (var post in posts)
                {
                    model.Add(GetPost(post));
                }
                if (count > 0)
                {
                    model = model.Take(count).ToList();
                }
                return model;
            }
        }
        public static List<PostModel> GetPosts(PostModel mainpost)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                List<PostModel> model = new List<PostModel>();
                List<Posts> posts = db.Posts.Where(p => p.Lang != LanguageHelper.DefaultLanguage && p.SharedID == mainpost.SharedID).ToList();
                foreach (var post in posts)
                {
                    model.Add(GetPost(post));
                }
                return model;
            }
        }
        public static List<PostModel> GetPosts(string type, string lang)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                List<PostModel> model = new List<PostModel>();
                List<Posts> posts = db.Posts.Where(p => p.Lang == lang).ToList();
                foreach (var post in posts)
                {
                    model.Add(GetPost(post));
                }
                return model;
            }
        }
        public static AdminPostDetailModel GetPostDetail(int id)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                PostModel post = GetPost(id);
                return new AdminPostDetailModel()
                {
                    Post = post,
                    Languages = AdminLanguageMethods.GetLanguages(),
                    Posts = GetPosts(post)
                };
            }
        }
        public static AdminWeeklyBeesModel GetWeeklyBees()
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                WeeklyBees _weeklyBees = db.WeeklyBees.FirstOrDefault();
                AdminWeeklyBeesModel WeeklyBees = new AdminWeeklyBeesModel();
                if (_weeklyBees != null && _weeklyBees.ID > 0)
                {
                    WeeklyBees.ID = _weeklyBees.ID;
                    WeeklyBees.IFrame = _weeklyBees.IFrame;
                }
                return WeeklyBees;
            }
        }
        public static List<AdminLatestPostModel> GetLatestEpisodes()
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                List<AdminLatestPostModel> model = new List<AdminLatestPostModel>();
                List<LatestEpisodes> latestEpisodes = db.LatestEpisodes.Where(p => p.Lang == LanguageHelper.CurrentLanguage).ToList();
                foreach (var latestEpisode in latestEpisodes)
                {
                    model.Add(new AdminLatestPostModel()
                    {
                        ID = latestEpisode.ID,
                        Url = latestEpisode.Url,
                        Thumbnail = latestEpisode.Thumbnail,
                        Title = latestEpisode.Title,
                        Description = latestEpisode.Description,
                        UpdateDate = latestEpisode.UpdateDate,
                        IFrame = latestEpisode.IFrame
                    });
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
                model.Url = "/posts";
                try
                {
                    List<Posts> posts = db.Posts.Where(p => p.SharedID == sharedid).ToList();
                    List<Categories> categories = db.Categories.Where(c => c.ID == categoryid).ToList();
                    foreach (var post in posts)
                    {
                        bool hasLanguage = categories.Any(c => c.Lang == post.Lang);
                        post.CategoryID = hasLanguage ? categories.First(c => c.Lang == post.Lang).ID : categories.First(c => c.Lang == LanguageHelper.DefaultLanguage).ID;
                    }
                    db.SubmitChanges();
                    model.Message = LanguageHelper.GetValue("SuccessMovePost");
                    model.Status = true;
                }
                catch (Exception)
                {
                    model.Message = LanguageHelper.GetValue("ErrorMovePost");
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
                    Posts Post = db.Posts.FirstOrDefault(l => l.ID == id);
                    switch (column)
                    {
                        case "Status":
                            foreach (var post in db.Posts.Where(p => p.SharedID == Post.SharedID).ToList())
                            {
                                post.Status = value == "true" ? false : true;
                            }
                            break;
                        default:
                            break;
                    }
                    db.SubmitChanges();
                    return AdminMethods.GetResult("SuccessUpdatePost", true, "/posts");
                }
                catch (Exception)
                {
                    return new AdminResultModel()
                    {
                        Message = LanguageHelper.GetValue("ErrorUpdatePost")
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
                    Posts defaultpost = db.Posts.FirstOrDefault(p => p.SharedID == sharedid && p.Lang == LanguageHelper.DefaultLanguage);
                    List<Posts> langposts = db.Posts.Where(p => p.SharedID == sharedid && p.Lang != LanguageHelper.DefaultLanguage).ToList();
                    PostModel dpost = GetPost(defaultpost);
                    dpost.ID = 0;
                    dpost.SharedID = 0;
                    if (db.Posts.Any(p => p.Url == dpost.Url))
                    {
                        int count = db.Posts.Where(p => p.Url.Contains(dpost.Url)).Count();
                        if (count == 1)
                        {
                            dpost.Title = dpost.Title + " - Kopya";
                            dpost.Url = dpost.Url + "-kopya";
                        }
                        else
                        {
                            dpost.Title = dpost.Title + " - Kopya " + (count - 1);
                            dpost.Url = dpost.Url + "-kopya-" + (count - 1);
                        }
                    }
                    model = Add(dpost, null, null, null);
                    if (model.Status)
                    {
                        Posts post = db.Posts.FirstOrDefault(p => p.Url == dpost.Url);
                        foreach (var langpost in langposts)
                        {
                            PostModel lpost = GetPost(langpost);
                            lpost.ID = 0;
                            lpost.SharedID = post.SharedID;
                            if (db.Posts.Any(p => p.Url == lpost.Url))
                            {
                                int lcount = db.Posts.Where(p => p.Url.Contains(lpost.Url)).Count();
                                if (lcount == 1)
                                {
                                    lpost.Title = lpost.Title + " - Kopya";
                                    lpost.Url = lpost.Url + "-kopya";
                                }
                                else
                                {
                                    lpost.Title = lpost.Title + " - Kopya " + (lcount - 1);
                                    lpost.Url = lpost.Url + "-kopya-" + (lcount - 1);
                                }
                            }
                            model = Add(lpost, null, null, null);
                        }
                    }
                    return AdminMethods.GetResult("SuccessCopyPost", true, "/posts");
                }
                catch (Exception)
                {
                    return AdminMethods.GetResult("ErrorCopyPost", false, "/posts");
                    throw;
                }
            }
        }
        public static AdminResultModel CopyAll(int[] sharedids)
        {
            AdminResultModel model = new AdminResultModel();
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                model.Url = "/posts";
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
        public static AdminResultModel Add(PostModel Post, HttpPostedFileBase thumbnail, HttpPostedFileBase banner, HttpPostedFileBase trailer, bool update = false)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                try
                {
                    Posts _Post = !update ? new Posts() : db.Posts.FirstOrDefault(p => p.ID == Post.ID);
                    foreach (var prop in Post.GetType().GetProperties())
                    {
                        foreach (var _prop in _Post.GetType().GetProperties())
                        {
                            if (prop.Name == _prop.Name)
                            {
                                if (prop.Name != "Banner" && prop.Name != "Thumbnail" && prop.Name != "Trailer")
                                {
                                    _prop.SetValue(_Post, prop.GetValue(Post));
                                }
                            }
                        }
                    }
                    if (Post.Title == null || string.IsNullOrEmpty(Post.Title.Trim()))
                    {
                        return AdminMethods.GetResult("ErrorTitleEmpty", false, "/posts", "Title");
                    }
                    else if (Post.Url == null || string.IsNullOrEmpty(Post.Url.Trim()))
                    {
                        return AdminMethods.GetResult("ErrorTitleEmpty", false, "/posts", "Url");
                    }
                    else if (db.Posts.Any(p => p.Url == Post.Url && p.ID != Post.ID))
                    {
                        return AdminMethods.GetResult("ErrorSameUrl", false, "/posts", "Url");
                    }
                    if (Post.CategoryID == 0)
                    {
                        return AdminMethods.GetResult("ErrorCategoryEmpty", false, "/posts", "CategoryID");
                    }
                    else
                    {
                        int _category = AdminCategoryMethods.GetCategoryDetail(Post.CategoryID).Category.SharedID;
                        _Post.CategoryID = _category;
                    }
                    if (thumbnail != null && !string.IsNullOrEmpty(thumbnail.FileName))
                    {
                        string _name = _Post.Title;
                        string _path = GlobalHelper.GetFilePath("/Content/img/post/", _name, thumbnail);
                        thumbnail.SaveAs(_path);
                        _Post.Thumbnail = "/Content/img/post/" + GlobalHelper.CreateUrl(_name) + Path.GetExtension(thumbnail.FileName);
                    }
                    if (banner != null && !string.IsNullOrEmpty(banner.FileName))
                    {
                        string _name = banner.FileName;
                        string _path = GlobalHelper.GetFilePath("/Content/img/post/", _name, banner);
                        banner.SaveAs(_path);
                        _Post.Banner = "/Content/img/post/" + GlobalHelper.CreateUrl(_name) + Path.GetExtension(_name);
                    }
                    if (trailer != null && !string.IsNullOrEmpty(trailer.FileName))
                    {
                        string _name = trailer.FileName;
                        string _path = GlobalHelper.GetFilePath("/Content/img/post/trailer/", _name, trailer);
                        trailer.SaveAs(_path);
                        _Post.Trailer = "/Content/img/post/trailer/" + GlobalHelper.CreateUrl(_name) + Path.GetExtension(_name);
                    }
                    if (!string.IsNullOrEmpty(_Post.Description))
                    {
                        _Post.Description = GlobalHelper.ClearStyle(_Post.Description);
                    }
                    if (!string.IsNullOrEmpty(Post.Alternates))
                    {
                        foreach (var alternate in Post.Alternates.Split(','))
                        {
                            AlternateCategories _alternate = new AlternateCategories()
                            {
                                PostID = Post.ID,
                                CategoryID = Convert.ToInt32(alternate)
                            };
                            db.AlternateCategories.InsertOnSubmit(_alternate);
                        }
                    }
                    else
                    {
                        List<AlternateCategories> _alternates = db.AlternateCategories.Where(a => a.PostID == Post.ID).ToList();
                        db.AlternateCategories.DeleteAllOnSubmit(_alternates);
                    }
                    if (!update)
                    {
                        db.Posts.InsertOnSubmit(_Post);
                    }
                    db.SubmitChanges();
                    if (!update)
                    {
                        Posts _this = db.Posts.FirstOrDefault(p => p.Url == Post.Url);
                        LatestEpisodes _latestEpisode = new LatestEpisodes()
                        {
                            PostID = _this.ID,
                            Url = _this.Url,
                            Thumbnail = _this.Thumbnail,
                            Title = _this.Title,
                            Description = _this.Description,
                            Lang = _this.Lang,
                            UpdateDate = DateTime.Now
                        };
                        db.LatestEpisodes.InsertOnSubmit(_latestEpisode);
                        db.SubmitChanges();
                        int _returnID = _this.ID;
                        if (_this.SharedID == 0)
                        {
                            _this.SharedID = _this.ID;
                            db.SubmitChanges();
                        }
                        else
                        {
                            _returnID = db.Posts.FirstOrDefault(p => p.SharedID == _this.SharedID && p.Lang == LanguageHelper.DefaultLanguage).ID;
                        }
                        return AdminMethods.GetResult("SuccessAddPost", true, "/edit-Post?id=" + _returnID);
                    }
                    return AdminMethods.GetResult("SuccessUpdatePost", true, "/posts");
                }
                catch (Exception)
                {
                    return AdminMethods.GetResult(!update ? "ErrorAddPost" : "ErrorUpdatePost", false, "/posts");
                    throw;
                }
            }
        }
        public static AdminResultModel UpdateWeeklyBees(AdminWeeklyBeesModel Model)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                try
                {
                    WeeklyBees _weeklyBees = db.WeeklyBees.FirstOrDefault(w => w.ID == Model.ID);
                    if (Model.ID > 0)
                    {
                        _weeklyBees.IFrame = Model.IFrame;
                    }
                    else
                    {
                        _weeklyBees = new WeeklyBees()
                        {
                            IFrame = Model.IFrame
                        };
                        db.WeeklyBees.InsertOnSubmit(_weeklyBees);
                    }
                    db.SubmitChanges();
                    return AdminMethods.GetResult("SuccessUpdateWeeklyBees", true, "/weekly-bees");
                }
                catch (Exception)
                {
                    return AdminMethods.GetResult("ErrorUpdateWeeklyBees", false, "/weekly-bees");
                    throw;
                }
            }
        }
        public static AdminResultModel UpdateLatestEpisodes(AdminLatestPostModel Model)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                if (Model.ID > 0)
                {
                    LatestEpisodes _latestEpisode = db.LatestEpisodes.FirstOrDefault(l => l.PostID == Model.ID && l.Lang == Model.Lang);
                    try
                    {
                        if (_latestEpisode != null)
                        {
                            _latestEpisode.Description = Model.Description;
                            _latestEpisode.UpdateDate = DateTime.Now;
                            _latestEpisode.IFrame = Model.IFrame;
                            db.SubmitChanges();
                            return AdminMethods.GetResult("SuccessUpdateLatestEpisodes", true, "/latest-posts");
                        }
                        else
                        {
                            _latestEpisode = new LatestEpisodes()
                            {
                                PostID = Model.ID,
                                Url = Model.Url,
                                Thumbnail = Model.Thumbnail,
                                Title = Model.Title,
                                Description = Model.Description,
                                Lang = Model.Lang,
                                UpdateDate = DateTime.Now,
                                IFrame = Model.IFrame
                            };
                            db.LatestEpisodes.InsertOnSubmit(_latestEpisode);
                            db.SubmitChanges();
                            return AdminMethods.GetResult("SuccessAddLatestEpisodes", true, "/latest-posts");
                        }
                    }
                    catch (Exception)
                    {
                        if (_latestEpisode != null)
                        {
                            return AdminMethods.GetResult("ErrorUpdateLatestEpisodes", false, "/latest-posts");
                        }
                        else
                        {
                            return AdminMethods.GetResult("ErrorAddLatestEpisodes", false, "/latest-posts");
                        }
                        throw;
                    }
                }
                else
                {
                    return AdminMethods.GetResult("ErrorSelectPost", false, "/latest-posts");
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
                model.Url = "/posts";
                try
                {
                    IEnumerable<Posts> posts = db.Posts.Where(p => p.SharedID == sharedid).ToList();
                    db.Posts.DeleteAllOnSubmit(posts);
                    db.SubmitChanges();
                    model.Message = LanguageHelper.GetValue("SuccessDeletePost");
                    model.Status = true;
                }
                catch (Exception)
                {
                    model.Message = LanguageHelper.GetValue("ErrorDeletePost");
                    throw;
                }
            }
            return model;
        }
        public static AdminResultModel DeleteTrailer(int id)
        {
            AdminResultModel model = new AdminResultModel();
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                model.Url = "/refresh";
                try
                {
                    Posts post = db.Posts.FirstOrDefault(p => p.ID == id);
                    post.Trailer = string.Empty;
                    db.SubmitChanges();
                    model.Message = LanguageHelper.GetValue("SuccessDeleteTrailer");
                    model.Status = true;
                }
                catch (Exception)
                {
                    model.Message = LanguageHelper.GetValue("ErrorDeleteTrailer");
                    throw;
                }
            }
            return model;
        }
        public static AdminResultModel DeleteLatestPost(int id)
        {
            AdminResultModel model = new AdminResultModel();
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                model.Url = "/refresh";
                try
                {
                    LatestEpisodes latestEpisode = db.LatestEpisodes.FirstOrDefault(l => l.ID == id);
                    db.LatestEpisodes.DeleteOnSubmit(latestEpisode);
                    db.SubmitChanges();
                    model.Message = LanguageHelper.GetValue("SuccessDeleteLatestEpisode");
                    model.Status = true;
                }
                catch (Exception)
                {
                    model.Message = LanguageHelper.GetValue("ErrorDeleteLatestEpisode");
                    throw;
                }
            }
            return model;
        }
        #endregion
    }
}