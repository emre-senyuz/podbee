using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using PodBeeMedia.Helpers;

namespace PodBeeMedia.Models
{
    public class AdminGenderMethods
    {
        #region Get Methods
        public static GenderModel GetGender(MemberGenders _Gender)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                GenderModel Gender = new GenderModel();
                try
                {
                    foreach (var prop in _Gender.GetType().GetProperties())
                    {
                        foreach (var _prop in Gender.GetType().GetProperties())
                        {
                            if (prop.Name == _prop.Name)
                            {
                                _prop.SetValue(Gender, prop.GetValue(_Gender));
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                return Gender;
            }
        }
        public static List<GenderModel> GetGenders()
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                List<GenderModel> model = new List<GenderModel>();
                List<MemberGenders> genders = db.MemberGenders.ToList();
                foreach (var gender in genders)
                {
                    model.Add(GetGender(gender));
                }
                return model;
            }
        }
        #endregion
    }
}