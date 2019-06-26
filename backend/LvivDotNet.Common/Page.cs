using System.Collections.Generic;

namespace LvivDotNet.Common
{
    /// <summary>
    /// Represents the generic paging object.
    /// </summary>
    /// <typeparam name="T"> Paging items type. </typeparam>
    public class Page<T>
    {
        /// <summary>
        /// Gets or sets items of page.
        /// </summary>
        public IEnumerable<T> Items { get; set; }

        /// <summary>
        /// gets or sets the total count of items in page.
        /// </summary>
        public int Total { get; set; }
    }
}