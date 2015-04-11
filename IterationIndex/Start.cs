using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolarDB;

namespace IterationIndexLib
{
    class Start
    {
        private static PaCell table;
        private static Stopwatch sw;
        private static Random rnd = new Random(7777);
        public static void Main(string[] args)
        {
             sw = new Stopwatch();
            string path = "../../../Databases/";
            int maxCount = 1000000;
            bool build = true;
           CreatePaCell(path, sw,  maxCount);
            
          //IterationIndex index  =new IterationIndex(path+"iteration index.pac", table.Root, o =>
          //{
          //    var cells = (object[])o;
          //    return new Row(cells[2], cells[1]);
          //},2);
          //  if(build) index.Build();
          // RunTest(index, o =>
          //{
          //    var cells = (object[])o;
          //    return new Row(cells[2], cells[1]);
          //}, new Row(12, "12"), () =>
          //{
          //    int next = rnd.Next(maxCount);
          //    return new Row(next, next.ToString());
          //});
           var testi = new IterationIndexMultiplyValues(new DirectoryInfo(path + "itst"), table.Root, o =>
           {
               var cells = (object[])o;
               return new Row(cells[2], cells[1], cells[0], cells[2], cells[1], cells[0], cells[2], cells[1]);
           }, 8, 10);
            for (int i = 0; i < 100000; i++)
            {
            testi.Add(new Row(i/10,i.ToString(),false, i, (i/2).ToString(), false, i, "9"),0l );
               

            }
            testi.Add(new Row(3,"4",false, 5,"5", false, 6, "9"),1l );
            bool contains = testi.SearchOffsets(new Row(3,"4",false, 5,"5")).Contains(1l);
            Console.WriteLine(contains);
        }
        private static void CreatePaCell(string path, Stopwatch sw, int maxCount)
        {
          
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
                // table.Root.AppendElement(new object[] { false, rnd.Next().ToString(), rnd.Next() });
            }

            table.Flush();
            sw.Stop();
            Console.WriteLine("Load ok. Duration={0}", sw.ElapsedMilliseconds);
        }
        private static void RunTest(IterationIndexMultiplyValues indexMultiplyValues, Func<object, Row> getKeyFromRow, Row key,
          Func<Row> randomKeyProducer)
        {
            PType pType = table.Root.Element(0).Type;
            foreach (var en in indexMultiplyValues.Search(key))
            {
                Console.WriteLine(pType.Interpret(en));
            }
            sw.Restart();

            int cnt = 0;
            for (int i = 0; i < 1000; i++)
            {
                int c = indexMultiplyValues.Search(randomKeyProducer()).Count();
                if (c > 1) Console.WriteLine("Unexpected Error: {0}", c);
                cnt += c;
            }
            sw.Stop();
            Console.WriteLine("1000 GetAllByKey ok. Duration={0} cnt={1}", sw.ElapsedMilliseconds, cnt);

            sw.Restart();
            foreach (PaEntry entry in table.Root.Elements())
            {
                object[] row = (object[])entry.Get();
               var k = getKeyFromRow(row);
                IEnumerable<object[]> rows = indexMultiplyValues.Search(k).Cast<object[]>().ToArray();
                if (rows.All(objects => new Row(objects).CompareTo(new Row(row)) != 0))
                    throw new Exception(string.Join(" ", row) + "   in    " + rows.Count());
            }
            sw.Stop();
            Console.WriteLine("1000 000 GetAllReadedByKey with results comparer ok. Duration={0} ",
                sw.ElapsedMilliseconds);
        }
    }
}
