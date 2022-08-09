using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PodBeeMedia.Helpers;

namespace PodBeeMedia.Models
{
    public class SocialModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public bool Status { get; set; }
    }
}