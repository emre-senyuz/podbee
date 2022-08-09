using System.Collections.Generic;
using System.Linq;
using PodBeeMedia.Helpers;

namespace PodBeeMedia.Models
{
    public class FooterModel
    {
        public List<CategoryModel> Podcast { get; set; }
        public List<CategoryModel> Blog { get; set; }
        public List<SocialModel> Socials { get; set; }
    }
}