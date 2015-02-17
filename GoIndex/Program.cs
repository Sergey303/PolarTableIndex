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
            PaCell table;
            int maxCount = 100;
            table = new PaCell(new PTypeSequence(new PTypeRecord(
                new NamedType("deleted", new PType(PTypeEnumeration.boolean)),
                new NamedType("name", new PType(PTypeEnumeration.sstring)),
                new NamedType("age", new PType(PTypeEnumeration.integer)))),
                "../../table.pac", false);
            table.Clear();
            table.Fill(new object[0]);
            for (int i = 0; i < maxCount; i++)
            {
                table.Root.AppendElement(new object[] { false, i.ToString(), i == 77 ? -1 : i });
            }

            table.Flush();

            Index0<string> n_index = new Index0<string>("", table.Root, en => (string)en.Field(1).Get());

            foreach (var en in n_index.GetAllByKey("77"))
            {
                Console.WriteLine(en.Type.Interpret(en.Get()));
            }
        }
    }
}
