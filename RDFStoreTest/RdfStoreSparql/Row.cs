using System;
using System.Collections.Generic;
using System.Linq;

    public class Row    :IComparable<Row>, IComparer<Row>
    {

        private readonly IComparable[] cells;

        public Row(params IComparable[] cells)
        {
            this.cells = cells;
        }

        public Row()
        {
                
        }

        public int Length { get { return cells.Length; }}

        public int CompareTo(Row other)
        {
            
            for (int i = 0; i < Math.Min(cells.Length, other.cells.Length); i++)
            {
                      int compareTo = cells[i].CompareTo(other.cells[i]);
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
