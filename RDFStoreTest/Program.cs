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
            bool load = false;
            string view;
            if (load)
            {
                timer.Restart();
               var query = RDFStoreTest.Turtle.LoadGraph(@"C:\deployed\" + Millions + "M.ttl");
               //  var query = Turtle.LoadGraph(@"D:\home\FactographDatabases\dataset\dataset1M.ttl");
                VeryEasyNametable ven = new VeryEasyNametable();
                foreach (var triple in query)
                {
                    int sCode = ven.InsertOne(triple.subject);
                    int pCode = ven.InsertOne(triple.predicate);
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

            // ========== Альтернативный ввод данных ===========
            bool toload2 = true;
            if (toload2)
            {
                // Формируемая таблица имен
                NameTable nt = new NameTable("../../../Databases/");
                // Множество идентификаторов, составляющих порцию
                HashSet<string> hs = new HashSet<string>();
                // Рзамер порции для буферизации
                int nportion = 100000;
                // Главное действующее лицо - буфер. Буферизирует обрабатываемые триплеты 
                Polar.Common.BufferredProcessing<VariantsTriple> buffer = new Polar.Common.BufferredProcessing<VariantsTriple>(nportion,
                    triples =>
                    {
                        // Массив для передаче в процедуру кодирования порции
                        string[] arr = hs.ToArray();
                        Array.Sort<string>(arr);
                        nt.InsertPortion(arr);
                        foreach (var triple in triples)
                        {
                            int sCode = nt.GetCodeByString(triple.subject);
                            int pCode = nt.GetCodeByString(triple.predicate);
                            if (triple.Object.Variant == ObjectVariantEnum.Iri)
                            {
                                int code = nt.GetCodeByString((string)(triple).Object.WritableValue);
                                spoTable.Root.AppendElement(new object[] { sCode, pCode, new OV_iriint(code).ToWritable() });
                            }
                            else if (triple.Object.Variant == ObjectVariantEnum.Other)
                            {
                                var ovTyped = ((OV_typed)triple.Object);
                                int code = nt.GetCodeByString(ovTyped.turi);
                                var ovTypedint = new OV_typedint(ovTyped.value, code);
                                var writable = ovTypedint.ToWritable();
                                spoTable.Root.AppendElement(new object[] { sCode, pCode, writable });
                            }
                            else
                            {
                                spoTable.Root.AppendElement(new object[] { sCode, pCode, triple.Object.ToWritable() });
                            }
                        }
                        // Обнуление HashSet
                        hs.Clear();
                    });

                // Основной процесс: триплеты складываются в буфер, а их uri добавляются в HashSet
                var query = Turtle.LoadGraph(@"D:\home\FactographDatabases\dataset\dataset1M.ttl");
                foreach (var triple in query)
                {
                    hs.Add(triple.subject);
                    hs.Add(triple.predicate);
                    if (triple.Object.Variant == ObjectVariantEnum.Iri)
                    {
                        hs.Add((string)(triple).Object.WritableValue);
                    }
                    else if (triple.Object.Variant == ObjectVariantEnum.Other)
                    {
                        var ovTyped = ((OV_typed)triple.Object);
                        hs.Add(ovTyped.turi);
                    }
                    // Главное - запись триплета в буфер
                    buffer.Add(triple);
                }
                buffer.Flush();
                spoTable.Flush();
            }


            timer.Restart();
            Index sIndex=new Index(path,"s.pac", spoTable.Root);
            sIndex.Build();
            timer.Stop();
            view = "build s " + timer.ElapsedMilliseconds + "ms.";
            File.WriteAllText("../../perfomance.txt", view);
            Console.WriteLine(view);
            Console.WriteLine(spoTable.Root.Count());
            //foreach (var element in spoTable.Root.Elements())
            //{
               //yt hf,jnfk  
            //}
            object[] rows = spoTable.Root.ElementValues().ToArray();
            foreach (var row in rows)
            {
                var rowObj = (object[]) row;
                var resultsRows = sIndex.Search1((int) rowObj[0]).Cast<object[]>().ToArray();
                if (!resultsRows.Any()) throw new Exception();
                if (resultsRows.Any(o => !o[0].Equals(rowObj[0]))) throw new Exception();
                IEnumerable<object> resultsRows1 = sIndex.Search2((int) rowObj[0], (int) rowObj[1]);
                if (!resultsRows1.Any()) throw new Exception();
                IEnumerable<object> resultsRows2 = sIndex.Search3((int) rowObj[0], (int) rowObj[1],
                    ObjectVariantsEx.Writeble2Comparable((object[]) rowObj[2]));
                if (!resultsRows2.Any()) throw new Exception();
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
