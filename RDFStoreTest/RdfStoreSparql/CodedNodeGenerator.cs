using System;
using System.IO;
using PolarDB;
using RdfStoreSparqlNamespace;
using SparqlParseRun.RdfCommon;
using SparqlParseRun.RdfCommon.Literals;


public class CodedNodeGenerator :INodeGenerator
    {
        public PaCell nameTable;
        private IndexNameTable indexByString;
        //private readonly Random random = new Random();       
        private IUriNode SpecialLiteralTypes_date;
        private IUriNode SpecialLiteralTypes_String;
        private IUriNode SpecialLiteralTypes_simpleLiteral;
        private IUriNode SpecialLiteralTypes_langString;
        private IUriNode SpecialLiteralTypes_int;
        private IUriNode SpecialLiteralTypes_decimal;
        private IUriNode SpecialLiteralTypes_double;
        private IUriNode SpecialLiteralTypes_bool;
        private IUriNode SpecialLiteralTypes_float;
        private IUriNode SpecialLiteralTypes_timeDuration;
        private IUriNode SpecialLiteralTypes_dateTime;
        private Func<object, INode>[] nodeByWritebleObject;
        private Func<object, ComparableObject>[] comparableByWritebleObject;
        
        private int nextBlankCount;
       

    public static CodedNodeGenerator Create()
    {
        CodedNodeGenerator nodeGenerator=new CodedNodeGenerator();
        
                    nodeGenerator.GenerateLiteralTypes();
        return nodeGenerator;
    }

    protected CodedNodeGenerator()
    {
        nameTable =
            new PaCell(new PTypeSequence(new PTypeRecord(new NamedType("code", new PType(PTypeEnumeration.integer)),
                new NamedType("str", new PType(PTypeEnumeration.sstring)))),
                NameTableSettings.Default.path + "nametable.pac", false);
        if (nameTable.IsEmpty) nameTable.Fill(new object[0]);
        indexByString = new IndexNameTable(new DirectoryInfo(NameTableSettings.Default.path + "\\nameTableIndex"),
            nameTable.Root, e => (string) e.Field(1).Get(), NameTableSettings.Default.Index_max_Changes_For_Rebuild);
    }

    protected void GenerateLiteralTypes()
        {
            SpecialLiteralTypes_date = CreateUriNode(SpecialTypes.Date);
            SpecialLiteralTypes_String = CreateUriNode(SpecialTypes.String);
            SpecialLiteralTypes_simpleLiteral = NameTableSettings.Default.simple_literal_equals_string_literal
                ? SpecialLiteralTypes_String
                : CreateUriNode(SpecialTypes.SimpleLiteral);
            SpecialLiteralTypes_langString = CreateUriNode(SpecialTypes.String);
            SpecialLiteralTypes_int = CreateUriNode(SpecialTypes.Integer);
            SpecialLiteralTypes_decimal = CreateUriNode(SpecialTypes.Decimal);
            SpecialLiteralTypes_double = CreateUriNode(SpecialTypes.Double);
            SpecialLiteralTypes_bool = CreateUriNode(SpecialTypes.Bool);
            SpecialLiteralTypes_float = CreateUriNode(SpecialTypes.Float);
            SpecialLiteralTypes_timeDuration = CreateUriNode(SpecialTypes.DayTimeDuration);
            SpecialLiteralTypes_dateTime = CreateUriNode(SpecialTypes.DateTime);
            nodeByWritebleObject = new Func<object, INode>[] 
            {        enm => new CodedUriNode(this, (int) enm), 
                     enm => (bool) enm ? BoolLiteralNode.TrueNode(SpecialLiteralTypes_bool) : BoolLiteralNode.FalseNode(SpecialLiteralTypes_bool),
                    enm => new LiteralofTypeStringNode((string) enm, SpecialLiteralTypes_String),
                    enm => new SimpleLiteralNode((string) enm, SpecialLiteralTypes_simpleLiteral), 
                    enm =>
                        new SparqlLanguageLiteralNode((string) ((object[]) enm)[0], (string) (((object[]) enm))[1],
                            SpecialLiteralTypes_langString), 
                    enm => new TypedLiteralNode((double) enm, SpecialLiteralTypes_double), 
                    enm => new TypedLiteralNode(Convert.ToDecimal(enm), SpecialLiteralTypes_decimal), 
                    enm => new TypedLiteralNode((float) (double) enm, SpecialLiteralTypes_float), 
                    enm => new TypedLiteralNode((int) enm, SpecialLiteralTypes_int), 
                    enm => new TypedLiteralNode((DateTimeOffset.FromFileTime((long) enm)), SpecialLiteralTypes_dateTime), 
                    enm => new TypedLiteralNode((DateTime.FromBinary((long) enm)), SpecialLiteralTypes_date), 
                    enm => new TypedLiteralNode((TimeSpan.FromTicks((long) enm)), SpecialLiteralTypes_timeDuration), 
                    enm => new TypedLiteralNode(((object[]) enm)[0].ToString(),
                                    new CodedUriNode(this, (int) ((object[]) enm)[1]))};
            comparableByWritebleObject = new Func<object, ComparableObject>[]
            {
                enm => new ComparableObject(0, (int) enm, null, null),
                enm => new ComparableObject(1, SpecialLiteralTypes_bool.GetCode(), (bool) enm, null),
                enm => new ComparableObject(2, SpecialLiteralTypes_String.GetCode(), (string) enm, null),
                enm => new ComparableObject(3, SpecialLiteralTypes_simpleLiteral.GetCode(), (string) enm, null),
                enm =>
                    new ComparableObject(4, SpecialLiteralTypes_langString.GetCode(), (string) ((object[]) enm)[0],
                        (string) (((object[]) enm))[1]),
                enm => new ComparableObject(5, SpecialLiteralTypes_double.GetCode(), (double) enm, null),
                enm => new ComparableObject(6, SpecialLiteralTypes_decimal.GetCode(), Convert.ToDecimal(enm), null),
                enm => new ComparableObject(7, SpecialLiteralTypes_float.GetCode(), (float) (double) enm, null),
                enm => new ComparableObject(8, (int) enm, SpecialLiteralTypes_int.GetCode(), null),
                enm =>
                    new ComparableObject(9, SpecialLiteralTypes_dateTime.GetCode(),
                        (DateTimeOffset.FromFileTime((long) enm)), null),
                enm =>
                    new ComparableObject(10, SpecialLiteralTypes_date.GetCode(), (DateTime.FromBinary((long) enm)), null),
                enm =>
                    new ComparableObject(11, SpecialLiteralTypes_timeDuration.GetCode(),
                        (TimeSpan.FromTicks((long) enm)), null),
                enm => new ComparableObject(12, (int) ((object[]) enm)[1], ((object[]) enm)[0].ToString(), null)
            };

        }

        public virtual IUriNode CreateUriNode(UriPrefixed uri)
        {
            return new CodedUriNode(this, CreateCode(uri.Namespace + uri.LocalName)); 
        }

        public virtual IUriNode GetUri(string uri)
        {
            UriPrefixed up = Prologue.SplitUndefined(uri);
         
            int code = GetCode(up.Namespace+up.LocalName);
            return code==-1 ? null : new CodedUriNode(this, code);
        }


        //public string CreateString(int code)
        //{
           
        //}
        public int GetCode(string s)
        {
            var exists = indexByString.Search(s);
            if (exists!=null) return (int)((object[])exists)[0];
            return -1;
        }
        public int CreateCode(string s)
        {
            var exists = indexByString.Search(s);
            if (exists!=null) return (int) ((object[])exists)[0];
            int newCode = (int)nameTable.Root.Count();                                         
            indexByString.Add(s, nameTable.Root.AppendElement(new object[] { newCode, s }));
            return newCode;
        }

        public string GetString(int code)
        {
            var uriPrefixed = GetUriRefixed(code);
            
            return uriPrefixed.Namespace + uriPrefixed.LocalName;
        }
        public ILiteralNode CreateLiteralNode(string p)
        {
            p = p.Trim('"', '\'');
            return new SimpleLiteralNode(p, SpecialLiteralTypes_simpleLiteral);
        }

        public ILiteralNode CreateLiteralNode(string s, string lang)
        {
            s = s.Trim('"', '\'');
            return new SparqlLanguageLiteralNode(s, lang, SpecialLiteralTypes_langString);
        }

        public ILiteralNode CreateLiteralNode(int parse)
        {
            return new TypedLiteralNode(parse, SpecialLiteralTypes_int);
        }

        public ILiteralNode CreateLiteralNode(decimal p)
        {
            return new TypedLiteralNode(p, SpecialLiteralTypes_decimal);
        }

        public ILiteralNode CreateLiteralNode(double p)
        {
         //   ILiteralNode literalNode;
            return new TypedLiteralNode(p, SpecialLiteralTypes_double);
        }

        public ILiteralNode CreateLiteralNode(bool p)
        {
            return p ? BoolLiteralNode.TrueNode(SpecialLiteralTypes_bool) : BoolLiteralNode.FalseNode(SpecialLiteralTypes_bool);
        }

        public ILiteralNode CreateLiteralNode(string p, IUriNode sparqlUriNode)
        {
            p = p.Trim('"', '\'');
            ILiteralNode newLiteralNode;
            if (Equals(sparqlUriNode, SpecialLiteralTypes_String))
                newLiteralNode = new LiteralofTypeStringNode(p, SpecialLiteralTypes_String);
            else if (Equals(sparqlUriNode, SpecialLiteralTypes_date))
            {
                DateTime date;
                if (!DateTime.TryParse(p, out date)) throw new ArgumentException(p);
                newLiteralNode = new TypedLiteralNode(date, SpecialLiteralTypes_date);
            }
            else if (Equals(sparqlUriNode, SpecialLiteralTypes_dateTime))
            {
                DateTimeOffset date;
                if (!DateTimeOffset.TryParse(p, out date)) throw new ArgumentException(p);
                newLiteralNode = new TypedLiteralNode(date, SpecialLiteralTypes_dateTime);
            }
            else if (Equals(sparqlUriNode, SpecialLiteralTypes_bool))
            {
                bool b;
                if (!bool.TryParse(p, out b)) throw new ArgumentException(p);
                newLiteralNode = CreateLiteralNode(b);
            }
            else if (Equals(sparqlUriNode, SpecialLiteralTypes_decimal))
            {
                decimal d;
                if (!decimal.TryParse(p.Replace(".", ","), out d)) throw new ArgumentException(p);
                newLiteralNode = new TypedLiteralNode(d, SpecialLiteralTypes_decimal);
            }
            else if (Equals(sparqlUriNode, SpecialLiteralTypes_double))
            {
                double d;
                if (!double.TryParse(p.Replace(".", ","), out d)) throw new ArgumentException(p);
                newLiteralNode = new TypedLiteralNode(d, SpecialLiteralTypes_double);
            }
            else if (Equals(sparqlUriNode, SpecialLiteralTypes_float))
            {
                float f;
                if (!float.TryParse(p.Replace(".", ","), out f)) throw new ArgumentException(p);
                newLiteralNode = new TypedLiteralNode(f, SpecialLiteralTypes_float);
            }
            else if (Equals(sparqlUriNode, SpecialLiteralTypes_int))
            {
                int i;
                if (!int.TryParse(p, out i)) throw new ArgumentException(p);
                newLiteralNode = new TypedLiteralNode(i, SpecialLiteralTypes_int);
            }
            else if (Equals(sparqlUriNode, SpecialLiteralTypes_timeDuration))
            {
                TimeSpan i;
                if (!TimeSpan.TryParse(p, out i)) throw new ArgumentException(p);
                newLiteralNode = new TypedLiteralNode(i, SpecialLiteralTypes_timeDuration);
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
            if (node is CodedUriNode) return new object[]{ 0, ((CodedUriNode)node).Code}; //uri or blank
            if (node is BoolLiteralNode) return new object[] {1, node.Equals(BoolLiteralNode.TrueNode(SpecialLiteralTypes_bool))};
            if (node is LiteralofTypeStringNode) return new object[] {2, ((LiteralofTypeStringNode)node).Content};
            if (node is SimpleLiteralNode) return new object[] {3, ((SimpleLiteralNode) node).Content};
            if (node is SparqlLanguageLiteralNode)  return new object[] {4, new object[]{ ((SparqlLanguageLiteralNode) node).Content, ((SparqlLanguageLiteralNode) node).Lang}};
            var typedLiteralNode = node as TypedLiteralNode;
            if (typedLiteralNode != null)
            {       
                if (typedLiteralNode.DataType.Equals(SpecialLiteralTypes_double))     
                  return new object[] {5, typedLiteralNode.Content};
                if (typedLiteralNode.DataType.Equals(SpecialLiteralTypes_decimal))
                    return new object[] { 6, Convert.ToDouble(typedLiteralNode.Content) };
                if (typedLiteralNode.DataType.Equals(SpecialLiteralTypes_float))     
                    return new object[] { 7, Convert.ToDouble(typedLiteralNode.Content) };
                if (typedLiteralNode.DataType.Equals(SpecialLiteralTypes_int))     
                    return new object[] { 8, typedLiteralNode.Content };
                if (typedLiteralNode.DataType.Equals(SpecialLiteralTypes_dateTime))     
                    return new object[] { 9, ((DateTimeOffset)typedLiteralNode.Content).ToFileTime() };
                if (typedLiteralNode.DataType.Equals(SpecialLiteralTypes_date))     
                    return new object[] { 10, ((DateTime)typedLiteralNode.Content).ToBinary() };
                if (typedLiteralNode.DataType.Equals(SpecialLiteralTypes_timeDuration))
                    return new object[] { 11, ((TimeSpan) typedLiteralNode.Content).Ticks };

                return new object[] {12, new object[] {typedLiteralNode.Content.ToString(), typedLiteralNode.DataType.GetCode()}};
            }
            throw new ArgumentOutOfRangeException();
        }
        public ComparableObject Node2ComparableObject(INode node)
        {

            if (node is CodedUriNode) return new ComparableObject(0, ((CodedUriNode) node).Code, null, null); //uri or blank
            if (node is BoolLiteralNode) return new ComparableObject( 1, SpecialLiteralTypes_bool.GetCode(), node.Equals(BoolLiteralNode.TrueNode(SpecialLiteralTypes_bool)), null );
            if (node is LiteralofTypeStringNode) return new ComparableObject(2, SpecialLiteralTypes_String.GetCode(), ((LiteralofTypeStringNode)node).Content, null );
            if (node is SimpleLiteralNode) return new ComparableObject( 3, SpecialLiteralTypes_simpleLiteral.GetCode(), ((SimpleLiteralNode)node).Content, null );
            if (node is SparqlLanguageLiteralNode) return new ComparableObject( 4, SpecialLiteralTypes_langString.GetCode(), ((SparqlLanguageLiteralNode)node).Content, ((SparqlLanguageLiteralNode)node).Lang );
            var typedLiteralNode = node as TypedLiteralNode;
            if (typedLiteralNode != null)
            {
                if (typedLiteralNode.DataType.Equals(SpecialLiteralTypes_double))
                    return new ComparableObject(5, SpecialLiteralTypes_double.GetCode(), typedLiteralNode.Content, null);
                if (typedLiteralNode.DataType.Equals(SpecialLiteralTypes_decimal))
                    return new ComparableObject(6, SpecialLiteralTypes_decimal.GetCode(), Convert.ToDouble(typedLiteralNode.Content), null);
                if (typedLiteralNode.DataType.Equals(SpecialLiteralTypes_float))
                    return new ComparableObject(7, SpecialLiteralTypes_float.GetCode(), Convert.ToDouble(typedLiteralNode.Content), null);
                if (typedLiteralNode.DataType.Equals(SpecialLiteralTypes_int))
                    return new ComparableObject(8, SpecialLiteralTypes_int.GetCode(), typedLiteralNode.Content, null);
                if (typedLiteralNode.DataType.Equals(SpecialLiteralTypes_dateTime))
                    return new ComparableObject(9, SpecialLiteralTypes_dateTime.GetCode(), ((DateTimeOffset)typedLiteralNode.Content).ToFileTime(), null);
                if (typedLiteralNode.DataType.Equals(SpecialLiteralTypes_date))
                    return new ComparableObject(10, SpecialLiteralTypes_date.GetCode(), ((DateTime)typedLiteralNode.Content).ToBinary(), null);
                if (typedLiteralNode.DataType.Equals(SpecialLiteralTypes_timeDuration))
                    return new ComparableObject(11, SpecialLiteralTypes_timeDuration.GetCode(), ((TimeSpan)typedLiteralNode.Content).Ticks, null);

                return new ComparableObject(12, typedLiteralNode.DataType.GetCode(), typedLiteralNode.Content.ToString(), null);
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
        public IUriNode GetUri(int code)
        {
            return new CodedUriNode(this, code);
        }

        public Triple ConvertToTriple(object[] row)
        {
            return new Triple(GetUri((int) row[0]), GetUri((int) row[1]), WritableObject2Node(row[2]));
        }

      

        public void Dispose()
        {
            indexByString.Dispose();
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
            GenerateLiteralTypes();
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

        public virtual UriPrefixed GetUriRefixed(int code)
        {
                  if (code < 0 || code >= nameTable.Root.Count()) throw new ArgumentOutOfRangeException();
            var prefixed = (string)nameTable.Root.Element(code).Field(1).Get();
            var uriPrefixed = Prologue.SplitPrefixed(prefixed);      
            return uriPrefixed;
        }
    }

    

