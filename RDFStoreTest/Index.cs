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
        private readonly PaEntry table;
        private readonly PaCell offsetsPaCell;
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
                return SearchFunc3(ptr);
            }, new Comparer3(null, null, null));
        }

        public IEnumerable<object> Search1(IComparable key)
        {
            return offsetsPaCell.Root.BinarySearchAll(offset_entry =>
            {
                ptr.offset = (long)offset_entry.Get();
                IComparable readed = SearchFunc1(ptr);
                return readed.CompareTo(key);
            })
           .Select(entry =>
           {
               ptr.offset = (long)entry.Get();
               return ptr.Get();
           });
        }
        public IEnumerable<object> Search2(IComparable k1, IComparable k2)
        {
            Comparer2 key2 = new Comparer2(k1, k2);
           return offsetsPaCell.Root.BinarySearchAll(offset_entry =>
            {
                ptr.offset = (long)offset_entry.Get();
                Comparer2 readed = SearchFunc2(ptr);
                return readed.CompareTo(key2);
            })
            .Select(entry =>
            {
                ptr.offset = (long) entry.Get();
                return ptr.Get();
            });
        }
        public IEnumerable<object> Search3(IComparable k1, IComparable k2, IComparable k3)
        {
            Comparer3 key3 = new Comparer3(k1, k2, k3);
            return offsetsPaCell.Root.BinarySearchAll(offset_entry =>
            {
                ptr.offset = (long)offset_entry.Get();
                Comparer3 readed = SearchFunc3(ptr);
                return readed.CompareTo(key3);
            })
             .Select(entry =>
             {
                 ptr.offset = (long)entry.Get();
                 return ptr.Get();
             });
        }
        private Func<PaEntry, IComparable> SearchFunc1 = ptr =>
        {
            return (IComparable)ptr.Field(0).Get();
        };
        private Func<PaEntry, Comparer2> SearchFunc2 = ptr =>
        {
            object[] row = (object[]) ptr.Get();
            return new Comparer2((IComparable)row[0], (IComparable)row[1]);
        };
        private Func<PaEntry, Comparer3> SearchFunc3 = ptr =>
        {
            object[] row = (object[])ptr.Get();
            return new Comparer3((IComparable)row[0], (IComparable)row[1], ObjectVariantsEx.Writeble2Comparable((object[]) row[2]));
        };
    }

    class Comparer2 :IComparer<Comparer2>, IComparable<Comparer2>  , IComparable
    {
        protected readonly IComparable k1;
        protected readonly IComparable k2;

        public Comparer2(IComparable k1, IComparable k2)
        {
            this.k1 = k1;
            this.k2 = k2;
        }

        public int Compare(Comparer2 x, Comparer2 y)
        {
           return x.CompareTo(y);
        }

        public int CompareTo(Comparer2 other)
        {   
            int compareK1 = k1.CompareTo(other.k1);
            if (compareK1 != 0) return compareK1 > 0 ? 1 : -1;
            else
            {
                int compareK2 = k2.CompareTo(other.k2);
                return compareK2 == 0 ? 0 : compareK2 > 0 ? 1 : -1;
            }
        }

        public int CompareTo(object obj)
        {
            //if (obj is Comparer3) return this.CompareTo((Comparer2) obj);
            if (obj is Comparer2) return this.CompareTo((Comparer2) obj);
            if (obj is IComparable)
            {
                int compareK1 = k1.CompareTo(((IComparable) obj));
                return compareK1 == 0 ? 0 : compareK1 > 0 ? 1 : -1;
            }
            throw new ArgumentException();
        }
    }
    class Comparer3 :Comparer2, IComparer<Comparer3>, IComparable<Comparer3>   , IComparable
    {
        
        private readonly IComparable k3;

        public Comparer3(IComparable k1, IComparable k2, IComparable k3) : base(k1,k2)
        {                                                                                 
            this.k3 = k3;
        }

        public int Compare(Comparer3 x, Comparer3 y)
        {
            return x.CompareTo(y);
        }

        public int CompareTo(Comparer3 other)
        {
            int compareK1 = k1.CompareTo(other.k1);
            if (compareK1 != 0) return compareK1 > 0 ? 1 : -1;
            else
            {
                int compareK2 = k2.CompareTo(other.k2);
                if (compareK2 != 0) return compareK2 > 0 ? 1 : -1;
                else
                {
                    int compareK3 = k3.CompareTo(other.k3);
                    return compareK3 == 0 ? 0 : compareK3 > 0 ? 1 : -1;
                }
            }
        }

        public new int CompareTo(object obj)
        { 
            if (obj is Comparer3) return this.CompareTo((Comparer3) obj);
            if (obj is Comparer2) return ((Comparer2)this).CompareTo((Comparer2) obj);
            if (obj is IComparable)
            {
                int compareK1 = k1.CompareTo(((IComparable) obj));
                return compareK1 == 0 ? 0 : compareK1 > 0 ? 1 : -1;
            }
            throw new ArgumentException();
        }
         
    }
}
