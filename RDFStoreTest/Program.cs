using System;
using System.Collections.Generic;

namespace RDFStoreTest
{
    public class Program
    {
        public static int Millions = 1;
        public static void Main()
        {
            Console.WriteLine("Start RDFStoreTest.");
            string path = "../../../Databases/";
                
            //var query = RDFStoreTest.Turtle.LoadGraph(@"C:\deployed\" + Millions + "M.ttl");
            var query = Turtle.LoadGraph(@"D:\home\FactographDatabases\dataset\dataset1M.ttl");
            VeryEasyNametable ven = new VeryEasyNametable();
            foreach (var triple in query)
            {
                ven.InsertOne(triple.subj);
                ven.InsertOne(triple.pred);
                if (triple .Object.Variant==ObjectVariantEnum.Iri) ven.InsertOne((string) (triple).Object.WritableValue);
                else if (triple .Object.Variant==ObjectVariantEnum.Other)
                {
                    ven.InsertOne(((OV_typed) triple.Object).turi);
                }

            }
            
            Console.WriteLine("Load ok. count={0}", ven.Count());

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
