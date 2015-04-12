using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using SparqlParseRun.RdfCommon;
using SparqlParseRun.SparqlClasses.GraphPattern.Triples.Node;
using SparqlParseRun.SparqlClasses.Query.Result;

namespace SparqlParseRun.SparqlClasses.GraphPattern
{
    public class SparqlServicePattern : ISparqlGraphPattern
    {
        private bool isSilent;
        private IUriNode uri;
        private string sparqlGraphPattern;
        private string prolog;
        private RdfQuery11Translator q;

        public void IsSilent()
        {
            this.isSilent = true;
        }

        internal void Create(IUriNode sparqlNode, string graphPattern, string prolog, RdfQuery11Translator q)
        {
            this.q = q;
            this.prolog = prolog;
            this.sparqlGraphPattern = graphPattern;
            this.uri = sparqlNode;
        }

        public IEnumerable<SparqlResult> Run(IEnumerable<SparqlResult> variableBindings)
        {
            HashSet<VariableNode> vars = new HashSet<VariableNode>();
            var bindings = variableBindings as SparqlResult[] ?? variableBindings.ToArray();
            foreach (var pair in bindings.SelectMany(variableBinding => variableBinding.row.Where(pair => !vars.Contains(pair.Key))))
                vars.Add(pair.Key);
            // string s = string.Format("VALUES ({0})", variableBindings.SelectMany(r=>r.row.Values).Select(binding => binding.));
            SparqlVariableBinding b;

            string query = string.Format("{0} SELECT * WHERE {1}   VALUES ({2}) {{ {3} }}", 
                prolog, sparqlGraphPattern, string.Join(" ", vars.Select(v => v.VariableName)),
                string.Join(Environment.NewLine, 
                        bindings.Select(variableBinding =>  "(" + string.Join(" ", 
                            vars.Select(var => variableBinding.row.TryGetValue(var, out b) ? b.Value.ToString() : "UNDEF"))
                                                        + ")")));                   
           
                try
                {
                    return Download(bindings, query).ToArray();
                }
                catch (Exception)
                {
                    if (isSilent) return Enumerable.Empty<SparqlResult>();
                    throw;
                }
               //TODO blank nodes
        }

        private IEnumerable<SparqlResult> Download(IEnumerable<SparqlResult> variableBindings, string query)
        {
            XNamespace xn = "http://www.w3.org/2005/sparql-results#";
            SparqlVariableBinding uriFromVar = null;
            var variableUri = uri as VariableNode;
            if (variableUri != null)
                foreach (var result in variableBindings
                    .Where(
                        binding => binding.row.TryGetValue(variableUri, out uriFromVar) && uriFromVar.Value is IUriNode)
                    .SelectMany(sourceBinding => XElement.Load(((IUriNode) uriFromVar.Value).UriString + "?query=" + query)
                        .Element(xn + "results")
                        .Elements()
                        .Select(xResult => new SparqlResult(xResult.Elements()
                            .Select(xb =>
                                new SparqlVariableBinding(new VariableNode(xb.Attribute("name").Value),
                                    Xml2Node(xn, xb.Elements().FirstOrDefault())))
                            .ToDictionary(b1 => b1.Variable)))))
                    yield return result;
            else
            {
                using (WebClient wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/sparql-query"; //"query="+ 
                    string HtmlResult = wc.UploadString(uri.UriString, query);


                    var load = XElement.Parse(HtmlResult);

                    foreach (var result in load
                        .Element(xn + "results")
                        .Elements()
                        .Select(xResult => new SparqlResult(xResult.Elements()
                            .Select(xb =>
                            {
                                var variable = q.GetVariable(xb.Attribute(xn + "name").Value);
                                var node = xb.Elements().FirstOrDefault();
                                return new SparqlVariableBinding(variable,
                                    Xml2Node(xn, node));
                            })
                            .ToDictionary(b1 => b1.Variable))))
                        //if(result.row.Values.All(b => ! sourceBinding.row.ContainsKey(b.Variable) ))
                        yield return result;
                }
            }
        }

        private INode Xml2Node(XNamespace xn, XElement b)
        {
            if (b.Name == xn + "uri")
            {
                return q.Store.NodeGenerator.CreateUriNode(q.prolog.GetFromString(b.Value));
            }
            else if (b.Name == xn + "bnode")
            {
                return q.CreateBlankNode(b.Value);
            }
            else if (b.Name == xn + "literal")
            {
                var lang = b.Attribute(xn + "lang");
                var type = b.Attribute(xn + "type");
                if (lang != null)
                    return q.Store.NodeGenerator.CreateLiteralNode(b.Value, lang.Value);
                else if (type != null)
                    return q.Store.NodeGenerator.CreateLiteralNode(b.Value, q.Store.NodeGenerator.CreateUriNode(q.prolog.GetFromString(type.Value)));
                else return q.Store.NodeGenerator.CreateLiteralNode(b.Value);
            }
            throw new ArgumentOutOfRangeException();
        }

public SparqlGraphPatternType PatternType { get{return SparqlGraphPatternType.Federated;} }
    }
}