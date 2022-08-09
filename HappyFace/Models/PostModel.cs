using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PodBeeMedia.Helpers;

namespace PodBeeMedia.Models
{
    public class PostModel
    {
        public int ID { get; set; }
        public int SharedID { get; set; }
        public string Alternates { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Url { get; set; }
        public int CategoryID { get; set; }
        public string Lang { get; set; }
        public bool Status { get; set; }
        public int Carousel { get; set; }
        public string Banner { get; set; }
        [AllowHtml]
        public string Content { get; set; }
        public string Thumbnail { get; set; }
        [AllowHtml]
        public string IFrame { get; set; }
        public string Trailer { get; set; }
        public string Spotify { get; set; }
        public string ApplePodcasts { get; set; }
        public string Deezer { get; set; }
        public string GooglePodcasts { get; set; }
        public int CommentCount { get; set; } = 0;
        public int VisitCount { get; set; } = 0;
    }
    public class PostListModel
    {
        public List<PostModel> Posts { get; set; }
        public List<CategoryModel> Categories { get; set; }
    }
    public class PostMethods
    {
        public static PostModel GetPost(string url)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                Posts post = db.Posts.FirstOrDefault(p => p.Url == url && p.Status);
                post.VisitCount = post.VisitCount + 1;
                db.SubmitChanges();
                PostModel _post = new PostModel();
                if (post != null)
                {
                    foreach (var prop in post.GetType().GetProperties())
                    {
                        foreach (var _prop in _post.GetType().GetProperties())
                        {
                            if (prop.Name == _prop.Name)
                            {
                                _prop.SetValue(_post, prop.GetValue(post));
                            }
                        }
                    }
                }
                return _post;
            }
        }
        public static PostModel GetRecentPost()
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                Posts post = db.Posts.FirstOrDefault(p => p.Trailer.Trim() != "");
                PostModel _post = new PostModel();
                if (post != null)
                {
                    foreach (var prop in post.GetType().GetProperties())
                    {
                        foreach (var _prop in _post.GetType().GetProperties())
                        {
                            if (prop.Name == _prop.Name)
                            {
                                _prop.SetValue(_post, prop.GetValue(post));
                            }
                        }
                    }
                }
                return _post;
            }
        }
        public static PostListModel GetPostList(string search = "")
        {
            List<PostModel> Posts = AdminPostMethods.GetPosts().Where(p => p.Status).ToList();
            if (!string.IsNullOrEmpty(search))
            {
                Posts = Posts.Where(p => p.Title.ToLower().Contains(search.ToLower()) || p.Content.ToLower().Contains(search.ToLower())).ToList();
            }
            return new PostListModel()
            {
                Posts = Posts,
                Categories = AdminCategoryMethods.GetCategories("Podcast", LanguageHelper.CurrentLanguage)
            };
        }
    }
}