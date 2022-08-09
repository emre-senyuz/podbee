using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PodBeeMedia.Helpers;

namespace PodBeeMedia.Models
{
    public class NewsletterModel
    {
        public int ID { get; set; }
        public string Email { get; set; }
    }
    public class NewsletterMethods
    {
        public static AdminResultModel Add(string Email)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                if (Email == null || string.IsNullOrEmpty(Email.Trim()))
                {
                    return AdminMethods.GetResult("ErrorEmailEmpty", false, "/", "Email");
                }
                else if (!new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(Email))
                {
                    return AdminMethods.GetResult("ErrorEmailInvalid", false, "/", "Email");
                }
                else if (db.Newsletters.Any(n => n.Email == Email))
                {
                    return AdminMethods.GetResult("ErrorEmailExist", false, "/", "Email");
                }
                Newsletters _newsletter = new Newsletters()
                {
                    Email = Email
                };
                db.Newsletters.InsertOnSubmit(_newsletter);
                db.SubmitChanges();
                return AdminMethods.GetResult("SuccessNewsletter", true, "/", "");
            }
        }
    }
}