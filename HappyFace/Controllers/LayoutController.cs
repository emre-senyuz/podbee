using System.Web.Mvc;
using PodBeeMedia.Models;
using PodBeeMedia.Helpers;
using System.Net.Mail;
using System;

namespace PodBeeMedia.Controllers
{
    public class LayoutController : BaseController
    {
        public JsonResult SetLanguage(string language, string type, int sharedID = 1)
        {
            if (TempData["SharedID"] != null)
            {
                sharedID = (int)TempData["SharedID"];
            }
            LanguageResponseModel model = LanguageMethods.GetUrl(new LanguageRequestModel()
            {
                Language = language,
                Type = type,
                SharedID = sharedID
            });
            if (model.Status)
            {
                Session["CurrentLanguage"] = language;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Header()
        {
            MemberModel _User = User.Identity.IsAuthenticated ? MemberMethods.Get(User.Identity.Name) : new MemberModel();
            HeaderModel model = new HeaderModel
            {
                RoleID = User.Identity.IsAuthenticated ? _User.RoleID : 0,
                UserName = User.Identity.IsAuthenticated ? _User.Name + " " + _User.Surname : "",
                Menu = HeaderMethods.GetCategoryMenu(0),
                Languages = HeaderMethods.GetLanguageMenu()
            };
            return PartialView(SiteVariables.GetValue("HeaderView"), model);
        }
        public ActionResult Sidebar()
        {
            HeaderModel model = new HeaderModel
            {
                RoleID = User.Identity.IsAuthenticated ? MemberMethods.Get(User.Identity.Name).RoleID : 0,
                Menu = HeaderMethods.GetCategoryMenu(0),
                Podcast = PostMethods.GetRecentPost()
            };
            return PartialView(SiteVariables.GetValue("SidebarView"), model);
        }
        public ActionResult MemberMenu()
        {
            Member model = new Member();
            if (User.Identity.IsAuthenticated)
            {
                MemberModel member = MemberMethods.Get(User.Identity.Name);
                model = new Member()
                {
                    Name = member.Name,
                    Surname = member.Surname
                };
            }
            return PartialView(SiteVariables.GetValue("HeaderMemberView"), model);
        }
        public ActionResult Footer()
        {
            FooterModel model = new FooterModel()
            {
                Podcast = AdminCategoryMethods.GetCategories("Podcast", LanguageHelper.CurrentLanguage),
                Blog = AdminCategoryMethods.GetCategories("Blog", LanguageHelper.CurrentLanguage),
                Socials = AdminSocialMethods.GetSocials()
            };
            return PartialView(SiteVariables.GetValue("FooterView"), model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Newsletter(string Email)
        {
            AdminResultModel result = new AdminResultModel()
            {
                Message = LanguageHelper.GetValue("ErrorCheckField")
            };
            if (ModelState.IsValid)
            {
                result = NewsletterMethods.Add(Email);
                if (result.Status)
                {
                    string to = Email;
                    string from = "info@podbeemedia.com";
                    MailMessage message = new MailMessage(from, to);
                    message.Subject = "Podbee Media - " + LanguageHelper.GetValue("Newsletter");
                    message.Body = LanguageHelper.GetValue("NewsletterRegister");
                    SmtpClient client = SmtpModel.Smtp;
                    try
                    {
                        client.Send(message);
                    }
                    catch (Exception ex)
                    {
                        return Json(new AdminResultModel() {
                            Message = LanguageHelper.GetValue("ErrorEmailSend"),
                            Status = false,
                            Url = "/",
                            Field = ""
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Comment(CommentModel Model, string Email, bool SystemSide = false)
        {
            AdminResultModel result = new AdminResultModel()
            {
                Message = LanguageHelper.GetValue("ErrorCheckField")
            };
            if (ModelState.IsValid)
            {
                result = CommentMethods.Send(Model, Email, SystemSide);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult VoteComment(int ID, bool IsLike = true)
        {
            AdminResultModel result = new AdminResultModel()
            {
                Message = LanguageHelper.GetValue("ErrorCheckField")
            };
            if (ModelState.IsValid)
            {
                result = CommentMethods.Vote(ID, IsLike);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult NotFound(string aspxerrorpath)
        {
            if (!string.IsNullOrEmpty(aspxerrorpath))
            {
                TempData["Request404"] = aspxerrorpath;
                return Redirect("404");
            }
            return PartialView(SiteVariables.GetValue("NotFoundView"));
        }
    }
}