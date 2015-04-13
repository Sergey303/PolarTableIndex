using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolarDB;

namespace Task15UniversalIndex
{
    public class Program
    {
        public static void Main() { Main7(); }
        public static void Main7()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            string path = "../../../Databases/";
            int NumberOfRecords = 100000000;
            Random rnd = new Random();
            Console.WriteLine("Start Universal Index. Main7()");

            PType tp_table_element = new PTypeRecord(
                new NamedType("name", new PType(PTypeEnumeration.sstring)),
                new NamedType("age", new PType(PTypeEnumeration.integer)));
            TableView table = new TableView(path + "table", tp_table_element);
            sw.Restart();
            bool tobuild = false;
            if (tobuild)
            {
                table.Fill(Enumerable.Range(0, NumberOfRecords).Select(i =>
                    (object)(new object[] { i.ToString(), i == NumberOfRecords / 2 ? -1 : i })));
                sw.Stop();
                Console.WriteLine("Load Table of {0} elements ok. Duration {1}", NumberOfRecords, sw.ElapsedMilliseconds);
            }
            else table.Warmup();
            sw.Restart();
            // Делаем индекс
            var ha = new IndexHalfkeyImmutable<string>(path + "dyna_index_str_half")
            {
                Table = table,
                KeyProducer = va => (string)((object[])(((object[])va)[1]))[0],
                HalfProducer = k => k.GetHashCode()
            };
            ha.Scale = new ScaleCell(path + "dyna_index_str_half") { IndexCell = ha.IndexCell };
            bool tobuild_h_index = false;
            if (tobuild_h_index)
            {
                ha.Build();
            }
            else
            {
                ha.Warmup();
                //ha.BuildScale();
            }
            IndexDynamic<string, IndexHalfkeyImmutable<string>> h_index = new IndexDynamic<string, IndexHalfkeyImmutable<string>>(true)
            {
                Table = table,
                KeyProducer = va => (string)((object[])(((object[])va)[1]))[0],
                IndexArray = ha
            };
            Console.WriteLine("h_index Build ok. Duration {0}", sw.ElapsedMilliseconds);
            sw.Restart();
            int cnt = 0;
            for (int i = 0; i < 1000; i++)
            {
                int c = h_index.GetAllByKey(rnd.Next(NumberOfRecords * 3 / 2 - 1).ToString()).Count();
                if (c > 1) Console.WriteLine("Unexpected Error: {0}", c);
                cnt += c;
            }
            sw.Stop();
            Console.WriteLine("1000 GetAllByKey ok. Duration={0} cnt={1}", sw.ElapsedMilliseconds, cnt);
        }
        public static void Main6()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            string path = "../../../Databases/";
            int NumberOfRecords = 5000000;
            Random rnd = new Random();
            Console.WriteLine("Start Universal Index. Main6()");

