using System.Linq;
using System;
using System.Threading;
using System.Resources;
using System.Globalization;
using PodBeeMedia.Models;

namespace PodBeeMedia.Helpers
{
    public class LanguageHelper
    {
        public static string CurrentLanguage
        {
            get
            {
                return Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            }
            set { }
        }
        public static string CurrentDirection
        {
            get
            {
                using (SQLDataDataContext db = new SQLDataDataContext())
                {
                    Languages language = db.Languages.First(l => l.Prefix == CurrentLanguage);
                    return language.Direction;
                }
            }
            set { }
        }
        public static string DefaultLanguage
        {
            get
            {
                using (SQLDataDataContext db = new SQLDataDataContext())
                {
                    Languages language = db.Languages.First(l => l.IsDefault);
                    return language.Prefix;
                }
            }
            set { }
        }
        public static void SetCulture(string language)
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(language);
        }
        public static LanguageResponseModel CheckCulture(LanguageRequestModel model)
        {
            try
            {
                SetCulture(model.Language);
            }
            catch (Exception)
            {
                return new LanguageResponseModel()
                {
                    Url = CurrentLanguage == DefaultLanguage ? "/" : "/" + CurrentLanguage,
                    Message = GetValue("ErrorInvalidLanguageName"),
                    Status = false
                };
                throw;
            }
            return new LanguageResponseModel()
            {
                Url = CurrentLanguage == DefaultLanguage ? "/" : "/" + CurrentLanguage,
                Message = GetValue("SuccessLanguageChanged"),
                Status = true
            };
        }
        public static string GetValue(string name)
        {
            ResourceManager variables = new ResourceManager("PodBeeMedia.App_GlobalResources." + CurrentLanguage, typeof(LanguageHelper).Assembly);
            return variables.GetString(name);
        }
    }
}