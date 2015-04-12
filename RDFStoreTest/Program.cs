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
            // путь можно указать программно, можно в настройках   storeSettings.settings и NameTableSettings.settings
            //RdfStoreSparqlNamespace.storeSettings.Default.root_dir_path = "../../store";            .
            //RdfStoreSparqlNamespace.NameTableSettings.Default.path = "../../name table";

            Console.WriteLine((Millions = 1)+"M");
            RdfStoreSparql ts = new RdfStoreSparql();
         
            bool load = true;
            if (load)
            {
                using (StreamReader file = new StreamReader(@"C:\deployed\" + Millions + "M.ttl")) // нужен путь к файлам 1M.ttl 10M.ttl  ...
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
                    //ts.ReCreateFrom(file.BaseStream);
                // в этом методе я закоментировал построение индексов при добавлении триплетов.
            }
            //  ts.store- объект хранилища с таблицей, индексами, поисками и добавлениями.      Есть 4 варианта    класса для этого объекта:            
            // неопробованы: RDFStoreStringsPrefixedQuads  RDFStoreQuadsCoded ( с CodedNodeGenerator или CodedPrefixedNodeGenerator ).
            // RDFStoreStringsQuads   - этот класс сейчас используется.

            //  ts.store.spogdTable - таблица
          //  Console.WriteLine(ts.store.spogdTable.Root.Count()+" triples");
        }

    }
}