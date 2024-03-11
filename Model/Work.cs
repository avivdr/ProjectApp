using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectApp.Model
{
    public class Work : TaggableItem
    {
        public int Id { get; set; }

        public int ComposerId { get; set; }

        public string Title { get; set; } = "";

        public Composer Composer { get; set; }

        public List<WorksUser> Works { get; set; } = new();

        public string Subtitle { get; set; } = "";

        public List<string> SearchTerms { get; set; } = new();

        public string Popular { get; set; } = "";

        public string Recommended { get; set; } = "";

        public string SearchMode { get; set; } = "";

        public string Catalogue { get; set; } = "";

        public string Catalogue_Number { get; set; } = "";

        public virtual List<Forum> Forums { get; set; } = new();

        public virtual List<Post> Posts { get; set; } = new();

        public virtual List<WorksUser> WorksUsers { get; set; } = new();

        [JsonIgnore] public string TitleWithComposersName { get => $"{Composer?.Name}: {Title}"; }

        [JsonIgnore] public override string String => TitleWithComposersName;
    }
}
