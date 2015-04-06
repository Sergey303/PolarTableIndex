using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IndexCommon;
using PolarDB;

namespace PolarTableIndex
{
    public class IndexInsideRecursive<TKey> : PaCell, IIndex<TKey>  where TKey:IComparable
    {
        private class OffsetsCell :PaCell
        {
            public OffsetsCell(string filePath, bool readOnly = true) : base(new PTypeSequence(new PType(PTypeEnumeration.longinteger)), filePath, readOnly)
            {

            }

            public void Insert(IEnumerable<long> offsets)
            {
                Clear();
                Fill(new object[0]);
                foreach (var offset in offsets)
                    Root.AppendElement(offset);
                Flush();
            }

            public IEnumerable<long> GetOffsets()
            {
                return Root.Elements().Select(entry => entry.Get()).Cast<long>();
            } 
        }

        private readonly DirectoryInfo dirPath;
    

        private PaEntry table;
      
        #region scale   
        private readonly Diapason[] diapasons;
          private int minCombinedkey=int.MaxValue;
          private int maxCombinedkey=0;
        private readonly int diapasonsCount;
        private readonly int fullKeyCollisionsMax4Exceptional;

        #endregion


        #region Inside Recursive               
        
        private readonly Dictionary<dynamic, OffsetsCell> insideIndexes = new Dictionary<dynamic, OffsetsCell>();
      
        /// <summary>
        /// если диапазон короче, то поиск сканированием, если длинее, то используется бинарный поиск.
        /// </summary>
        private static long maxDiapason4Binarysearch=1000;

        private Func<object[], TKey> keyF;
        private readonly Predicate<object[]> testOnDeleted;

        #endregion


      

