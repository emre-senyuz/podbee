using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using PodBeeMedia.Helpers;

namespace PodBeeMedia.Models
{
    public class AdminLanguageModel
    {
        public List<LanguageModel> data { get; set; }
        public MetaModel meta { get; set; }
    }
    public class AdminLanguageMethods
    {
        public static LanguageModel GetLanguage(Languages _language)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                LanguageModel language = new LanguageModel();
                try
                {
                    foreach (var prop in _language.GetType().GetProperties())
                    {
                        foreach (var _prop in language.GetType().GetProperties())
                        {
                            if (prop.Name == _prop.Name)
                            {
                                _prop.SetValue(language, prop.GetValue(_language));
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                return language;
            }
        }
        public static LanguageModel GetLanguage(int id)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                Languages _language = db.Languages.FirstOrDefault(l => l.ID == id);
                return GetLanguage(_language);
            }
        }
        public static List<LanguageModel> GetLanguages()
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                List<LanguageModel> model = new List<LanguageModel>();
                List<Languages> languages = db.Languages.ToList();
                foreach (var language in languages)
                {
                    model.Add(GetLanguage(language));
                }
                return model;
            }
        }
        #region Column Update
        public static AdminResultModel Update(string prefix, string column, string value)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                try
                {
                    Languages language = db.Languages.FirstOrDefault(l => l.Prefix == prefix);
                    LanguageModel _language = new LanguageModel();
                    foreach (var prop in language.GetType().GetProperties())
                    {
                        foreach (var _prop in _language.GetType().GetProperties())
                        {
                            if (prop.Name == _prop.Name)
                            {
                                if (prop.Name != "Icon")
                                {
                                    _prop.SetValue(_language, prop.GetValue(language));
                                }
                            }
                        }
                    }
                    switch (column)
                    {
                        case "IsDefault":
                            _language.IsDefault = value == "true" ? false : true;
                            break;
                        case "Status":
                            _language.Status = value == "true" ? false : true;
                            break;
                        default:
                            break;
                    }
                    return Add(_language, null, true);
                }
                catch (Exception)
                {
                    return AdminMethods.GetResult("ErrorUpdateLanguage", false, "/languages");
                    throw;
                }
            }
        }
        #endregion
        #region Insert & Update
        public static AdminResultModel Add(LanguageModel language, HttpPostedFileBase file, bool update = false)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                try
                {
                    Languages _language = !update ? new Languages() : db.Languages.FirstOrDefault(l => l.ID == language.ID);
                    foreach (var prop in language.GetType().GetProperties())
                    {
                        foreach (var _prop in _language.GetType().GetProperties())
                        {
                            if (prop.Name == _prop.Name)
                            {
                                if ((prop.Name == "Icon" && !update) || prop.Name != "Icon")
                                {
                                    _prop.SetValue(_language, prop.GetValue(language));
                                }
                            }
                        }
                    }
                    if (language.Title == null || string.IsNullOrEmpty(language.Title.Trim()))
                    {
                        return AdminMethods.GetResult("ErrorTitleEmpty", false, "/languages", "Title");
                    }
                    if (language.Prefix == null || string.IsNullOrEmpty(language.Prefix.Trim()))
                    {
                        return AdminMethods.GetResult("ErrorPrefixEmpty", false, "/languages", "Prefix");
                    }
                    if (!language.Status)
                    {
                        if (!db.Languages.Any(l => l.Status && l.ID != language.ID))
                        {
                            return AdminMethods.GetResult("ErrorLastLanguage", false, "/languages");
                        }
                        else if (language.IsDefault)
                        {
                            if (!db.Languages.Any(l => l.IsDefault && l.Status && l.ID != language.ID))
                            {
                                _language.IsDefault = false;
                                Languages dlanguage = db.Languages.First(l => !l.IsDefault && l.Status && l.ID != language.ID);
                                dlanguage.IsDefault = true;
                            }
                            else
                            {
                                return AdminMethods.GetResult("ErrorPassiveDefaultLanguage", false, "/languages");
                            }
                        }
                    }
                    else
                    {
                        if (language.IsDefault)
                        {
                            if (db.Languages.Any(l => l.IsDefault && l.ID != language.ID))
                            {
                                Languages dlanguage = db.Languages.FirstOrDefault(l => l.IsDefault && l.ID != language.ID);
                                dlanguage.IsDefault = false;
                            }
                        }
                        else if (!db.Languages.Any(l => l.IsDefault && l.Status && l.ID != language.ID))
                        {
                            if (db.Languages.Any(l => !l.IsDefault && l.Status && l.ID != language.ID))
                            {
                                Languages dlanguage = db.Languages.First(l => !l.IsDefault && l.Status && l.ID != language.ID);
                                dlanguage.IsDefault = true;
                            }
                            else
                            {
                                return AdminMethods.GetResult("ErrorLastLanguage", false, "/languages");
                            }
                        }
                    }
                    if (file != null && !string.IsNullOrEmpty(file.FileName))
                    {
                        string _name = _language.Prefix;
                        string _path = GlobalHelper.GetFilePath("/Content/img/flag/", _name, file);
                        file.SaveAs(_path);
                        _language.Icon = "/Content/img/flag/" + GlobalHelper.CreateUrl(_name) + Path.GetExtension(file.FileName);
                    }
                    if (!update)
                    {
                        db.Languages.InsertOnSubmit(_language);
                    }
                    db.SubmitChanges();
                    return AdminMethods.GetResult(!update ? "SuccessAddLanguage" : "SuccessUpdateLanguage", true, "/languages");
                }
                catch (Exception)
                {
                    return AdminMethods.GetResult(!update ? "ErrorAddLanguage" : "ErrorUpdateLanguage", false, "/languages");
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
                model.Url = "/languages";
                if (db.Languages.ToList().Count > 1)
                {
                    try
                    {
                        Languages language = db.Languages.FirstOrDefault(l => l.ID == id);
                        db.Languages.DeleteOnSubmit(language);
                        if (language.IsDefault)
                        {
                            Languages ndefault = db.Languages.First(l => !l.IsDefault);
                            ndefault.IsDefault = true;
                        }
                        db.SubmitChanges();
                        model.Message = LanguageHelper.GetValue("SuccessDeleteLanguage");
                        model.Status = true;
                    }
                    catch (Exception)
                    {
                        model.Message = LanguageHelper.GetValue("ErrorDeleteLanguage");
                        throw;
                    }
                }
                else
                {
                    model.Message = LanguageHelper.GetValue("ErrorLastLanguage");
                }
            }
            return model;
        }
        #endregion
    }
}