using System.Web.Mvc;
using System.Linq;
using PodBeeMedia.Helpers;

namespace PodBeeMedia.Controllers
{
    public abstract class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                if (Session["CurrentLanguage"] != null)
                {
                    LanguageHelper.SetCulture(Session["CurrentLanguage"].ToString());
                }
                else
                {
                    LanguageHelper.SetCulture(LanguageHelper.DefaultLanguage);
                }
                if (filterContext.RouteData.Values.Keys.Any(k => k.Equals("lang")))
                {
                    string language = filterContext.HttpContext.Request.Path.Substring(1).Split('/').First();
                    if (db.Languages.Any(l => l.Prefix == language))
                    {
                        Session["CurrentLanguage"] = language;
                        LanguageHelper.SetCulture(language);
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult(LanguageHelper.CurrentLanguage == LanguageHelper.DefaultLanguage ? "/" : "/" + LanguageHelper.CurrentLanguage);
                    }
                }
                if (filterContext.RouteData.Values.Keys.Any(k => k.Equals("type")))
                {
                    string[] parameters = filterContext.HttpContext.Request.Path.Split('/');
                    TempData["CategoryPath"] = parameters.Last();
                }
            }
        }
    }
}