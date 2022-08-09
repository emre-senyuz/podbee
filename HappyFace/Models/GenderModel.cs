using System.Collections.Generic;
using System.Linq;

namespace PodBeeMedia.Models
{
    public class GenderModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
    }
    public class GenderMethods
    {
        public static List<GenderModel> GetGenders()
        {
            List<GenderModel> _genders = new List<GenderModel>();
            try
            {
                using (SQLDataDataContext db = new SQLDataDataContext())
                {
                    foreach (var gender in db.MemberGenders.ToList())
                    {
                        _genders.Add(new GenderModel() { ID = gender.ID, Title = gender.Title });
                    }
                }
            }
            catch (System.Exception)
            {

                throw;
            }
            return _genders;
        }
    }
}