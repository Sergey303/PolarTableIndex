using System;
using System.Collections.Generic;
using System.Linq;
using PolarDB;

namespace Task15UniversalIndex
{
    public class TableViewImmutable : IBearingTableImmutable
    {
        protected PaCell table_cell;
        public TableViewImmutable() { }
        public TableViewImmutable(string path_name, PType e_type)
        {
            table_cell = new PaCell(new PTypeSequence(e_type), path_name + ".pac", false);
        }
        public void Clear()
        {
            table_cell.Clear();
        }

        public virtual void Fill(IEnumerable<object> elements)
        {
            Clear();
            table_cell.Fill(new object[0]);
            foreach (var el in elements) table_cell.Root.AppendElement(el);
            table_cell.Flush();
        }
        public TableViewImmutable FillTV(IEnumerable<object> elements)
        {
            Fill(elements);
            return this;
        }

        public void Scan(Func<long, object, bool> doit)
        {
            table_cell.Root.Scan(doit);
        }

        public PolarDB.PaEntry Element(long ind)
        {
            return table_cell.Root.Element(ind);
        }

        //public object GetValue(long offset)
        //{
        //    PaEntry entry = table_cell.Root.Element(0);
        //    entry.offset = offset;
        //    return entry.Get();
        //}

        public long Count()
        {
            return table_cell.Root.Count();
        }
    }
}
