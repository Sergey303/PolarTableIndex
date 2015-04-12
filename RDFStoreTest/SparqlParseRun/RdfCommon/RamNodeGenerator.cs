using System;
using System.Collections.Generic;
using SparqlParseRun.RdfCommon.Literals;

namespace SparqlParseRun.RdfCommon
{
    public class RamNodeGenerator : INodeGenerator
    {
        public Dictionary<string, UriNode> IriNodes = new Dictionary<string, UriNode>();
        public Dictionary<ILiteralNode, ILiteralNode> literalNodes = new Dictionary<ILiteralNode, ILiteralNode>();
        
        public Dictionary<string, BlankNode> blankNodes = new Dictionary<string, BlankNode>();
       

      public IUriNode CreateUriNode(UriPrefixed uri)
        {
            UriNode uriNode;
            if (IriNodes.TryGetValue(uri.ToString(), out uriNode)) return uriNode;
            IriNodes.Add(uri.ToString(), uriNode = new UriNode(uri.Namespace+uri.LocalName));
            return uriNode;
        }

        public ILiteralNode CreateLiteralNode(string p)
        {
            p = p.Trim('"','\'');
            ILiteralNode literalNode;
            var newLiteralNode = new SimpleLiteralNode(p, CreateUriNode(SpecialTypes.SimpleLiteral));    //TODO
            if (literalNodes.TryGetValue(newLiteralNode, out literalNode )) 
                return literalNode;
            literalNodes.Add(newLiteralNode, newLiteralNode);
            return newLiteralNode;
        }

        public ILiteralNode CreateLiteralNode(string s, string lang)
        {
           s = s.Trim('"','\'');
            ILiteralNode literalNode;
            var newLiteralNode = new SparqlLanguageLiteralNode(s, lang, CreateUriNode(SpecialTypes.LangString));
            if (literalNodes.TryGetValue(newLiteralNode, out literalNode))
                return literalNode;
            literalNodes.Add(newLiteralNode, newLiteralNode);
            return newLiteralNode;
        }

        public ILiteralNode CreateLiteralNode(int parse)
        {
            ILiteralNode literalNode;
            var newLiteralNode = new TypedLiteralNode(parse, CreateUriNode(SpecialTypes.Integer));
            if (literalNodes.TryGetValue(newLiteralNode, out literalNode))
                return literalNode;
            literalNodes.Add(newLiteralNode, newLiteralNode);
            return newLiteralNode;
        }

        public ILiteralNode CreateLiteralNode(decimal p)
        {
            ILiteralNode literalNode;
            var newLiteralNode = new TypedLiteralNode(p, CreateUriNode(SpecialTypes.Decimal));
            if (literalNodes.TryGetValue(newLiteralNode, out literalNode))
                return literalNode;
            literalNodes.Add(newLiteralNode, newLiteralNode);
            return newLiteralNode;
        }

        public ILiteralNode CreateLiteralNode(double p)
        {
            ILiteralNode literalNode;
            var newLiteralNode = new TypedLiteralNode(p, CreateUriNode(SpecialTypes.Double));
            if (literalNodes.TryGetValue(newLiteralNode, out literalNode))
                return literalNode;
            literalNodes.Add(newLiteralNode, newLiteralNode);
            return newLiteralNode;
        }

        public ILiteralNode CreateLiteralNode(bool p)
        {
            return p ? BoolLiteralNode.TrueNode(CreateUriNode(SpecialTypes.Bool)) : BoolLiteralNode.FalseNode(CreateUriNode(SpecialTypes.Bool));
        }

        public ILiteralNode CreateLiteralNode(string p, IUriNode sparqlUriNode)
        {
            p = p.Trim('"','\'');
            ILiteralNode literalNode;
            ILiteralNode newLiteralNode;
            if (sparqlUriNode ==  CreateUriNode(SpecialTypes.String))
                newLiteralNode = new LiteralofTypeStringNode(p, CreateUriNode(SpecialTypes.String));
            else if (sparqlUriNode ==  CreateUriNode(SpecialTypes.Date))
            {
                DateTime date;
                if(!DateTime.TryParse(p, out date)) throw new ArgumentException(p);
                newLiteralNode = new TypedLiteralNode(date, CreateUriNode(SpecialTypes.Date));
            }
            else if (sparqlUriNode ==  CreateUriNode(SpecialTypes.DateTime))
            {
                DateTimeOffset date;
                if (!DateTimeOffset.TryParse(p, out date)) throw new ArgumentException(p);
                newLiteralNode = new TypedLiteralNode(date, CreateUriNode(SpecialTypes.DateTime));
            }
            else if (sparqlUriNode == CreateUriNode(SpecialTypes.Bool))
            {
                bool b;
                if (!bool.TryParse(p, out b)) throw new ArgumentException(p);
                newLiteralNode = CreateLiteralNode(b);
            }
            else if (sparqlUriNode ==  CreateUriNode(SpecialTypes.Decimal))
            {
                decimal d;
                if (!decimal.TryParse(p.Replace(".", ","), out d)) throw new ArgumentException(p);
                newLiteralNode = new TypedLiteralNode(d, CreateUriNode(SpecialTypes.Decimal));
            }
            else if (sparqlUriNode ==  CreateUriNode(SpecialTypes.Double))
            {
                double d;
                if (!double.TryParse(p.Replace(".", ","), out d)) throw new ArgumentException(p);
                newLiteralNode = new TypedLiteralNode(d, CreateUriNode(SpecialTypes.Double));
            }
            else if (sparqlUriNode ==  CreateUriNode(SpecialTypes.Float))
            {
                float f;
                if (!float.TryParse(p.Replace(".",","), out f)) throw new ArgumentException(p);
                newLiteralNode = new TypedLiteralNode(f, CreateUriNode(SpecialTypes.Float));
            }
            else if (sparqlUriNode ==  CreateUriNode(SpecialTypes.Integer))
            {
                int i;
                if (!int.TryParse(p, out i)) throw new ArgumentException(p);
                newLiteralNode = new TypedLiteralNode(i, CreateUriNode(SpecialTypes.Integer));
            }
            else if (sparqlUriNode ==  CreateUriNode(SpecialTypes.DayTimeDuration))
            {
                TimeSpan i;
                if (!TimeSpan.TryParse(p, out i)) throw new ArgumentException(p);
                newLiteralNode = new TypedLiteralNode(i, CreateUriNode(SpecialTypes.DayTimeDuration));
            }
            else 
                newLiteralNode = new TypedLiteralNode(p, sparqlUriNode);

            if (literalNodes.TryGetValue(newLiteralNode, out literalNode))
                return literalNode;
            literalNodes.Add(newLiteralNode, newLiteralNode);
            return newLiteralNode;
        
        }

        public IBlankNode CreateBlankNode(string graph, string blankNodeString = null)
        {
            //if (graph != null) blankNodeString = graph + "/" + blankNodeString;
            if(blankNodeString==null)
                blankNodeString = "blank" + (long)(random.NextDouble() * 1000 * 1000 * 1000 * 1000);

            BlankNode blankNode;
            if (blankNodes.TryGetValue(blankNodeString, out blankNode)) return blankNode;
            blankNodes.Add(blankNodeString, blankNode = new BlankNode(blankNodeString));
            return blankNode;
        }

        private Random random = new Random();

        public IUriNode GetUri(string uri)
        {
            UriNode uriNode;
            return IriNodes.TryGetValue(uri, out uriNode) ? uriNode : null;
        }
    }
    
}