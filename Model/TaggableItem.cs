using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectApp.Model
{
    public class TaggableItem
    {
        public virtual string String => "Tag Work or Composer";
        public virtual string ImageSource => null;
        public virtual bool IsImage => false;
    }
}
