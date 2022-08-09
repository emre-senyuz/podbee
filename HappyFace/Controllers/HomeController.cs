using System.Collections.Generic;
using System.Web.Mvc;
using PodBeeMedia.Helpers;
using PodBeeMedia.Models;

namespace PodBeeMedia.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            TempData["Type"] = "Home";
            HomepageModel model = new HomepageModel();
            model.Critical = GlobalHelper.ReadCss("~/Public/css/critical/home.min.css");
            model.Slides = AdminSlideMethods.GetSlides();
            model.Blogs = AdminBlogMethods.GetBlogs(3);
            model.Podcasts = AdminPostMethods.GetPosts(8);
            model.WeeklyBees = AdminPostMethods.GetWeeklyBees();
            model.LatestEpisodes = AdminPostMethods.GetLatestEpisodes();
            return View(model);
        }
        public ActionResult Ads()
        {
            return View();
        }
        public ActionResult AboutUs()
        {
            return View();
        }
        public ActionResult Faq()
        {
            return View();
        }
        public ActionResult ContactUs()
        {
            List<SocialModel> model = AdminSocialMethods.GetSocials();
            return View(model);
        }
        public ActionResult Reference()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ContactUs(ContactModel Model)
        {
            AdminResultModel result = new AdminResultModel()
            {
                Message = LanguageHelper.GetValue("ErrorCheckField")
            };
            if (ModelState.IsValid)
            {
                result = ContactMethods.Send(Model);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Blog()
        {
            BlogListModel model = BlogMethods.GetBlogList();
            return View(model);
        }
        public ActionResult BlogDetail()
        {
            TempData["Type"] = "Blog";
            BlogDetailModel model = new BlogDetailModel();
            if (TempData["CategoryPath"] != null)
            {
                model = BlogMethods.GetBlogDetail(TempData["CategoryPath"].ToString());
                TempData["SharedID"] = model.Blog.SharedID;
            }
            if (model.Blog.ID == 0)
            {
                return Redirect(LanguageHelper.CurrentLanguage == LanguageHelper.DefaultLanguage ? "/" : "/" + LanguageHelper.CurrentLanguage);
            }
            return View(model);
        }
        public ActionResult LatestEpisode()
        {
            List<AdminLatestPostModel> model = AdminPostMethods.GetLatestEpisodes();
            return View(model);
        }
        public ActionResult Podcast(string search = "")
        {
            PostListModel model = PostMethods.GetPostList(search);
            return View(model);
        }
        public ActionResult PodcastDetail()
        {
            TempData["Type"] = "Podcast";
            PostModel model = new PostModel();
            if (TempData["CategoryPath"] != null)
            {
                model = PostMethods.GetPost(TempData["CategoryPath"].ToString());
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