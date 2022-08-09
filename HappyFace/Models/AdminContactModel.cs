using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using PodBeeMedia.Helpers;

namespace PodBeeMedia.Models
{
    public class AdminContactModel
    {
        public List<ContactModel> data { get; set; }
        public MetaModel meta { get; set; }
    }
    public class AdminContactDetailModel
    {
        public ContactModel Contact { get; set; }
        public AdminAnswerModel Answer { get; set; }
    }
    public class AdminContactMethods
    {
        #region Get Methods
        public static ContactModel GetContact(Contacts _Contact)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                ContactModel Contact = new ContactModel();
                try
                {
                    foreach (var prop in _Contact.GetType().GetProperties())
                    {
                        foreach (var _prop in Contact.GetType().GetProperties())
                        {
                            if (prop.Name == _prop.Name)
                            {
                                _prop.SetValue(Contact, prop.GetValue(_Contact));
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                return Contact;
            }
        }
        public static AdminContactDetailModel GetContact(int id)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                Contacts _Contact = db.Contacts.FirstOrDefault(b => b.ID == id);
                _Contact.IsReaded = true;
                db.SubmitChanges();
                AdminContactDetailModel model = new AdminContactDetailModel()
                {
                    Contact = GetContact(_Contact),
                    Answer = db.Answers.Any(a => a.ContactID == id) ? AdminAnswerMethods.GetAnswer(db.Answers.FirstOrDefault(a => a.ContactID == id)) : new AdminAnswerModel()
                };
                return model;
            }
        }
        public static List<ContactModel> GetContacts()
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                List<ContactModel> model = new List<ContactModel>();
                List<Contacts> contacts = db.Contacts.ToList();
                foreach (var contact in contacts)
                {
                    model.Add(GetContact(contact));
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
                model.Url = "/contacts";
                try
                {
                    IEnumerable<Contacts> contacts = db.Contacts.Where(n => n.ID == id).ToList();
                    db.Contacts.DeleteAllOnSubmit(contacts);
                    IEnumerable<Answers> answers = db.Answers.Where(n => n.ContactID == id).ToList();
                    db.Answers.DeleteAllOnSubmit(answers);
                    db.SubmitChanges();
                    model.Message = LanguageHelper.GetValue("SuccessDeleteContact");
                    model.Status = true;
                }
                catch (Exception)
                {
                    model.Message = LanguageHelper.GetValue("ErrorDeleteContact");
                    throw;
                }
            }
            return model;
        }
        #endregion
    }
}