using System.Collections.Generic;

namespace PodBeeMedia.Models
{
    public class RegisterModel
    {
        public MemberModel Member { get; set; }
        public List<GenderModel> Genders { get; set; }
        public List<RoleModel> Roles { get; set; }
    }
}