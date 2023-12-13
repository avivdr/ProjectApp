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
        public string Content { get; set; }
        public DateTime UploadDateTime { get; set; }
        public User Creator { get; set; }
        public Forum Forum { get; set; }

        public ForumComment()
        {
            Content = "";
            UploadDateTime = DateTime.Now;
        }
    }
}
