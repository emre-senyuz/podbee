using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Security;
using PodBeeMedia.Helpers;
using PodBeeMedia.Models;

namespace PodBeeMedia.Controllers
{
    public class MemberController : BaseController
    {
        #region Profile
        [Authorize]
        public ActionResult Index()
        {
            TempData["Type"] = "Profile";
            MemberCommentModel model = new MemberCommentModel();
            model.Member = MemberMethods.Get(User.Identity.Name);
            if (!model.Member.Activated)
            {
                TempData["ActivationStatus"] = "InActive";
                return Redirect(LanguageMethods.GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlTypeInformation")));
            }
            model.Comments = AdminCommentMethods.GetComments(model.Member.ID, 5);
            return View(model);
        }
        #endregion
        #region Login
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect(LanguageMethods.GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlTypeProfile")));
            }
            TempData["Type"] = "Login";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Login(LoginModel user, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                using (SQLDataDataContext db = new SQLDataDataContext())
                {
                    if (user.Email == null || string.IsNullOrEmpty(user.Email.Trim()))
                    {
                        return Json(MemberMethods.FormatResult(false, "ErrorEmailEmpty", "UrlTypeLogin", "Email"), JsonRequestBehavior.AllowGet);
                    }
                    else if (!new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(user.Email))
                    {
                        return Json(MemberMethods.FormatResult(false, "ErrorEmailInvalid", "UrlTypeLogin", "Email"), JsonRequestBehavior.AllowGet);
                    }
                    else if (!db.Members.Any(m => m.Email == user.Email))
                    {
                        return Json(MemberMethods.FormatResult(false, "ErrorEmailUndefined", "UrlTypeLogin", "Email"), JsonRequestBehavior.AllowGet);
                    }
                    else if (user.Password == null || string.IsNullOrEmpty(user.Password.Trim()))
                    {
                        return Json(MemberMethods.FormatResult(false, "ErrorPasswordEmpty", "UrlTypeLogin", "Password"), JsonRequestBehavior.AllowGet);
                    }
                    MemberModel model = MemberMethods.Get(user.Email, user.Password);
                    if (model != null && model.ID > 0)
                    {
                        if (!model.Activated)
                        {
                            TempData["ActivationStatus"] = "InActive";
                        }
                        FormsAuthentication.SetAuthCookie(model.Email, user.Remember);
                        if (!string.IsNullOrEmpty(ReturnUrl))
                        {
                            return Json(new OperationResult()
                            {
                                Status = true,
                                Message = LanguageHelper.GetValue("SuccessLogin"),
                                Redirect = ReturnUrl
                            }, JsonRequestBehavior.AllowGet);
                        }
                        return Json(MemberMethods.FormatResult(true, "SuccessLogin", "UrlTypeProfile"), JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(MemberMethods.FormatResult(false, "ErrorLogin", "UrlTypeLogin"), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Login Router
        public ActionResult LoginRouter(string ReturnUrl)
        {
            return Redirect(LanguageMethods.GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlTypeLogin")) + "?ReturnUrl=" + ReturnUrl);
        }
        #endregion
        #region Register
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect(LanguageMethods.GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlTypeProfile")));
            }
            TempData["Type"] = "Register";
            RegisterModel model = new RegisterModel();
            model.Genders = GenderMethods.GetGenders();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Register(MemberModel member)
        {
            if (ModelState.IsValid)
            {
                using (SQLDataDataContext db = new SQLDataDataContext())
                {
                    if (member.Name == null || string.IsNullOrEmpty(member.Name.Trim()))
                    {
                        return Json(MemberMethods.FormatResult(false, "ErrorNameEmpty", "UrlTypeRegister", "Name"), JsonRequestBehavior.AllowGet);
                    }
                    else if (member.Surname == null || string.IsNullOrEmpty(member.Surname.Trim()))
                    {
                        return Json(MemberMethods.FormatResult(false, "ErrorSurnameEmpty", "UrlTypeRegister", "Surname"), JsonRequestBehavior.AllowGet);
                    }
                    else if (member.Email == null || string.IsNullOrEmpty(member.Email.Trim()))
                    {
                        return Json(MemberMethods.FormatResult(false, "ErrorEmailEmpty", "UrlTypeRegister", "Email"), JsonRequestBehavior.AllowGet);
                    }
                    else if (!new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(member.Email))
                    {
                        return Json(MemberMethods.FormatResult(false, "ErrorEmailInvalid", "UrlTypeRegister", "Email"), JsonRequestBehavior.AllowGet);
                    }
                    else if (db.Members.Any(m => m.Email == member.Email))
                    {
                        return Json(MemberMethods.FormatResult(false, "ErrorEmailExist", "UrlTypeLogin", "Email"), JsonRequestBehavior.AllowGet);
                    }
                    else if (member.Password == null || string.IsNullOrEmpty(member.Password.Trim()))
                    {
                        return Json(MemberMethods.FormatResult(false, "ErrorPasswordEmpty", "UrlTypeRegister", "Password"), JsonRequestBehavior.AllowGet);
                    }
                    else if (member.Password.Trim().Length < 6)
                    {
                        return Json(MemberMethods.FormatResult(false, "ErrorPasswordMinLength", "UrlTypeRegister", "Password"), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        OperationResult model = MemberMethods.Add(member, "Register");
                        if (model.Status)
                        {
                            var token = Guid.NewGuid().ToString();
                            if (MemberMethods.AddToken(member.Email, token, "Activation"))
                            {
                                string to = member.Email;
                                string from = "info@podbeemedia.com";
                                MailMessage message = new MailMessage(from, to);
                                message.Subject = "Podbee Media - " + LanguageHelper.GetValue("PageActivationTitle");
                                message.Body = SmtpModel.Domain + LanguageMethods.GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlTypeActivation")) + "?token=" + token;
                                SmtpClient client = SmtpModel.Smtp;
                                try
                                {
                                    client.Send(message);
                                }
                                catch (Exception)
                                {
                                    return Json(MemberMethods.FormatResult(false, "ErrorEmailSend", "UrlTypeRegister"), JsonRequestBehavior.AllowGet);
                                }
                            }
                            FormsAuthentication.SetAuthCookie(member.Email, false);
                            return Json(MemberMethods.FormatResult(true, "SuccessRegister", "UrlTypeInformation"), JsonRequestBehavior.AllowGet);
                        }
                        return Json(model, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(MemberMethods.FormatResult(false, "ErrorRegister", "UrlTypeRegister"), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region ForgetPassword
        public ActionResult ForgetPassword()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect(LanguageMethods.GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlTypeProfile")));
            }
            TempData["Type"] = "ForgetPassword";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ForgetPassword(string Email)
        {
            if (ModelState.IsValid)
            {
                using (SQLDataDataContext db = new SQLDataDataContext())
                {
                    if (Email == null || string.IsNullOrEmpty(Email.Trim()))
                    {
                        return Json(MemberMethods.FormatResult(false, "ErrorEmailEmpty", "UrlTypeForgetPassword", "Email"), JsonRequestBehavior.AllowGet);
                    }
                    else if (!new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(Email))
                    {
                        return Json(MemberMethods.FormatResult(false, "ErrorEmailInvalid", "UrlTypeForgetPassword", "Email"), JsonRequestBehavior.AllowGet);
                    }
                    else if (!db.Members.Any(m => m.Email == Email))
                    {
                        return Json(MemberMethods.FormatResult(false, "ErrorEmailExist", "UrlTypeLogin", "Email"), JsonRequestBehavior.AllowGet);
                    }
                    MemberModel model = MemberMethods.Find(Email);
                    if (model != null && model.ID > 0)
                    {
                        var token = Guid.NewGuid().ToString();
                        if (MemberMethods.AddToken(model.Email, token, "ResetPassword"))
                        {
                            string to = model.Email;
                            string from = "info@podbeemedia.com";
                            MailMessage message = new MailMessage(from, to);
                            message.Subject = "Happy Faces " + LanguageHelper.GetValue("PageResetPasswordTitle");
                            message.Body = SmtpModel.Domain + LanguageMethods.GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlTypeResetPassword")) + "?token=" + token;
                            SmtpClient client = SmtpModel.Smtp;
                            try
                            {
                                client.Send(message);
                            }
                            catch (Exception)
                            {
                                return Json(MemberMethods.FormatResult(false, "ErrorEmailSend", "UrlTypeForgetPassword"), JsonRequestBehavior.AllowGet);
                            }
                        }
                        return Json(MemberMethods.FormatResult(true, "SuccessForgetPassword", "UrlTypeLogin"), JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(MemberMethods.FormatResult(false, "ErrorForgetPassword", "UrlTypeForgetPassword"), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region ResetPassword
        public ActionResult ResetPassword(string token)
        {
            TempData["Type"] = "ResetPassword";
            ActivationModel model = new ActivationModel();
            if (User.Identity.IsAuthenticated)
            {
                return Redirect(LanguageMethods.GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlTypeProfile")));
            }
            if (string.IsNullOrEmpty(token))
            {
                return Redirect(LanguageMethods.GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlTypeLogin")));
            }
            else
            {
                model = MemberMethods.CheckToken(token, "ResetPassword");
                if (model.Type)
                {
                    MemberModel member = MemberMethods.Get(model.Email);
                    if (!member.Activated)
                    {
                        TempData["ActivationStatus"] = "InActive";
                        return Redirect(LanguageMethods.GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlTypeInformation")));
                    }
                }
                else
                {
                    return Redirect(LanguageMethods.GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlTypeProfile")));
                }
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ResetPassword(string email, string NewPassword, string NewPasswordConfirm)
        {
            if (ModelState.IsValid)
            {
                MemberModel model = MemberMethods.Get(email);
                if (model.ID == 0)
                {
                    return Json(MemberMethods.FormatResult(false, "ErrorEmail", "UrlTypeChangePassword", "Email"), JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(NewPassword.Trim()))
                {
                    return Json(MemberMethods.FormatResult(false, "ErrorNewPasswordEmpty", "UrlTypeChangePassword", "NewPassword"), JsonRequestBehavior.AllowGet);
                }
                else if (string.IsNullOrEmpty(NewPasswordConfirm.Trim()))
                {
                    return Json(MemberMethods.FormatResult(false, "ErrorNewPasswordConfirmEmpty", "UrlTypeChangePassword", "NewPasswordConfirm"), JsonRequestBehavior.AllowGet);
                }
                else if (NewPassword != NewPasswordConfirm)
                {
                    return Json(MemberMethods.FormatResult(false, "ErrorNewPasswordMustBeSame", "UrlTypeChangePassword", "NewPasswordConfirm"), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    model.Password = NewPassword;
                    OperationResult result = MemberMethods.Update(model, "ChangePassword");
                    if (result.Status)
                    {
                        FormsAuthentication.SetAuthCookie(model.Email, false);
                        result.Message = LanguageHelper.GetValue("SuccessChangePassword");
                    }
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(MemberMethods.FormatResult(false, "ErrorLogin", "UrlTypeLogin"), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region ChangePassword
        [Authorize]
        public ActionResult ChangePassword()
        {
            MemberModel member = MemberMethods.Get(User.Identity.Name);
            if (!member.Activated)
            {
                TempData["ActivationStatus"] = "InActive";
                return Redirect(LanguageMethods.GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlTypeInformation")));
            }
            TempData["Type"] = "ChangePassword";
            return View(member);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ChangePassword(string OldPassword, string NewPassword, string NewPasswordConfirm)
        {
            var token = Guid.NewGuid().ToString();
            if (ModelState.IsValid)
            {
                MemberModel model = MemberMethods.Get(User.Identity.Name, OldPassword);
                if (OldPassword == null || string.IsNullOrEmpty(OldPassword.Trim()))
                {
                    return Json(MemberMethods.FormatResult(false, "ErrorPasswordEmpty", "UrlTypeChangePassword", "OldPassword"), JsonRequestBehavior.AllowGet);
                }
                else if (model.ID == 0)
                {
                    return Json(MemberMethods.FormatResult(false, "ErrorPassword", "UrlTypeChangePassword", "OldPassword"), JsonRequestBehavior.AllowGet);
                }
                if (NewPassword == null || string.IsNullOrEmpty(NewPassword.Trim()))
                {
                    return Json(MemberMethods.FormatResult(false, "ErrorNewPasswordEmpty", "UrlTypeChangePassword", "NewPassword"), JsonRequestBehavior.AllowGet);
                }
                else if (NewPasswordConfirm == null || string.IsNullOrEmpty(NewPasswordConfirm.Trim()))
                {
                    return Json(MemberMethods.FormatResult(false, "ErrorNewPasswordConfirmEmpty", "UrlTypeChangePassword", "NewPasswordConfirm"), JsonRequestBehavior.AllowGet);
                }
                else if (NewPassword != NewPasswordConfirm)
                {
                    return Json(MemberMethods.FormatResult(false, "ErrorNewPasswordMustBeSame", "UrlTypeChangePassword", "NewPasswordConfirm"), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    model.Password = NewPassword;
                    OperationResult result = MemberMethods.Update(model, "ChangePassword");
                    if (result.Status)
                    {
                        result.Message = LanguageHelper.GetValue("SuccessChangePassword");
                    }
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(MemberMethods.FormatResult(false, "ErrorLogin", "UrlTypeLogin"), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Activation
        public ActionResult Activation(string token)
        {
            TempData["Type"] = "Activation";
            ActivationModel model = new ActivationModel();
            if (User.Identity.IsAuthenticated)
            {
                return Redirect(LanguageMethods.GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlTypeProfile")));
            }
            if (string.IsNullOrEmpty(token))
            {
                return Redirect(LanguageMethods.GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlTypeLogin")));
            }
            else
            {
                model = MemberMethods.CheckToken(token, "Activation");
                if (model.Type)
                {
                    TempData["ActivationStatus"] = "Active";
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                }
            }
            return View(model);
        }
        #endregion
        #region ReActivation
        public ActionResult ReActivation()
        {
            TempData["Type"] = "ReActivation";
            if (User.Identity.IsAuthenticated)
            {
                return Redirect(LanguageMethods.GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlTypeProfile")));
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ReActivation(string Email)
        {
            if (ModelState.IsValid)
            {
                using (SQLDataDataContext db = new SQLDataDataContext())
                {
                    if (Email == null || string.IsNullOrEmpty(Email.Trim()))
                    {
                        return Json(MemberMethods.FormatResult(false, "ErrorEmailEmpty", "UrlTypeReActivation", "Email"), JsonRequestBehavior.AllowGet);
                    }
                    else if (!new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(Email))
                    {
                        return Json(MemberMethods.FormatResult(false, "ErrorEmailInvalid", "UrlTypeReActivation", "Email"), JsonRequestBehavior.AllowGet);
                    }
                    else if (!db.Members.Any(m => m.Email == Email))
                    {
                        return Json(MemberMethods.FormatResult(false, "ErrorEmailExist", "UrlTypeLogin", "Email"), JsonRequestBehavior.AllowGet);
                    }
                    MemberModel model = MemberMethods.Find(Email);
                    if (model != null && model.ID > 0)
                    {
                        var token = Guid.NewGuid().ToString();
                        if (MemberMethods.AddToken(model.Email, token, "Activation"))
                        {
                            string to = model.Email;
                            string from = "info@podbeemedia.com";
                            MailMessage message = new MailMessage(from, to);
                            message.Subject = "Happy Faces " + LanguageHelper.GetValue("PageActivationTitle");
                            message.Body = SmtpModel.Domain + LanguageMethods.GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlTypeActivation")) + "?token=" + token;
                            SmtpClient client = SmtpModel.Smtp;
                            try
                            {
                                client.Send(message);
                            }
                            catch (Exception)
                            {
                                return Json(MemberMethods.FormatResult(false, "ErrorEmailSend", "UrlTypeReActivation"), JsonRequestBehavior.AllowGet);
                            }
                        }
                        return Json(MemberMethods.FormatResult(true, "SuccessForgetPassword", "UrlTypeLogin"), JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(MemberMethods.FormatResult(false, "ErrorForgetPassword", "UrlTypeReActivation"), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Information
        [Authorize]
        public ActionResult Information()
        {
            if (User.Identity.IsAuthenticated && TempData["ActivationStatus"] != null && TempData["ActivationStatus"].ToString() == "Active")
            {
                return Redirect(LanguageMethods.GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlTypeProfile")));
            }
            FormsAuthentication.SignOut();
            TempData["Type"] = "Information";
            return View(new InformationModel()
            {
                Type = TempData["ActivationStatus"] == null ? "NeedActivation" : TempData["ActivationStatus"].ToString()
            });
        }
        #endregion
        #region Personel Information
        [Authorize]
        public ActionResult PersonelInformation()
        {
            TempData["Type"] = "PersonelInformation";
            RegisterModel model = new RegisterModel();
            model.Member = MemberMethods.Get(User.Identity.Name);
            if (!model.Member.Activated)
            {
                TempData["ActivationStatus"] = "InActive";
                return Redirect(LanguageMethods.GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlTypeInformation")));
            }
            model.Genders = GenderMethods.GetGenders();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult PersonelInformation(MemberModel member)
        {
            if (ModelState.IsValid)
            {
                using (SQLDataDataContext db = new SQLDataDataContext())
                {
                    if (member.Name == null || string.IsNullOrEmpty(member.Name.Trim()))
                    {
                        return Json(MemberMethods.FormatResult(false, "ErrorNameEmpty", "UrlTypeRegister", "Name"), JsonRequestBehavior.AllowGet);
                    }
                    else if (member.Surname == null || string.IsNullOrEmpty(member.Surname.Trim()))
                    {
                        return Json(MemberMethods.FormatResult(false, "ErrorSurnameEmpty", "UrlTypeRegister", "Surname"), JsonRequestBehavior.AllowGet);
                    }
                    else if (member.Email == null || string.IsNullOrEmpty(member.Email.Trim()))
                    {
                        return Json(MemberMethods.FormatResult(false, "ErrorEmailEmpty", "UrlTypeRegister", "Email"), JsonRequestBehavior.AllowGet);
                    }
                    else if (!new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(member.Email))
                    {
                        return Json(MemberMethods.FormatResult(false, "ErrorEmailInvalid", "UrlTypeRegister", "Email"), JsonRequestBehavior.AllowGet);
                    }
                    else if (db.Members.Any(m => m.ID != member.ID && m.Email == member.Email))
                    {
                        return Json(MemberMethods.FormatResult(false, "ErrorEmailExist", "UrlTypeLogin", "Email"), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        OperationResult model = MemberMethods.Update(member, "PersonelInformation");
                        if (model.Status)
                        {
                            return Json(MemberMethods.FormatResult(true, "SuccessUpdateInformation", "UrlTypePersonelInformation"), JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            return Json(MemberMethods.FormatResult(false, "ErrorUpdateInformation", "UrlTypePersonelInformation"), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Comments
        [Authorize]
        public ActionResult Comments()
        {
            TempData["Type"] = "Comments";
            MemberCommentModel model = new MemberCommentModel();
            model.Member = MemberMethods.Get(User.Identity.Name);
            if (!model.Member.Activated)
            {
                TempData["ActivationStatus"] = "InActive";
                return Redirect(LanguageMethods.GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlTypeInformation")));
            }
            model.Comments = AdminCommentMethods.GetComments(model.Member.ID, 0);
            return View(model);
        }
        #endregion
        #region Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect(LanguageHelper.CurrentLanguage == LanguageHelper.DefaultLanguage ? "/" : "/" + LanguageHelper.CurrentLanguage);
        }
        #endregion
    }
}