        public IndexInsideRecursive(DirectoryInfo dirPath, PaEntry root, int diapasonsCount, int fullKeyCollisionsMax4Exceptional, Func<object[], TKey> keyF, Predicate<object[]> testOnDeleted=null) :
            base(new PTypeSequence(
                new PTypeRecord(  new NamedType("t1", Class2PolarType.ToPType(typeof (TKey))),
                        new NamedType("link", new PType(PTypeEnumeration.longinteger))
                        )),
                dirPath + "\\index.pac", false)
        {
            this.table = root;
            this.dirPath = dirPath;
            this.diapasonsCount = diapasonsCount;
            this.fullKeyCollisionsMax4Exceptional = fullKeyCollisionsMax4Exceptional;
            this.keyF = keyF;
            this.testOnDeleted = testOnDeleted;


            diapasons = new Diapason[diapasonsCount];

            if (!IsEmpty)
            {
                foreach (var subDir in dirPath.EnumerateDirectories())
                {
                    OffsetsCell indexInsideRecursive = new OffsetsCell(subDir.FullName + "offsets.pac", true);
                    //ключ неизвестного типа хранится как строка в имени папки.
                    var key = Convert.ChangeType(Convert.FromBase64String(subDir.Name), typeof (TKey));
                    insideIndexes.Add(key, indexInsideRecursive);
                }

                //создание диапазонов сканированием таблицы индексов
                //если остался последний столбец, ни поиск, ни диапазоны не нужны.

                // GetSubKey не работает без этих величин
                minCombinedkey = Convert.ToInt32((TKey) ((object[]) Root.Element(0).Get())[0]);
                    // Combine(.Select(o1 => (dynamic)Convert.ChangeType(o1, rowTypes[0])));
                maxCombinedkey = Convert.ToInt32((TKey) ((object[]) Root.Element(Root.Count() - 1).Get())[0]);
                    //Combine(((object[])Root.Element(Root.Count() - 1).Get()).Select(o1 => (dynamic)Convert.ChangeType(o1, rowTypes[0])));

                Root.Scan(o =>
                {
                    var subKey = GetSubKey((TKey) ((object[]) o)[0]);
                    diapasons[subKey] = diapasons[subKey].AddNumb(1);
                    return true;
                });
            }
        }
        public void Build()
        {
             Clear();
     

            //первое сканирование- подсчёт количеств одинаковых элементов первого столбца
            var counts = new Dictionary<dynamic, int>();


            table.Scan((offset, obj) =>
            {
                var key = keyF((object[]) obj);

                if (counts.ContainsKey(key))
                    counts[key]++;
                else counts.Add(key, 1);
                return true;
            });

            //второе сканирование-распределение строк на вложенные индексы или в таблицу индексов, из последних вычисляется максимальная и минимальная строка
            var offset4InsideIndex = new Dictionary<dynamic, List<long>>();
            table.Scan((offset, obj) =>
            {
                var objects = (object[])obj;
                var key = keyF(objects);

                if (counts[key] >= fullKeyCollisionsMax4Exceptional)
                {
                    List<long> offsets;
                    if (!offset4InsideIndex.TryGetValue(key, out offsets))
                        offset4InsideIndex.Add(key, offsets = new List<long>());
                    offsets.Add(offset);
                    return true;
                }
                unchecked
                {      
                    var iKey = Math.Abs(Convert.ToInt32(key));
                    if (iKey < Math.Abs(minCombinedkey)) minCombinedkey = iKey;
                    if (iKey > Math.Abs(maxCombinedkey))
                        maxCombinedkey = iKey;
                }
                return true;
            });


            table.Scan((offset, obj) =>
            {
                var objects = (object[])obj;
                var first = keyF(objects);
                if (counts[first] >= fullKeyCollisionsMax4Exceptional)
                    return true;

                int subKey = GetSubKey(first);
                diapasons[subKey] = diapasons[subKey].AddNumb(1);
                return true;
            });
            long sum = 0;
            for (int i = 0; i < diapasons.Length; i++)
            {
                diapasons[i] = diapasons[i].AddStart(sum); //start must be 0
                sum += diapasons[i].numb;
            }


            long lastDiapasonCount = diapasons[diapasonsCount - 1].numb;
            var empty = new List<object>(){ default(TKey), 0l};
            table.Scan((offset, obj) =>
            {
                var objects = (object[])obj;
                var first = keyF(objects);

                if (counts[first] >= fullKeyCollisionsMax4Exceptional)
                    return true;


                //diapasons предполагается не пустое, раз есть хоть один не исключительный ключ.                   

                var diapasonIndex = GetSubKey(first);
                long position;
                if (diapasonIndex < diapasons.Length - 1)
                    position = diapasons[diapasonIndex + 1].start - diapasons[diapasonIndex].numb;
                else
                    position = diapasons[diapasonIndex].start + lastDiapasonCount - diapasons[diapasonIndex].numb;
                diapasons[diapasonIndex] = diapasons[diapasonIndex].AddNumb(-1);
                var list = new List<object>(){ first, offset};

                if (Root.Count() <= position)
                {
                    for (int i = 0; i < position - Root.Count(); i++)
                        Root.AppendElement(empty.ToArray());
                    Root.AppendElement(list.ToArray());
                }
                else
                    Root.Element(position).Set(list.ToArray());

                return true;
            });

            Flush();


            for (int i = 0; i < diapasons.Length - 1; i++)
                diapasons[i] = diapasons[i].AddNumb(diapasons[i + 1].start - diapasons[i].start); //numb must be 0
            var count = Root.Count();
            diapasons[diapasons.Length - 1] =
                diapasons[diapasons.Length - 1].AddNumb(count - diapasons[diapasons.Length - 1].start);
            for (int i = 0; i < diapasons.Length; i++)
                Root.SortByKey(diapasons[i].start, diapasons[i].numb, o => (TKey)((object[])o)[0], null);
           //рекурсивное создание и пострроение внутренних индексов
            foreach (var kv in offset4InsideIndex)
            {
                DirectoryInfo subDir = new DirectoryInfo(dirPath + "\\" + Convert.ToBase64String(BitConverter.GetBytes(kv.Key)));
            subDir.Create();

                OffsetsCell indexInsideRecursive = new OffsetsCell(subDir.FullName, false);
              
                indexInsideRecursive.Insert(kv.Value);
                insideIndexes.Add(kv.Key,
                    indexInsideRecursive);                                                
            }
        }
        public void BuildScale(int N) { }

        public new void Clear()
        {
            ((PCell) this).Clear();
            foreach (var directory in dirPath.EnumerateDirectories())
                directory.Delete();
            if(diapasons!=null)
            for (int i = 0; i < diapasonsCount; i++)
                diapasons[i] = new Diapason();
            Fill(new object[0]);
        }

