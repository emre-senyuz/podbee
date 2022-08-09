using System.Collections.Generic;
using System.Linq;
using System.Web.Security;

namespace PodBeeMedia.Models
{
    public class RoleModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public bool Status { get; set; }
    }
    public class RoleMethods
    {
        public static List<RoleModel> GetRoles()
        {
            List<RoleModel> _roles = new List<RoleModel>();
            try
            {
                using (SQLDataDataContext db = new SQLDataDataContext())
                {
                    foreach (var role in db.MemberRoles.Where(r => r.Status).ToList())
                    {
                        _roles.Add(new RoleModel() { ID = role.ID, Title = role.Title });
                    }
                }
            }
            catch (System.Exception)
            {

                throw;
            }
            return _roles;
        }
    }
}