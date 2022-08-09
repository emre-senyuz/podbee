using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using PodBeeMedia.Helpers;

namespace PodBeeMedia.Models
{
    public class CommentModel
    {
        public int ID { get; set; }
        public int PostID { get; set; }
        public int UserID { get; set; }
        public string Type { get; set; }
        public int LikeCount { get; set; } = 0;
        public int DislikeCount { get; set; } = 0;
        public string Comment { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public bool Status { get; set; } = false;
        public int ParentID { get; set; } = 0;
        public bool IsConfirmed { get; set; } = false;
    }
    public class CommentPostModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
    public class CommentDetailModel
    {
        public CommentModel Comment { get; set; }
        public List<CommentDetailModel> Answers { get; set; }
        public MemberModel User { get; set; }
        public CommentPostModel Post { get; set; }
    }
    public class CommentMethods
    {
        public static AdminResultModel SendAdminMail(CommentDetailModel Model)
        {
            string to = "info@podbeemedia.com";
            string from = "info@podbeemedia.com";
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Podbee Media - " + LanguageHelper.GetValue("AdminNewComment");
            message.IsBodyHtml = true;
            message.Body = "<table border=\"0\" width=\"600\" cellpadding=\"0\" cellspacing=\"0\">" +
                                "<tr>" +
                                    "<td width=\"150\" valign=\"top\">" +
                                        "<strong>" + LanguageHelper.GetValue("FormName") + "</strong>" +
                                    "</td>" +
                                    "<td>" +
                                        Model.User.Name + " " + Model.User.Surname +
                                    "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td width=\"150\" valign=\"top\">" +
                                        "<strong>" + LanguageHelper.GetValue("FormTitle") + "</strong>" +
                                    "</td>" +
                                    "<td>" +
                                        "<a href=\"" + Model.Post.Url + "\">" + Model.Post.Title + "</a>" +
                                    "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td width=\"150\" valign=\"top\">" +
                                        "<strong>" + LanguageHelper.GetValue("FormComment") + "</strong>" +
                                    "</td>" +
                                    "<td>" +
                                        Model.Comment.Comment +
                                    "</td>" +
                                "</tr>" +
                            "</table>";
            SmtpClient client = SmtpModel.Smtp;
            try
            {
                client.Send(message);
                return AdminMethods.GetResult("SuccessComment", true, "/", "");
            }
            catch (Exception)
            {
                return AdminMethods.GetResult("ErrorEmailSend", false, "/", "");
            }
        }
        public static AdminResultModel SendMemberMail(string emailTo, bool reply)
        {
            string to = emailTo;
            string from = "info@podbeemedia.com";
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Podbee Media - " + (reply ? LanguageHelper.GetValue("YourCommentAnswered") : LanguageHelper.GetValue("YourCommentSended"));
            message.Body = (reply ? LanguageHelper.GetValue("CommentAnsweredText") : LanguageHelper.GetValue("CommentSendedText"));
            SmtpClient client = SmtpModel.Smtp;
            try
            {
                client.Send(message);
                return AdminMethods.GetResult("SuccessComment", true, "/", "");
            }
            catch (Exception)
            {
                return AdminMethods.GetResult("ErrorEmailSend", false, "/", "");
            }
        }
        public static AdminResultModel Send(CommentModel Model, string Email, bool SystemSide)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                CommentDetailModel Comment = new CommentDetailModel()
                {
                    Comment = Model,
                    User = MemberMethods.Get(Email),
                    Post = new CommentPostModel()
                };
                string langPath = LanguageHelper.CurrentLanguage == "tr" ? "/" + LanguageHelper.GetValue("UrlTypeBlog") + "/" : "/" + LanguageHelper.CurrentLanguage + "/" + LanguageHelper.GetValue("UrlTypeBlog") + "/";
                if (Model.Type == "Blog")
                {
                    Blogs _Post = db.Blogs.FirstOrDefault(b => b.ID == Model.PostID);
                    _Post.CommentCount = db.Comments.Where(c => c.PostID == Model.PostID && c.Status && c.IsConfirmed).Count();
                    db.SubmitChanges();
                    Comment.Post = new CommentPostModel()
                    {
                        ID = _Post.ID,
                        Title = _Post.Title,
                        Url = SmtpModel.Domain + langPath + _Post.Url
                    };
                }
                else if (Model.Type == "Podcast")
                {
                    Posts _Post = db.Posts.FirstOrDefault(p => p.ID == Model.PostID);
                    _Post.CommentCount = db.Comments.Where(c => c.PostID == Model.PostID).Count();
                    db.SubmitChanges();
                    Comment.Post = new CommentPostModel()
                    {
                        ID = _Post.ID,
                        Title = _Post.Title,
                        Url = SmtpModel.Domain + langPath + _Post.Url
                    };
                }
                if (Comment.User.ID <= 0)
                {
                    return AdminMethods.GetResult("ErrorAuthenticateComment", false, "/", "");
                }
                if (Model.Comment == null || string.IsNullOrEmpty(Model.Comment.Trim()))
                {
                    return AdminMethods.GetResult("ErrorCommentEmpty", false, "/", "Comment");
                }
                Comments _comment = new Comments()
                {
                    Comment = Model.Comment,
                    UserID = Comment.User.ID,
                    PostID = Comment.Post.ID,
                    Type = Model.Type,
                    ParentID = Model.ParentID,
                    CreateDate = Model.CreateDate
                };
                db.Comments.InsertOnSubmit(_comment);
                db.SubmitChanges();
                if (!SystemSide)
                {
                    SendAdminMail(Comment);
                }
                SendMemberMail(Comment.User.Email, SystemSide);
                return AdminMethods.GetResult("SuccessComment", true, "/", "");
            }
        }
        public static AdminResultModel Vote(int ID, bool IsLike)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                Comments _Comment = db.Comments.FirstOrDefault(c => c.ID == ID);
                if (_Comment != null && _Comment.ID > 0)
                {
                    if (IsLike)
                    {
                        _Comment.LikeCount = _Comment.LikeCount + 1;
                    }
                    else
                    {
                        _Comment.DislikeCount = _Comment.DislikeCount + 1;
                    }
                    db.SubmitChanges();
                    return AdminMethods.GetResult("SuccessVoteComment", true, "/", "");
                }
                return AdminMethods.GetResult("ErrorVoteComment", false, "/", "");
            }
        }
    }
}