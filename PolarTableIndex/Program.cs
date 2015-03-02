using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CommonRDF;
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
            var r = new Random();
            int j = 0;
            for (int i = 0; i < 1000 * 1000; i++)
            {
                j = r.Next(10000);
                //   for (int j = 0; j < i;j++)
                    table.Root.AppendElement(new object[] { false, j.ToString(), j });
            }

            table.Flush();

            IndexWithExceptionalAndScale index=null;
            Perfomance.ComputeTime(() =>
            {
                index = new IndexWithExceptionalAndScale("../../", table, o => (int)((object[])o)[2], fullkey => fullkey / 1000, 1000);                
            },"create ");
            // Console.WriteLine( string.Join(" ", index.GetRowsByKey(50).Select(entry => entry.Field(2).Get())));
            //PerformanceCounter c=new PerformanceCounter();
            IEnumerable<object> tests = null;
            Perfomance.ComputeTime(() =>
            {
                tests = index.GetRowsByKey(j).Select(entry => entry.Field(2).Get()).ToArray();
            }, j+": ");

            Console.WriteLine();
            Console.WriteLine(string.Join(" ",tests));
            // Console.WriteLine(string.Join(" ", index.GetRowsByDiapasonsOfKeys(-1001, 1).Select(entry => entry.Field(2).Get())));
        }

       
    }
}
