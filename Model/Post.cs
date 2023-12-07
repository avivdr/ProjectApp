﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        public int? Work { get; set; }

        public int? Composer { get; set; }

        public List<Comment> Comments { get; set; }

        public User Creator { get; set; }

        [JsonIgnore]
        public FileStream File { get; set; }

        public Post()
        {
            Id = 0;
            CreatorId = 0;
            Title = "";
            Content = "";
            UploadDateTime = DateTime.Now;
            Comments = new();
            Creator = new();
        }
    }
}
