using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectApp.Model
{
    public class Composer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CompleteName { get; set; }
        public List<Work> Works { get; set; } 
        public DateOnly Birth { get; set; }
        public DateOnly Death { get; set; }
        public string Epoch { get; set; }
        public string Portrait { get; set; }

        public Composer()
        {
            Id = 0;
            Name = "";
            CompleteName = "";
            Works = new();
            Birth = new();
            Death = new();
            Epoch = "";
            Portrait = "";
        }
    }
}