            PType tp_table_element = new PTypeRecord(
                new NamedType("name", new PType(PTypeEnumeration.sstring)),
                new NamedType("age", new PType(PTypeEnumeration.integer)));
            TableView table = new TableView(path + "table", tp_table_element);
            sw.Restart();
            bool tobuild = false;
            if (tobuild)
            {
                table.Fill(Enumerable.Range(0, NumberOfRecords).Select(i =>
                    (object)(new object[] { i.ToString(), i == NumberOfRecords / 2 ? -1 : i })));
                sw.Stop();
                Console.WriteLine("Load Table of {0} elements ok. Duration {1}", NumberOfRecords, sw.ElapsedMilliseconds);
            }
            else table.Warmup();
            sw.Restart();
            // Делаем индекс
            var ha = new IndexHalfkeyImmutable<string>(path + "dyna_index_str_half")
            {
                Table = table,
                KeyProducer = va => (string)((object[])(((object[])va)[1]))[0],
                HalfProducer = k => k.GetHashCode()
            };
            ha.Scale = new ScaleMemory() { IndexCell = ha.IndexCell };
            bool tobuild_h_index = true;
            if (tobuild_h_index) ha.Build();
            else
            {
                //ha.Warmup();
                ha.BuildScale();
            }
            IndexDynamic<string, IndexHalfkeyImmutable<string>> h_index = new IndexDynamic<string, IndexHalfkeyImmutable<string>>(true)
            {
                Table = table,
                KeyProducer = va => (string)((object[])(((object[])va)[1]))[0],
                IndexArray = ha
            };
            Console.WriteLine("h_index Build ok. Duration {0}", sw.ElapsedMilliseconds);
            sw.Restart();
            int cnt = 0;
            for (int i = 0; i < 1000; i++)
            {
                int c = h_index.GetAllByKey(rnd.Next(NumberOfRecords * 3 / 2 - 1).ToString()).Count();
                if (c > 1) Console.WriteLine("Unexpected Error: {0}", c);
                cnt += c;
            }
            sw.Stop();
            Console.WriteLine("1000 GetAllByKey ok. Duration={0} cnt={1}", sw.ElapsedMilliseconds, cnt);
        }
        public static void Main5()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            string path = "../../../Databases/";
            int NumberOfRecords = 1000000;
            Random rnd = new Random();
            Console.WriteLine("Start Universal Index. Main5()");

            PType tp_table_element = new PTypeRecord(
                new NamedType("name", new PType(PTypeEnumeration.sstring)),
                new NamedType("age", new PType(PTypeEnumeration.integer)));
            TableView table = new TableView(path + "table", tp_table_element);
            sw.Restart();
            bool tobuild = true;
            if (tobuild)
            {
                table.Fill(Enumerable.Range(0, NumberOfRecords).Select(i =>
                    (object)(new object[] { i.ToString(), i == NumberOfRecords / 2 ? -1 : i })));
                sw.Stop();
                Console.WriteLine("Load Table of {0} elements ok. Duration {1}", NumberOfRecords, sw.ElapsedMilliseconds);
            }
            else table.Warmup();
            sw.Restart();
            // Делаем индекс
            var aa = new IndexKeyImmutable<int>(path + "dyna_index_int")
            {
                Table = table,
                KeyProducer = va => (int)((object[])(((object[])va)[1]))[1]
            };
            bool tobuild_a_index = true;
            if (tobuild_a_index) aa.Build();
            else aa.Warmup();
            IndexDynamic<int, IndexKeyImmutable<int>> a_index = new IndexDynamic<int, IndexKeyImmutable<int>>(true)
            {
                Table = table,
                KeyProducer = va => (int)((object[])(((object[])va)[1]))[1],
                IndexArray = aa
            };
            Console.WriteLine("a_index Build ok. Duration {0}", sw.ElapsedMilliseconds);
            sw.Restart();
            int cnt = 0;
            for (int i = 0; i < 1000; i++)
            {
                int c = a_index.GetAllByKey(rnd.Next(NumberOfRecords * 3 / 2 - 1)).Count();
                if (c > 1) Console.WriteLine("Unexpected Error: {0}", c);
                cnt += c;
            }
            sw.Stop();
            Console.WriteLine("1000 GetAllByKey ok. Duration={0} cnt={1}", sw.ElapsedMilliseconds, cnt);
        }
        public static void Main4()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            string path = "../../../Databases/";
            int NumberOfRecords = 1000000;
            Random rnd = new Random();
            Console.WriteLine("Start Universal Index. Main4()");

            PType tp_table_element = new PTypeRecord(
                new NamedType("name", new PType(PTypeEnumeration.sstring)),
                new NamedType("age", new PType(PTypeEnumeration.integer)));
            TableView table = new TableView(path + "table", tp_table_element);
            sw.Restart();
            bool tobuild = false;
            if (tobuild)
            {
                table.Fill(Enumerable.Range(0, NumberOfRecords).Select(i =>
                    (object)(new object[] { i.ToString(), i == NumberOfRecords / 2 ? -1 : i })));
                sw.Stop();
                Console.WriteLine("Load Table of {0} elements ok. Duration {1}", NumberOfRecords, sw.ElapsedMilliseconds);
            }
            else table.Warmup();
            sw.Restart();
            // Делаем индекс
            var aa = new IndexViewImmutable<int>(path + "dyna_index_int")
            {
                Table = table,
                KeyProducer = va => (int)((object[])(((object[])va)[1]))[1]
            };
            bool tobuild_s_index = false;
            if (tobuild_s_index) aa.Build();
            else aa.Warmup();
            IndexDynamic<int, IndexViewImmutable<int>> a_index = new IndexDynamic<int, IndexViewImmutable<int>>(true)
            {
                Table = table,
                KeyProducer = va => (int)((object[])(((object[])va)[1]))[1],
                IndexArray = aa
            };
            Console.WriteLine("a_index Build ok. Duration {0}", sw.ElapsedMilliseconds);
            sw.Restart();
            int cnt = 0;
            for (int i = 0; i < 1000; i++)
            {
                int c = a_index.GetAllByKey(rnd.Next(NumberOfRecords * 3 / 2 - 1)).Count();
                if (c > 1) Console.WriteLine("Unexpected Error: {0}", c);
                cnt += c;
            }
            sw.Stop();
            Console.WriteLine("1000 GetAllByKey ok. Duration={0} cnt={1}", sw.ElapsedMilliseconds, cnt);
        }
        public static void Main3()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            string path = "../../../Databases/";
            int NumberOfRecords = 1000000;
            Random rnd = new Random();
            Console.WriteLine("Start Universal Index. Main3()");

            PType tp_table_element = new PTypeRecord(
                new NamedType("name", new PType(PTypeEnumeration.sstring)),
                new NamedType("age", new PType(PTypeEnumeration.integer)));
            TableView table = new TableView(path + "table", tp_table_element);
            sw.Restart();
            bool tobuild = false;
            if (tobuild)
            {
                table.Fill(Enumerable.Range(0, NumberOfRecords).Select(i =>
                    (object)(new object[] { i.ToString(), i == NumberOfRecords / 2 ? -1 : i })));
                sw.Stop();
                Console.WriteLine("Load Table of {0} elements ok. Duration {1}", NumberOfRecords, sw.ElapsedMilliseconds);
            }
            else table.Warmup();
            sw.Restart();
            // Делаем индекс
            var ia = new IndexViewImmutable<string>(path + "dyna_index_string")
            {
                Table = table,
                KeyProducer = va => (string)((object[])(((object[])va)[1]))[0]
            };
            bool tobuild_s_index = false;
            if (tobuild_s_index) ia.Build();
            else ia.Warmup();
            IndexDynamic<string, IndexViewImmutable<string>> s_index = new IndexDynamic<string, IndexViewImmutable<string>>(true)
            {
                Table = table,
                KeyProducer = va => (string)((object[])(((object[])va)[1]))[0],
                IndexArray = ia
            };
            Console.WriteLine("s_index Build ok. Duration {0}", sw.ElapsedMilliseconds);
            sw.Restart();
            int cnt = 0;
            for (int i = 0; i < 1000; i++)
            {
                int c = s_index.GetAllByKey(rnd.Next(NumberOfRecords * 3 / 2 - 1).ToString()).Count();
                if (c > 1) Console.WriteLine("Unexpected Error: {0}", c);
                cnt += c;
            }
            sw.Stop();
            Console.WriteLine("1000 GetAllByKey ok. Duration={0} cnt={1}", sw.ElapsedMilliseconds, cnt);
        }
        public static void Main2()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            string path = "../../../Databases/";
            int NumberOfRecords = 1000000;
            Random rnd = new Random();
            Console.WriteLine("Start Universal Index. Main2()");

            PType tp_table_element = new PTypeRecord(
                new NamedType("name", new PType(PTypeEnumeration.sstring)),
                new NamedType("age", new PType(PTypeEnumeration.integer)));
            IBearingTable table = new TableView(path + "table", tp_table_element);
            sw.Restart();
            bool tobuild = true;
            if (tobuild)
            {
                table.Fill(Enumerable.Range(0, NumberOfRecords).Select(i =>
                    (object)(new object[] { i.ToString(), i == NumberOfRecords / 2 ? -1 : i })));
                sw.Stop();
                Console.WriteLine("Load Table of {0} elements ok. Duration {1}", NumberOfRecords, sw.ElapsedMilliseconds);
            }
            IIndexImmutable<string> s_index = new IndexViewImmutable<string>(path + "s_index")
            {
                Table = table,
                KeyProducer = va => (string)((object[])(((object[])va)[1]))[0]
            };
            if (tobuild)
            {
                sw.Restart();
                s_index.Build();
                sw.Stop();
                Console.WriteLine("s_index Build ok. Duration {0}", sw.ElapsedMilliseconds);
            }
            sw.Restart();
            int cnt = 0;
            for (int i = 0; i < 1000; i++)
            {
                int c = s_index.GetAllByKey(rnd.Next(NumberOfRecords * 3 / 2 - 1).ToString()).Count();
                if (c > 1) Console.WriteLine("Unexpected Error: {0}", c);
                cnt += c;
            }
            sw.Stop();
            Console.WriteLine("1000 GetAllByKey ok. Duration={0} cnt={1}", sw.ElapsedMilliseconds, cnt);
        }
        public static void Main1()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            string path = "../../../Databases/";
            int NumberOfRecords = 1000000;
            Random rnd = new Random();
            Console.WriteLine("Start Universal Index");

            PType tp_table_element = new PTypeRecord(
                new NamedType("name", new PType(PTypeEnumeration.sstring)),
                new NamedType("age", new PType(PTypeEnumeration.integer)));
            IBearingTableImmutable table = new TableViewImmutable(path + "table", tp_table_element);
            sw.Restart();
            bool tobuild = false;
            if (tobuild) 
            {
                table.Fill(Enumerable.Range(0, NumberOfRecords).Select(i => 
                    (object)(new object[] {i.ToString(), i == NumberOfRecords/2 ? -1 : i})));
                sw.Stop();
                Console.WriteLine("Load Table of {0} elements ok. Duration {1}", NumberOfRecords, sw.ElapsedMilliseconds);
            }
            IIndexImmutable<string> s_index = new IndexViewImmutable<string>(path + "s_index")
            {
                Table = table,
                KeyProducer = va => (string)((object[])va)[0]
            };
            if (tobuild)
            {
                sw.Restart();
                s_index.Build();
                sw.Stop();
                Console.WriteLine("s_index Build ok. Duration {0}", sw.ElapsedMilliseconds);
            }
            sw.Restart();
            int cnt = 0;
            for (int i = 0; i < 1000; i++)
            {
                int c = s_index.GetAllByKey(rnd.Next(NumberOfRecords - 1).ToString()).Count();
                if (c > 1) Console.WriteLine("Unexpected Error: {0}", c);
                cnt += c;
            }
            sw.Stop();
            Console.WriteLine("1000 GetAllByKey ok. Duration={0} cnt={1}", sw.ElapsedMilliseconds, cnt);
        }
    }
}
