using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace PodBeeMedia.Helpers
{
    [XmlRoot(ElementName = "variable")]
    public class SiteSetting
    {
        [XmlAttribute(AttributeName = "key")]
        public string Key { get; set; }
        [XmlAttribute(AttributeName = "state")]
        public string State { get; set; }
        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }
    [XmlRoot(ElementName = "variables")]
    public class SiteSettings
    {
        [XmlElement(ElementName = "variable")]
        public List<SiteSetting> ListSettings { get; set; }
    }
    public static class SiteVariables
    {
        private static SiteSettings SiteSettings
        {
            get
            {
                return GetList();
            }
            set { SiteSettings = value; }
        }
        private static SiteSettings GetList()
        {
            try
            {
                var basedir = AppDomain.CurrentDomain.BaseDirectory;
                XmlSerializer serializer = new XmlSerializer(typeof(SiteSettings));
                StreamReader reader = new StreamReader(Path.Combine(basedir + "/SiteVariables.config"));
                var values = (SiteSettings)serializer.Deserialize(reader);
                reader.Close();
                return values;
            }
            catch (Exception)
            {
                return new SiteSettings { ListSettings = new List<SiteSetting>() };
            }
        }
        public static string GetValue(string key, bool getFromCache = true)
        {
            SiteSetting variable = SiteSettings.ListSettings.FirstOrDefault(_ => _.Key.ToLowerInvariant() == key.ToLowerInvariant());
            return variable != null ? variable.Value : null;
        }
        public static string GetState(string key, bool getFromCache = true)
        {
            SiteSetting variable = SiteSettings.ListSettings.FirstOrDefault(_ => _.Key.ToLowerInvariant() == key.ToLowerInvariant());
            return variable != null ? variable.State : null;
        }
    }
}