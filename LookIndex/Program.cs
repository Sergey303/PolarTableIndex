using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GoIndex;
using IndexCommon;
using PolarDB;
using PolarTableIndex;


namespace LookIndex
{
    public class Program
    {
        private static PaCell table;
        private static Stopwatch sw;
        private static Random rnd = new Random();


        public static void Main()
        {
            sw = new Stopwatch();
            string path = "../../../Databases/";
            int maxCount = 1000000;
            bool build = true;
            table = CreatePaCell(path, sw,  maxCount);
            sw.Stop();
            Console.WriteLine("Load ok. Duration={0}", sw.ElapsedMilliseconds);

            sw.Restart();

            Console.WriteLine("start string halfkey=GetHashCode index_withScale");
            IIndex<string> index;

            index = new IndexWithScale<string>(path + "index_withScale1", table.Root, en => (string) en[1],
                key => key.GetHashCode(), true);
            if (build) { sw.Restart(); index.Build(); sw.Stop(); Console.WriteLine("biuld " + sw.ElapsedMilliseconds); }
            RunTest<string>((IIndex<string>)index, row => row[1].ToString(), (maxCount / 2).ToString(), () => rnd.Next(maxCount * 2).ToString());

            //Console.WriteLine("start string key index_withScale");
            //index = new IndexWithScale<string>(path + "index_withScale2", table.Root, en => (string)en[1],
            //    null, true);
            //if (build) { sw.Restart(); index.Build(); sw.Stop(); Console.WriteLine("biuld " + sw.ElapsedMilliseconds); }
            //RunTest<string>((IIndex<string>)index, row => row[1].ToString(), (maxCount / 2).ToString(), () => rnd.Next(maxCount * 2).ToString());


            Console.WriteLine("start int key index_withScale");
            IIndex<int> index1 = new IndexWithScale<int>(path + "index_withScale3", table.Root, en => (int)en[2], null, true);
            if (build) { sw.Restart(); index1.Build(); sw.Stop(); Console.WriteLine("biuld " + sw.ElapsedMilliseconds); }
             RunTest<int>(index1, row => (int)row[2], (maxCount / 2), () => rnd.Next(maxCount * 2));  
          //   return;
            index = new GoIndex.Index<string>(path + "n_index", table.Root, en => (string)en[1], key => key.GetHashCode());
            if (build) { sw.Restart(); index.Build(); sw.Stop(); Console.WriteLine("biuld " + sw.ElapsedMilliseconds); }
            if (build) { sw.Restart(); ((GoIndex.Index<string>)index).Build2(); sw.Stop(); Console.WriteLine("biuld2 " + sw.ElapsedMilliseconds); }
            RunTest<string>((IIndex<string>) index, row => row[1].ToString(), (maxCount / 2).ToString(), () => rnd.Next(maxCount * 2).ToString());


            Console.WriteLine("start string key GoIndex");
            index = new GoIndex.Index<string>(path + "n_index2", table.Root, en => (string)en[1], null /* key => key.GetHashCode()*/);
            if (build) { sw.Restart(); index.Build(); sw.Stop(); Console.WriteLine("biuld " + sw.ElapsedMilliseconds); }
            if (build) { sw.Restart(); index.Build2(); sw.Stop(); Console.WriteLine("biuld2 " + sw.ElapsedMilliseconds); }
            RunTest<string>((IIndex<string>) index, row => row[1].ToString(), (maxCount / 2).ToString(), () => rnd.Next(maxCount * 2).ToString());


                Console.WriteLine("start int key GoIndex");
            index1 = new GoIndex.Index<int>(path + "n_index_ints1", table.Root, en => (int)en[2], null /* key => key.GetHashCode()*/);
           if (build) { sw.Restart(); index1.Build(); sw.Stop(); Console.WriteLine("biuld " + sw.ElapsedMilliseconds); }
           if (build) { sw.Restart(); ((GoIndex.Index<int>)index1).Build2(); sw.Stop(); Console.WriteLine("biuld2 " + sw.ElapsedMilliseconds); }


           RunTest<int>(index1, row => (int)row[2], (maxCount / 2), () => rnd.Next(maxCount * 2));  

        }

        private static void RunTest<T>(IIndex<T> index, Func<object[], T> getKeyFromRow, T key,
            Func<T> randomKeyProducer) where T : IComparable
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
            Console.WriteLine("1000 GetAllByKey ok. Duration={0} cnt={1}", sw.ElapsedMilliseconds, cnt);

            //sw.Restart();
            //foreach (PaEntry entry in table.Root.Elements())
            //{
            //    object[] row = (object[])entry.Get();
            //    T k = getKeyFromRow(row);
            //    IEnumerable<object[]> rows = index.GetAllReadedByKey(k).ToArray();
            //    if (!rows.Any(objects => Enumerable.Range(0, row.Length).All(i => row[i].Equals(objects[i]))))
            //        throw new Exception(string.Join(" ", row) + "   in    " + rows.Count());
            //}
            //sw.Stop();
            //Console.WriteLine("1000 000 GetAllReadedByKey with results comparer ok. Duration={0} ",
            //    sw.ElapsedMilliseconds);
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
               // table.Root.AppendElement(new object[] { false, rnd.Next().ToString(), rnd.Next() });
            }

            table.Flush();
            return table;
        }

        public static object Index { get; set; }
    }
}

