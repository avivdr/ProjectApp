using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectApp.Model
{
    public class TaggableItem
    {
        public virtual string String { get; }
        public virtual string ImageSource => null;
        public virtual bool IsImage => false;

        public TaggableItem(string str = null)
        {
            String = str;
        }
    }
}
