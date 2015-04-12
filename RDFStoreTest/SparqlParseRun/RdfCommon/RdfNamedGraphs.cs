using System.Collections.Generic;
using System.Linq;

namespace SparqlParseRun.RdfCommon
{
    public class RdfNamedGraphs :IStoreNamedGraphs
    {
       private readonly Dictionary<IUriNode,IGraph> named=new Dictionary<IUriNode, IGraph>();

        public IEnumerable<IGrouping<IUriNode, Triple>> GetTriplesFromGraphs(DataSet graphs)
        {
            return graphs.Where(dataset => Named.ContainsKey(dataset))
                .Select(dataset => new Grouping<IUriNode, Triple>(dataset, Named[dataset].GetTriples()));
        }

    
        public IEnumerable<IGrouping<IUriNode, INode>> GetTriplesWithSubjectPredicateFromGraphs(ISubjectNode subjectNode, IUriNode predicateNode, DataSet graphs)
        {    
            return graphs.Where(dataset => Named.ContainsKey(dataset))
                .Select(dataset => new Grouping<IUriNode, INode>(dataset, Named[dataset].GetTriplesWithSubjectPredicate(subjectNode, predicateNode)));
        }

        public IEnumerable<IGrouping<IUriNode, IUriNode>> GetTriplesWithSubjectObjectFromGraphs(ISubjectNode subjectNode, INode objectNode, DataSet graphs)
        {
            return graphs.Where(dataset => Named.ContainsKey(dataset))
              .Select(dataset => new Grouping<IUriNode, IUriNode>(dataset, Named[dataset].GetTriplesWithSubjectObject(subjectNode,objectNode)));
        }

        public IEnumerable<IGrouping<IUriNode, Triple>> GetTriplesWithSubjectFromGraphs(ISubjectNode subjectNode, DataSet graphs)
        {
            return graphs.Where(dataset => Named.ContainsKey(dataset))
              .Select(dataset => new Grouping<IUriNode, Triple>(dataset, Named[dataset].GetTriplesWithSubject(subjectNode)));
        }

        public IEnumerable<IGrouping<IUriNode, ISubjectNode>> GetTriplesWithPredicateObjectFromGraphs(IUriNode predicateNode, INode objectNode, DataSet graphs)
        {
            return graphs.Where(dataset => Named.ContainsKey(dataset))
              .Select(dataset => new Grouping<IUriNode, ISubjectNode>(dataset,  Named[dataset].GetTriplesWithPredicateObject(predicateNode, objectNode)));
        }

        public IEnumerable<IGrouping<IUriNode, Triple>> GetTriplesWithPredicateFromGraphs(IUriNode predicateNode, DataSet graphs)
        {
            return graphs.Where(dataset => Named.ContainsKey(dataset))
              .Select(dataset => new Grouping<IUriNode, Triple>(dataset,  Named[dataset].GetTriplesWithPredicate(predicateNode)));
        }

        public IEnumerable<IGrouping<IUriNode, Triple>> GetTriplesWithObjectFromGraphs(INode objectNode, DataSet graphs)
        {
            return  graphs.Where(dataset => Named.ContainsKey(dataset))
              .Select(dataset => new Grouping<IUriNode, Triple>(dataset,  Named[dataset].GetTriplesWithObject(objectNode)));
        }

        public IGraph CreateGraph(IUriNode sparqlUriNode)
        {
            if (sparqlUriNode==null || Named.ContainsKey(sparqlUriNode)) throw new ContainsGraphWhenCreatingException(sparqlUriNode);
            var rdfInMemoryGraph = new RamListOfTriplesGraph(sparqlUriNode.UriString);
            Named.Add(sparqlUriNode, rdfInMemoryGraph);
            return rdfInMemoryGraph;
        }

        public bool Contains(ISubjectNode subject, IUriNode predicate, INode @object, DataSet graphs)
        {
            IGraph g;
            return graphs.Any(uri => Named.TryGetValue(uri, out g) && g.Contains(subject, predicate, @object));
        }

        public void DropGraph(IUriNode updateGraph)
        {
            if (!Named.ContainsKey(updateGraph))
                throw new NoGraphExeption(updateGraph);
            Named.Remove(updateGraph);
        }



        public void Clear(IUriNode uri)
        {
            IGraph g;
            if (!Named.TryGetValue(uri, out g))
                throw new NoGraphExeption(uri);
            g.Clear();
        }



        public bool ContainsGraph(IUriNode uri)
        {
            return Named.ContainsKey(uri);
        }
        public void Delete(IUriNode uri, IEnumerable<Triple> triples)
        {
           Named[uri].Delete(triples);
        }
        public void Insert(IUriNode name, IEnumerable<Triple> triples)
        {
            if(name==null) ((IGraph)this).Insert(triples);
            else  Named[name].Insert(triples);
        }

        public IGraph TryGetGraph(IUriNode graphUriNode)
        {
            throw new System.NotImplementedException();
        }


        public DataSet GetGraphs(ISubjectNode sValue, IUriNode pValue, INode oValue, DataSet graphsSeq)
        {
           var graphs = graphsSeq.ToArray();
            IGraph graph;
            return new DataSet(
                graphs.Length == 0
                    ? Named.Where(g => g.Value.Contains(sValue, pValue, oValue)).Select(pair => pair.Key)
                    : graphs.Where(g => Named.TryGetValue(g, out graph) && graph.Contains(sValue, pValue, oValue)));
        }

        public void AddGraph(IUriNode to, IGraph fromGraph)
        {
            named.Add(to, fromGraph);
        }

        public void ReplaceGraph(IUriNode to, IGraph graph)
        {
            named.Add(to, graph);
        }

        public IEnumerable<KeyValuePair<IUriNode, long>> GetAllGraphCounts()
        {
            return Named.Select(pair => new KeyValuePair<IUriNode, long>(pair.Key, pair.Value.GetTriplesCount()));
        }

        public void ClearAllNamedGraphs()
        {
            named.Clear();
        }

        public Dictionary<IUriNode, IGraph> Named { get { return named; } }

        public IGraph GetGraph(IUriNode uri)
        {
            IGraph g;
            if (!Named.TryGetValue(uri, out g))
                throw new NoGraphExeption(uri);
            return g;
        }

        public IEnumerable<ISubjectNode> GetAllSubjects(IUriNode graphUri)
        {
            throw new System.NotImplementedException();
        }

        public bool Any(IUriNode graphUri)
        {
            throw new System.NotImplementedException();
        }
    }
}