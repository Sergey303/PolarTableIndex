using PolarDB;
using SparqlParseRun;
using SparqlParseRun.RdfCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDFStoreTest
{
    class Program
    {
        private static int Millions = 1;
        static void Main(string[] args)
        {
         
            Console.WriteLine((Millions = 1)+"M");
           PaCell spoTable=new PaCell(new PTypeSequence(new PTypeRecord(new NamedType("s", new PType(PTypeEnumeration.sstring)),
               new NamedType("p", new PType(PTypeEnumeration.sstring)),
               new NamedType("o", new PTypeUnion(
                   new NamedType("uri", new PType(PTypeEnumeration.sstring)),
                new NamedType("bool", new PType(PTypeEnumeration.boolean)),
                new NamedType("str", new PType(PTypeEnumeration.sstring)),
                new NamedType("str", new PType(PTypeEnumeration.sstring)),
                new NamedType("lang str", new PTypeRecord(new NamedType("str", new PType(PTypeEnumeration.sstring)), new NamedType("lang", new PType(PTypeEnumeration.sstring)))),
                new NamedType("double", new PType(PTypeEnumeration.real)),
                new NamedType("decimal", new PType(PTypeEnumeration.real)),
                new NamedType("float", new PType(PTypeEnumeration.real)),
                new NamedType("int", new PType(PTypeEnumeration.integer)),
                new NamedType("date time offset", new PType(PTypeEnumeration.longinteger)),
                new NamedType("date time", new PType(PTypeEnumeration.longinteger)),
                new NamedType("time", new PType(PTypeEnumeration.longinteger)),
                new NamedType("typed", new PTypeRecord(new NamedType("str", new PType(PTypeEnumeration.sstring)), new NamedType("type", new PType(PTypeEnumeration.sstring)))))))),
            "../../Database/spo full strings.pac", false);
            spoTable.Clear();
            spoTable.Fill(new object[0]);
                          Stopwatch timer=new Stopwatch(); 
            bool load = true;
            if (load)
            {
                timer.Restart();
                using (StreamReader file = new StreamReader(@"C:\deployed\" + Millions + "M.ttl")) // нужен путь к файлам 1M.ttl 10M.ttl  ...
                    TurtleParserFullstringsThread.TurtleThread(file.BaseStream,(s, p, o) =>
                    {
                       // spoTable.Root.AppendElement(new object []{s, p, o.AsArray});
                    });
                timer.Stop();
                var view = "load "+spoTable.Root.Count()+" "+timer.ElapsedMilliseconds+"ms.";
                File.WriteAllText("../../perfomance.txt",view);
                Console.WriteLine(view);
            }
         
        }

    }
}