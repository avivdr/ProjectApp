using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectApp.Model
{
    public static class ObservableCollectionExtension
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                if (!collection.Any(x => x.Equals(item)))
                    collection.Add(item);
            }
        }

        public static void Empty<T>(this ObservableCollection<T> collection)
        {
            int count = collection.Count;
            for (var i = 0; i < count; i++)
            {
                collection.RemoveAt(0);
            }
        }

        public static void SetFromList<T>(this ObservableCollection<T> collection, List<T> list)
        {
            collection.Empty();
            collection.AddRange(list);
        }
    }
}
