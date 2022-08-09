using System.Web.Mvc;
using PodBeeMedia.Helpers;
using PodBeeMedia.Models;

namespace PodBeeMedia.Controllers
{
    public class CategoryController : BaseController
    {
        // GET: Category
        public ActionResult Index()
        {
            TempData["Type"] = "Category";
            CategoryModel model = new CategoryModel();
            if (TempData["CategoryPath"] != null)
            {
                model = CategoryMethods.GetCategory(TempData["CategoryPath"].ToString());
                TempData["SharedID"] = model.SharedID;
            }
            if (model.ID == 0)
            {
                return Redirect(LanguageHelper.CurrentLanguage == LanguageHelper.DefaultLanguage ? "/" : "/" + LanguageHelper.CurrentLanguage);
            }
            return View(model);
        }
    }
}