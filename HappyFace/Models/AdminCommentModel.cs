using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using PodBeeMedia.Helpers;

namespace PodBeeMedia.Models
{
    public class AdminCommentModel
    {
        public List<CommentModel> data { get; set; }
        public MetaModel meta { get; set; }
    }
    public class AdminCommentDetailModel
    {
        public CommentModel Comment { get; set; }
        public CommentModel Answer { get; set; }
    }
    public class AdminCommentMethods
    {
        #region Get Methods
        public static CommentModel GetComment(Comments _Comment)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                CommentModel Comment = new CommentModel();
                try
                {
                    foreach (var prop in _Comment.GetType().GetProperties())
                    {
                        foreach (var _prop in Comment.GetType().GetProperties())
                        {
                            if (prop.Name == _prop.Name)
                            {
                                _prop.SetValue(Comment, prop.GetValue(_Comment));
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                return Comment;
            }
        }
        public static CommentDetailModel GetComment(int id, bool clientside = false)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                Comments _Comment = db.Comments.FirstOrDefault(c => c.ID == id);
                CommentDetailModel model = new CommentDetailModel()
                {
                    Comment = GetComment(_Comment),
                    Answers = GetComments(_Comment.PostID, _Comment.Type, _Comment.ID, clientside),
                    User = MemberMethods.Get(_Comment.UserID),
                    Post = new CommentPostModel()
                };
                string langPath = LanguageHelper.CurrentLanguage == "tr" ? "/" + LanguageHelper.GetValue("UrlTypeBlog") + "/" : "/" + LanguageHelper.CurrentLanguage + "/" + LanguageHelper.GetValue("UrlTypeBlog") + "/";
                if (_Comment.Type == "Blog")
                {
                    BlogModel _Post = AdminBlogMethods.GetBlog(_Comment.PostID);
                    model.Post = new CommentPostModel()
                    {
                        ID = _Post.ID,
                        Title = _Post.Title,
                        Url = SmtpModel.Domain + langPath + _Post.Url
                    };
                }
                else if (_Comment.Type == "Podcast")
                {
                    PostModel _Post = AdminPostMethods.GetPost(_Comment.PostID);
                    model.Post = new CommentPostModel()
                    {
                        ID = _Post.ID,
                        Title = _Post.Title,
                        Url = SmtpModel.Domain + langPath + _Post.Url
                    };
                }
                return model;
            }
        }
        public static List<CommentModel> GetComments(int ParentID = 0)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                List<CommentModel> model = new List<CommentModel>();
                List<Comments> comments = db.Comments.ToList();
                if (ParentID > 0)
                {
                    comments = comments.Where(c => c.ParentID == ParentID).ToList();
                }
                foreach (var comment in comments)
                {
                    model.Add(GetComment(comment));
                }
                return model;
            }
        }
        public static List<CommentDetailModel> GetComments(int ID, string Type, int ParentID, bool clientside = false)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                List<CommentDetailModel> model = new List<CommentDetailModel>();
                List<Comments> comments = db.Comments.Where(c => c.PostID == ID && c.Type == Type && c.ParentID == ParentID).ToList();
                if (clientside)
                {
                    comments = comments.Where(c => c.Status && c.IsConfirmed).ToList();
                }
                foreach (var comment in comments)
                {
                    model.Add(GetComment(comment.ID, clientside));
                }
                return model;
            }
        }
        public static List<CommentDetailModel> GetComments(int UserID, int Count = 0)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                List<CommentDetailModel> model = new List<CommentDetailModel>();
                List<Comments> comments = db.Comments.Where(c => c.UserID == UserID).ToList();
                if (Count > 0)
                {
                    comments = comments.Take(Count).ToList();
                }
                foreach (var comment in comments)
                {
                    model.Add(GetComment(comment.ID));
                }
                return model;
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
                    Comments comment = db.Comments.FirstOrDefault(c => c.ID == id);
                    switch (column)
                    {
                        case "IsConfirmed":
                            comment.IsConfirmed = value == "true" ? false : true;
                            break;
                        case "Status":
                            comment.Status = value == "true" ? false : true;
                            break;
                        default:
                            break;
                    }
                    db.SubmitChanges();
                    if (comment.Type == "Blog")
                    {
                        Blogs _Post = db.Blogs.FirstOrDefault(b => b.ID == comment.PostID);
                        _Post.CommentCount = db.Comments.Where(c => c.PostID == comment.PostID && c.Status && c.IsConfirmed).Count();
                    }
                    else if (comment.Type == "Podcast")
                    {
                        Posts _Post = db.Posts.FirstOrDefault(p => p.ID == comment.PostID);
                        _Post.CommentCount = db.Comments.Where(c => c.PostID == comment.PostID).Count();
                    }
                    db.SubmitChanges();
                    return AdminMethods.GetResult("SuccessUpdateComment", true, "/comments");
                }
                catch (Exception)
                {
                    return new AdminResultModel()
                    {
                        Message = LanguageHelper.GetValue("ErrorUpdateComment")
                    };
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
                model.Url = "/comments";
                try
                {
                    IEnumerable<Comments> comments = db.Comments.Where(n => n.ID == id).ToList();
                    db.Comments.DeleteAllOnSubmit(comments);
                    IEnumerable<Comments> answers = db.Comments.Where(n => n.ParentID == id).ToList();
                    db.Comments.DeleteAllOnSubmit(answers);
                    db.SubmitChanges();
                    model.Message = LanguageHelper.GetValue("SuccessDeleteComment");
                    model.Status = true;
                }
                catch (Exception)
                {
                    model.Message = LanguageHelper.GetValue("ErrorDeleteComment");
                    throw;
                }
            }
            return model;
        }
        #endregion
    }
}