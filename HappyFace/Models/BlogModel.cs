using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PodBeeMedia.Helpers;

namespace PodBeeMedia.Models
{
    public class BlogModel
    {
        public int ID { get; set; }
        public int SharedID { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Url { get; set; }
        public int CategoryID { get; set; }
        public string Category { get; set; }
        public string Lang { get; set; }
        public bool Status { get; set; }
        public int Carousel { get; set; }
        public string Banner { get; set; }
        [AllowHtml]
        public string Content { get; set; }
        public string Thumbnail { get; set; }
        public int CommentCount { get; set; } = 0;
    }
    public class BlogDetailModel
    {
        public BlogModel Blog { get; set; }
        public List<CommentDetailModel> Comments { get; set; }
    }
    public class BlogListModel
    {
        public List<BlogModel> Blogs { get; set; }
        public List<CategoryModel> Categories { get; set; }
    }
    public class BlogMethods
    {
        public static BlogModel GetBlog(string url)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                Blogs blog = db.Blogs.FirstOrDefault(b => b.Url == url && b.Status);
                BlogModel _blog = new BlogModel();
                if (blog != null)
                {
                    foreach (var prop in blog.GetType().GetProperties())
                    {
                        foreach (var _prop in _blog.GetType().GetProperties())
                        {
                            if (prop.Name == _prop.Name)
                            {
                                _prop.SetValue(_blog, prop.GetValue(blog));
                            }
                        }
                    }
                }
                return _blog;
            }
        }
        public static BlogDetailModel GetBlogDetail(string url)
        {
            BlogModel Blog = GetBlog(url);
            return new BlogDetailModel()
            {
                Blog = Blog,
                Comments = AdminCommentMethods.GetComments(Blog.ID, "Blog", 0, true)
            };
        }
        public static BlogListModel GetBlogList()
        {
            return new BlogListModel()
            {
                Blogs = AdminBlogMethods.GetBlogs().Where(b => b.Status).ToList(),
                Categories = AdminCategoryMethods.GetCategories("Blog", LanguageHelper.CurrentLanguage)
            };
        }
    }
}