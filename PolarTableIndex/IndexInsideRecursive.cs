using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using PolarDB;

namespace PolarTableIndex
{
    public class IndexInsideRecursive  :PaCell 
    {
        private readonly DirectoryInfo dirPath;
        private readonly Type[] rowTypes;
     

        private PaEntry table;
      
        #region scale   
        private readonly Diapason[] diapasons;
          private BigInteger minCombinedkey = Int32.MaxValue;
        private BigInteger maxCombinedkey = Int32.MinValue;
        private readonly int diapasonsCount;
        #endregion


        #region Inside Recursive               
        
        private readonly Dictionary<dynamic, IndexInsideRecursive> insideIndexes = new Dictionary<dynamic, IndexInsideRecursive>();
      
        /// <summary>
        /// если диапазон короче, то поиск сканированием, если длинее, то используется бинарный поиск.
        /// </summary>
        private static long maxDiapason4Binarysearch=1000;

      

        #endregion

        public IndexInsideRecursive(DirectoryInfo dirPath, Type[] rowTypes, int diapasonsCount) :
            base(new PTypeSequence(
                rowTypes.Length==0 ? new PType(PTypeEnumeration.longinteger) :
                new PTypeRecord(
                    new List<PType>(rowTypes.Select(Class2PolarType.ToPType))
                        .AddAndThen(new PType(PTypeEnumeration.longinteger))
                        .Select((pt, i) => new NamedType("t" + i, pt))
                        .ToArray())),
                dirPath + "\\index.pac", false)
        {
            this.dirPath = dirPath;
            this.rowTypes = rowTypes;
            this.diapasonsCount = diapasonsCount;

            if (rowTypes.Length > 0)
                diapasons = new Diapason[diapasonsCount];

            if (!IsEmpty)
            {
                foreach (var subDir in dirPath.EnumerateDirectories())
                {
                    var indexInsideRecursive = new IndexInsideRecursive(subDir, rowTypes.Skip(1).ToArray(),
                        diapasonsCount);
                    //ключ неизвестного типа хранится как строка в имени папки.
                    var key = Convert.ChangeType(Convert.FromBase64String(subDir.Name), rowTypes[0]);
                    insideIndexes.Add(key, indexInsideRecursive);
                }

            //создание диапазонов сканированием таблицы индексов
            //если остался последний столбец, ни поиск, ни диапазоны не нужны.
            
            if (rowTypes.Length > 0)
            {
                    // GetSubKey не работает без этих величин
                    minCombinedkey = Combine(((object[]) Root.Element(0).Get()));
                    maxCombinedkey = Combine((object[]) Root.Element(Root.Count() - 1).Get());

                    Root.Scan(o =>
                    {
                        var subKey = GetSubKey(Combine((object[]) o));
                        diapasons[subKey] = diapasons[subKey].AddNumb(1);
                        return true;
                    });
                }
            }
        }

