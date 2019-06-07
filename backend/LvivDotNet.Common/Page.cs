using System.Collections.Generic;

namespace LvivDotNet.Common
{
    public class Page<T>
    {
        public IEnumerable<T> Items { get; set; }

        public int Total { get; set; }
    }
}