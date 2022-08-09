using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using PodBeeMedia.Helpers;

namespace PodBeeMedia.Models
{
    public class AdminAnswerModel
    {
        public int ID { get; set; }
        public int ContactID { get; set; }
        [AllowHtml]
        public string Content { get; set; }
    }
    public class AdminAnswerMethods
    {
        #region Get Methods
        public static AdminAnswerModel GetAnswer(Answers _Answer)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                AdminAnswerModel Answer = new AdminAnswerModel();
                try
                {
                    foreach (var prop in _Answer.GetType().GetProperties())
                    {
                        foreach (var _prop in Answer.GetType().GetProperties())
                        {
                            if (prop.Name == _prop.Name)
                            {
                                _prop.SetValue(Answer, prop.GetValue(_Answer));
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                return Answer;
            }
        }
        public static AdminAnswerModel GetAnswer(int id)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                Answers _Answer = db.Answers.FirstOrDefault(b => b.ID == id);
                return GetAnswer(_Answer);
            }
        }
        #endregion
        #region Answer
        public static AdminResultModel Answer(AdminAnswerModel Answer, string Email)
        {
            AdminResultModel model = new AdminResultModel();
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                model.Url = "/contacts";
                try
                {
                    Answers _Answer = new Answers();
                    foreach (var prop in Answer.GetType().GetProperties())
                    {
                        foreach (var _prop in _Answer.GetType().GetProperties())
                        {
                            if (prop.Name == _prop.Name)
                            {
                                 _prop.SetValue(_Answer, prop.GetValue(Answer));
                            }
                        }
                    }
                    if (Answer.Content == null || string.IsNullOrEmpty(Answer.Content.Trim()))
                    {
                        return AdminMethods.GetResult("ErrorAnswerEmpty", false, "/contacts", "Content");
                    }
                    db.Answers.InsertOnSubmit(_Answer);
                    db.SubmitChanges();
                    model.Message = LanguageHelper.GetValue("SuccessAnswerContact");
                    model.Status = true;
                    string to = Email;
                    string from = "info@podbeemedia.com";
                    MailMessage message = new MailMessage(from, to);
                    message.Subject = "Podbee Media - " + LanguageHelper.GetValue("YourMessageAnswered");
                    message.IsBodyHtml = true;
                    message.Body = Answer.Content;
                    SmtpClient client = SmtpModel.Smtp;
                    try
                    {
                        client.Send(message);
                    }
                    catch (Exception)
                    {
                        model.Message = LanguageHelper.GetValue("ErrorEmailSend");
                    }
                }
                catch (Exception)
                {
                    model.Message = LanguageHelper.GetValue("ErrorAnswerContact");
                    throw;
                }
            }
            return model;
        }
        #endregion
    }
}