using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PodBeeMedia.Helpers;

namespace PodBeeMedia.Models
{
    public class CategoryModel
    {
        public int ID { get; set; }
        public int SharedID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public string Lang { get; set; }
        public bool Status { get; set; }
        public bool ShowMenu { get; set; }
        public int Layout { get; set; }
        public int Carousel { get; set; }
        public string Banner { get; set; }
        public bool Parent { get; set; }
        public int ParentID { get; set; }
        [AllowHtml]
        public string Content { get; set; }
        public string Thumbnail { get; set; }
    }
    public class CategoryMethods
    {
        public static CategoryModel GetCategory(string url)
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                Categories category = db.Categories.FirstOrDefault(c => c.Url == url && c.Status);
                CategoryModel _category = new CategoryModel();
                if (category != null)
                {
                    foreach (var prop in category.GetType().GetProperties())
                    {
                        foreach (var _prop in _category.GetType().GetProperties())
                        {
                            if (prop.Name == _prop.Name)
                            {
                                _prop.SetValue(_category, prop.GetValue(category));
                            }
                        }
                    }
                }
                return _category;
            }
        }
    }
}