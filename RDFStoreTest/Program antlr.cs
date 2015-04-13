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
    class Program_antl
    {
        private static int Millions = 1;

       
        static void Main1(string[] args)
        {
         
            Console.WriteLine((Millions = 1)+"M");
            PaCell spoTable=new PaCell(new PTypeSequence(new PTypeRecord(new NamedType("s", new PType(PTypeEnumeration.sstring)),
               new NamedType("p", new PType(PTypeEnumeration.sstring)),
               new NamedType("o", ObjectVariantsPolarType.ObjectVariantPolarType))),
            "../../Database/spo full strings.pac", false);
            spoTable.Clear();
            spoTable.Fill(new object[0]);
                          Stopwatch timer=new Stopwatch(); 
            bool load = true;
            if (load)
            {
                timer.Restart();
                //using (StreamReader file = new StreamReader(@"D:\home\FactographDatabases\dataset\dataset" + Millions + "M.ttl")) // нужен путь к файлам 1M.ttl 10M.ttl  ...
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