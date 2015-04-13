using System;                                            
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using SparqlParseRun.RdfCommon;

namespace SparqlParseRun
{
    public class RDF2DictionaryGraph : RamNodeGenerator, IGraph
    {
        readonly Dictionary<ISubjectNode, Dictionary<IUriNode, HashSet<INode>>> objectBySubjectByPredicate =new Dictionary<ISubjectNode, Dictionary<IUriNode, HashSet<INode>>>();
        readonly Dictionary<INode, Dictionary<IUriNode,HashSet<ISubjectNode>>> subjectByObjectByPredicate =new Dictionary<INode, Dictionary<IUriNode, HashSet<ISubjectNode>>>();
        private INodeGenerator nodeGenerator;

        public RDF2DictionaryGraph(string name)
        {
            Name = name; //Guid.NewGuid().ToString();            
        }
        public string Name { get; private set; }
        public INodeGenerator NodeGenerator { get{return this; } }

        public void Clear()
        {
            objectBySubjectByPredicate.Clear();
            subjectByObjectByPredicate.Clear();
        }

     
        public IEnumerable<Triple> GetTriplesWithObject(INode n)
        {
            Dictionary<IUriNode, HashSet<ISubjectNode>> byPredicate;
            if (!subjectByObjectByPredicate.TryGetValue(n, out byPredicate)) yield break;
            foreach (var kv in byPredicate)
                foreach (var subjectNode in kv.Value)
                    yield return new Triple(subjectNode, kv.Key, n);
        }

        public IEnumerable<Triple> GetTriplesWithPredicate(IUriNode n)
        {
            HashSet<INode> objects = null;
            foreach (KeyValuePair<ISubjectNode, Dictionary<IUriNode, HashSet<INode>>> kv in objectBySubjectByPredicate)
            {
                if (kv.Value.TryGetValue(n, out objects))
                    foreach (INode o in objects)
                        yield return new Triple(kv.Key, n, o);
            }
        }

        public IEnumerable<Triple> GetTriplesWithSubject(ISubjectNode n)
        {
            Dictionary<IUriNode, HashSet<INode>> byPredicate;
            if (!objectBySubjectByPredicate.TryGetValue(n, out byPredicate)) yield break;
            foreach (var kv in byPredicate)
                foreach (var objNode in kv.Value)
                    yield return new Triple(n, kv.Key, objNode);
        }

        public IEnumerable<INode> GetTriplesWithSubjectPredicate(ISubjectNode subj, IUriNode pred)
        {
            Dictionary<IUriNode, HashSet<INode>> byPredicate;
            HashSet<INode> objects;
            if (objectBySubjectByPredicate.TryGetValue(subj, out byPredicate) &&
                byPredicate.TryGetValue(pred, out objects))
                return objects;
            return Enumerable.Empty<INode>();
        }

        public IEnumerable<IUriNode> GetTriplesWithSubjectObject(ISubjectNode subj, INode obj)
        {
        
            Dictionary<IUriNode, HashSet<INode>> byPredicate;
            if (objectBySubjectByPredicate.TryGetValue(subj, out byPredicate))
                return byPredicate.Where(pair => pair.Value.Contains(obj)).Select(pair => pair.Key);  
            return Enumerable.Empty<IUriNode>();
        }

        public IEnumerable<ISubjectNode> GetTriplesWithPredicateObject(IUriNode pred, INode obj)
        {
            Dictionary<IUriNode, HashSet<ISubjectNode>> byPredicate;
            HashSet<ISubjectNode> subjects;
            if (subjectByObjectByPredicate.TryGetValue(obj, out byPredicate) &&
                byPredicate.TryGetValue(pred, out subjects))
                return subjects;
            return Enumerable.Empty<ISubjectNode>();
        }

        public IEnumerable<Triple> GetTriples()
        {
            return
                objectBySubjectByPredicate.SelectMany(
                    sp => sp.Value.SelectMany(pair => pair.Value.Select(obj => new Triple(sp.Key, pair.Key, obj))));
        }

