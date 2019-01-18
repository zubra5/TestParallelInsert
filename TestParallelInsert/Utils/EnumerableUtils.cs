using System;
using System.Collections.Generic;

namespace TestParallelInsert.Utils
{
    public static class EnumerableUtils
    {
        public static IEnumerable<T> Init<T>(Func<int, T> createItem)
        {
            if (createItem == null) throw new ArgumentNullException(nameof(createItem));

            var i = 0;
            while (true)
            {
                yield return createItem(i++);
            }
        }
    }
}
