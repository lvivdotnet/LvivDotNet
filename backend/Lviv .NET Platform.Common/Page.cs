using System.Collections.Generic;

namespace Lviv_.NET_Platform.Common
{
    public class Page<T>
    {
        public IEnumerable<T> Items { get; set; }

        public int Total { get; set; }
    }
}