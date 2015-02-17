using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolarDB;

namespace PolarTableIndex
{
    class Program
    {
        private static PaCell table;
        private static PaCell index;
        private static int maxIndex = 100;
        private static Func<string, int> hkey;

        static void Main(string[] args)
        {
            table = new PaCell(new PTypeSequence(new PTypeRecord(
                new NamedType("deleted", new PType(PTypeEnumeration.boolean)),
                new NamedType("name", new PType(PTypeEnumeration.sstring)),
                new NamedType("age", new PType(PTypeEnumeration.integer)))),
                "../../table.pac", false);
            table.Clear();
            table.Fill(new object[0]);
            for (int i = 0; i < maxIndex; i++)
            {
                table.Root.AppendElement(new object[] { false, i.ToString(), i});
            }
            
             table.Flush();
            index=new PaCell(new PTypeSequence(new PTypeRecord(new NamedType("hkey", new PType(PTypeEnumeration.integer)), new NamedType("offset",new PType(PTypeEnumeration.longinteger)))),"../../index.pac",false );

            
            hkey = str => str.GetHashCode();;
            index.Clear();
            index.Fill(new object[0]) ;

         
                //table.Root.Scan((offset, row) =>
                //{
                //index.Root.AppendElement(new object[] {hkey(row), offset});
                    
                //});
            for (int i = 0; i < maxIndex; i++)
            {
                var row = table.Root.Element(i);

                index.Root.AppendElement(new object[] {hkey(row.Field(1).Get().ToString()), row.offset});
            }
             table.Flush();

             var entry = table.Root.Element(0);
             for (int i = 0; i < maxIndex; i++)
            {
                long offset = (long) index.Root.Element(i).Field(1).Get();
                entry.offset = offset;
                Console.WriteLine(entry.Field(1).Get());
            }
           
            var random = new Random();
                 var name=random.Next(maxIndex).ToString();

            PaEntry primarySeachByName;
            PaEntry indexSeachByName;

            Console.WriteLine(name);
             Stopwatch timer=new Stopwatch();
            //timer.Start();
            //primarySeachByName = PrimarySeachByName(name);
            //timer.Stop();
            //Console.WriteLine("primarySeachByName "+timer.Elapsed.TotalMilliseconds+"ms "+ primarySeachByName.Field(1).Get().ToString());
            
            timer.Reset();
            indexSeachByName = IndexSeachByName(name);
            timer.Stop();
            Console.WriteLine("indexSeachByName "+timer.Elapsed.TotalMilliseconds+"ms "+ indexSeachByName.Field(1).Get().ToString());

        }

        //static PaEntry PrimarySeachByName(string name)
        //{      
        //      return table.Root.BinarySearchFirst(entry => entry.Field(1).Get().ToString().CompareTo(name));

        //}
        static PaEntry IndexSeachByName(string name)
        {
            var searchedHkey = hkey(name);

            var allHkeyRows = index.Root.BinarySearchAll(entry =>
            {
                var readedHkey = (int) entry.Field(0).Get();
                return readedHkey > searchedHkey ? -1 : readedHkey==searchedHkey ? 0 :1;
            });
            PaEntry row0 = table.Root.Element(0);
            foreach (var offset in allHkeyRows.Select(entry => entry.Field(1).Get()).Cast<long>())
            {
                row0.offset = offset;
                var findedName = row0.Field(1).Get().ToString();

                if (findedName.Equals(name))
                    return row0;
            }
            return PaEntry.Empty;                
        }
    }
}
