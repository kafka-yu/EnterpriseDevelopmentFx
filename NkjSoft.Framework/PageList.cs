using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NkjSoft.Framework
{
    [Serializable]
    public class PageList<T>
    {
        public int total { get; set; }

        public ICollection<T> rows { get; set; }

        public PageList(int total, ICollection<T> data)
        {
            this.total = total;
            this.rows = data;
        }
    }

    public static class extensions
    {
        public static PageList<T> AsPagedList<T>(this ICollection<T> obj, int totalRows = 1)
        {
            return new PageList<T>(totalRows, obj);
        }
    }
}
