using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolarDB;

namespace RDFStoreTest
{
    class Index
    {
        private PaEntry table;
        private PaCell offsetsPaCell;
        private PaEntry ptr;

        public Index( string path, string name, PaEntry table)
        {
            this.table = table;
            offsetsPaCell=new PaCell(new PTypeSequence(new PType(PTypeEnumeration.longinteger)), path+name, false );
        }

        public void Build()
        {
            offsetsPaCell.Clear();
            offsetsPaCell.Fill(new object[0]);
            table.Scan((offset, row_obj) =>
            {   
                offsetsPaCell.Root.AppendElement(offset);
                return true;
            });
            offsetsPaCell.Flush();
            ptr = table.Element(0);
            offsetsPaCell.Root.SortByKey(offset =>
            {
                ptr.offset = (long) offset;
                return (int) ptr.Field(0).Get();
            });
        }

        public IEnumerable<object> Search(int key)
        {
           return offsetsPaCell.Root.BinarySearchAll(offset_entry =>
            {
                ptr.offset = (long)offset_entry.Get();
                return SeachFunc(ptr);
            })
            .Select(entry =>
            {
                ptr.offset = (long) entry.Get();
                return ptr.Get();
            });
        }

        private Func<PaEntry, int> SeachFunc = ptr => (int) ptr.Field(0).Get();
    }
}
