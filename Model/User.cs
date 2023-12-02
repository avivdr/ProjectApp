using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjectApp.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Pwsd { get; set; }
        public string Email { get; set; }
        public List<Comment> Comments { get; set; }
        public List<ForumComment> ForumComments { get; set; }
        public List<Forum> Forums { get; set; }
        public List<Post> Posts { get; set; }
    }
}
