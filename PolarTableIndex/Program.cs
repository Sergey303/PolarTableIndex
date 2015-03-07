using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            var r = new Random();
            int j = 0;
            for (int i = 0; i < 1000 * 1000; i++)
            {
                j = r.Next(100);
                //   for (int j = 0; j < i;j++)
                    table.Root.AppendElement(new object[] { false, j.ToString(), j });
            }

            table.Flush();

            IndexInsideRecursive index=null;
           // Perfomance.ComputeTime(() =>
            {
                var dir = new DirectoryInfo("../../index");
                if(dir.Exists)
                dir.Delete(true);
                dir.Create();
                index = new IndexInsideRecursive(dir, new []{typeof(int), typeof(bool)}, 1000);
                index.Build(table.Root, new Func<object[], dynamic>[]{ o => (int)o[2], o => (bool)o[0]},  1000);                
            }//,"create ");
            // Console.WriteLine( string.Join(" ", index.GetRowsByKey(50).Select(entry => entry.Field(2).Get())));
            //PerformanceCounter c=new PerformanceCounter();
            PaEntry[] tests = null;
           // Perfomance.ComputeTime(() =>
            {
                tests = index.GetRowsByKey(new dynamic[]{j, false}).ToArray();
            }//, j+": ");

            Console.WriteLine();
            Console.WriteLine(!tests.FirstOrDefault().IsEmpty? tests.FirstOrDefault().Get() : "");
            Console.WriteLine(tests.Count());
        //    Console.WriteLine(string.Join(" ", tests.Select(entry => entry.Field(1).Get())));
            // Console.WriteLine(string.Join(" ", index.GetRowsByDiapasonsOfKeys(-1001, 1).Select(entry => entry.Field(2).Get())));
        }

       
    }
}
