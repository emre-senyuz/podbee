using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PodBeeMedia.Helpers;

namespace PodBeeMedia.Models
{
    public class SlideModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public string Url { get; set; }
        public string Lang { get; set; }
        public bool Status { get; set; }
        public string Banner { get; set; }
    }
    public class SlideMethods
    {
        public static List<SlideModel> GetSlides()
        {
            using (SQLDataDataContext db = new SQLDataDataContext())
            {
                List<SlideModel> model = AdminSlideMethods.GetSlides();
                return model;
            }
        }
    }
}