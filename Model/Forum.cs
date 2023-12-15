using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectApp.Model
{
    public class Forum
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ForumDescription { get; set; }
        public int CreatorId { get; set; }
        public DateTime CreatedDadeTime { get; set; }
        public int? ComposerId { get; set; }
        public int? WorkId { get; set; }
        public Composer Composer { get; set; }
        public User Creator { get; set; }
        public List<ForumComment> ForumComments { get; set; }
        public Work Work { get; set; }

        public Forum()
        {
            Title = "";
            ForumDescription = "";
            CreatedDadeTime = DateTime.Now;
            ForumComments = new();
        }
    }
}
