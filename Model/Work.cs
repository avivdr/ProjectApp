using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectApp.Model
{
    public class Work
    {
        public int Id { get; set; }

        public int ComposerId { get; set; }

        public string Title { get; set; }

        public byte Genre { get; set; }

        public Composer Composer { get; set; }

        public List<WorksUser> Works { get; set; }

        public string Subtitle { get; set; }

        public List<string> SearchTerms { get; set; }

        public string Popular { get; set; }

        public string Recommended { get; set; }

        public string SearchMode { get; set; }

        public string Catalogue { get; set; }

        public string Catalogue_Number { get; set; }

        [JsonIgnore]
        public string TitleWithComposersName { get => $"{Composer?.Name}: {Title}"; }

        public Work()
        {
            Title = "";
            Genre = 0;
            Works = new();
            Subtitle = "";
            SearchTerms = new();
            Popular = "";
            Recommended = "";
            SearchMode = "";
            Catalogue = "";
            Catalogue_Number = "";
        }
    }
}
