﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectApp.Model
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = "";

        public string Password { get; set; } = "";

        public string Email { get; set; } = "";

        public bool IsAdmin { get; set; } = false;

        public List<Comment> Comments { get; set; } = new();

        public List<ForumComment> ForumComments { get; set; } = new();

        public List<Forum> Forums { get; set; } = new();

        public List<Post> Posts { get; set; } = new();

        private List<WorksUser> WorksUsers { get; set; } = new();

        [JsonIgnore]
        public List<Work> Works { get => WorksUsers.Select(x => x.Work).ToList(); }
    }
}
