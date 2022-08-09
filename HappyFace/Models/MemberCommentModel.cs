using System.Collections.Generic;

namespace PodBeeMedia.Models
{
    public class MemberCommentModel
    {
        public MemberModel Member { get; set; }
        public List<CommentDetailModel> Comments { get; set; }
    }
}