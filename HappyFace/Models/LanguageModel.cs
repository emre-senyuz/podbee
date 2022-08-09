using System.Collections.Generic;
using System.Linq;
using PodBeeMedia.Helpers;

namespace PodBeeMedia.Models
{
    public class LanguageRequestModel
    {
        public string Language { get; set; }
        public string Type { get; set; }
        public int SharedID { get; set; }
    }
    public class LanguageResponseModel
    {
        public string Url { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }
    }
    public class LanguageModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Prefix { get; set; }
        public string Icon { get; set; }
        public string Direction { get; set; }
        public bool IsDefault { get; set; }
        public bool Status { get; set; }
    }
    public class LanguageMethods
    {
        public static List<PageTypes> PageTypes
        {
            get
            {
                using (SQLDataDataContext db = new SQLDataDataContext())
                {
                    return db.PageTypes.ToList();
                }
            }
            set { }
        }
        public static string GetLanguageUrl(string type, string url)
        {
            return (LanguageHelper.CurrentLanguage == LanguageHelper.DefaultLanguage ? "/" : "/" + LanguageHelper.CurrentLanguage + "/") + LanguageHelper.GetValue(type) + "/" + url;
        }
        public static LanguageResponseModel GetPageType(LanguageRequestModel model)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                LanguageResponseModel response = LanguageHelper.CheckCulture(model);
                switch (model.Type)
                {
                    case "Admin":
                        response.Url = "/";
                        break;
                    case "Category":
                        Categories category = db.Categories.FirstOrDefault(c => c.Lang == model.Language && c.SharedID == model.SharedID);
                        if (category != null)
                        {
                            response.Url = GetLanguageUrl("UrlTypeCategory", category.Url);
                        }
                        break;
                    case "Blog":
                        response.Url = LanguageHelper.GetValue("UrlTypeBlog");
                        break;
                    case "BlogDetail":
                        Blogs blog = db.Blogs.FirstOrDefault(b => b.Lang == model.Language && b.SharedID == model.SharedID);
                        if (blog != null)
                        {
                            response.Url = GetLanguageUrl("UrlTypeBlog", blog.Url);
                        }
                        break;
                    case "LatestEpisode":
                        response.Url = LanguageHelper.GetValue("UrlTypeLatestEpisode");
                        break;
                    case "Podcast":
                        response.Url = LanguageHelper.GetValue("UrlTypePodcast");
                        break;
                    case "PodcastDetail":
                        Posts post = db.Posts.FirstOrDefault(p => p.Lang == model.Language && p.SharedID == model.SharedID);
                        if (post != null)
                        {
                            response.Url = GetLanguageUrl("UrlTypePodcast", post.Url);
                        }
                        break;
                    case "Ads":
                        response.Url = LanguageHelper.GetValue("UrlTypeAds");
                        break;
                    case "AboutUs":
                        response.Url = LanguageHelper.GetValue("UrlTypeAboutUs");
                        break;
                    case "Faq":
                        response.Url = LanguageHelper.GetValue("UrlTypeFaq");
                        break;
                    case "ContactUs":
                        response.Url = LanguageHelper.GetValue("UrlTypeContactUs");
                        break;
                    case "Reference":
                        response.Url = LanguageHelper.GetValue("UrlTypeReference");
                        break;
                    case "Register":
                        response.Url = GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlTypeRegister"));
                        break;
                    case "Login":
                        response.Url = GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlTypeLogin"));
                        break;
                    case "Profile":
                        response.Url = GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlTypeProfile"));
                        break;
                    case "ForgetPassword":
                        response.Url = GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlTypeForgetPassword"));
                        break;
                    case "ResetPassword":
                        response.Url = GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlTypeResetPassword"));
                        break;
                    case "ChangePassword":
                        response.Url = GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlTypeChangePassword"));
                        break;
                    case "Activation":
                        response.Url = GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlTypeActivation"));
                        break;
                    case "ReActivation":
                        response.Url = GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlTypeReActivation"));
                        break;
                    case "Information":
                        response.Url = GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlTypeInformation"));
                        break;
                    case "PersonelInformation":
                        response.Url = GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlTypePersonelInformation"));
                        break;
                    case "Comments":
                        response.Url = GetLanguageUrl("UrlTypeMember", LanguageHelper.GetValue("UrlTypeComments"));
                        break;
                    default:
                        break;
                }
                return response;
            }
        }
        public static LanguageResponseModel GetUrl(LanguageRequestModel model)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                List<Languages> languages = db.Languages.ToList();
                if (languages.Any(l => l.Prefix == model.Language) && !languages.FirstOrDefault(l => l.Prefix == model.Language).Status)
                {
                    return new LanguageResponseModel()
                    {
                        Url = LanguageHelper.CurrentLanguage == LanguageHelper.DefaultLanguage ? "/" : "/" + LanguageHelper.CurrentLanguage,
                        Message = LanguageHelper.GetValue("ErrorPassiveLanguage"),
                        Status = false
                    };
                }
                else if (!languages.Any(l => l.Prefix == model.Language))
                {
                    return new LanguageResponseModel()
                    {
                        Url = LanguageHelper.CurrentLanguage == LanguageHelper.DefaultLanguage ? "/" : "/" + LanguageHelper.CurrentLanguage,
                        Message = LanguageHelper.GetValue("ErrorUndefinedLanguage"),
                        Status = false
                    };
                }
                else if (!PageTypes.Any(t => t.Type == model.Type))
                {
                    return new LanguageResponseModel()
                    {
                        Url = LanguageHelper.CurrentLanguage == LanguageHelper.DefaultLanguage ? "/" : "/" + LanguageHelper.CurrentLanguage,
                        Message = LanguageHelper.GetValue("ErrorIncorrectPageType"),
                        Status = false
                    };
                }
                else
                {
                    return GetPageType(model);
                }
            }
        }
    }
}