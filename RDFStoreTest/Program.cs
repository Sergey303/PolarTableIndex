using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using PolarDB;

namespace RDFStoreTest
{
    public class Program
    {
        public static int Millions = 1;

        public static void Main()
        {
            Console.WriteLine("Start RDFStoreTest.");
            string path = "../../../Databases/";
            PaCell spoTable =
                new PaCell(new PTypeSequence(
                    new PTypeRecord(new NamedType("s", new PType(PTypeEnumeration.integer)),
                    //new PTypeRecord(new NamedType("s", new PType(PTypeEnumeration.sstring)),
                    new NamedType("p", 
                        new PType(PTypeEnumeration.integer)),
                        //new PType(PTypeEnumeration.sstring)),
                    new NamedType("o", ObjectVariantsPolarType.ObjectVariantPolarType))),
                    path + @"spo full strings.pac", false);
            spoTable.Clear();
            spoTable.Fill(new object[0]);
            Stopwatch timer = new Stopwatch();
            bool load = true;
            string view;
            if (load)
            {
                timer.Restart();
                //var query = RDFStoreTest.Turtle.LoadGraph(@"C:\deployed\" + Millions + "M.ttl");
                var query = Turtle.LoadGraph(@"D:\home\FactographDatabases\dataset\dataset1M.ttl");
                VeryEasyNametable ven = new VeryEasyNametable();
                foreach (var triple in query)
                {
                    int sCode = ven.InsertOne(triple.subj);
                    int pCode = ven.InsertOne(triple.pred);
                    if (triple.Object.Variant == ObjectVariantEnum.Iri)
                    {
                        int code = ven.InsertOne((string) (triple).Object.WritableValue);
                        //spoTable.Root.AppendElement(new object[] {triple.subj, triple.pred, new OV_iriint(code).ToWritable()});
                        spoTable.Root.AppendElement(new object[] {sCode, pCode, new OV_iriint(code).ToWritable()});
                    }
                    else if (triple.Object.Variant == ObjectVariantEnum.Other)
                    {
                        var ovTyped = ((OV_typed) triple.Object);
                        int code = ven.InsertOne(ovTyped.turi);
                        var ovTypedint = new OV_typedint(ovTyped.value, code);
                        var writable = ovTypedint.ToWritable();
                        spoTable.Root.AppendElement(new object[]    {sCode, pCode, writable});
                        //spoTable.Root.AppendElement(new object[]    {triple.subj, triple.pred, writable});
                    }
                    else
                    {
                        spoTable.Root.AppendElement(new object[] {sCode, pCode, triple.Object.ToWritable()});
                       // spoTable.Root.AppendElement(new object[] {triple.subj, triple.pred, triple.Object.ToWritable()});
                    }
                }
                timer.Stop();
                view = "load " + spoTable.Root.Count() + " " + timer.ElapsedMilliseconds + "ms.";
                File.WriteAllText("../../perfomance.txt", view);
                Console.WriteLine(view);
                Console.WriteLine("Load ok. count={0}", ven.Count());

            }
            timer.Restart();
            Index sIndex=new Index(path,"s.pac", spoTable.Root);
            sIndex.Build();
            timer.Stop();
            view = "build s " + timer.ElapsedMilliseconds + "ms.";
            File.WriteAllText("../../perfomance.txt", view);
            Console.WriteLine(view);
            foreach (var row in spoTable.Root.Elements())
            {
                IEnumerable<object> resultsRows = sIndex.Search((int) ((object[]) row.Get())[0]);
                if(!resultsRows.Any()) throw new Exception();
            }
        }

    }
    public class VeryEasyNametable
    {
        private readonly Dictionary<string, int> dic = new Dictionary<string, int>();
        private int nextcode = 0;
        public int InsertOne(string id)
        {
            int c;
            if (dic.TryGetValue(id, out c)) return c;
            c = nextcode;
            nextcode++;
            dic.Add(id, c);
            return c;
        }
        public Func<string, int> GetCodeByString;
        public VeryEasyNametable()
        {
            GetCodeByString = (string id) => dic[id];
        }
        public int Count()
        {
            return dic.Count;
        }
    }
}
