using System;
using System.IO;
using PolarDB;
using RdfStoreSparqlNamespace;
using SparqlParseRun.RdfCommon;
using SparqlParseRun.RdfCommon.Literals;


public class StringNodeGenerator :INodeGenerator
    {
        public PaCell nameTable;
        private IndexNameTable indexByString;
        //private readonly Random random = new Random();
    private Func<object, INode>[] nodeByWritebleObject;
    private Func<object, ComparableObject>[] comparableByWritebleObject;
        
        private int nextBlankCount;
  

    public static StringNodeGenerator Create()
    {
        StringNodeGenerator nodeGenerator = new StringNodeGenerator();
        nodeGenerator.specialTypes.GenerateLiteralTypes(nodeGenerator, NameTableSettings.Default.simple_literal_equals_string_literal);

        return nodeGenerator;
    }

    private readonly SpecialTypes specialTypes=new SpecialTypes();
    protected StringNodeGenerator()
    {
        nameTable =
            new PaCell(new PTypeSequence(new PTypeRecord(new NamedType("code", new PType(PTypeEnumeration.integer)),
                new NamedType("str", new PType(PTypeEnumeration.sstring)))),
                NameTableSettings.Default.path + "nametable.pac", false);
        if (nameTable.IsEmpty) nameTable.Fill(new object[0]);
        indexByString = new IndexNameTable(new DirectoryInfo(NameTableSettings.Default.path + "\\nameTableIndex"),

            nameTable.Root, e => (string) e.Field(1).Get(), NameTableSettings.Default.Index_max_Changes_For_Rebuild);
        nodeByWritebleObject = new Func<object, INode>[] 
        {        enm => new UriNode((string) enm), 
            enm => (bool) enm ? BoolLiteralNode.TrueNode(specialTypes.SpecialLiteralTypes_bool) : BoolLiteralNode.FalseNode(specialTypes.SpecialLiteralTypes_bool),
            enm => new LiteralofTypeStringNode((string) enm, specialTypes.SpecialLiteralTypes_String),
            enm => new SimpleLiteralNode((string) enm, specialTypes.SpecialLiteralTypes_simpleLiteral), 
            enm =>
                new SparqlLanguageLiteralNode((string) ((object[]) enm)[0], (string) (((object[]) enm))[1],
                    specialTypes.SpecialLiteralTypes_langString), 
            enm => new TypedLiteralNode((double) enm, specialTypes.SpecialLiteralTypes_double), 
            enm => new TypedLiteralNode(Convert.ToDecimal(enm), specialTypes.SpecialLiteralTypes_decimal), 
            enm => new TypedLiteralNode((float) (double) enm, specialTypes.SpecialLiteralTypes_float), 
            enm => new TypedLiteralNode((int) enm, specialTypes.SpecialLiteralTypes_int), 
            enm => new TypedLiteralNode(new DateTimeOffset(DateTime.FromBinary((long) enm)), specialTypes.SpecialLiteralTypes_dateTime), 
            enm => new TypedLiteralNode(new DateTime((long) enm), specialTypes.SpecialLiteralTypes_date), 
            enm => new TypedLiteralNode((TimeSpan.FromTicks((long) enm)), specialTypes.SpecialLiteralTypes_timeDuration), 
            enm => new TypedLiteralNode(((object[]) enm)[0].ToString(),
                new UriNode((string) ((object[]) enm)[1]))};
        comparableByWritebleObject = new Func<object, ComparableObject>[]
        {
            enm => new ComparableObject(0, (string) enm, null, null),
            enm => new ComparableObject(1, specialTypes.SpecialLiteralTypes_bool, (bool) enm, null),
            enm => new ComparableObject(2, specialTypes.SpecialLiteralTypes_String, (string) enm, null),
            enm =>
                new ComparableObject(3, specialTypes.SpecialLiteralTypes_simpleLiteral, (string) enm, null),
            enm =>
                new ComparableObject(4, specialTypes.SpecialLiteralTypes_langString,
                    (string) ((object[]) enm)[0],
                    (string) (((object[]) enm))[1]),
            enm => new ComparableObject(5, specialTypes.SpecialLiteralTypes_double, (double) enm, null),
            enm =>
                new ComparableObject(6, specialTypes.SpecialLiteralTypes_decimal, Convert.ToDecimal(enm),
                    null),
            enm =>
                new ComparableObject(7, specialTypes.SpecialLiteralTypes_float, (float) (double) enm, null),
            enm => new ComparableObject(8, (int) enm, specialTypes.SpecialLiteralTypes_int, null),
            enm =>
                new ComparableObject(9, specialTypes.SpecialLiteralTypes_dateTime,  (long) enm, null),
            enm =>
                new ComparableObject(10, specialTypes.SpecialLiteralTypes_date, (long) enm, null),
            enm =>
                new ComparableObject(11, specialTypes.SpecialLiteralTypes_timeDuration, (long) enm, null),
            enm => new ComparableObject(12, (string) ((object[]) enm)[1], ((object[]) enm)[0].ToString(), null)
        };
    }

    public virtual IUriNode CreateUriNode(UriPrefixed uri)
        {
            return new UriNode(uri.Namespace + uri.LocalName); 
        }

        public virtual IUriNode GetUri(string uri)
        {
            UriPrefixed up = Prologue.SplitUndefined(uri);

            return new UriNode(up.Namespace+up.LocalName);
        }


        //public string CreateString(int code)
        //{
           
        //}
        
        public ILiteralNode CreateLiteralNode(string p)
        {
            p = p.Trim('"', '\'');
            return new SimpleLiteralNode(p, specialTypes.SpecialLiteralTypes_simpleLiteral);
        }

        public ILiteralNode CreateLiteralNode(string s, string lang)
        {
            s = s.Trim('"', '\'');
            return new SparqlLanguageLiteralNode(s, lang, specialTypes.SpecialLiteralTypes_langString);
        }

        public ILiteralNode CreateLiteralNode(int parse)
        {
            return new TypedLiteralNode(parse, specialTypes.SpecialLiteralTypes_int);
        }

        public ILiteralNode CreateLiteralNode(decimal p)
        {
            return new TypedLiteralNode(p, specialTypes.SpecialLiteralTypes_decimal);
        }

        public ILiteralNode CreateLiteralNode(double p)
        {
         //   ILiteralNode literalNode;
            return new TypedLiteralNode(p, specialTypes.SpecialLiteralTypes_double);
        }

        public ILiteralNode CreateLiteralNode(bool p)
        {
            return p ? BoolLiteralNode.TrueNode(specialTypes.SpecialLiteralTypes_bool) : BoolLiteralNode.FalseNode(specialTypes.SpecialLiteralTypes_bool);
        }

        public ILiteralNode CreateLiteralNode(string p, IUriNode sparqlUriNode)
        {
            p = p.Trim('"', '\'');
            ILiteralNode newLiteralNode;
            if (Equals(sparqlUriNode, specialTypes.SpecialLiteralTypes_String))
                newLiteralNode = new LiteralofTypeStringNode(p, specialTypes.SpecialLiteralTypes_String);
            else if (Equals(sparqlUriNode, specialTypes.SpecialLiteralTypes_date))
            {
                DateTime date;
                if (!DateTime.TryParse(p, out date)) throw new ArgumentException(p);
                newLiteralNode = new TypedLiteralNode(date, specialTypes.SpecialLiteralTypes_date);
            }
            else if (Equals(sparqlUriNode, specialTypes.SpecialLiteralTypes_dateTime))
            {
                DateTimeOffset date;
                if (!DateTimeOffset.TryParse(p, out date)) throw new ArgumentException(p);
                newLiteralNode = new TypedLiteralNode(date, specialTypes.SpecialLiteralTypes_dateTime);
            }
            else if (Equals(sparqlUriNode, specialTypes.SpecialLiteralTypes_bool))
            {
                bool b;
                if (!bool.TryParse(p, out b)) throw new ArgumentException(p);
                newLiteralNode = CreateLiteralNode(b);
            }
            else if (Equals(sparqlUriNode, specialTypes.SpecialLiteralTypes_decimal))
            {
                decimal d;
                if (!decimal.TryParse(p.Replace(".", ","), out d)) throw new ArgumentException(p);
                newLiteralNode = new TypedLiteralNode(d, specialTypes.SpecialLiteralTypes_decimal);
            }
            else if (Equals(sparqlUriNode, specialTypes.SpecialLiteralTypes_double))
            {
                double d;
                if (!double.TryParse(p.Replace(".", ","), out d)) throw new ArgumentException(p);
                newLiteralNode = new TypedLiteralNode(d, specialTypes.SpecialLiteralTypes_double);
            }
            else if (Equals(sparqlUriNode, specialTypes.SpecialLiteralTypes_float))
            {
                float f;
                if (!float.TryParse(p.Replace(".", ","), out f)) throw new ArgumentException(p);
                newLiteralNode = new TypedLiteralNode(f, specialTypes.SpecialLiteralTypes_float);
            }
            else if (Equals(sparqlUriNode, specialTypes.SpecialLiteralTypes_int))
            {
                int i;
                if (!int.TryParse(p, out i)) throw new ArgumentException(p);
                newLiteralNode = new TypedLiteralNode(i, specialTypes.SpecialLiteralTypes_int);
            }
            else if (Equals(sparqlUriNode, specialTypes.SpecialLiteralTypes_timeDuration))
            {
                TimeSpan i;
                if (!TimeSpan.TryParse(p, out i)) throw new ArgumentException(p);
                newLiteralNode = new TypedLiteralNode(i, specialTypes.SpecialLiteralTypes_timeDuration);
            }
            else
                newLiteralNode = new TypedLiteralNode(p, sparqlUriNode);
            return newLiteralNode; 
        }

        public IBlankNode CreateBlankNode(string graph, string blankNodeString = null)
        {
            if (blankNodeString == null) blankNodeString = "blank" + nextBlankCount++;//(long)(random.NextDouble() * 1000 * 1000 * 1000 * 1000);
           // if (graph != null) blankNodeString = graph.Name + "/" + blankNodeString;     
            return (IBlankNode)CreateUriNode(new UriPrefixed(":", blankNodeString, graph));
        }

     

       

        public INode WritableObject2Node(object obj)
        {
            var lit = (object[]) obj;
            int tag = (int) lit[0];
            return nodeByWritebleObject[tag](lit[1]);
           // throw new ArgumentOutOfRangeException();
        }

        public object[] Node2WritableObject(INode node)
        {
            if (node is IUriNode) return new object[]{ 0, node.ToString()}; //uri or blank
            if (node is BoolLiteralNode) return new object[] {1, node.Equals(BoolLiteralNode.TrueNode(specialTypes.SpecialLiteralTypes_bool))};
            if (node is LiteralofTypeStringNode) return new object[] {2, ((LiteralofTypeStringNode)node).Content};
            if (node is SimpleLiteralNode) return new object[] {3, ((SimpleLiteralNode) node).Content};
            if (node is SparqlLanguageLiteralNode)  return new object[] {4, new object[]{ ((SparqlLanguageLiteralNode) node).Content, ((SparqlLanguageLiteralNode) node).Lang}};
            var typedLiteralNode = node as TypedLiteralNode;
            if (typedLiteralNode != null)
            {
                if (typedLiteralNode.DataType.Equals(specialTypes.SpecialLiteralTypes_double))     
                  return new object[] {5, typedLiteralNode.Content};
                if (typedLiteralNode.DataType.Equals(specialTypes.SpecialLiteralTypes_decimal))
                    return new object[] { 6, Convert.ToDouble(typedLiteralNode.Content) };
                if (typedLiteralNode.DataType.Equals(specialTypes.SpecialLiteralTypes_float))     
                    return new object[] { 7, Convert.ToDouble(typedLiteralNode.Content) };
                if (typedLiteralNode.DataType.Equals(specialTypes.SpecialLiteralTypes_int))     
                    return new object[] { 8, typedLiteralNode.Content };
                if (typedLiteralNode.DataType.Equals(specialTypes.SpecialLiteralTypes_dateTime))     
                    return new object[] { 9, ((DateTimeOffset)typedLiteralNode.Content).Ticks };
                if (typedLiteralNode.DataType.Equals(specialTypes.SpecialLiteralTypes_date))     
                    return new object[] { 10, ((DateTime)typedLiteralNode.Content).Ticks };
                if (typedLiteralNode.DataType.Equals(specialTypes.SpecialLiteralTypes_timeDuration))
                    return new object[] { 11, ((TimeSpan) typedLiteralNode.Content).Ticks };

                return new object[] {12, new object[] {typedLiteralNode.Content.ToString(), typedLiteralNode.DataType.ToString()}};
            }
            throw new ArgumentOutOfRangeException();
        }
        public ComparableObject Node2ComparableObject(INode node)
        {

            if (node is IUriNode) return new ComparableObject(0, node.ToString(), null, null); //uri or blank
            if (node is BoolLiteralNode) return new ComparableObject(1, specialTypes.SpecialLiteralTypes_bool, node.Equals(BoolLiteralNode.TrueNode(specialTypes.SpecialLiteralTypes_bool)), null);
            if (node is LiteralofTypeStringNode) return new ComparableObject(2, specialTypes.SpecialLiteralTypes_String, ((LiteralofTypeStringNode)node).Content, null);
            if (node is SimpleLiteralNode) return new ComparableObject(3, specialTypes.SpecialLiteralTypes_simpleLiteral, ((SimpleLiteralNode)node).Content, null);
            if (node is SparqlLanguageLiteralNode) return new ComparableObject(4, specialTypes.SpecialLiteralTypes_langString, ((SparqlLanguageLiteralNode)node).Content, ((SparqlLanguageLiteralNode)node).Lang);
            var typedLiteralNode = node as TypedLiteralNode;
            if (typedLiteralNode != null)
            {
                if (typedLiteralNode.DataType.Equals(specialTypes.SpecialLiteralTypes_double))
                    return new ComparableObject(5, specialTypes.SpecialLiteralTypes_double, typedLiteralNode.Content, null);
                if (typedLiteralNode.DataType.Equals(specialTypes.SpecialLiteralTypes_decimal))
                    return new ComparableObject(6, specialTypes.SpecialLiteralTypes_decimal, Convert.ToDouble(typedLiteralNode.Content), null);
                if (typedLiteralNode.DataType.Equals(specialTypes.SpecialLiteralTypes_float))
                    return new ComparableObject(7, specialTypes.SpecialLiteralTypes_float, Convert.ToDouble(typedLiteralNode.Content), null);
                if (typedLiteralNode.DataType.Equals(specialTypes.SpecialLiteralTypes_int))
                    return new ComparableObject(8, specialTypes.SpecialLiteralTypes_int, typedLiteralNode.Content, null);
                if (typedLiteralNode.DataType.Equals(specialTypes.SpecialLiteralTypes_dateTime))
                    return new ComparableObject(9, specialTypes.SpecialLiteralTypes_dateTime, ((DateTimeOffset)typedLiteralNode.Content).Ticks, null);
                if (typedLiteralNode.DataType.Equals(specialTypes.SpecialLiteralTypes_date))
                    return new ComparableObject(10, specialTypes.SpecialLiteralTypes_date, ((DateTime)typedLiteralNode.Content).Ticks, null);
                if (typedLiteralNode.DataType.Equals(specialTypes.SpecialLiteralTypes_timeDuration))
                    return new ComparableObject(11, specialTypes.SpecialLiteralTypes_timeDuration, ((TimeSpan)typedLiteralNode.Content).Ticks, null);

                return new ComparableObject(12, typedLiteralNode.DataType, typedLiteralNode.Content, null);
            }
            throw new ArgumentOutOfRangeException();
        }
        public ComparableObject Writable2ComparableObject(object writableNode)
        {
            var enm = (object[])writableNode;
            int tag = (int)enm[0];
            return comparableByWritebleObject[tag](enm[1]);
           // throw new ArgumentOutOfRangeException();
        }
        

        public Triple ConvertToTriple(object[] row)
        {
            return new Triple(GetUri((string) row[0]), GetUri((string) row[1]), WritableObject2Node(row[2]));
        }

      

      
        public void Build()
        {
            indexByString.Build();
        }

     

        public virtual void Delete()
        {
               indexByString.Close();
            nameTable.Close();     
            File.Delete(NameTableSettings.Default.path);
            new DirectoryInfo(NameTableSettings.Default.path+"\\nameTableIndex").Delete(true);
        }

        public virtual void Clear()
        {
           nameTable.Clear();
            nameTable.Fill(new object[0]);
            indexByString.Clear();
            //indexMultiplyValues.Add(new Row(SpecialTypes.Date), nameTable.Root.AppendElement(new object[] { (int)nameTable.Root.Count(), SpecialTypes.Date }));
            //indexMultiplyValues.Add(new Row(SpecialTypes.String), nameTable.Root.AppendElement(new object[] { (int)nameTable.Root.Count(), SpecialTypes.String }));
            //if(!NameTableSettings.Default.simple_literal_equals_string_literal)
            //    indexMultiplyValues.Add(new Row(NameTableSettings.Default.simple_literal), nameTable.Root.AppendElement(new object[] { (int)nameTable.Root.Count(), NameTableSettings.Default.simple_literal }));
            //indexMultiplyValues.Add(new Row(SpecialTypes.LangString), nameTable.Root.AppendElement(new object[] { (int)nameTable.Root.Count(), SpecialTypes.LangString }));
            //indexMultiplyValues.Add(new Row(SpecialTypes.Integer), nameTable.Root.AppendElement(new object[] { (int)nameTable.Root.Count(), SpecialTypes.Integer }));
            //indexMultiplyValues.Add(new Row(SpecialTypes.Decimal), nameTable.Root.AppendElement(new object[] { (int)nameTable.Root.Count(), SpecialTypes.Decimal }));
            //indexMultiplyValues.Add(new Row(SpecialTypes.Double), nameTable.Root.AppendElement(new object[] { (int)nameTable.Root.Count(), SpecialTypes.Double }));
            //indexMultiplyValues.Add(new Row(SpecialTypes.Bool), nameTable.Root.AppendElement(new object[] { (int)nameTable.Root.Count(), SpecialTypes.Bool }));
            //indexMultiplyValues.Add(new Row(SpecialTypes.Float), nameTable.Root.AppendElement(new object[] { (int)nameTable.Root.Count(), SpecialTypes.Float }));
            //indexMultiplyValues.Add(new Row(SpecialTypes.DayTimeDuration), nameTable.Root.AppendElement(new object[] { (int)nameTable.Root.Count(), SpecialTypes.DayTimeDuration }));
            //indexMultiplyValues.Add(new Row(SpecialTypes.DateTime), nameTable.Root.AppendElement(new object[] { (int)nameTable.Root.Count(), SpecialTypes.DateTime }));
        specialTypes.GenerateLiteralTypes(this, NameTableSettings.Default.simple_literal_equals_string_literal);
        }

        public virtual void Close()
        {
            nameTable.Close();
            indexByString.Close();
        }

        public virtual void Warm()
        {
            foreach (var element in nameTable.Root.Elements()) ;
            indexByString.Warm();
        }

        
    }

    

