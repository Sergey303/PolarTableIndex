using System;
using System.Diagnostics;
using System.Linq;
using PolarDB;

namespace PolarTableIndex
{
    class Program
    {
        private static PaCell table;
        static void Main(string[] args)
        {
            table = new PaCell(new PTypeSequence(new PTypeRecord(
            new NamedType("deleted", new PType(PTypeEnumeration.boolean)),
            new NamedType("name", new PType(PTypeEnumeration.sstring)),
            new NamedType("age", new PType(PTypeEnumeration.integer)))),
            "../../table.pac", false);
            table.Clear();
            table.Fill(new object[0]);
            for (int i = 0; i < 2000; i++)
            {
                for (int j = 0; j < i;j++)
                    table.Root.AppendElement(new object[] { false, i.ToString(), i });
            }

            table.Flush();
            var index=new IndexWithExceptionalAndScale("../../", table, o => (int) ((object[]) o)[2], fullkey => fullkey/1000, 1000);
           // Console.WriteLine( string.Join(" ", index.GetRowsByKey(50).Select(entry => entry.Field(2).Get())));
           // Console.WriteLine(string.Join(" ",index.GetRowsByKey(1001).Select(entry => entry.Field(2).Get())));
            Console.WriteLine(string.Join(" ", index.GetRowsByDiapasonsOfKeys(-1001, 1).Select(entry => entry.Field(2).Get())));
        }

       
    }
}
