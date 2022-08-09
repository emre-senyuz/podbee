using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using PodBeeMedia.Helpers;

namespace PodBeeMedia.Models
{
    public class AdminNewsletterModel
    {
        public List<NewsletterModel> data { get; set; }
        public MetaModel meta { get; set; }
    }
    public class AdminNewsletterMethods
    {
        #region Get Methods
        public static NewsletterModel GetNewsletter(Newsletters _Newsletter)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                NewsletterModel Newsletter = new NewsletterModel();
                try
                {
                    foreach (var prop in _Newsletter.GetType().GetProperties())
                    {
                        foreach (var _prop in Newsletter.GetType().GetProperties())
                        {
                            if (prop.Name == _prop.Name)
                            {
                                _prop.SetValue(Newsletter, prop.GetValue(_Newsletter));
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                return Newsletter;
            }
        }
        public static List<NewsletterModel> GetNewsletters()
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                List<NewsletterModel> model = new List<NewsletterModel>();
                List<Newsletters> newsletters = db.Newsletters.ToList();
                foreach (var newsletter in newsletters)
                {
                    model.Add(GetNewsletter(newsletter));
                }
                return model;
            }
        }
        #endregion
        #region Delete
        public static AdminResultModel Delete(int id)
        {
            AdminResultModel model = new AdminResultModel();
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                model.Url = "/newsletters";
                try
                {
                    IEnumerable<Newsletters> newsletters = db.Newsletters.Where(n => n.ID == id).ToList();
                    db.Newsletters.DeleteAllOnSubmit(newsletters);
                    db.SubmitChanges();
                    model.Message = LanguageHelper.GetValue("SuccessDeleteNewsletter");
                    model.Status = true;
                }
                catch (Exception)
                {
                    model.Message = LanguageHelper.GetValue("ErrorDeleteNewsletter");
                    throw;
                }
            }
            return model;
        }
        #endregion
    }
}