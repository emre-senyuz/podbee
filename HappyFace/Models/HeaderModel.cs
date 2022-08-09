using System.Collections.Generic;
using System.Linq;
using PodBeeMedia.Helpers;

namespace PodBeeMedia.Models
{
    public class Category
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Path { get; set; }
        public List<Category> Sub { get; set; }
        public bool MenuActive { get; set; }
        public string Type { get; set; }
    }
    public class Language
    {
        public string Title { get; set; }
        public string Prefix { get; set; }
        public string Icon { get; set; }
    }
    public class HeaderModel
    {
        public string Logo { get; set; }
        public string UserName { get; set; }
        public int RoleID { get; set; }
        public List<Category> Menu { get; set; }
        public List<Language> Languages { get; set; }
        public PostModel Podcast { get; set; }
    }
    public class HeaderMethods
    {
        public static CategoryModel GetCategoryItem(Categories category)
        {
            CategoryModel _category = new CategoryModel();
            try
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
            catch (System.Exception)
            {

                throw;
            }
            return _category;
        }
        public static List<Category> GetCategoryMenu(int parent)
        {
            List<Category> menu = new List<Category>();
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                List<CategoryModel> categories = new List<CategoryModel>();
                List<Categories> _categories = db.Categories.Where(l => l.Status && l.ParentID == parent && l.Lang == LanguageHelper.CurrentLanguage).OrderBy(c => c.Carousel).ToList();
                try
                {
                    foreach (var _category in _categories)
                    {
                        categories.Add(GetCategoryItem(_category));
                    }
                    foreach (var category in categories)
                    {
                        menu.Add(new Category()
                        {
                            Title = category.Title,
                            Url = category.Url,
                            Path = category.Banner,
                            Sub = category.Parent ? GetCategoryMenu(category.ID) : new List<Category>(),
                            MenuActive = category.ShowMenu,
                            Type = category.Type
                        });
                    }
                }
                catch (System.Exception)
                {
                    throw;
                }
            }
            return menu;
        }
        public static List<Language> GetLanguageMenu()
        {
            List<Language> languages = new List<Language>();
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                List<Languages> list = db.Languages.Where(l => l.Status).ToList();
                foreach (var item in list)
                {
                    languages.Add(new Language()
                    {
                        Title = item.Title,
                        Prefix = item.Prefix,
                        Icon = item.Icon
                    });
                }
            }
            return languages;
        }
    }
}