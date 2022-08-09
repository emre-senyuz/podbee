using System;
using System.Linq;
using PodBeeMedia.Helpers;

namespace PodBeeMedia.Models
{
    public class AdminModel
    {
        // dashboard
    }
    public class AdminResultModel
    {
        public string Message { get; set; } = string.Empty;
        public bool Status { get; set; } = false;
        public string Url { get; set; } = "/dashboard";
        public string Field { get; set; } = string.Empty;
    }
    public class MetaModel
    {
        public string field { get; set; } = "ID";
        public int page { get; set; } = 1;
        public int pages { get; set; }
        public int perpage { get; set; } = 10;
        public string sort { get; set; } = "asc";
        public int total { get; set; }
    }
    public class AdminMethods
    {
        public static AdminResultModel GetResult(string message, bool status, string url, string field = "")
        {
            return new AdminResultModel()
            {
                Message = LanguageHelper.GetValue(message),
                Status = status,
                Url = url,
                Field = field
            };
        }
    }
}