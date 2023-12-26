using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectApp.Model
{
    public class Composer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [JsonPropertyName("Complete_Name")]
        public string CompleteName { get; set; }

        public List<Work> Works { get; set; } 

        public DateTime Birth { get; set; }

        public DateTime? Death { get; set; }

        public string Epoch { get; set; }

        public string Portrait { get; set; }


        public Composer()
        {
            Name = "";
            CompleteName = "";
            Works = new();
            Birth = new();
            Death = new();
            Epoch = "";
            Portrait = "";
        }

        public override bool Equals(object obj)
        {
            if (obj is Composer)
            {
                return CompleteName == (obj as Composer).CompleteName;
            }

            return base.Equals(obj);
        }
    }
}
