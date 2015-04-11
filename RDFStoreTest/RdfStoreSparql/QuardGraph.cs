using System.Collections.Generic;
using System.Linq;
using SparqlParseRun.RdfCommon;


    public class QuadGraph : IGraph
    {
        protected IUriNode graphUri;
        protected DataSet thisDataSet;
        protected IStore store;

        public QuadGraph(IUriNode graphUri, IStore store)
        {
            this.graphUri = graphUri;
            this.store = store;
            thisDataSet=new DataSet(){graphUri};
        }

        protected QuadGraph()
        {
        }

        public string Name { get { return graphUri.ToString(); } }
        public INodeGenerator NodeGenerator { get { return store.NodeGenerator; } }

        public IEnumerable<Triple> GetTriplesWithObject(INode n)
        {
            return store.NamedGraphs.GetTriplesWithObjectFromGraphs(n,thisDataSet).SelectMany(g=>g);
        }

        public IEnumerable<Triple> GetTriplesWithPredicate(IUriNode n)
        {
            return store.NamedGraphs.GetTriplesWithPredicateFromGraphs(n, thisDataSet).SelectMany(g => g);
        }

        public IEnumerable<Triple> GetTriplesWithSubject(ISubjectNode n)
        {
            return store.NamedGraphs.GetTriplesWithSubjectFromGraphs(n, thisDataSet).SelectMany(g => g);
        }

        public IEnumerable<INode> GetTriplesWithSubjectPredicate(ISubjectNode subj, IUriNode pred)
        {
            return store.NamedGraphs.GetTriplesWithSubjectPredicateFromGraphs(subj, pred, thisDataSet).SelectMany(g => g);
        }

        public IEnumerable<IUriNode> GetTriplesWithSubjectObject(ISubjectNode subj, INode obj)
        {
            return store.NamedGraphs.GetTriplesWithSubjectObjectFromGraphs(subj, obj, thisDataSet).SelectMany(g => g);
        }

        public IEnumerable<ISubjectNode> GetTriplesWithPredicateObject(IUriNode pred, INode obj)
        {
            return store.NamedGraphs.GetTriplesWithPredicateObjectFromGraphs(pred, obj, thisDataSet).SelectMany(g => g);

        }

        public IEnumerable<Triple> GetTriples()
        {
            return store.NamedGraphs.GetTriplesFromGraphs(thisDataSet).SelectMany(g => g);     
        } 
      

        public IEnumerable<ISubjectNode> GetAllSubjects()
        {
            return store.NamedGraphs.GetAllSubjects(graphUri);
        }

        public long GetTriplesCount()
        {
            return GetTriples().Count();
        }

        public bool Any()
        {
            return store.NamedGraphs.Any(graphUri);
        }

        #region Graph edit
      
        public void Clear()
        {
            store.NamedGraphs.Clear(graphUri);
        }
        public void Add(ISubjectNode s, IUriNode p, INode o)
        {
           Add(new Triple(s,p,o));

        }

        public void Insert(IEnumerable<Triple> triples)
        {
            store.NamedGraphs.Insert(graphUri, triples);
        }

        public void Add(Triple t)
        {
            Insert(Enumerable.Repeat(t,1));
        }

        public bool Contains(ISubjectNode subject, IUriNode predicate, INode obj)
        {
            return store.NamedGraphs.Contains(subject, predicate, obj, thisDataSet);
        }

        public void Delete(IEnumerable<Triple> triples)
        {
            store.NamedGraphs.Delete(graphUri, triples);
        } 
        #endregion

    }
