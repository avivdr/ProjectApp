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
        public static void AddRange(this ObservableCollection<Composer> collection, IEnumerable<Composer> items)
        {
            foreach (var item in items)
            {
                if (!collection.Any(x => x.CompleteName == item.CompleteName))
                    collection.Add(item);
            }
        }
        public static void AddRange(this ObservableCollection<Work> collection, IEnumerable<Work> items)
        {
            foreach (var item in items)
            {
                if (!collection.Any(x => x.Title == item.Title))
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

        public static void SetFromList(this ObservableCollection<Composer> collection, List<Composer> list)
        {
            collection.Empty();
            collection.AddRange(list);
        }
        public static void SetFromList(this ObservableCollection<Work> collection, List<Work> list)
        {
            collection.Empty();
            collection.AddRange(list);
        }
    }
}
