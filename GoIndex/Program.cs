using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolarDB;

namespace GoIndex
{
    public class Program
    {
        public static void Main()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            string path = "../../";
            PaCell table;
            int maxCount = 1000000;
            table = new PaCell(new PTypeSequence(new PTypeRecord(
                new NamedType("deleted", new PType(PTypeEnumeration.boolean)),
                new NamedType("name", new PType(PTypeEnumeration.sstring)),
                new NamedType("age", new PType(PTypeEnumeration.integer)))),
                "../../table.pac", false);
            Console.WriteLine("Start GoIndex");
            sw.Restart();
            table.Clear();
            table.Fill(new object[0]);
            for (int i = 0; i < maxCount; i++)
            {
                table.Root.AppendElement(new object[] { false, i.ToString(), i == maxCount/2 ? -1 : i });
            }

            table.Flush();
            sw.Stop();
            Console.WriteLine("Load ok. Duration={0}", sw.ElapsedMilliseconds);

            sw.Restart();
            //Index0<string> n_index = new Index0<string>("", table.Root, en => (string)en.Field(1).Get());
            IndexKeyView<string> n_index = new IndexKeyView<string>(path + "n_index", table.Root,
                en => (string)en.Field(1).Get(), key => key.GetHashCode());
            n_index.Build();
            sw.Stop();
            Console.WriteLine("Index ok. Duration={0}", sw.ElapsedMilliseconds);

            foreach (var en in n_index.GetAllByKey((maxCount/2).ToString()))
            {
                Console.WriteLine(en.Type.Interpret(en.Get()));
            }
            sw.Restart();
            Random rnd = new Random();
            int cnt = 0;
            for (int i = 0; i < 1000; i++)
            {
                int r = rnd.Next(maxCount * 2);
                int c = n_index.GetAllByKey(r.ToString()).Count();
                if (c > 1) Console.WriteLine("Unexpected Error: {0}", c);
                cnt += c;
            }
            sw.Stop();
            Console.WriteLine("1000 GetAllByKey ok. Duration={0} cnt={1}", sw.ElapsedMilliseconds, cnt);
        }
    }
}
