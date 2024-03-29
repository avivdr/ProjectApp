﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectApp.Model
{
    public class Composer : TaggableItem
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        [JsonPropertyName("Complete_Name")]
        public string CompleteName { get; set; } = "";

        public List<Work> Works { get; set; } = new();

        public DateTime Birth { get; set; } = new();

        public DateTime? Death { get; set; }

        public string Epoch { get; set; } = "";

        public string Portrait { get; set; } = "";

        public List<Forum> Forums { get; set; } = new();

        public List<Post> Posts { get; set; } = new();

        [JsonIgnore] public override string String => CompleteName;
        [JsonIgnore] public override string ImageSource => Portrait;
        [JsonIgnore] public override bool IsImage => true;
    }
}
