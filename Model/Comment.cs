using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectApp.Model
{
    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int CreatorId { get; set; }
        public string Content { get; set; }
        public DateTime UploadDateTime { get; set; }
        public User Creator { get; set; }
        public Post Post { get; set; }

        public Comment()
        {
            Id = 0;
            PostId = 0;
            CreatorId = 0;
            Content = "";
            UploadDateTime = DateTime.Now;
            Creator = new();
            Post = new();

            if (Creator != null)
                CreatorId = Creator.Id;

            if(Post != null)
                PostId = Post.Id;
        }
    }
}
