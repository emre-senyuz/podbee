using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using PodBeeMedia.Helpers;
using PodBeeMedia.Models;

namespace PodBeeMedia.Controllers
{
    public class AdminController : BaseController
    {
        #region Dashboard
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult Index()
        {
            return View();
        }
        #endregion
        #region Header
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult Header()
        {
            AdminHeaderModel model = new AdminHeaderModel()
            {
                Member = MemberMethods.Get(User.Identity.Name),
                Languages = HeaderMethods.GetLanguageMenu(),
                QucikPanel = new QucikPanel()
            };
            return PartialView("Header/Header", model);
        }
        #endregion
        #region SideMenu
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult SideMenu()
        {
            PermissionModel model = MemberMethods.GetPermissions(User.Identity.Name);
            return PartialView("SideMenu", model);
        }
        #endregion
        #region General
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult GetUrlString(string text)
        {
            return Json(new {
                url = GlobalHelper.CreateUrl(text)
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Category
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult GetParentCategory(string type)
        {
            return Json(AdminCategoryMethods.GetParentCategory(type), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult CheckParentCategory(int id)
        {
            return Json(AdminCategoryMethods.CheckParentCategory(id), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult DeleteSubCategory(int id)
        {
            return Json(AdminCategoryMethods.DeleteSub(id), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult MoveCategory(int id, int parentid)
        {
            return Json(AdminCategoryMethods.SetParentCat(id, parentid), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult CopyCategory(int id)
        {
            return Json(AdminCategoryMethods.Copy(id), JsonRequestBehavior.AllowGet);
        }
        #region Categories
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult Categories()
        {
            return View(new AdminCategoryViewModel());
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult CategoriesJson(int parent = 0)
        {
            List<CategoryModel> data = AdminCategoryMethods.GetCategories(parent);
            AdminCategoryModel model = new AdminCategoryModel()
            {
                data = data,
                meta = new MetaModel()
                {
                    pages = data.Count / 10,
                    total = data.Count
                }
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CategoryTreeJson(int id, int parent = 0)
        {
            List<AdminCategoryItemModel> model = new List<AdminCategoryItemModel>();
            List<CategoryModel> categories = AdminCategoryMethods.GetCategories(parent);
            if (parent == 0)
            {
                model.Add(new AdminCategoryItemModel()
                {
                    id = 0,
                    text = LanguageHelper.GetValue("MainCategory"),
                    children = false
                });
            }
            foreach (var category in categories)
            {
                if (category.ID != id)
                {
                    model.Add(new AdminCategoryItemModel()
                    {
                        id = category.ID,
                        text = category.Title,
                        children = category.Parent
                    });
                }
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Add Category
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult NewCategory()
        {
            return View(new AdminCategoryViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult NewCategory(CategoryModel model, HttpPostedFileBase thumbnail, HttpPostedFileBase banner)
        {
            AdminResultModel result = new AdminResultModel()
            {
                Message = LanguageHelper.GetValue("ErrorCheckField")
            };
            if (ModelState.IsValid)
            {
                result = AdminCategoryMethods.Add(model, thumbnail, banner);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Update Category
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult EditCategory(int id)
        {
            AdminCategoryDetailModel model = AdminCategoryMethods.GetCategoryDetail(id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditCategory(CategoryModel model, HttpPostedFileBase thumbnail, HttpPostedFileBase banner)
        {
            AdminResultModel result = new AdminResultModel()
            {
                Message = LanguageHelper.GetValue("ErrorCheckField")
            };
            if (ModelState.IsValid)
            {
                result = AdminCategoryMethods.Add(model, thumbnail, banner, true);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateCategory(int id, string column, string value)
        {
            AdminResultModel result = new AdminResultModel()
            {
                Message = LanguageHelper.GetValue("ErrorUpdateCategory")
            };
            if (ModelState.IsValid)
            {
                result = AdminCategoryMethods.Update(id, column, value);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Delete Category
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult DeleteCategory(int id, bool isparent, bool withchild)
        {
            AdminResultModel model = AdminCategoryMethods.Delete(id, isparent, withchild);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
        #region Post
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult MovePost(int id, int categoryid)
        {
            return Json(AdminPostMethods.SetCategory(id, categoryid), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult CopyPost(int id)
        {
            return Json(AdminPostMethods.Copy(id), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult WeeklyBees()
        {
            return View(AdminPostMethods.GetWeeklyBees());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult WeeklyBees(AdminWeeklyBeesModel Model)
        {
            AdminResultModel result = new AdminResultModel()
            {
                Message = LanguageHelper.GetValue("ErrorCheckField")
            };
            if (ModelState.IsValid)
            {
                result = AdminPostMethods.UpdateWeeklyBees(Model);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult LatestPosts()
        {
            return View(AdminPostMethods.GetPosts());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult LatestPosts(AdminLatestPostModel Model)
        {
            AdminResultModel result = new AdminResultModel()
            {
                Message = LanguageHelper.GetValue("ErrorCheckField")
            };
            if (ModelState.IsValid)
            {
                result = AdminPostMethods.UpdateLatestEpisodes(Model);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult LatestPostsJson()
        {
            List<AdminLatestPostModel> data = AdminPostMethods.GetLatestEpisodes();
            AdminLatestPostsModel model = new AdminLatestPostsModel()
            {
                data = data,
                meta = new MetaModel()
                {
                    pages = data.Count / 10,
                    total = data.Count
                }
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult DeleteLatestPost(int id)
        {
            AdminResultModel model = AdminPostMethods.DeleteLatestPost(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #region Posts
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult Posts()
        {
            return View(new AdminPostViewModel());
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult PostsJson()
        {
            List<PostModel> data = AdminPostMethods.GetPosts();
            AdminPostModel model = new AdminPostModel()
            {
                data = data,
                meta = new MetaModel()
                {
                    pages = data.Count / 10,
                    total = data.Count
                }
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Add Post
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult NewPost()
        {
            return View(new AdminPostViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult NewPost(PostModel model, HttpPostedFileBase thumbnail, HttpPostedFileBase banner, HttpPostedFileBase trailer)
        {
            AdminResultModel result = new AdminResultModel()
            {
                Message = LanguageHelper.GetValue("ErrorCheckField")
            };
            if (ModelState.IsValid)
            {
                result = AdminPostMethods.Add(model, thumbnail, banner, trailer);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Update Post
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult EditPost(int id)
        {
            AdminPostDetailModel model = AdminPostMethods.GetPostDetail(id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditPost(PostModel model, HttpPostedFileBase thumbnail, HttpPostedFileBase banner, HttpPostedFileBase trailer)
        {
            AdminResultModel result = new AdminResultModel()
            {
                Message = LanguageHelper.GetValue("ErrorCheckField")
            };
            if (ModelState.IsValid)
            {
                result = AdminPostMethods.Add(model, thumbnail, banner, trailer, true);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdatePost(int id, string column, string value)
        {
            AdminResultModel result = new AdminResultModel()
            {
                Message = LanguageHelper.GetValue("ErrorUpdatePost")
            };
            if (ModelState.IsValid)
            {
                result = AdminPostMethods.Update(id, column, value);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Delete Post
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult DeletePost(int sharedid)
        {
            AdminResultModel model = AdminPostMethods.Delete(sharedid);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Delete Trailer
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult DeleteTrailer(int id)
        {
            AdminResultModel model = AdminPostMethods.DeleteTrailer(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
        #region Blog
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult MoveBlog(int id, int categoryid)
        {
            return Json(AdminBlogMethods.SetCategory(id, categoryid), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult CopyBlog(int id)
        {
            return Json(AdminBlogMethods.Copy(id), JsonRequestBehavior.AllowGet);
        }
        #region Blogs
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult Blogs()
        {
            return View(new AdminBlogViewModel());
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult BlogsJson()
        {
            List<BlogModel> data = AdminBlogMethods.GetBlogs();
            AdminBlogModel model = new AdminBlogModel()
            {
                data = data,
                meta = new MetaModel()
                {
                    pages = data.Count / 10,
                    total = data.Count
                }
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Add Blog
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult NewBlog()
        {
            return View(new AdminBlogViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult NewBlog(BlogModel model, HttpPostedFileBase thumbnail, HttpPostedFileBase banner)
        {
            AdminResultModel result = new AdminResultModel()
            {
                Message = LanguageHelper.GetValue("ErrorCheckField")
            };
            if (ModelState.IsValid)
            {
                result = AdminBlogMethods.Add(model, thumbnail, banner);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Update Blog
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult EditBlog(int id)
        {
            AdminBlogDetailModel model = AdminBlogMethods.GetBlogDetail(id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditBlog(BlogModel model, HttpPostedFileBase thumbnail, HttpPostedFileBase banner)
        {
            AdminResultModel result = new AdminResultModel()
            {
                Message = LanguageHelper.GetValue("ErrorCheckField")
            };
            if (ModelState.IsValid)
            {
                result = AdminBlogMethods.Add(model, thumbnail, banner, true);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateBlog(int id, string column, string value)
        {
            AdminResultModel result = new AdminResultModel()
            {
                Message = LanguageHelper.GetValue("ErrorUpdateBlog")
            };
            if (ModelState.IsValid)
            {
                result = AdminBlogMethods.Update(id, column, value);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Delete Blog
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult DeleteBlog(int sharedid)
        {
            AdminResultModel model = AdminBlogMethods.Delete(sharedid);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
        #region Slide
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult CopySlide(int id)
        {
            return Json(AdminSlideMethods.Copy(id), JsonRequestBehavior.AllowGet);
        }
        #region Slides
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult Slides()
        {
            return View(new AdminSlideViewModel());
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult SlidesJson()
        {
            List<SlideModel> data = AdminSlideMethods.GetSlides();
            AdminSlideModel model = new AdminSlideModel()
            {
                data = data,
                meta = new MetaModel()
                {
                    pages = data.Count / 10,
                    total = data.Count
                }
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Add Slide
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult NewSlide()
        {
            return View(new AdminSlideViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult NewSlide(SlideModel model, HttpPostedFileBase banner)
        {
            AdminResultModel result = new AdminResultModel()
            {
                Message = LanguageHelper.GetValue("ErrorCheckField")
            };
            if (ModelState.IsValid)
            {
                result = AdminSlideMethods.Add(model, banner);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Update Slide
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult EditSlide(int id)
        {
            AdminSlideDetailModel model = AdminSlideMethods.GetSlideDetail(id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditSlide(SlideModel model, HttpPostedFileBase banner)
        {
            AdminResultModel result = new AdminResultModel()
            {
                Message = LanguageHelper.GetValue("ErrorCheckField")
            };
            if (ModelState.IsValid)
            {
                result = AdminSlideMethods.Add(model, banner, true);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateSlide(int id, string column, string value)
        {
            AdminResultModel result = new AdminResultModel()
            {
                Message = LanguageHelper.GetValue("ErrorUpdateSlide")
            };
            if (ModelState.IsValid)
            {
                result = AdminSlideMethods.Update(id, column, value);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Delete Slide
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult DeleteSlide(int id)
        {
            AdminResultModel model = AdminSlideMethods.Delete(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
        #region Social
        #region Socials
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult Socials()
        {
            return View(AdminSocialMethods.GetSocials());
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult SocialsJson()
        {
            List<SocialModel> data = AdminSocialMethods.GetSocials();
            AdminSocialModel model = new AdminSocialModel()
            {
                data = data,
                meta = new MetaModel()
                {
                    pages = data.Count / 10,
                    total = data.Count
                }
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Add Social
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult NewSocial()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult NewSocial(SocialModel model, HttpPostedFileBase icon)
        {
            AdminResultModel result = new AdminResultModel()
            {
                Message = LanguageHelper.GetValue("ErrorCheckField")
            };
            if (ModelState.IsValid)
            {
                result = AdminSocialMethods.Add(model, icon);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Update Social
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult EditSocial(int id)
        {
            SocialModel model = AdminSocialMethods.GetSocial(id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditSocial(SocialModel model, HttpPostedFileBase icon)
        {
            AdminResultModel result = new AdminResultModel()
            {
                Message = LanguageHelper.GetValue("ErrorCheckField")
            };
            if (ModelState.IsValid)
            {
                result = AdminSocialMethods.Add(model, icon, true);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateSocial(int id, string column, string value)
        {
            AdminResultModel result = new AdminResultModel()
            {
                Message = LanguageHelper.GetValue("ErrorUpdateSocial")
            };
            if (ModelState.IsValid)
            {
                result = AdminSocialMethods.Update(id, column, value);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Delete Social
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult DeleteSocial(int id)
        {
            AdminResultModel model = AdminSocialMethods.Delete(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
        #region Newsletter
        #region Newsletters
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult Newsletters()
        {
            return View(AdminNewsletterMethods.GetNewsletters());
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult NewslettersJson()
        {
            List<NewsletterModel> data = AdminNewsletterMethods.GetNewsletters();
            AdminNewsletterModel model = new AdminNewsletterModel()
            {
                data = data,
                meta = new MetaModel()
                {
                    pages = data.Count / 10,
                    total = data.Count
                }
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Delete Newsletter
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult DeleteNewsletter(int id)
        {
            AdminResultModel model = AdminNewsletterMethods.Delete(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
        #region Comment
        #region Comments
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult Comments()
        {
            return View(AdminCommentMethods.GetComments());
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult CommentsJson()
        {
            List<CommentModel> data = AdminCommentMethods.GetComments();
            AdminCommentModel model = new AdminCommentModel()
            {
                data = data,
                meta = new MetaModel()
                {
                    pages = data.Count / 10,
                    total = data.Count
                }
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult CommentAnswersJson(int parent)
        {
            List<CommentModel> data = AdminCommentMethods.GetComments(parent);
            AdminCommentModel model = new AdminCommentModel()
            {
                data = data,
                meta = new MetaModel()
                {
                    pages = data.Count / 10,
                    total = data.Count
                }
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Answer Comment
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult AnswerComment(int id)
        {
            CommentDetailModel model = AdminCommentMethods.GetComment(id);
            return View(model);
        }
        public JsonResult UpdateComment(int id, string column, string value)
        {
            AdminResultModel result = new AdminResultModel()
            {
                Message = LanguageHelper.GetValue("ErrorUpdateComment")
            };
            if (ModelState.IsValid)
            {
                result = AdminCommentMethods.Update(id, column, value);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Delete Comment
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult DeleteComment(int id)
        {
            AdminResultModel model = AdminCommentMethods.Delete(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
        #region Contact
        #region Contacts
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult Contacts()
        {
            return View(AdminContactMethods.GetContacts());
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult ContactsJson()
        {
            List<ContactModel> data = AdminContactMethods.GetContacts();
            AdminContactModel model = new AdminContactModel()
            {
                data = data,
                meta = new MetaModel()
                {
                    pages = data.Count / 10,
                    total = data.Count
                }
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Answer Contact
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult AnswerContact(int id)
        {
            AdminContactDetailModel model = AdminContactMethods.GetContact(id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AnswerContact(AdminAnswerModel Answer, string Email)
        {
            AdminResultModel result = new AdminResultModel()
            {
                Message = LanguageHelper.GetValue("ErrorCheckField")
            };
            if (ModelState.IsValid)
            {
                result = AdminAnswerMethods.Answer(Answer, Email);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Delete Newsletter
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult DeleteContact(int id)
        {
            AdminResultModel model = AdminContactMethods.Delete(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
        #region Member
        #region Members
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult Members()
        {
            return View(new AdminSlideViewModel());
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult MembersJson()
        {
            List<MemberModel> data = AdminMemberMethods.GetMembers();
            AdminMemberModel model = new AdminMemberModel()
            {
                data = data,
                meta = new MetaModel()
                {
                    pages = data.Count / 10,
                    total = data.Count
                }
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Update Member
        public JsonResult UpdateMember(int id, string column, string value)
        {
            AdminResultModel result = new AdminResultModel()
            {
                Message = LanguageHelper.GetValue("ErrorUpdateMember")
            };
            if (ModelState.IsValid)
            {
                result = AdminMemberMethods.Update(id, column, value);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Delete Member
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult DeleteMember(int id)
        {
            AdminResultModel model = AdminMemberMethods.Delete(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
        #region Language
        #region Languages
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult Languages()
        {
            return View();
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult LanguagesJson()
        {
            List<LanguageModel> data = AdminLanguageMethods.GetLanguages();
            AdminLanguageModel model = new AdminLanguageModel()
            {
                data = data,
                meta = new MetaModel()
                {
                    pages = data.Count / 10,
                    total = data.Count
                }
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Add Language
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult NewLanguage()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult NewLanguage(LanguageModel model, HttpPostedFileBase file)
        {
            AdminResultModel result = new AdminResultModel() {
                Message = LanguageHelper.GetValue("ErrorCheckField")
            };
            if (ModelState.IsValid)
            {
                result = AdminLanguageMethods.Add(model, file);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Update Language
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult EditLanguage(int id)
        {
            LanguageModel model = AdminLanguageMethods.GetLanguage(id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditLanguage(LanguageModel model, HttpPostedFileBase file)
        {
            AdminResultModel result = new AdminResultModel()
            {
                Message = LanguageHelper.GetValue("ErrorCheckField")
            };
            if (ModelState.IsValid)
            {
                result = AdminLanguageMethods.Add(model, file, true);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateLanguage(string prefix, string column, string value)
        {
            AdminResultModel result = new AdminResultModel()
            {
                Message = LanguageHelper.GetValue("ErrorUpdateLanguage")
            };
            if (ModelState.IsValid)
            {
                result = AdminLanguageMethods.Update(prefix, column, value);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Delete Language
        [Authorize(Roles = "SuperAdmin,Admin")]
        public JsonResult DeleteLanguage(int id, string prefix)
        {
            AdminResultModel model = AdminLanguageMethods.Delete(id);
            if (model.Status && LanguageHelper.CurrentLanguage == prefix)
            {
                Session["CurrentLanguage"] = LanguageHelper.DefaultLanguage;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
    }
}