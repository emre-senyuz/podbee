using System;
using System.Collections.Generic;

namespace PodBeeMedia.Models
{
    public class Notification
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
    public class Log
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
    public class QucikPanel
    {
        public List<Notification> Notifications { get; set; } = new List<Notification>();
        public List<Log> Logs { get; set; } = new List<Log>();
        public string Settings { get; set; }
    }
    public class AdminHeaderModel
    {
        public MemberModel Member { get; set; }
        public List<Language> Languages { get; set; }
        public QucikPanel QucikPanel { get; set; }
    }
}