        public void Build(PaEntry root, Func<object[], dynamic>[] keyF, int fullKeyCollisionsMax4Exceptional,
            IEnumerable<long> offsets4This=null)
        { 
            //сканирование  по всей таблице или только по списку (для вложенных индексов) 
            Action<Func<long, object, bool>> scan = offsets4This == null
                ? func => root.Scan(func)
                : (Action<Func<long, object, bool>>) (func => root.ScanRowsByOffsets(offsets4This, func));


            
            this.table = root;
             Clear();
             //если это листовой индекс.
            if (rowTypes.Length == 0)
            {    
                scan((offset, obj) =>
                {
                    Root.AppendElement(offset);
                    return true;
                });
                Flush();
                return;
            }      

            //первое сканирование- подсчёт количеств одинаковых элементов первого столбца
            var counts = new Dictionary<dynamic, int>();
            Func<object[], dynamic> firstKey = keyF.First();

            scan((offset, obj) =>
            {
                var key = firstKey((object[]) obj);

                if (counts.ContainsKey(key))
                    counts[key]++;
                else counts.Add(key, 1);
                return true;
            });

            //второе сканирование-распределение строк на вложенные индексы или в таблицу индексов, из последних вычисляется максимальная и минимальная строка
            var offset4InsideIndex = new Dictionary<dynamic, List<long>>();
            scan((offset, obj) =>
            {
                var key = firstKey((object[]) obj);

                if (counts[key] >= fullKeyCollisionsMax4Exceptional)
                {
                    List<long> offsets;
                    if (!offset4InsideIndex.TryGetValue(key, out offsets))
                        offset4InsideIndex.Add(key, offsets = new List<long>());
                    offsets.Add(offset);
                    return true;
                }

                var combined = Combine((object[]) obj);

                if (combined < minCombinedkey) minCombinedkey = combined;
                if (combined > maxCombinedkey)
                {
                    maxCombinedkey = combined;
                    //    maxRow = obj;
                }
                return true;
            });


            scan((offset, obj) =>
            {
                var first = firstKey((object[]) obj);
                if (counts[first] >= fullKeyCollisionsMax4Exceptional)
                    return true;
                var combined = Combine((object[]) obj);
                uint subKey = GetSubKey(combined);
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
            var empty = Enumerable.Range(0, keyF.Length).Select(i => Convert.ChangeType(false, rowTypes[i])).ToList();
            empty.Add(0l);
            scan((offset, obj) =>
            {
                var first = firstKey((object[]) obj);

                if (counts[first] >= fullKeyCollisionsMax4Exceptional)
                    return true;


                //diapasons предполагается не пустое, раз есть хоть один не исключительный ключ.
                var combined = Combine(((object[]) obj));
                var diapasonIndex = GetSubKey(combined);
                long position;
                if (diapasonIndex < diapasons.Length - 1)
                    position = diapasons[diapasonIndex + 1].start - diapasons[diapasonIndex].numb;
                else
                    position = diapasons[diapasonIndex].start + lastDiapasonCount - diapasons[diapasonIndex].numb;
                diapasons[diapasonIndex] = diapasons[diapasonIndex].AddNumb(-1);
                var list = keyF.Select(func => func((object[]) obj)).Cast<object>().ToList();
                list.Add(offset);

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
                Root.SortByKey<dynamic[]>(diapasons[i].start, diapasons[i].numb, o => ((object[])o).Reverse().Skip(1).Reverse().Select((o1, j) => (dynamic)Convert.ChangeType(o1, rowTypes[j])).ToArray(), new RowComperer());
           //рекурсивное создание и пострроение внутренних индексов
            foreach (var kv in offset4InsideIndex)
            {
                DirectoryInfo subDir = new DirectoryInfo(dirPath + "\\" + Convert.ToBase64String(BitConverter.GetBytes(kv.Key)));
            subDir.Create();
                
                var indexInsideRecursive = new IndexInsideRecursive(subDir, rowTypes.Skip(1).ToArray(), diapasonsCount);
                indexInsideRecursive.Build(root, keyF.Skip(1).ToArray(),
                        fullKeyCollisionsMax4Exceptional, kv.Value);
                insideIndexes.Add(kv.Key,
                    indexInsideRecursive);                                                
            }
        }

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
        private uint GetSubKey(BigInteger combined)
        {
            return (uint)((diapasonsCount - 1)*(double)((combined - minCombinedkey)/maxCombinedkey));
        }
       /// <summary>
        ///   биекция от строки с простыми типами (включая bool) в BigInteger=комбинирование. 
        /// 1) Приведение элементов в соответсвующие типы  rowTypes (они должны быть известны)
        /// 2) создание BigInteger   из каждого элемента
        /// 3) комбинирование в один BigInteger по формуле all=all*max+one
        /// </summary>                                     
       /// <param name="objects"></param>
       /// <returns></returns>
        private BigInteger Combine(object[] objects)
        {
            return objects.Select(o1 => (dynamic) Convert.ChangeType(o1, rowTypes[0]))
                          .Aggregate(new BigInteger(), (integer, o) => (integer * Int32.MaxValue) + new BigInteger(o));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys">можно было сделать массив object, и конвертировать ниже</param>
        /// <returns></returns>
        public IEnumerable<long> GetRowsOffsetsByKey(dynamic[] keys)
        {
            //поиск не нужен, нужны все строки (часто, для вложенного индекса)
            if (keys.Length == 0)
            {
                //это листовой индекс, в нём нет вложенных.     Таблица в нём не содержит Polar Record. 
                if (rowTypes.Length == 0)
                    return Root.Elements().Select(entry => entry.Get()).Cast<long>();
                var current = Root.Elements().Select(entry => entry.Field(rowTypes.Length).Get()).Cast<long>();
                var insides = insideIndexes.Values.SelectMany(subIndex => subIndex.GetRowsOffsetsByKey(keys));
                return current.Concat(insides);
            }
            //поиск
            IndexInsideRecursive index;
             //между ключами вложенных индексов и первым столбцом нет пересечений, если есть ключ здесь, его нет в таблице.
            if (insideIndexes.TryGetValue(keys[0], out index))
                return
                    index.GetRowsOffsetsByKey(keys.Skip(1).ToArray());
            //поиск диапазона
            //комбинирование строки ключа дополнено умножением на max^(n-l), т.к.в keys строке l<n элемнетов, комбинирование даст многочлен (a_0*max^l+...), а строки скомбинированы многочленом степени n. 
            var combined = Combine(keys)*Int32.MaxValue ^ (rowTypes.Length - keys.Length); 
            var subkey = GetSubKey(combined);
            if (subkey >= diapasons.Length) return Enumerable.Empty<long>();
            var diapason = diapasons[subkey];         
            //короткие диапазоны быстрее просканировать
            if (diapason.numb >= maxDiapason4Binarysearch)
                return Root.BinarySearchAll(diapason.start, diapason.numb,
                    entry =>
                        CompareRows(keys,
                            ((object[]) entry.Get()).Select(o1 => (dynamic) Convert.ChangeType(o1, rowTypes[0]))
                                .ToArray()))
                    .Select(entry => (long) entry.Field(rowTypes.Length).Get());
            else
                return Root.Elements(diapason.start, diapason.numb)
                    .Select(entry => ((object[]) entry.Get()).Cast<int>().ToArray())
                    .SkipWhile(
                        row =>
                            CompareRows(keys,
                                row.Select(o1 => (dynamic) Convert.ChangeType(o1, rowTypes[0])).ToArray()) == -1)
                    .TakeWhile(
                        row =>
                            CompareRows(keys,
                                row.Select(o1 => (dynamic) Convert.ChangeType(o1, rowTypes[0])).ToArray()) == 0)
                    .Select(row => (long) row[row.Length - 1]);
        }
       /// <summary>
       /// Сравнение строк-массивов элементов простых типов. 
       /// </summary>
       /// <param name="keys"></param>
       /// <param name="row"></param>
       /// <returns></returns>
        private static int CompareRows(dynamic[] keys, dynamic[] row)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i] == row[i]) continue;
                if (keys[i] < row[i]) return 1;
                else return -1; // keys[i] < row[i]
            }
            return 0;
        }

        public IEnumerable<PaEntry> GetRowsByKey(dynamic[] keys)
        {
            if (keys.Length == 0)
                return Root.Elements();
            var entryRow = table.Element(0);
            return GetRowsOffsetsByKey(keys).Select(
                offset =>
                {
                    entryRow.offset = offset;
                    return entryRow;
                });
        }
    }

    internal class RowComperer : IComparer<object[]>
    {


        public int Compare(dynamic[] x, dynamic[] y)
        {
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] == y[i]) continue;
                if (x[i] > y[i]) return 1;
                else return -1; // keys[i] < row[i]
            }
            return 0;
        }
    }

    public static class FlowList
    {
        public static List<T> AddAndThen<T>(this List<T> list, T element)
        {
                      list.Add(element);
            return list;
        }
           public static List<T> AddRangeAndThen<T>(this List<T> list, IEnumerable<T> elements)
        {
                      list.AddRange(elements);
            return list;
        }
    }
}
