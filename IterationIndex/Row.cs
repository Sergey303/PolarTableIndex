using System;
using System.Collections.Generic;
using System.Linq;

namespace IterationIndexLib
{
    public class Row    :IComparable<Row>, IComparer<Row>
    {

        private readonly object[] cells;

        public Row(params object[] cells)
        {
            this.cells = cells;
        }

        public Row()
        {
                
        }

        public int Length { get { return cells.Length; }}

        public int CompareTo(Row other)
        {
            foreach (int i in Enumerable.Range(0, Math.Min(cells.Length, other.cells.Length)))
            {
                int compareTo = ((IComparable) cells[i]).CompareTo(other.cells[i]);
                if (compareTo != 0) return compareTo;
            }
            return 0;
        }

        public int Compare(Row x, Row y)
        {
            return x.CompareTo(y);
        }

        public object this[int i]
        {
            get { return cells[i]; }
        }
    }
}