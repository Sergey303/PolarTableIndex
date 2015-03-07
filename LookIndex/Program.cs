using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonRDF;
using GoIndex;
using PolarDB;
using PolarTableIndex;

namespace LookIndex
{
    class Program
    {
       
        static void Main(string[] args)
        {
            Func<string, IEnumerable<PaEntry>> getAllByKey;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            string path = "../../";
            PaCell table;
            int maxCount = 1000000;
            table = new PaCell(new PTypeSequence(new PTypeRecord(
                new NamedType("deleted", new PType(PTypeEnumeration.boolean)),
                new NamedType("name", new PType(PTypeEnumeration.sstring)),
                new NamedType("age", new PType(PTypeEnumeration.integer)))),
                "../../table.pac", false);
            Console.WriteLine("Start ");
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

            getAllByKey = CreateInsideRecursiveIndexGetAllBy(table);


          //  getAllByKey = CreateIndexKeyViewGetAllByKey(path, table);

            sw.Stop();
            Console.WriteLine("Index ok. Duration={0}", sw.ElapsedMilliseconds);

            foreach (var en in getAllByKey((maxCount / 2).ToString()))
            {
                Console.WriteLine(en.Type.Interpret(en.Get()));
            }
            sw.Restart();
            Random rnd = new Random();
            int cnt = 0;
            for (int i = 0; i < 1000; i++)
            {
                int r = rnd.Next(maxCount * 2);
                var key = r.ToString();
                int c = getAllByKey(key).Count();
                if (c > 1) Console.WriteLine("Unexpected Error: {0}", c);
                cnt += c;
            }
            sw.Stop();
            Console.WriteLine("1000 GetAllByKey ok. Duration={0} cnt={1}", sw.ElapsedMilliseconds, cnt);

        }

        private static Func<string, IEnumerable<PaEntry>> CreateInsideRecursiveIndexGetAllBy(PaCell table)
        {


            var dir = new DirectoryInfo("../../index");
            if (dir.Exists)
                dir.Delete(true);
            dir.Create();
            var index = new IndexInsideRecursive(dir, new[] { typeof(int) }, 1000);
            index.Build(table.Root, new Func<object[], dynamic>[] {o => ((string) o[1]).GetHashCode()}, 1000);
         return  key => index.GetRowsByKey(new[] {(dynamic) key.GetHashCode()}).Where(entry =>
            {
                var o = (object[]) entry.Get();
                return (bool) o[0] != true && (string) o[1] == key;
            });
        }

        private static Func<string, IEnumerable<PaEntry>> CreateIndexKeyViewGetAllByKey(string path, PaCell table)
        {
            IndexKeyView<string> n_index = new IndexKeyView<string>(path + "n_index", table.Root,
                en => (string) en.Field(1).Get(), null /* key => key.GetHashCode()*/);
            n_index.Build();
            return n_index.GetAllByKey;
        }
    }
}
