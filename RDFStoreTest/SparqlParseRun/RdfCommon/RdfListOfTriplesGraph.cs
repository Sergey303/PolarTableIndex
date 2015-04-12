using System;
using System.Collections.Generic;
using System.Linq;

namespace SparqlParseRun.RdfCommon
{
    public class RamListOfTriplesGraph : RamNodeGenerator, IGraph
    {
        public RamListOfTriplesGraph(string name)
        {
          //  Name = Guid.NewGuid().ToString();
            Name = name;
        }
        private readonly List<Triple> triples=new List<Triple>();
        public IEnumerable<INode> GetTriplesWithSubjectPredicate(ISubjectNode subjectNode, IUriNode predicateNode)
        {
            return triples.Where(triple => triple.Subject .Equals( subjectNode) && triple.Predicate .Equals( predicateNode)).Select(triple => triple.Object);
        }

        public IEnumerable<IUriNode> GetTriplesWithSubjectObject(ISubjectNode subjectNode, INode objectNode)
        {
            return triples.Where(triple => triple.Subject .Equals( subjectNode) && triple.Object .Equals( objectNode)).Select(triple => triple.Predicate);

        }

        public IEnumerable<Triple> GetTriplesWithSubject(ISubjectNode subjectNode)
        {
            return triples.Where(triple => triple.Subject .Equals( subjectNode));
        }

        public IEnumerable<ISubjectNode> GetTriplesWithPredicateObject(IUriNode predicateNode, INode objectNode)
        {
            return triples.Where(triple => triple.Predicate .Equals( predicateNode) && triple.Object .Equals( objectNode)).Select(triple => triple.Subject);
        }

        public IEnumerable<Triple> GetTriplesWithPredicate(IUriNode predicateNode)
        {
            return triples.Where(triple => triple.Predicate .Equals( predicateNode));
        }

     

        public IEnumerable<Triple> GetTriplesWithObject(INode objectNode)
        {
            return triples.Where(triple => triple.Object .Equals( objectNode));
        }

        public IEnumerable<Triple> GetTriples()
        {
            return triples;
        }

        public bool Contains(ISubjectNode subject, IUriNode predicate, INode @object)
        {
           return triples.Any(triple => triple.Subject.Equals(subject) && triple.Predicate.Equals(predicate) && triple.Object.Equals(@object));
        }

        public void Delete(IEnumerable<Triple> ts)
        {
            foreach (var triple in ts)
                triples.Remove(triple);
        }

   

        public IEnumerable<ISubjectNode> GetAllSubjects()
        {
            return triples.Select(t => t.Subject).Distinct();
        }

        public long GetTriplesCount()
        {
            return triples.Count;
        }

        public bool Any()
        {
            throw new NotImplementedException();
        }


        public string Name { get; private set; }
        public INodeGenerator NodeGenerator { get; private set; }

        public void Clear()
        {
           triples.Clear();
        }

    
        public void Add(ISubjectNode s, IUriNode p, INode o)
        {
           triples.Add(new Triple(s,p,o));
        }

    
        //public void LoadFrom(IUriNode @from)
        //{
        //    throw new NotImplementedException();
        //}


        

        public void Insert(IEnumerable<Triple> triples)
        {
          this.triples.AddRange(triples);
        }

        public void Add(Triple t)
        {
         triples.Add(t);
        }

        public void AddRange(IEnumerable<Triple> triples)
        {
            this.triples.AddRange(triples);
        }
    }
}
