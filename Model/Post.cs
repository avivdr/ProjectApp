using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectApp.Model
{
    public class Post
    {
        public int Id { get; set; }

        public int CreatorId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime UploadDateTime { get; set; }

        public int? Work { get; set; }

        public int? Composer { get; set; }

        public List<Comment> Comments { get; set; }

        public User Creator { get; set; }
    }
}
