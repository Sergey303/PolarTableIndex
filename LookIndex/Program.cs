using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using IndexCommon;
using PolarDB;



namespace LookIndex
{
    public class Program
    {
        private static PaCell table;
        private static Stopwatch sw;
        private static Random rnd = new Random();
        //private static IIndex<dynamic> index;

        public static void Main()
        {
            sw = new Stopwatch();
            string path = "../../../Databases/";
            int maxCount = 1000000;
            table = CreatePaCell(path, sw,  maxCount);
            sw.Stop();
            Console.WriteLine("Load ok. Duration={0}", sw.ElapsedMilliseconds);

            sw.Restart();

            Console.WriteLine("start string half key GoIndex");
           IIndex<string> index_s = new GoIndex.Index<string>(path + "n_index", table.Root,  en => (string)en.Field(1).Get(), null /* key => key.GetHashCode()*/);
           index_s.Build();
        //   RunTest(index_s, row => (string)row[1], "16780", () => rnd.Next(maxCount * 2).ToString()); //(maxCount / 2).ToString(),

           Console.WriteLine("start string half key IndexWithScale");
          index_s = new PolarTableIndex.IndexWithScale<string>(path + "n_index with scale", table.Root, en => (string)en.Field(1).Get(), key => key.GetHashCode(), true);
           index_s.Build();
           RunTest(index_s, row => (string)row[1], "16780", () => rnd.Next(maxCount * 2).ToString()); //(maxCount / 2).ToString(),
            //Console.WriteLine("start string half key HalfKeyForIndex InsideRecursive");
            //index_s = new PolarTableIndex.HalfKeyForIndex<string, int>(row => row[1].ToString(), s => s.GetHashCode(), PolarTableIndex.IndexConstructor.CreateInsideRecursive(path + "strings", table.Root, row => row[1].ToString().GetHashCode(), row => (bool)row[0] != true));// Index<string>(path + "n_index", table.Root, en => (string)en.Field(1).Get(), null /* key => key.GetHashCode()*/);
            //index_s.Build();
            //RunTest(index_s, row => (string)row[1], "16780", () => rnd.Next(maxCount * 2).ToString());

            Console.WriteLine("start int key GoIndex");
           IIndex<int> index = new GoIndex.Index<int>(path + "n_index_ints1", table.Root, en => (int)en.Field(2).Get(), null /* key => key.GetHashCode()*/);
            index.Build();
            RunTest(index, row => (int)row[2], (maxCount / 2), () => rnd.Next(maxCount * 2));

            //Console.WriteLine("start int InsideRecursive");
            //index = PolarTableIndex.IndexConstructor.CreateInsideRecursive<int>(path + "ints1", table.Root, row => (int)row[2], row => (bool)row[0] != true);
            //index.Build();
            //RunTest(index, row => (int)row[2], (maxCount / 2), () => rnd.Next(maxCount * 2));

            Console.WriteLine("start key = int+ str.gethash GoIndex");
            index = new GoIndex.Index<int>(path + "n_index_ints2", table.Root, en => (int)en.Field(2).Get() + ((string)en.Field(1).Get()).GetHashCode(), null /* key => key.GetHashCode()*/);
            index.Build();
            RunTest(index, row => (int)row[2]+row[1].ToString().GetHashCode(), (maxCount / 2), () => rnd.Next(maxCount * 2));

            //Console.WriteLine("start key = int+ str.gethash InsideRecursive");
            //index = PolarTableIndex.IndexConstructor.CreateInsideRecursive<int>(path + "ints2", table.Root, row => (int)row[2] + ((string)row[1]).GetHashCode(), row => (bool)row[0] != true);
            //index.Build();
            //RunTest(index, row => (int)row[2] + row[1].ToString().GetHashCode(), (maxCount / 2), () => rnd.Next(maxCount * 2));

            sw.Stop();
            Console.WriteLine("Index ok. Duration={0}", sw.ElapsedMilliseconds);


        }

        private static void RunTest<T>(IIndex<T> index, Func<object[], T> getKeyFromRow, T key, Func<T> randomKeyProducer) where T : IComparable
        {
            PType pType = table.Root.Element(0).Type;
            foreach (var en in index.GetAllReadedByKey(key))
            {
                Console.WriteLine(pType.Interpret(en));
            }
            sw.Restart();

            int cnt = 0;
            for (int i = 0; i < 1000; i++)
            {
                int c = index.GetAllReadedByKey(randomKeyProducer()).Count();
                if (c > 1) Console.WriteLine("Unexpected Error: {0}", c);
                cnt += c;
            }
            sw.Stop();
            Console.WriteLine("1000 GetAllReadedByKey ok. Duration={0} cnt={1}", sw.ElapsedMilliseconds, cnt);

            sw.Restart();
            foreach (PaEntry entry in table.Root.Elements())
            {
                object[] row = (object[])entry.Get();
                T k = getKeyFromRow(row);
                IEnumerable<object[]> rows = index.GetAllReadedByKey(k).ToArray();
                if (!rows.Any(objects => Enumerable.Range(0, row.Length).All(i => row[i].Equals(objects[i])))) throw new Exception(string.Join(" ", row) + "   in    " + rows.Count());
            }
            sw.Stop();
            Console.WriteLine("1000 000 GetAllReadedByKey with results comparer ok. Duration={0} cnt={1}", sw.ElapsedMilliseconds, cnt);
        }

        private static PaCell CreatePaCell(string path, Stopwatch sw, int maxCount)
        {
            PaCell table;
         
            table = new PaCell(new PTypeSequence(new PTypeRecord(
                new NamedType("deleted", new PType(PTypeEnumeration.boolean)),
                new NamedType("name", new PType(PTypeEnumeration.sstring)),
                new NamedType("age", new PType(PTypeEnumeration.integer)))),
                path + "table.pac", false);
            Console.WriteLine("Start");
            sw.Restart();
            table.Clear();
            table.Fill(new object[0]);
            for (int i = 0; i < maxCount; i++)
            {
                table.Root.AppendElement(new object[] {false, i.ToString(), i == maxCount/2 ? -1 : i});
            }

            table.Flush();
            return table;
        }
    }
}
