using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolarDB;

namespace PolarTableIndex
{
    class Program
    {
        static void Main(string[] args)
        {
            var table = new PaCell(new PTypeSequence(new PTypeRecord(
                new NamedType("deleted", new PType(PTypeEnumeration.boolean)),
                new NamedType("name", new PType(PTypeEnumeration.sstring)),
                new NamedType("age", new PType(PTypeEnumeration.integer)))),
                "../../table.pac", false);
            table.Clear();
            table.Fill(new object[0]);
            for (int i = 0; i < 100; i++)
            {
                table.Root.AppendElement(new object[] { false, i.ToString(), i});
            }
        }
    }
}
