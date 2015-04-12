using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using SparqlParseRun.RdfCommon;
using SparqlParseRun.RdfCommon.Literals;

namespace SparqlParseRun.SparqlClasses.Query.Result
{
    public static class RdfGraphSerialization
    {
        public static XElement ToXml(this IGraph g)
        {
            XNamespace rdf = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
            var prefixes = new Dictionary<string, string>();                    
                
            var RDF = new XElement(rdf + "RDF", new XAttribute(XNamespace.Xmlns + "rdf", rdf));
            int i = 0;
            foreach (var s in g.GetAllSubjects())
            {
                XAttribute id = null;
                switch (s.Type)
                {
                    case NodeType.Uri:
                        id = new XAttribute(rdf + "about", ((IUriNode) s).UriString);
                        break;
                    case NodeType.Blank:
                        id = new XAttribute(rdf + "nodeID", s.ToString());
                        break;
                    case NodeType.Variable:
                    case NodeType.Literal:
                    case NodeType.Undefined:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                RDF.Add(new XElement(rdf + "Description", id,
                    g.GetTriplesWithSubject(s).Select(t =>
                    {
                        string p;
                        string localName;
                        var ns = GetNsAndLocalName(t.Predicate.UriString, out localName);
                        if (!prefixes.TryGetValue(ns, out p))
                        {
                            prefixes.Add(ns, p="ns" + i++);
                            RDF.Add(new XAttribute(XNamespace.Xmlns+p, ns));
                        }
                        var x = new XElement(XName.Get(localName,p));
                        switch (t.Object.Type)
                        {
                            case NodeType.Uri:
                                x.Add(new XAttribute(rdf + "resource", ((IUriNode) t.Object).UriString));
                                break;
                            case NodeType.Literal:
                                switch (((ILiteralNode) t.Object).LiteralType)
                                {
                                    case LiteralType.TypedObject:
                                        x.Add(new XAttribute(rdf + "datatype", ((ILiteralNode) t.Object).DataType));
                                        break;
                                    case LiteralType.LanguageType:
                                        x.Add(new XAttribute(XNamespace.Xml + "lang",
                                            ((ILanguageLiteral) t.Object).Lang));
                                        break;
                                    case LiteralType.Simple:

                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                                x.Add(((ILiteralNode) t.Object).Content);

                                break;
                            case NodeType.Blank:
                                x.Add(new XAttribute(rdf + "nodeID", t.Object.ToString()));
                                break;
                            case NodeType.Variable:
                            case NodeType.Undefined:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        return x;
                    })));
            };
       
            return RDF;
        }

        private static string GetNsAndLocalName(string uri, out string localName)
        {
            var lastIndexOf1 = uri.LastIndexOf('\\');
            var lastIndexOf2 = uri.LastIndexOf('/');
            var lastIndexOf3 = uri.LastIndexOf('#');
            var lastIndex = Math.Max(lastIndexOf1, Math.Max(lastIndexOf2, lastIndexOf3));
            if(lastIndex==-1) throw new Exception();

            localName = uri.Substring(lastIndex+1);

            return uri.Substring(0, lastIndex);
        }

        public static string ToJson(this IGraph g)
        {       
            return string.Format("{{ {0} }}",
                string.Join(","+Environment.NewLine,
                    g.GetAllSubjects().Select(s => string.Format("{0} : {{ {1} }}",
                        s,
                        g.GetTriplesWithSubject(s).GroupBy(t => t.Predicate).Select(pGroup =>
                            pGroup.Count() > 1
                                ? string.Format("{0} : [{1}]", pGroup.Key,
                                    string.Join("," + Environment.NewLine, pGroup.Select(t => t.Object).Select(ToJson)))
                                : string.Format("{0} : {1}", pGroup.Key, ToJson(pGroup.First().Object)))))));
        }

        public static string ToJson(this INode b)
        {
            switch (b.Type)
            {
                case NodeType.Uri:
                    return "{ \"type\" : \"uri\", \"value\" : \"" + ((IUriNode)b).UriString + "\" }";
                    break;
                case NodeType.Literal:
                    var literalNode = ((ILiteralNode)b);
                    switch (literalNode.LiteralType)
                    {
                        case LiteralType.TypedObject:
                            return "{ \"type\" : \"literal\", \"value\" : \"" + literalNode.Content +
                                   "\", \"datatype\": \"" + literalNode.DataType.UriString + "\" }";
                            break;
                        case LiteralType.LanguageType:
                            return "{ \"type\" : \"literal\", \"value\" : \"" + literalNode.Content +
                                   "\", \"xml:lang\": \"" + ((ILanguageLiteral)literalNode).Lang + "\" }";
                            break;
                        case LiteralType.Simple:
                            return "{ \"type\" : \"literal\", \"value\" : \"" + literalNode.Content + "\" }";
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;

                case NodeType.Blank:
                    return "{ \"type\" : \"bnode\", \"value\" : \"" + b + "\" }";
                    break;
                case NodeType.Undefined:
                case NodeType.Variable:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static string ToTurtle(this IGraph g)
        {
            return
                string.Join("." + Environment.NewLine,
                    g.GetAllSubjects().Select(s =>
                        string.Format("{0} {1}", s,
                        string.Join(";" + Environment.NewLine,
                            g.GetTriplesWithSubject(s).GroupBy(t => t.Predicate).Select(pGroup =>
                                string.Format("{0} {1}", pGroup.Key,
                                    string.Join("," + Environment.NewLine, pGroup.Select(t => t.Object))))))));
        }

        public static void FromXml(this IGraph g, XElement x)
        {
            throw new NotImplementedException();
        }

        public static XElement ToXml(this IGraph g, Prologue prolog)
        {
            XNamespace rdf = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
            var prefixes = new Dictionary<string, string>();

            var RDF = new XElement(rdf + "RDF");
            int i = 0;
            foreach (var s in g.GetAllSubjects())
            {
                XAttribute id = null;
                switch (s.Type)
                {
                    case NodeType.Uri:
                        id = new XAttribute(rdf + "about", ((IUriNode)s).UriString);
                        break;
                    case NodeType.Blank:
                        id = new XAttribute(rdf + "nodeID", s.ToString());
                        break;
                    case NodeType.Variable:
                    case NodeType.Literal:
                    case NodeType.Undefined:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                RDF.Add(new XElement(rdf + "Description", id,
                    g.GetTriplesWithSubject(s).Select(t =>
                    {
                        string p;
                        var ns = prolog.SetThisPrefix(t.Predicate.UriPrefixed);
                        if (!prefixes.TryGetValue(ns.Namespace, out p))
                        {
                            prefixes.Add(ns.Namespace, p= ns.Prefix);
                            RDF.Add(new XAttribute(XNamespace.Xmlns + ns.Prefix.Substring(0, ns.Prefix.Length-1), ns));
                        }
                        var x = new XElement(XName.Get(ns.LocalName, p));
                        switch (t.Object.Type)
                        {
                            case NodeType.Uri:
                                x.Add(new XAttribute(rdf + "resource", ((IUriNode)t.Object).UriString));
                                break;
                            case NodeType.Literal:
                                switch (((ILiteralNode)t.Object).LiteralType)
                                {
                                    case LiteralType.TypedObject:
                                        x.Add(new XAttribute(rdf + "datatype", ((ILiteralNode)t.Object).DataType));
                                        break;
                                    case LiteralType.LanguageType:
                                        x.Add(new XAttribute(XNamespace.Xml + "lang",
                                            ((ILanguageLiteral)t.Object).Lang));
                                        break;
                                    case LiteralType.Simple:

                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                                x.Add(((ILiteralNode)t.Object).Content);

                                break;
                            case NodeType.Blank:
                                x.Add(new XAttribute(rdf + "nodeID", t.Object.ToString()));
                                break;
                            case NodeType.Variable:
                            case NodeType.Undefined:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        return x;
                    })));
            };

            return RDF; 
        }
    }
}