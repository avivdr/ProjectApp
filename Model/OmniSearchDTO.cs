using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectApp.Model
{
    public class OmniSearchDTO
    {
        public List<Work> Works {  get; set; } = new List<Work>();
        public List<Composer> Composers { get; set; } = new List<Composer>();
    }
}
