using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectApp.Model
{
    public class ForumComment
    {
        public int Id { get; set; }
        public int ForumId { get; set; }
        public int CreatorId { get; set; }
        public string Content { get; set; }
        public DateTime UploadDateTime { get; set; }
        public User Creator { get; set; }
        public Forum Forum { get; set; }

        public ForumComment()
        {
            Id = 0;
            ForumId = 0;
            CreatorId = 0;
            Content = "";
            UploadDateTime = DateTime.Now;
            Creator = new();
            Forum = new();

            if (Creator != null)
                CreatorId = Creator.Id;

            if (Forum != null)
                ForumId = Forum.Id;
            
        }
    }
}
