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
            Console.WriteLine("Load ok. ntriples={0}", query.Count());
        }
    }
}
