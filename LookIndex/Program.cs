using System;
using System.Linq;
using IndexCommon;
using PolarDB;



namespace LookIndex
{
    public class Program
    {
        public static void Main()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            string path = "../../../Databases/";
            PaCell table;
            int maxCount = 1000000;
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
                table.Root.AppendElement(new object[] { false, i.ToString(), i == maxCount / 2 ? -1 : i });
            }

            table.Flush();
            sw.Stop();
            Console.WriteLine("Load ok. Duration={0}", sw.ElapsedMilliseconds);

            sw.Restart();

            IIndex<string> n_index;



            n_index = new GoIndex.Index<string>(path + "n_index", table.Root,  en => (string)en.Field(1).Get(), null /* key => key.GetHashCode()*/);
            //n_index = new PolarTableIndex.HalfKeyForIndex<string, int>(row => row[1].ToString(), s => s.GetHashCode(),PolarTableIndex.IndexConstructor.CreateInsideRecursive(path, table.Root, row => row[1].ToString().GetHashCode(), row => (bool)row[0] != true));// Index<string>(path + "n_index", table.Root, en => (string)en.Field(1).Get(), null /* key => key.GetHashCode()*/);
            
            
            
            n_index.Build();
            sw.Stop();
            Console.WriteLine("Index ok. Duration={0}", sw.ElapsedMilliseconds);

            PType pType = table.Root.Element(0).Type;
            foreach (var en in n_index.GetAllReadedByKey((maxCount / 2).ToString()))
            {
                Console.WriteLine(pType.Interpret(en));
            }
            sw.Restart();
            Random rnd = new Random();
            int cnt = 0;
            for (int i = 0; i < 1000; i++)
            {
                int r = rnd.Next(maxCount * 2);
                int c = n_index.GetAllReadedByKey(r.ToString()).Count();
                if (c > 1) Console.WriteLine("Unexpected Error: {0}", c);
                cnt += c;
            }
            sw.Stop();
            Console.WriteLine("1000 GetAllByKey ok. Duration={0} cnt={1}", sw.ElapsedMilliseconds, cnt);


            sw.Restart();
            IIndex<int> a_index;

            a_index = new GoIndex.Index<int>(path + "a_index", table.Root, en => (int)en.Field(2).Get(), null);
            a_index.Build();
            sw.Stop();
            Console.WriteLine("int Index ok. Duration={0}", sw.ElapsedMilliseconds);

            sw.Restart();
            rnd = new Random();
            cnt = 0;
            for (int i = 0; i < 1000; i++)
            {
                int r = rnd.Next(maxCount * 2);
                int c = a_index.GetAllReadedByKey(r).Count();
                if (c > 1) Console.WriteLine("Unexpected Error: {0}", c);
                cnt += c;
            }
            sw.Stop();
            Console.WriteLine("1000 GetAllByKey INT ok. Duration={0} cnt={1}", sw.ElapsedMilliseconds, cnt);
        }
    }
}
