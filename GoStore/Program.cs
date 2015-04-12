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
    }
}
