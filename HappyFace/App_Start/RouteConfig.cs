using System.Web.Mvc;
using System.Web.Routing;
using PodBeeMedia.Helpers;

namespace PodBeeMedia
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Homepage",
                url: "",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute( 
                name: "HomepageLanguage",
                url: "{lang}",
                defaults: new { controller = "Home", action = "Index", lang = UrlParameter.Optional, id = UrlParameter.Optional },
                constraints: new { lang = @"(\w{2})" }
            );
            routes.MapRoute(
                name: "Category",
                url: "{type}/{url}",
                defaults: new { controller = "Category", action = "Index", url = UrlParameter.Optional, id = UrlParameter.Optional },
                constraints: new { type = "kategori|category" }
            );
            routes.MapRoute(
                name: "CategoryLanguage",
                url: "{lang}/{type}/{url}",
                defaults: new { controller = "Category", action = "Index", lang = LanguageHelper.CurrentLanguage, url = UrlParameter.Optional, id = UrlParameter.Optional },
                constraints: new { lang = @"(\w{2})", type = "kategori|category" }
            );
            /* Layout Routes */
            routes.MapRoute(
                name: "SetLanguage",
                url: "setlanguage",
                defaults: new { controller = "Layout", action = "SetLanguage", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "404",
                url: "404",
                defaults: new { controller = "Layout", action = "NotFound", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "MemberMenu",
                url: "membermenu",
                defaults: new { controller = "Layout", action = "MemberMenu", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Newsletter",
                url: "newsletter",
                defaults: new { controller = "Layout", action = "Newsletter", id = UrlParameter.Optional }
            );
            /* Layout Routes */
            /* Static Pages */
            routes.MapRoute(
                name: "Blog",
                url: "{path}",
                defaults: new { controller = "Home", action = "Blog", path = LanguageHelper.GetValue("UrlTypeBlog"), id = UrlParameter.Optional },
                constraints: new { path = "blog" }
            );
            routes.MapRoute(
                name: "BlogDetail",
                url: "{type}/{url}",
                defaults: new { controller = "Home", action = "BlogDetail", url = UrlParameter.Optional, id = UrlParameter.Optional },
                constraints: new { type = "blog" }
            );
            routes.MapRoute(
                name: "BlogDetailLanguage",
                url: "{lang}/{type}/{url}",
                defaults: new { controller = "Home", action = "BlogDetail", lang = LanguageHelper.CurrentLanguage, url = UrlParameter.Optional, id = UrlParameter.Optional },
                constraints: new { lang = @"(\w{2})", type = "blog" }
            );
            routes.MapRoute(
                name: "LatestEpisode",
                url: "{path}",
                defaults: new { controller = "Home", action = "LatestEpisode", path = LanguageHelper.GetValue("UrlTypeLatestEpisode"), id = UrlParameter.Optional },
                constraints: new { path = "son-eklenen-bolumler|latest-episodes" }
            );
            routes.MapRoute(
                name: "Podcast",
                url: "{path}",
                defaults: new { controller = "Home", action = "Podcast", path = LanguageHelper.GetValue("UrlTypePodcast"), id = UrlParameter.Optional },
                constraints: new { path = "podcast" }
            );
            routes.MapRoute(
                name: "PodcastDetail",
                url: "{type}/{url}",
                defaults: new { controller = "Home", action = "PodcastDetail", url = UrlParameter.Optional, id = UrlParameter.Optional },
                constraints: new { type = "podcast" }
            );
            routes.MapRoute(
                name: "PodcastDetailLanguage",
                url: "{lang}/{type}/{url}",
                defaults: new { controller = "Home", action = "PodcastDetail", lang = LanguageHelper.CurrentLanguage, url = UrlParameter.Optional, id = UrlParameter.Optional },
                constraints: new { lang = @"(\w{2})", type = "podcast" }
            );
            routes.MapRoute(
                name: "Ads",
                url: "{path}",
                defaults: new { controller = "Home", action = "Ads", path = LanguageHelper.GetValue("UrlTypeAds"), id = UrlParameter.Optional },
                constraints: new { path = "reklam|ads" }
            );
            routes.MapRoute(
                name: "AboutUs",
                url: "{path}",
                defaults: new { controller = "Home", action = "AboutUs", path = LanguageHelper.GetValue("UrlTypeAboutUs"), id = UrlParameter.Optional },
                constraints: new { path = "hakkimizda|about-us" }
            );
            routes.MapRoute(
                name: "Faq",
                url: "{path}",
                defaults: new { controller = "Home", action = "Faq", path = LanguageHelper.GetValue("UrlTypeFaq"), id = UrlParameter.Optional },
                constraints: new { path = "sik-sorulan-sorular|frequently-asked-questions" }
            );
            routes.MapRoute(
                name: "ContactUs",
                url: "{path}",
                defaults: new { controller = "Home", action = "ContactUs", path = LanguageHelper.GetValue("UrlTypeContactUs"), id = UrlParameter.Optional },
                constraints: new { path = "iletisim|contact-us" }
            );
            routes.MapRoute(
                name: "Reference",
                url: "{path}",
                defaults: new { controller = "Home", action = "Reference", path = LanguageHelper.GetValue("UrlTypeReference"), id = UrlParameter.Optional },
                constraints: new { path = "referanslar|references" }
            );
            /* Static Pages */
            /* Member Routes */
            routes.MapRoute(
                name: "LoginRouter",
                url: "login",
                defaults: new { controller = "Member", action = "LoginRouter", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Login",
                url: "{type}/{path}",
                defaults: new { controller = "Member", action = "Login", path = LanguageHelper.GetValue("UrlTypeLogin"), id = UrlParameter.Optional },
                constraints: new { type = "uye|member", path = "giris|login" }
            );
            routes.MapRoute(
                name: "LoginLanguage",
                url: "{lang}/{type}/{path}",
                defaults: new { controller = "Member", action = "Login", lang = LanguageHelper.CurrentLanguage, path = LanguageHelper.GetValue("UrlTypeLogin"), id = UrlParameter.Optional },
                constraints: new { lang = @"(\w{2})", type = "uye|member", path = "giris|login" }
            );
            routes.MapRoute(
                name: "Register",
                url: "{type}/{path}",
                defaults: new { controller = "Member", action = "Register", path = LanguageHelper.GetValue("UrlTypeRegister"), id = UrlParameter.Optional },
                constraints: new { type = "uye|member", path = "kayit|sign-up" }
            );
            routes.MapRoute(
                name: "RegisterLanguage",
                url: "{lang}/{type}/{path}",
                defaults: new { controller = "Member", action = "Register", lang = LanguageHelper.CurrentLanguage, path = LanguageHelper.GetValue("UrlTypeRegister"), id = UrlParameter.Optional },
                constraints: new { lang = @"(\w{2})", type = "uye|member", path = "kayit|sign-up" }
            );
            routes.MapRoute(
                name: "Profile",
                url: "{type}/{path}",
                defaults: new { controller = "Member", action = "Index", path = LanguageHelper.GetValue("UrlTypeProfile"), id = UrlParameter.Optional },
                constraints: new { type = "uye|member", path = "profil|profile" }
            );
            routes.MapRoute(
                name: "ProfileLanguage",
                url: "{lang}/{type}/{path}",
                defaults: new { controller = "Member", action = "Index", lang = LanguageHelper.CurrentLanguage, path = LanguageHelper.GetValue("UrlTypeProfile"), id = UrlParameter.Optional },
                constraints: new { lang = @"(\w{2})", type = "uye|member", path = "profil|profile" }
            );
            routes.MapRoute(
                name: "ForgetPassword",
                url: "{type}/{path}",
                defaults: new { controller = "Member", action = "ForgetPassword", path = LanguageHelper.GetValue("UrlTypeForgetPassword"), id = UrlParameter.Optional },
                constraints: new { type = "uye|member", path = "sifremi-unuttum|forget-password" }
            );
            routes.MapRoute(
                name: "ForgetPasswordLanguage",
                url: "{lang}/{type}/{path}",
                defaults: new { controller = "Member", action = "ForgetPassword", lang = LanguageHelper.CurrentLanguage, path = LanguageHelper.GetValue("UrlTypeForgetPassword"), id = UrlParameter.Optional },
                constraints: new { lang = @"(\w{2})", type = "uye|member", path = "sifremi-unuttum|forget-password" }
            );
            routes.MapRoute(
                name: "ResetPassword",
                url: "{type}/{path}",
                defaults: new { controller = "Member", action = "ResetPassword", path = LanguageHelper.GetValue("UrlTypeResetPassword"), id = UrlParameter.Optional },
                constraints: new { type = "uye|member", path = "sifre-sifirlama|reset-password" }
            );
            routes.MapRoute(
                name: "ResetPasswordLanguage",
                url: "{lang}/{type}/{path}",
                defaults: new { controller = "Member", action = "ResetPassword", lang = LanguageHelper.CurrentLanguage, path = LanguageHelper.GetValue("UrlTypeResetPassword"), id = UrlParameter.Optional },
                constraints: new { lang = @"(\w{2})", type = "uye|member", path = "sifre-sifirlama|reset-password" }
            );
            routes.MapRoute(
                name: "ChangePassword",
                url: "{type}/{path}",
                defaults: new { controller = "Member", action = "ChangePassword", path = LanguageHelper.GetValue("UrlTypeChangePassword"), id = UrlParameter.Optional },
                constraints: new { type = "uye|member", path = "sifre-degistir|change-password" }
            );
            routes.MapRoute(
                name: "ChangePasswordLanguage",
                url: "{lang}/{type}/{path}",
                defaults: new { controller = "Member", action = "ChangePassword", lang = LanguageHelper.CurrentLanguage, path = LanguageHelper.GetValue("UrlTypeChangePassword"), id = UrlParameter.Optional },
                constraints: new { lang = @"(\w{2})", type = "uye|member", path = "sifre-degistir|change-password" }
            );
            routes.MapRoute(
                name: "Activation",
                url: "{type}/{path}",
                defaults: new { controller = "Member", action = "Activation", path = LanguageHelper.GetValue("UrlTypeActivation"), id = UrlParameter.Optional },
                constraints: new { type = "uye|member", path = "aktivasyon|activation" }
            );
            routes.MapRoute(
                name: "ActivationLanguage",
                url: "{lang}/{type}/{path}",
                defaults: new { controller = "Member", action = "Activation", lang = LanguageHelper.CurrentLanguage, path = LanguageHelper.GetValue("UrlTypeActivation"), id = UrlParameter.Optional },
                constraints: new { lang = @"(\w{2})", type = "uye|member", path = "aktivasyon|activation" }
            );
            routes.MapRoute(
                name: "ReActivation",
                url: "{type}/{path}",
                defaults: new { controller = "Member", action = "ReActivation", path = LanguageHelper.GetValue("UrlTypeReActivation"), id = UrlParameter.Optional },
                constraints: new { type = "uye|member", path = "yeni-aktivasyon|re-activation" }
            );
            routes.MapRoute(
                name: "ReActivationLanguage",
                url: "{lang}/{type}/{path}",
                defaults: new { controller = "Member", action = "ReActivation", lang = LanguageHelper.CurrentLanguage, path = LanguageHelper.GetValue("UrlTypeReActivation"), id = UrlParameter.Optional },
                constraints: new { lang = @"(\w{2})", type = "uye|member", path = "yeni-aktivasyon|re-activation" }
            );
            routes.MapRoute(
                name: "Information",
                url: "{type}/{path}",
                defaults: new { controller = "Member", action = "Information", path = LanguageHelper.GetValue("UrlTypeInformation"), id = UrlParameter.Optional },
                constraints: new { type = "uye|member", path = "bilgilendirme|information" }
            );
            routes.MapRoute(
                name: "InformationLanguage",
                url: "{lang}/{type}/{path}",
                defaults: new { controller = "Member", action = "Information", lang = LanguageHelper.CurrentLanguage, path = LanguageHelper.GetValue("UrlTypeInformation"), id = UrlParameter.Optional },
                constraints: new { lang = @"(\w{2})", type = "uye|member", path = "bilgilendirme|information" }
            );
            routes.MapRoute(
                name: "PersonelInformation",
                url: "{type}/{path}",
                defaults: new { controller = "Member", action = "PersonelInformation", path = LanguageHelper.GetValue("UrlTypeInformation"), id = UrlParameter.Optional },
                constraints: new { type = "uye|member", path = "kisisel-bilgiler|personel-information" }
            );
            routes.MapRoute(
                name: "PersonelInformationLanguage",
                url: "{lang}/{type}/{path}",
                defaults: new { controller = "Member", action = "PersonelInformation", lang = LanguageHelper.CurrentLanguage, path = LanguageHelper.GetValue("UrlTypeInformation"), id = UrlParameter.Optional },
                constraints: new { lang = @"(\w{2})", type = "uye|member", path = "kisisel-bilgiler|personel-information" }
            );
            routes.MapRoute(
                name: "MemberComments",
                url: "{type}/{path}",
                defaults: new { controller = "Member", action = "Comments", path = LanguageHelper.GetValue("UrlTypeInformation"), id = UrlParameter.Optional },
                constraints: new { type = "uye|member", path = "yorumlar|comments" }
            );
            routes.MapRoute(
                name: "MemberCommentsLanguage",
                url: "{lang}/{type}/{path}",
                defaults: new { controller = "Member", action = "Comments", lang = LanguageHelper.CurrentLanguage, path = LanguageHelper.GetValue("UrlTypeInformation"), id = UrlParameter.Optional },
                constraints: new { lang = @"(\w{2})", type = "uye|member", path = "yorumlar|comments" }
            );
            routes.MapRoute(
                name: "Logout",
                url: "logout",
                defaults: new { controller = "Member", action = "Logout", id = UrlParameter.Optional }
            );
            /* Member Routes */
            /* Admin Routes */
            routes.MapRoute(
                name: "Dashboard",
                url: "dashboard",
                defaults: new { controller = "Admin", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "GetUrlString",
                url: "get-url-string",
                defaults: new { controller = "Admin", action = "GetUrlString", id = UrlParameter.Optional }
            );
            /* Category */
            routes.MapRoute(
                name: "Categories",
                url: "categories",
                defaults: new { controller = "Admin", action = "Categories", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "CategoriesJson",
                url: "json-categories",
                defaults: new { controller = "Admin", action = "CategoriesJson", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "CategoryTreeJson",
                url: "json-category-tree",
                defaults: new { controller = "Admin", action = "CategoryTreeJson", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "CategoryAdd",
                url: "new-category",
                defaults: new { controller = "Admin", action = "NewCategory", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "CategoryEdit",
                url: "edit-category",
                defaults: new { controller = "Admin", action = "EditCategory", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "CategoryUpdate",
                url: "update-category",
                defaults: new { controller = "Admin", action = "UpdateCategory", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "CategoryDelete",
                url: "delete-category",
                defaults: new { controller = "Admin", action = "DeleteCategory", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "GetParentCategory",
                url: "get-parent-category",
                defaults: new { controller = "Admin", action = "GetParentCategory", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "CheckParentCategory",
                url: "check-parent-category",
                defaults: new { controller = "Admin", action = "CheckParentCategory", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "DeleteSubCategory",
                url: "delete-sub-category",
                defaults: new { controller = "Admin", action = "DeleteSubCategory", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "MoveCategory",
                url: "move-category",
                defaults: new { controller = "Admin", action = "MoveCategory", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "CopyCategory",
                url: "copy-category",
                defaults: new { controller = "Admin", action = "CopyCategory", id = UrlParameter.Optional }
            );
            /* Category */
            /* Language */
            routes.MapRoute(
                name: "Languages",
                url: "languages",
                defaults: new { controller = "Admin", action = "Languages", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "LanguageAdd",
                url: "new-language",
                defaults: new { controller = "Admin", action = "NewLanguage", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "LanguageEdit",
                url: "edit-language",
                defaults: new { controller = "Admin", action = "EditLanguage", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "LanguageUpdate",
                url: "update-language",
                defaults: new { controller = "Admin", action = "UpdateLanguage", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "LanguageDelete",
                url: "delete-language",
                defaults: new { controller = "Admin", action = "DeleteLanguage", id = UrlParameter.Optional }
            );
            /* Language */
            /* Post */
            routes.MapRoute(
                name: "Posts",
                url: "posts",
                defaults: new { controller = "Admin", action = "Posts", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "PostsJson",
                url: "json-posts",
                defaults: new { controller = "Admin", action = "PostsJson", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "PostAdd",
                url: "new-post",
                defaults: new { controller = "Admin", action = "NewPost", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "PostEdit",
                url: "edit-post",
                defaults: new { controller = "Admin", action = "EditPost", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "PostUpdate",
                url: "update-post",
                defaults: new { controller = "Admin", action = "UpdatePost", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "PostDelete",
                url: "delete-post",
                defaults: new { controller = "Admin", action = "DeletePost", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "MovePost",
                url: "move-post",
                defaults: new { controller = "Admin", action = "MovePost", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "CopyPost",
                url: "copy-post",
                defaults: new { controller = "Admin", action = "CopyPost", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "TrailerDelete",
                url: "delete-trailer",
                defaults: new { controller = "Admin", action = "DeleteTrailer", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "WeeklyBees",
                url: "weekly-bees",
                defaults: new { controller = "Admin", action = "WeeklyBees", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "LatestPosts",
                url: "latest-posts",
                defaults: new { controller = "Admin", action = "LatestPosts", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "LatestPostsJson",
                url: "json-latest-posts",
                defaults: new { controller = "Admin", action = "LatestPostsJson", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "LatestPostDelete",
                url: "delete-latest-post",
                defaults: new { controller = "Admin", action = "DeleteLatestPost", id = UrlParameter.Optional }
            );
            /* Post */
            /* Blog */
            routes.MapRoute(
                name: "Blogs",
                url: "blogs",
                defaults: new { controller = "Admin", action = "Blogs", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "BlogsJson",
                url: "json-blogs",
                defaults: new { controller = "Admin", action = "BlogsJson", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "BlogAdd",
                url: "new-blog",
                defaults: new { controller = "Admin", action = "NewBlog", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "BlogEdit",
                url: "edit-blog",
                defaults: new { controller = "Admin", action = "EditBlog", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "BlogUpdate",
                url: "update-blog",
                defaults: new { controller = "Admin", action = "UpdateBlog", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "BlogDelete",
                url: "delete-blog",
                defaults: new { controller = "Admin", action = "DeleteBlog", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "MoveBlog",
                url: "move-blog",
                defaults: new { controller = "Admin", action = "MoveBlog", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "CopyBlog",
                url: "copy-blog",
                defaults: new { controller = "Admin", action = "CopyBlog", id = UrlParameter.Optional }
            );
            /* Blog */
            /* Slide */
            routes.MapRoute(
                name: "Slides",
                url: "slides",
                defaults: new { controller = "Admin", action = "Slides", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "SlidesJson",
                url: "json-slides",
                defaults: new { controller = "Admin", action = "SlidesJson", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "SlideAdd",
                url: "new-slide",
                defaults: new { controller = "Admin", action = "NewSlide", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "SlideEdit",
                url: "edit-slide",
                defaults: new { controller = "Admin", action = "EditSlide", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "SlideUpdate",
                url: "update-slide",
                defaults: new { controller = "Admin", action = "UpdateSlide", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "SlideDelete",
                url: "delete-slide",
                defaults: new { controller = "Admin", action = "DeleteSlide", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "CopySlide",
                url: "copy-slide",
                defaults: new { controller = "Admin", action = "CopySlide", id = UrlParameter.Optional }
            );
            /* Slide */
            /* Social */
            routes.MapRoute(
                name: "Socials",
                url: "socials",
                defaults: new { controller = "Admin", action = "Socials", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "SocialsJson",
                url: "json-socials",
                defaults: new { controller = "Admin", action = "SocialsJson", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "SocialAdd",
                url: "new-social",
                defaults: new { controller = "Admin", action = "NewSocial", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "SocialEdit",
                url: "edit-social",
                defaults: new { controller = "Admin", action = "EditSocial", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "SocialUpdate",
                url: "update-social",
                defaults: new { controller = "Admin", action = "UpdateSocial", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "SocialDelete",
                url: "delete-social",
                defaults: new { controller = "Admin", action = "DeleteSocial", id = UrlParameter.Optional }
            );
            /* Social */
            /* Newsletter */
            routes.MapRoute(
                name: "Newsletters",
                url: "newsletters",
                defaults: new { controller = "Admin", action = "Newsletters", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "NewslettersJson",
                url: "json-newsletters",
                defaults: new { controller = "Admin", action = "NewslettersJson", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "NewsletterDelete",
                url: "delete-newsletter",
                defaults: new { controller = "Admin", action = "DeleteNewsletter", id = UrlParameter.Optional }
            );
            /* Newsletter */
            /* Contact */
            routes.MapRoute(
                name: "Contacts",
                url: "contacts",
                defaults: new { controller = "Admin", action = "Contacts", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "CommentUpdate",
                url: "update-comment",
                defaults: new { controller = "Admin", action = "UpdateComment", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "ContactsJson",
                url: "json-contacts",
                defaults: new { controller = "Admin", action = "ContactsJson", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "ContactAnswer",
                url: "answer-contact",
                defaults: new { controller = "Admin", action = "AnswerContact", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "ContactDelete",
                url: "delete-contact",
                defaults: new { controller = "Admin", action = "DeleteContact", id = UrlParameter.Optional }
            );
            /* Contact */
            /* Comment */
            routes.MapRoute(
                name: "Comment",
                url: "comment",
                defaults: new { controller = "Layout", action = "Comment", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Comments",
                url: "comments",
                defaults: new { controller = "Admin", action = "Comments", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "CommentsJson",
                url: "json-comments",
                defaults: new { controller = "Admin", action = "CommentsJson", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "CommentAnswersJson",
                url: "json-comment-answers",
                defaults: new { controller = "Admin", action = "CommentAnswersJson", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "CommentAnswer",
                url: "answer-comment",
                defaults: new { controller = "Admin", action = "AnswerComment", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "CommentDelete",
                url: "delete-comment",
                defaults: new { controller = "Admin", action = "DeleteComment", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "CommentVote",
                url: "vote-comment",
                defaults: new { controller = "Layout", action = "VoteComment", id = UrlParameter.Optional }
            );
            /* Comment */
            /* Slide */
            routes.MapRoute(
                name: "Members",
                url: "members",
                defaults: new { controller = "Admin", action = "Members", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "MembersJson",
                url: "json-members",
                defaults: new { controller = "Admin", action = "MembersJson", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "MemberAdd",
                url: "new-member",
                defaults: new { controller = "Admin", action = "NewMember", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "MemberEdit",
                url: "edit-member",
                defaults: new { controller = "Admin", action = "EditMember", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "MemberUpdate",
                url: "update-member",
                defaults: new { controller = "Admin", action = "UpdateMember", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "MemberDelete",
                url: "delete-member",
                defaults: new { controller = "Admin", action = "DeleteMember", id = UrlParameter.Optional }
            );
            /* Slide */
            /* Admin Routes */
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
