using System;
using System.Collections.Generic;
using System.Linq;
using PolarDB;

namespace Task15UniversalIndex
{
    public class TableView : TableViewImmutable, IBearingTable 
    {
        PType tp_rec;
        public TableView(string path_name, PType e_type)
        {
            tp_rec = new PTypeRecord(
                new NamedType("deleted", new PType(PTypeEnumeration.boolean)),
                new NamedType("evalue", e_type));
            table_cell = new PaCell(new PTypeSequence(tp_rec), path_name + ".pac", false);
        }
        public override void Fill(IEnumerable<object> values)
        {
            Clear();
            table_cell.Fill(new object[0]);
            foreach (var el in values) table_cell.Root.AppendElement(new object[] { false, el });
            table_cell.Flush();
        }
        public void Warmup() { foreach (var v in table_cell.Root.ElementValues()); }
        List<IIndexCommon> indexes = new List<IIndexCommon>();
        // По имеющейса опорной таблице и коннекторам индексов (в списке indexes), (заново) построить индексы 
        public void BuildIndexes() { foreach (var index in indexes) index.Build(); }


        public PaEntry AppendValue(object value)
        {
            long offset = table_cell.Root.AppendElement(new object[] { false, value });
            table_cell.Flush();
            PaEntry entry = new PaEntry(tp_rec, offset, table_cell);
            foreach (var index in indexes) index.OnAppendElement(entry);
            return entry;
        }

        public void DeleteEntry(PaEntry record)
        {
            record.Field(0).Set(true);
        }

        public IEnumerable<PaEntry> GetUndeleted(IEnumerable<PaEntry> elements)
        {
            throw new NotImplementedException();
        }
        public void RegisterIndex(IIndexCommon index) 
        {
            indexes.Add(index); 
        }
        public void UnregisterIndex(IIndexCommon index) 
        {
            indexes.Remove(index);
        }
    }

}
