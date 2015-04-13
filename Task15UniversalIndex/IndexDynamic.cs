using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolarDB;

namespace Task15UniversalIndex
{
    public class IndexDynamic<Tkey, IndexImmut> : IIndex<Tkey> where IndexImmut : IIndexImmutable<Tkey>
    {
        //internal void DropIndex() { throw new NotImplementedException(); }
        //TODO: Экономнее было бы обойтись длинными offset'ами: Dictionary<Tkey, long>. Но откуда брать тип и ячейку для конструирования? Или надо возвращать также офсеты?
        private Dictionary<Tkey, PaEntry> keyent = new Dictionary<Tkey, PaEntry>(); // для уникального
        private Dictionary<Tkey, List<PaEntry>> keyents = new Dictionary<Tkey, List<PaEntry>>(); // стандартно
        public void OnAppendElement(PolarDB.PaEntry entry)
        {
            Tkey key = KeyProducer(entry.Get());
            if (_unique) keyent.Add(key, entry); // Надо бы что-то проверить...
            else
            {
                List<PaEntry> entset;
                if (keyents.TryGetValue(key, out entset))
                {
                    entset.Add(entry);
                }
                else
                {
                    keyents.Add(key, Enumerable.Repeat<PaEntry>(entry, 1).ToList());
                }
            }
        }

        public PaCell IndexCell { get { return null; } } // Этот интерфейс для динамического индекса кажется лишним
        public Func<object, Tkey> KeyProducer { get; set; }
        public TableView Table { get; set; }
        public IndexImmut IndexArray { get; set; } //??
        private bool _unique;
        public bool Unique { get { return _unique; } }
        public IndexDynamic(bool unique)
        {
            this._unique = unique;
        }
        public long Count() { return IndexArray.Count(); }
        public void Build()
        {
            throw new NotImplementedException();
        }
        public void Warmup() { IndexArray.Warmup(); }

        public IEnumerable<PolarDB.PaEntry> GetAllByKey(long start, long number, Tkey key)
        {
            throw new Exception("No implementation (no need) of GetAllByKey(long start, long number, Tkey key)");
        }

        //Такой вариант не получается из-за использования шкалы
        //public IEnumerable<PolarDB.PaEntry> GetAllByKey(Tkey key)
        //{
        //    return GetAllByKey(0, IndexArray.Count(), key);
        //}
        public IEnumerable<PolarDB.PaEntry> GetAllByKey(Tkey key)
        {
            if (_unique)
            {
                PaEntry entry;
                if (keyent.TryGetValue(key, out entry))
                {
                    return Enumerable.Repeat<PaEntry>(entry, 1).Concat<PaEntry>(IndexArray.GetAllByKey(key));
                }
                return IndexArray.GetAllByKey(key);
            }
            else
            {
                List<PaEntry> entries;
                if (keyents.TryGetValue(key, out entries))
                {
                    return entries.Concat<PaEntry>(IndexArray.GetAllByKey(key));
                }
                return IndexArray.GetAllByKey(key);
            }
            //return GetAllByKey(0, IndexArray.Count(), key);
        }
        public IEnumerable<PaEntry> GetAllByLevel(Func<PaEntry, int> LevelFunc)
        {
            throw new Exception("GetAllByLevel dois not implemented in DinamicIndexUnique");
        }
        public void DropIndex()
        {
            throw new NotImplementedException();
        }
    }
}