        public void Add(ISubjectNode s, IUriNode p, INode o)
        {
            Dictionary<IUriNode, HashSet<INode>> byPredicate;
            if (objectBySubjectByPredicate.TryGetValue(s, out byPredicate))
            {
                HashSet<INode> objects;
                if (byPredicate.TryGetValue(p, out objects))
                    objects.Add(o);
                else byPredicate.Add(p, new HashSet<INode> {o});
            }   else objectBySubjectByPredicate.Add(s, new Dictionary<IUriNode, HashSet<INode>> {{p, new HashSet<INode>(){o}}});

            Dictionary<IUriNode, HashSet<ISubjectNode>> byPredicate2;
            if (subjectByObjectByPredicate.TryGetValue(o, out byPredicate2))
            {
                HashSet<ISubjectNode> subjects;
                if (byPredicate2.TryGetValue(p, out subjects))
                    subjects.Add(s);
                else byPredicate2.Add(p, new HashSet<ISubjectNode> {s});
            }   else subjectByObjectByPredicate.Add(o, new Dictionary<IUriNode, HashSet<ISubjectNode>>(){{p, new HashSet<ISubjectNode>(){s}}});
        }

        public void LoadFrom(IUriNode @from)
        {
      //      WebRequest request = WebRequest.Create(@from.Uri);
          
         
            WebClient wc = new WebClient();
          var gs=  wc.DownloadString(@from.UriString);
            switch (Path.GetExtension(from.UriString).ToLower())
            {
                case".ttl":
                {
                    TurtleParser.FromTurtle(this, gs);
                }
                    return;
                default :
                    throw new NotImplementedException();
            }
        }

        public void Insert(IEnumerable<Triple> triples)
        {
            foreach (var triple in triples)
                Add(triple.Subject, triple.Predicate, triple.Object);
        }

        public void Add(Triple t)
        {
            Add(t.Subject, t.Predicate, t.Object);
        }

        public bool Contains(ISubjectNode subject, IUriNode predicate, INode node)
        {
            Dictionary<IUriNode, HashSet<INode>> byPredicate;
            HashSet<INode> objects;
            return objectBySubjectByPredicate.TryGetValue(subject, out byPredicate) &&
                   byPredicate.TryGetValue(predicate, out objects) && objects.Contains(node);
        }

        public void Delete(IEnumerable<Triple> triples)
        {
            foreach (var triple in triples)
            {
                Dictionary<IUriNode, HashSet<INode>> byPredicate;
                if (objectBySubjectByPredicate.TryGetValue(triple.Subject, out byPredicate))
                {
                    HashSet<INode> objects;
                    if (byPredicate.TryGetValue(triple.Predicate, out objects))
                    {
                        objects.Remove(triple.Object);
                        if (objects.Count == 0)
                        {
                            byPredicate.Remove(triple.Predicate);
                            if (byPredicate.Count == 0)
                                objectBySubjectByPredicate.Remove(triple.Subject);
                        }
                    }
                }
                Dictionary<IUriNode, HashSet<ISubjectNode>> byPredicate2;
                HashSet<ISubjectNode> subjects;
                if (!subjectByObjectByPredicate.TryGetValue(triple.Object, out byPredicate2) ||
                    !byPredicate2.TryGetValue(triple.Predicate, out subjects)) continue;
                subjects.Remove(triple.Subject);
                if (subjects.Count != 0) continue;
                byPredicate2.Remove(triple.Predicate);
                if (byPredicate2.Count != 0) continue;
                subjectByObjectByPredicate.Remove(triple.Object);
            }
        }

   

        public IEnumerable<ISubjectNode> GetAllSubjects()
        {
            return objectBySubjectByPredicate.Keys;
        }

        public long GetTriplesCount()
        {
            return objectBySubjectByPredicate.Sum(pair => pair.Value.Count);
        }

        public bool Any()
        {
            throw new NotImplementedException();
        }
    }
}