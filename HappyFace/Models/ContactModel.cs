using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using PodBeeMedia.Helpers;

namespace PodBeeMedia.Models
{
    public class ContactModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsReaded { get; set; }
    }
    public class ContactMethods
    {
        public static AdminResultModel SendAdminMail(ContactModel Model)
        {
            string to = "info@podbeemedia.com";
            string from = "info@podbeemedia.com";
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Podbee Media - " + LanguageHelper.GetValue("PageContactUsTitle") + "-" + Model.Subject;
            message.IsBodyHtml = true;
            message.Body = "<table border=\"0\" width=\"600\" cellpadding=\"0\" cellspacing=\"0\">" +
                                "<tr>" +
                                    "<td width=\"150\" valign=\"top\">" +
                                        "<strong>" + LanguageHelper.GetValue("FormName") + "</strong>" +
                                    "</td>" +
                                    "<td>" +
                                        Model.Name +
                                    "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td width=\"150\" valign=\"top\">" +
                                        "<strong>" + LanguageHelper.GetValue("FormEmail") + "</strong>" +
                                    "</td>" +
                                    "<td>" +
                                        Model.Email +
                                    "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td width=\"150\" valign=\"top\">" +
                                        "<strong>" + LanguageHelper.GetValue("FormSubject") + "</strong>" +
                                    "</td>" +
                                    "<td>" +
                                        Model.Subject +
                                    "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td width=\"150\" valign=\"top\">" +
                                        "<strong>" + LanguageHelper.GetValue("FormMessage") + "</strong>" +
                                    "</td>" +
                                    "<td>" +
                                        Model.Message +
                                    "</td>" +
                                "</tr>" +
                            "</table>";
            SmtpClient client = SmtpModel.Smtp;
            try
            {
                client.Send(message);
                return AdminMethods.GetResult("SuccessContact", true, "/", "");
            }
            catch (Exception)
            {
                return AdminMethods.GetResult("ErrorEmailSend", false, "/", "");
            }
        }
        public static AdminResultModel SendMemberMail(string emailTo)
        {
            string to = emailTo;
            string from = "info@podbeemedia.com";
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Podbee Media - " + LanguageHelper.GetValue("YourMessageSended");
            message.Body = LanguageHelper.GetValue("MessageSendedText");
            SmtpClient client = SmtpModel.Smtp;
            try
            {
                client.Send(message);
                return AdminMethods.GetResult("SuccessContact", true, "/", "");
            }
            catch (Exception)
            {
                return AdminMethods.GetResult("ErrorEmailSend", false, "/", "");
            }
        }
        public static AdminResultModel Send(ContactModel Model)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                if (Model.Name == null || string.IsNullOrEmpty(Model.Name.Trim()))
                {
                    return AdminMethods.GetResult("ErrorNameEmpty", false, "/", "Name");
                }
                if (Model.Email == null || string.IsNullOrEmpty(Model.Email.Trim()))
                {
                    return AdminMethods.GetResult("ErrorEmailEmpty", false, "/", "Email");
                }
                else if (!new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(Model.Email))
                {
                    return AdminMethods.GetResult("ErrorEmailInvalid", false, "/", "Email");
                }
                if (Model.Subject == null || string.IsNullOrEmpty(Model.Subject.Trim()))
                {
                    return AdminMethods.GetResult("ErrorSubjectEmpty", false, "/", "Subject");
                }
                if (Model.Message == null || string.IsNullOrEmpty(Model.Message.Trim()))
                {
                    return AdminMethods.GetResult("ErrorMessageEmpty", false, "/", "Message");
                }
                Contacts _contact = new Contacts()
                {
                    Name = Model.Name,
                    Email = Model.Email,
                    Subject = Model.Subject,
                    Message = Model.Message,
                    IsReaded = false
                };
                db.Contacts.InsertOnSubmit(_contact);
                db.SubmitChanges();
                SendAdminMail(Model);
                SendMemberMail(Model.Email);
                return AdminMethods.GetResult("SuccessContact", true, "/", "");
            }
        }
    }
}