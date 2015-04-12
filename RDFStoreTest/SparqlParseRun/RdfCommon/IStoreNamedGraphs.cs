using System.Collections.Generic;
using System.Linq;

namespace SparqlParseRun.RdfCommon
{
    public interface IStoreNamedGraphs
    {
        IEnumerable<IGrouping<IUriNode, INode>> GetTriplesWithSubjectPredicateFromGraphs(ISubjectNode subjectNode, IUriNode predicateNode, DataSet graphs);
        IEnumerable<IGrouping<IUriNode, IUriNode>> GetTriplesWithSubjectObjectFromGraphs(ISubjectNode subjectNode, INode objectNode, DataSet graphs);
        IEnumerable<IGrouping<IUriNode, Triple>> GetTriplesWithSubjectFromGraphs(ISubjectNode subjectNode, DataSet graphs);
        IEnumerable<IGrouping<IUriNode, ISubjectNode>> GetTriplesWithPredicateObjectFromGraphs(IUriNode predicateNode, INode objectNode, DataSet graphs);
        IEnumerable<IGrouping<IUriNode, Triple>> GetTriplesWithPredicateFromGraphs(IUriNode predicateNode, DataSet graphs);
        IEnumerable<IGrouping<IUriNode, Triple>> GetTriplesWithObjectFromGraphs(INode objectNode, DataSet graphs);
        IEnumerable<IGrouping<IUriNode, Triple>> GetTriplesFromGraphs(DataSet graphs);
        IGraph CreateGraph(IUriNode sparqlUriNode);
        bool Contains(ISubjectNode sValue, IUriNode pValue, INode oValue, DataSet graphs);
        void  DropGraph(IUriNode sparqlGrpahRefTypeEnum);
        void Clear(IUriNode uri);
       
        void Delete(IUriNode g, IEnumerable<Triple> triples);
        void Insert(IUriNode name, IEnumerable<Triple> triples);

      //  IGraph TryGetGraph(IUriNode graphUriNode);

        DataSet GetGraphs(ISubjectNode sValue, IUriNode pValue, INode oValue, DataSet graphs);
       //  Dictionary<IUriNode,IGraph> Named { get;  }
        void AddGraph(IUriNode to, IGraph fromGraph);
        void ReplaceGraph(IUriNode to, IGraph graph);
        IEnumerable<KeyValuePair<IUriNode, long>> GetAllGraphCounts();
        void ClearAllNamedGraphs();
       // bool ContainsGraph(IUriNode to);

        IGraph GetGraph(IUriNode graphUriNode);
        IEnumerable<ISubjectNode> GetAllSubjects(IUriNode graphUri);
        bool Any(IUriNode graphUri);
    }
}