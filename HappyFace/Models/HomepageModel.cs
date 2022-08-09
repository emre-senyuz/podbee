using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PodBeeMedia.Models
{
    public class HomepageModel
    {
        public string Critical { get; set; }
        public List<SlideModel> Slides { get; set; }
        public List<BlogModel> Blogs { get; set; }
        public List<PostModel> Podcasts { get; set; }
        public List<AdminLatestPostModel> LatestEpisodes { get; set; }
        public AdminWeeklyBeesModel WeeklyBees { get; set; }
    }
}