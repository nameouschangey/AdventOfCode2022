using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class TreeFlatten
    {
        // flatten a tree into a list, ordered by heirarchy level (all tier 1, then all tier 2, etc.)
        public static IEnumerable<T> Flatten<T>(T obj, Func<T, IEnumerable<T>> childSelector)
        {
            var flattened = new List<T>() { obj };
            int i = 0;
            while (i < flattened.Count)
            {
                flattened.AddRange(childSelector(flattened[i]));
                i++;
            }
            return flattened;
        }
    }
}
