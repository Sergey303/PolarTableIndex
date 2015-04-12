using SparqlParseRun;
using SparqlParseRun.RdfCommon;
using System;
using System.Collections.Generic;
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
         
         
            bool load = true;
            if (load)
            {
                using (StreamReader file = new StreamReader(@"D:\home\FactographDatabases\dataset\dataset" + Millions + "M.ttl")) // нужен путь к файлам 1M.ttl 10M.ttl  ...
                    TurtleParserFullstringsThread.TurtleThread(file.BaseStream,(s, p, o) =>
                    {
                        if (o.Tag == 0)
                        {
                            var iri = ((string)o.Content);
                        }
                        else if(o.Tag==12)
                        {
                            var typedLiteralType = ((object[])o.Content)[1];
                          //  Console.WriteLine(typedLiteralType);
                        }
                    }); 
              
            }
         
        }

    }
}