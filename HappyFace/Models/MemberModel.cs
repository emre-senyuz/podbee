using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using PodBeeMedia.Helpers;

namespace PodBeeMedia.Models
{
    public class Member
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
    public class OperationResult
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string Redirect { get; set; }
        public string Field { get; set; }
    }
    public class MemberModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string IdentityNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public int GenderID { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime LastLoginDate { get; set; } = DateTime.Now;
        public int RoleID { get; set; } = 3;
        public bool Status { get; set; } = true;
        public bool Activated { get; set; } = false;
    }
    public class PermissionModel
    {
        public int RoleID { get; set; }
        public bool Page { get; set; }
        public bool Blog { get; set; }
        public bool News { get; set; }
        public bool Product { get; set; }
        public bool Category { get; set; }
        public bool Menu { get; set; }
        public bool Gallery { get; set; }
        public bool Member { get; set; }
        public bool Role { get; set; }
        public bool Language { get; set; }
    }
    public class InformationModel
    {
        public string Type { get; set; }
    }
    public class ActivationModel
    {
        public bool Type { get; set; } = false;
        public string Email { get; set; } = string.Empty;
    }
    public class TokenModel
    {
        public int ID { get; set; }
        public string Token { get; set; }
        public string Type { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
    }
    public class SmtpModel
    {
        public static string Domain
        {
            get
            {
                return "http://localhost:3960";
            }
            set { }
        }
        public static SmtpClient Smtp
        {
            get
            {
                SmtpClient client = new SmtpClient();
                client.UseDefaultCredentials = true;
                client.EnableSsl = false;
                client.Credentials = new NetworkCredential("info@podbeemedia.com", "PodBmedia15..");
                client.Host = "mail.podbeemedia.com"; //Or Your SMTP Server Address
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                return client;
            }
            set { }
        }
    }
    public class MemberMethods
    {
        public static PermissionModel GetPermissions(string email)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                PermissionModel _model = new PermissionModel();
                MemberPermissions model = db.MemberPermissions.FirstOrDefault(p => p.RoleID == Get(email).RoleID);
                try
                {
                    foreach (var prop in model.GetType().GetProperties())
                    {
                        foreach (var _prop in _model.GetType().GetProperties())
                        {
                            if (prop.Name == _prop.Name)
                            {
                                _prop.SetValue(_model, prop.GetValue(model));
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                return _model;
            }
        }
        public static bool AddToken(string email, string key, string type)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                MemberTokens token = new MemberTokens();
                token.Token = key;
                token.Type = type;
                token.Email = email;
                token.Status = true;
                db.MemberTokens.InsertOnSubmit(token);
                db.SubmitChanges();
                return true;
            }
        }
        public static ActivationModel CheckToken(string token, string type)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                MemberTokens model = db.MemberTokens.FirstOrDefault(t => t.Token == token && t.Type == type && t.Status);
                if (model != null && model.ID > 0)
                {
                    model.Status = false;
                    Members member = db.Members.FirstOrDefault(m => m.Email == model.Email);
                    if (type == "Activation")
                    {
                        member.Activated = true;
                    }
                    db.SubmitChanges();
                    return new ActivationModel()
                    {
                        Type = true,
                        Email = member.Email
                    };
                }
                else
                {
                    return new ActivationModel();
                }
            }
        }
        public static OperationResult FormatResult(bool status, string message, string redirect, string field = "")
        {
            return new OperationResult()
            {
                Status = status,
                Message = LanguageHelper.GetValue(message),
                Redirect = LanguageMethods.GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue(redirect)),
                Field = field
            };
        }
        public static MemberModel Find(string Email)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                Members member = db.Members.FirstOrDefault(m => m.Email == Email);
                MemberModel _member = new MemberModel();
                try
                {
                    foreach (var prop in member.GetType().GetProperties())
                    {
                        foreach (var _prop in _member.GetType().GetProperties())
                        {
                            if (prop.Name == _prop.Name)
                            {
                                _prop.SetValue(_member, prop.GetValue(member));
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                return _member;
            }
        }
        public static MemberModel Get(string email)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                if (db.Members.Any(m => m.Email == email))
                {
                    return Find(email);
                }
            }
            return new MemberModel();
        }
        public static MemberModel Get(int ID)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                if (db.Members.Any(m => m.ID == ID))
                {
                    Members _member = db.Members.FirstOrDefault(m => m.ID == ID);
                    return Find(_member.Email);
                }
            }
            return new MemberModel();
        }
        public static MemberModel Get(string email, string password)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                if (db.Members.Any(m => m.Email == email && m.Password == GlobalHelper.Encrypt(password)))
                {
                    return Find(email);
                }
            }
            return new MemberModel();
        }
        public static OperationResult Check(MemberModel member, string eredirect)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                if (db.Members.Any(m => m.Email == member.Email))
                {
                    return FormatResult(false, LanguageHelper.GetValue("ErrorEmailExist"), LanguageMethods.GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlType" + eredirect)), "Email");
                }
            }
            return new OperationResult()
            {
                Status = true
            };
        }
        public static OperationResult Insert(MemberModel member, string sredirect, string eredirect)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                try
                {
                    Members _member = new Members();
                    foreach (var prop in member.GetType().GetProperties())
                    {
                        foreach (var _prop in _member.GetType().GetProperties())
                        {
                            if (prop.Name == _prop.Name)
                            {
                                _prop.SetValue(_member, prop.GetValue(member));
                            }
                        }
                    }
                    db.Members.InsertOnSubmit(_member);
                    db.SubmitChanges();
                    return new OperationResult()
                    {
                        Status = true,
                        Message = LanguageHelper.GetValue("SuccessRegister"),
                        Redirect = LanguageMethods.GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlType" + sredirect))
                    };
                }
                catch (Exception)
                {
                    return new OperationResult()
                    {
                        Status = false,
                        Message = LanguageHelper.GetValue("ErrorRegister"),
                        Redirect = LanguageMethods.GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlType" + eredirect))
                    };
                    throw;
                }
            }
        }
        public static OperationResult Add(MemberModel member, string eredirect)
        {
            OperationResult check = Check(member, eredirect);
            if (!check.Status)
            {
                return check;
            }
            member.Password = GlobalHelper.Encrypt(member.Password);
            return Insert(member, "Profile", eredirect);
        }
        public static OperationResult Update(MemberModel member, string eredirect)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                try
                {
                    Members _member = db.Members.FirstOrDefault(m => m.ID == member.ID);
                    if (!string.IsNullOrEmpty(member.Password))
                    {
                        member.Password = GlobalHelper.Encrypt(member.Password);
                    }
                    foreach (var prop in member.GetType().GetProperties())
                    {
                        foreach (var _prop in _member.GetType().GetProperties())
                        {
                            if (prop.Name == _prop.Name)
                            {
                                if (prop.Name != "Password" || (prop.Name == "Password" && !string.IsNullOrEmpty(member.Password)))
                                {
                                    _prop.SetValue(_member, prop.GetValue(member));
                                }
                            }
                        }
                    }
                    db.SubmitChanges();
                    return new OperationResult()
                    {
                        Status = true,
                        Message = LanguageHelper.GetValue("SuccessChangePassword"),
                        Redirect = LanguageMethods.GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlTypeProfile"))
                    };
                }
                catch (Exception ex)
                {
                    return new OperationResult()
                    {
                        Status = false,
                        Message = ex.Message,
                        Redirect = LanguageMethods.GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlType" + eredirect))
                    };
                    throw;
                }
            }
        }
    }
}