        /// <summary>
        /// монотонная функция, сужающая длинные числа от <c>minCombinedkey</c>   до <c>maxCombinedkey</c>    в диапазон от 0 до  diapasonsCount - 1
        /// </summary>
        /// <param name="combined">длинное число соответсвующее строки</param>
        /// <returns></returns>
        private int GetSubKey(TKey combined)
        {
            return (int)((diapasonsCount - 1) * Convert.ToDouble((Math.Abs(Convert.ToInt32(combined) - minCombinedkey)) / (maxCombinedkey - minCombinedkey)));
        }
      /// <summary>
        /// 
        /// </summary>
        /// <param name="key">можно было сделать массив object, и конвертировать ниже</param>
        /// <returns></returns>
        public IEnumerable<long> GetRowsOffsetsByKey(TKey key)
        {
           
            //поиск
            OffsetsCell index;
             //между ключами вложенных индексов и первым столбцом нет пересечений, если есть ключ здесь, его нет в таблице.
            if (insideIndexes.TryGetValue(key, out index))
                return
                    index.GetOffsets();
            //поиск диапазона
            //комбинирование строки ключа дополнено умножением на max^(n-l), т.к.в keys строке l<n элемнетов, комбинирование даст многочлен (a_0*max^l+...), а строки скомбинированы многочленом степени n. 
           
            var subkey = GetSubKey(key);
            if (subkey >= diapasons.Length) return Enumerable.Empty<long>();
            var diapason = diapasons[subkey];         
            //короткие диапазоны быстрее просканировать
            if (diapason.numb >= maxDiapason4Binarysearch)
                return Root.BinarySearchAll(diapason.start, diapason.numb,
                    entry =>key .CompareTo(((TKey)(((object[]) entry.Get())[0]))))
                    .Select(entry => (long) entry.Field(1).Get());
            else
                return Root.Elements(diapason.start, diapason.numb)
                    .Select(entry =>((object[]) entry.Get()))
                    .SkipWhile(row => ((TKey)row[0]).CompareTo(key) == -1)
                                        .SkipWhile(row => ((TKey)row[0]).CompareTo(key) == 0)
                    .Select(row => (long) row[row.Length - 1]);
        }

        

       

        public void Warmup()
        {
            foreach (var element in Root.Elements()) ;
            
        }

        public IEnumerable<PaEntry> GetAllByKey(long start, long number, TKey key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object[]> GetAllReadedByKey(long start, long number, TKey key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PaEntry> GetAllByKey(TKey key)
        {                        
            var entryRow = table.Element(0);
            IEnumerable<PaEntry> allByKey = GetRowsOffsetsByKey(key).Select(offset =>
                {
                    entryRow.offset = offset;
                    return entryRow;
                });
            if (testOnDeleted != null)
                allByKey = allByKey.Where(entry => testOnDeleted((object[]) entry.Get()));
            return allByKey;
        }

        public IEnumerable<object[]> GetAllReadedByKey(TKey key)
        {
            var entryRow = table.Element(0);
            IEnumerable<object[]> allByKey = GetRowsOffsetsByKey(key).Select(offset =>
            {
                entryRow.offset = offset;
                return entryRow;
            }).Select(entry => (object[])entry.Get());
            if (testOnDeleted != null)
                allByKey = allByKey.Where(row => testOnDeleted(row));
            return allByKey;
        }

        public PaEntry Table { get { return table; } }
        public TKey KeyProducer(PaEntry entry)
        {
            throw new NotImplementedException();
        }

        public long Count()
        {
            throw new NotImplementedException();
        }

        public void Build2()
        {
            throw new NotImplementedException();
        }

        public void Build3()
        {
            throw new NotImplementedException();
        }
    }

    public class IndexConstructor
    {
        public static IndexInsideRecursive<T> CreateInsideRecursive<T>(string dirPath, PaEntry root, Func<object[], T> keyF,
            Predicate<object[]> testOnDeleted = null) where T : IComparable
        {
            dirPath += "\\index";
            DirectoryInfo dir = new DirectoryInfo(dirPath);
            if (dir.Exists) dir.Delete(true);
            dir.Create();
            return new IndexInsideRecursive<T>(dir, root, 1000, 128, keyF, testOnDeleted);
        }
    }
}
