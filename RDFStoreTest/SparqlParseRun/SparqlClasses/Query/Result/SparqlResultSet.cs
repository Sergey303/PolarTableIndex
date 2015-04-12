using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using SparqlParseRun.RdfCommon;
using SparqlParseRun.RdfCommon.Literals;
using SparqlParseRun.SparqlClasses.GraphPattern.Triples.Node;
using SparqlParseRun.SparqlClasses.Update;

namespace SparqlParseRun.SparqlClasses.Query.Result
{
    public class SparqlResultSet
    {
        private readonly Prologue prolog;
        public IEnumerable<SparqlResult> Results = new List<SparqlResult>() {new SparqlResult()};
        public IGraph GraphResult;
        internal ResultType ResultType;

      

        public bool AnyResult
        {
            get { return Results.Any(); }
        }

        public string UpdateMessage;

        public SparqlUpdateStatus UpdateStatus;

        internal Dictionary<string, VariableNode> Variables = new Dictionary<string, VariableNode>();

        public SparqlResultSet(Prologue prolog)
        {
            this.prolog = prolog;
        }

        public XElement ToXml()
        {
            XNamespace xn = "http://www.w3.org/2005/sparql-results#";
            switch (ResultType)
            {
                case ResultType.Select:
                    return new XElement(xn + "sparql", new XAttribute(XNamespace.Xmlns + "ns", xn),
                        new XElement(xn + "head",
                            Variables.Select(v => new XElement(xn + "variable", new XAttribute(xn + "name", v.Key)))),
                        new XElement(xn + "results",
                            Results.Select(result =>
                                new XElement(xn + "result",
                                    result.row.Values.Select(binding => 
                                        new XElement(xn + "binding",    
                                            new XAttribute(xn + "name", binding.Variable.VariableName),
                                            BindingToXml(xn, binding.Value)))))));
                case ResultType.Describe:
                case ResultType.Construct:
                    return GraphResult.ToXml(prolog);
                case ResultType.Ask:
                    return new XElement(xn + "sparql", //new XAttribute(XNamespace.Xmlns , xn),
                        new XElement(xn + "head",
                            Variables.Select(v => new XElement(xn + "variable", new XAttribute(xn + "name", v.Key)))),
                        new XElement(xn + "boolean", AnyResult));
                case ResultType.Update:
                    return new XElement("update", new XAttribute("status", UpdateStatus.ToString()));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        //public JsonConvert ToJson()

        private XElement BindingToXml(XNamespace xn, INode b)
        {
            switch (b.Type)
            {
                case NodeType.Uri:
                    return new XElement(xn + "uri", b.ToString());
                    break;
                case NodeType.Literal:
                    var literalNode = ((ILiteralNode) b);
                    switch (literalNode.LiteralType)
                    {
                        case LiteralType.TypedObject:
                            return new XElement(xn + "literal", new XAttribute(xn + "type", (literalNode).DataType),
                                literalNode.Content);
                            break;
                        case LiteralType.LanguageType:
                            return new XElement(xn + "literal",
                                new XAttribute(xn + "lang", ((ILanguageLiteral) literalNode).Lang), literalNode.Content);
                            break;
                        case LiteralType.Simple:
                            return new XElement(xn + "literal", literalNode.Content);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;

                case NodeType.Blank:
                    return new XElement(xn + "bnode", b.ToString());
                    break;
                case NodeType.Undefined:
                case NodeType.Variable:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override string ToString()
        {
            switch (ResultType)
            {
                case ResultType.Describe:
                case ResultType.Construct:
                    return GraphResult.ToString();
                case ResultType.Select:
                    //  return Results.ag.ToString();
                case ResultType.Ask:
                    return AnyResult.ToString();

                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        public string ToJson()
        {
            string headVars;
            switch (ResultType)
            {
                case ResultType.Select:
                        headVars = string.Format(@"""head"": {{ ""vars"": [ {0} ] }}",
                            string.Join("," + Environment.NewLine,
                                Variables.Keys.Select(v => string.Format("\"{0}\"", v))));
                    return
                     string.Format(@"{{ {0}, ""results"": {{ ""bindings"" : [{1}] }} }}", headVars,
                         string.Join("," + Environment.NewLine, Results.Select(result => string.Format("{{{0}}}",
                             string.Join("," + Environment.NewLine,
                                 result.row.Values.Select(binding =>
                                     string.Format("\"{0}\" : {1}", binding.Variable.VariableName,
                                         binding.Value.ToJson())))))) );
                case ResultType.Describe:
                case ResultType.Construct:
                    return GraphResult.ToJson();
                case ResultType.Ask:
                    headVars = string.Format(@"""head"": {{ ""vars"": [ {0} ] }}",
                        string.Join("," + Environment.NewLine,
                            Variables.Keys.Select(v => string.Format("\"{0}\"", v))));
                    return string.Format("{{ {0}, \"boolean\" : {1}}}", headVars, AnyResult);
                case ResultType.Update:
                    headVars = string.Format(@"""head"": {{ ""vars"": [ {0} ] }}",
                        string.Join("," + Environment.NewLine,
                            Variables.Keys.Select(v => string.Format("\"{0}\"", v))));
                    return string.Format("{{ {0}, \"status\" : {1}}}", headVars, UpdateStatus);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    
    }


    internal enum ResultType
    {
        Describe, Select, Construct, Ask,
        Update
    }

   
}