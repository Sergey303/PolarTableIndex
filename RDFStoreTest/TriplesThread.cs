using System.Collections.Generic;
using System.IO;
using System.Linq;
using PolarDB;
using RdfStoreSparqlNamespace;
using SparqlParseRun.RdfCommon;
using System;
public class TriplesThread : IStore
{
    Action<ISubjectNode, IUriNode, INode> ForeachTriple;
    StringNodeGenerator nodeGenerator = StringNodeGenerator.Create();
    public TriplesThread(Action<ISubjectNode, IUriNode, INode> ForeachTriple)
    {
        this.ForeachTriple = ForeachTriple;
    }
    public void ClearAll()
    {
        throw new NotImplementedException();
    }

    public void Close()
    {
        throw new NotImplementedException();
    }

    public IStoreNamedGraphs NamedGraphs
    {
        get { throw new NotImplementedException(); }
    }

    public void Add(Triple t)
    {
        ForeachTriple(t.Subject,  t.Predicate, t.Object);        
    }

    public void Add(ISubjectNode s, IUriNode p, INode o)
    {
        ForeachTriple(s, p, o);
    }

    public bool Any()
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public bool Contains(ISubjectNode subject, IUriNode predicate, INode node)
    {
        throw new NotImplementedException();
    }

    public void Delete(IEnumerable<Triple> triples)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ISubjectNode> GetAllSubjects()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Triple> GetTriples()
    {
        throw new NotImplementedException();
    }

    public long GetTriplesCount()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Triple> GetTriplesWithObject(INode n)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Triple> GetTriplesWithPredicate(IUriNode n)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ISubjectNode> GetTriplesWithPredicateObject(IUriNode pred, INode obj)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Triple> GetTriplesWithSubject(ISubjectNode n)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IUriNode> GetTriplesWithSubjectObject(ISubjectNode subj, INode obj)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<INode> GetTriplesWithSubjectPredicate(ISubjectNode subj, IUriNode pred)
    {
        throw new NotImplementedException();
    }

    public void Insert(IEnumerable<Triple> triples)
    {
        throw new NotImplementedException();
    }

    public string Name
    {
        get { throw new NotImplementedException(); }
    }

    public INodeGenerator NodeGenerator
    {
        get { return nodeGenerator; }
    }
   

}
