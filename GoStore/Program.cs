using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStore
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("Start GoStore.");
            string path = "../../../Databases/";
            var query = Turtle.LoadGraph(@"D:\home\FactographDatabases\dataset\dataset1M.ttl");
            VeryEasyNametable ven = new VeryEasyNametable();
            foreach (var triple in query)
            {
                ven.InsertOne(triple.subj);
                ven.InsertOne(triple.pred);
                if (triple is OTriple) ven.InsertOne(((OTriple)triple).obj);
            }
            
            Console.WriteLine("Load ok. count={0}", ven.Count());

        }
    }
    public class VeryEasyNametable
    {
        private Dictionary<string, int> dic = new Dictionary<string, int>();
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

        private void Dummy()
        {
//new PTypeUnion(
//                new NamedType("uri", new PType(PTypeEnumeration.sstring)),
//                new NamedType("bool", new PType(PTypeEnumeration.boolean)),
//                new NamedType("str", new PType(PTypeEnumeration.sstring)),
//                new NamedType("str", new PType(PTypeEnumeration.sstring)),
//                new NamedType("lang str", 
//                    new PTypeRecord(
//                        new NamedType("str", new PType(PTypeEnumeration.sstring)), 
//                        new NamedType("lang", new PType(PTypeEnumeration.sstring)))),
//                new NamedType("double", new PType(PTypeEnumeration.real)),
//                new NamedType("decimal", new PType(PTypeEnumeration.real)),
//                new NamedType("float", new PType(PTypeEnumeration.real)),
//                new NamedType("int", new PType(PTypeEnumeration.integer)),
//                new NamedType("date time offset", new PType(PTypeEnumeration.longinteger)),
//                new NamedType("date time", new PType(PTypeEnumeration.longinteger)),
//                new NamedType("time", new PType(PTypeEnumeration.longinteger)),
//                new NamedType("typed", 
//                    new PTypeRecord(
//                        new NamedType("str", new PType(PTypeEnumeration.sstring)), 
//                        new NamedType("type", new PType(PTypeEnumeration.sstring)))))        
        }
    }


}
