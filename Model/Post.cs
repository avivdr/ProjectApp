using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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

        public int? ComposerId { get; set; }

        public int? WorkId { get; set; }

        public List<Comment> Comments { get; set; }

        public User Creator { get; set; }

        public Composer Composer { get; set; }

        public Work Work { get; set; }

        [JsonIgnore] public Stream File { get; set; }

        public Post()
        {
            Title = "";
            Content = "";
            UploadDateTime = DateTime.Now;
            Comments = new();
        }
    }
}
