using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using PodBeeMedia.Helpers;

namespace PodBeeMedia.Models
{
    public class AdminMemberModel
    {
        public List<MemberModel> data { get; set; }
        public MetaModel meta { get; set; }
    }
    public class AdminMemberViewModel
    {
        public List<GenderModel> Genders { get; set; } = AdminGenderMethods.GetGenders().ToList();
    }
    public class AdminMemberDetailModel
    {
        public MemberModel Member { get; set; }
        public List<GenderModel> Genders { get; set; }
    }
    public class AdminMemberMethods
    {
        #region Get Methods
        public static MemberModel GetMember(Members _Member)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                MemberModel Member = new MemberModel();
                try
                {
                    foreach (var prop in _Member.GetType().GetProperties())
                    {
                        foreach (var _prop in Member.GetType().GetProperties())
                        {
                            if (prop.Name == _prop.Name)
                            {
                                _prop.SetValue(Member, prop.GetValue(_Member));
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                return Member;
            }
        }
        public static MemberModel GetMember(int id)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                Members _Member = db.Members.FirstOrDefault(m => m.ID == id);
                return GetMember(_Member);
            }
        }
        public static List<MemberModel> GetMembers()
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                List<MemberModel> model = new List<MemberModel>();
                List<Members> members = db.Members.ToList();
                foreach (var member in members)
                {
                    model.Add(GetMember(member));
                }
                return model;
            }
        }
        public static AdminMemberDetailModel GetSlideDetail(int id)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                AdminMemberDetailModel slide = new AdminMemberDetailModel()
                {
                    Member = GetMember(id),
                    Genders = AdminGenderMethods.GetGenders()
                };
                return slide;
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
                    switch (column)
                    {
                        case "Activated":
                            foreach (var member in db.Members.Where(m => m.ID == id).ToList())
                            {
                                member.Activated = value == "true" ? false : true;
                            }
                            break;
                        case "Status":
                            foreach (var member in db.Members.Where(m => m.ID == id).ToList())
                            {
                                member.Status = value == "true" ? false : true;
                            }
                            break;
                        default:
                            break;
                    }
                    db.SubmitChanges();
                    return AdminMethods.GetResult("SuccessUpdateMember", true, "/members");
                }
                catch (Exception)
                {
                    return new AdminResultModel()
                    {
                        Message = LanguageHelper.GetValue("ErrorUpdateMember")
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
                model.Url = "/members";
                try
                {
                    IEnumerable<Members> members = db.Members.Where(m => m.ID == id).ToList();
                    db.Members.DeleteAllOnSubmit(members);
                    db.SubmitChanges();
                    model.Message = LanguageHelper.GetValue("SuccessDeleteMember");
                    model.Status = true;
                }
                catch (Exception)
                {
                    model.Message = LanguageHelper.GetValue("ErrorDeleteMember");
                    throw;
                }
            }
            return model;
        }
        #endregion
    }
}