using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using SparqlParseRun.RdfCommon;

namespace SparqlParseRun
{
    public class RamDictionaryGraph : RamNodeGenerator, IGraph
    {
        readonly Dictionary<INode, KeyValuePair<Dictionary<IUriNode, HashSet<INode>>, Dictionary<IUriNode, HashSet<ISubjectNode>>>> items = new Dictionary<INode, KeyValuePair<Dictionary<IUriNode, HashSet<INode>>, Dictionary<IUriNode, HashSet<ISubjectNode>>>>();

        public RamDictionaryGraph()
        {
            Name = Guid.NewGuid().ToString();            
        }

        public string Name { get; private set; }
        public INodeGenerator NodeGenerator { get; private set; }

        public void Clear()
        {
            items.Clear();
        }

     
        public IEnumerable<Triple> GetTriplesWithObject(INode n)
        {
            KeyValuePair<Dictionary<IUriNode, HashSet<INode>>, Dictionary<IUriNode, HashSet<ISubjectNode>>> byPredicate;
            if (!items.TryGetValue(n, out byPredicate)) yield break;
            foreach (var kv in byPredicate.Value)
                foreach (var subjectNode in kv.Value)
                    yield return new Triple(subjectNode, kv.Key, n);
        }

        public IEnumerable<Triple> GetTriplesWithPredicate(IUriNode n)
        {
            HashSet<INode> objects = null;
            foreach (KeyValuePair<INode, KeyValuePair<Dictionary<IUriNode, HashSet<INode>>, Dictionary<IUriNode, HashSet<ISubjectNode>>>> kv in items)
            {
                if (kv.Value.Key.TryGetValue(n, out objects))
                    foreach (INode o in objects)
                        yield return new Triple((ISubjectNode) kv.Key, n, o);
              
            }
        }

        public IEnumerable<Triple> GetTriplesWithSubject(ISubjectNode n)
        {
            KeyValuePair<Dictionary<IUriNode, HashSet<INode>>, Dictionary<IUriNode, HashSet<ISubjectNode>>> byPredicate;
            if (!items.TryGetValue(n, out byPredicate)) yield break;
            foreach (var kv in byPredicate.Key)
                foreach (var objNode in kv.Value)
                    yield return new Triple(n, kv.Key, objNode);
        }

        public IEnumerable<INode> GetTriplesWithSubjectPredicate(ISubjectNode subj, IUriNode pred)
        {
            KeyValuePair<Dictionary<IUriNode, HashSet<INode>>, Dictionary<IUriNode, HashSet<ISubjectNode>>> byPredicate;
            HashSet<INode> objects;
            if (items.TryGetValue(subj, out byPredicate) &&
                byPredicate.Key.TryGetValue(pred, out objects))
                return objects;
            return Enumerable.Empty<INode>();
        }

        public IEnumerable<IUriNode> GetTriplesWithSubjectObject(ISubjectNode subj, INode obj)
        {
        
            KeyValuePair<Dictionary<IUriNode, HashSet<INode>>, Dictionary<IUriNode, HashSet<ISubjectNode>>> byPredicate;
            if (items.TryGetValue(subj, out byPredicate))
                return byPredicate.Key.Where(pair => pair.Value.Contains(obj)).Select(pair => pair.Key);  
            return Enumerable.Empty<IUriNode>();
        }

        public IEnumerable<ISubjectNode> GetTriplesWithPredicateObject(IUriNode pred, INode obj)
        {
            KeyValuePair<Dictionary<IUriNode, HashSet<INode>>, Dictionary<IUriNode, HashSet<ISubjectNode>>> byPredicate;
            HashSet<ISubjectNode> subjects;
            if (items.TryGetValue(obj, out byPredicate) &&
                byPredicate.Value.TryGetValue(pred, out subjects))
                return subjects;
            return Enumerable.Empty<ISubjectNode>();
        }

        public IEnumerable<Triple> GetTriples()
        {
            return
                items.SelectMany(
                    sp => sp.Value.Key.SelectMany(pair => pair.Value.Select(obj => new Triple((ISubjectNode) sp.Key, pair.Key, obj))));
        }

        public void Add(ISubjectNode s, IUriNode p, INode o)
        {
            KeyValuePair<Dictionary<IUriNode, HashSet<INode>>, Dictionary<IUriNode, HashSet<ISubjectNode>>> byPredicate;
            if (items.TryGetValue(s, out byPredicate))
            {
                HashSet<INode> objects;
                if (byPredicate.Key.TryGetValue(p, out objects))
                    objects.Add(o);
                else byPredicate.Key.Add(p, new HashSet<INode> {o});
            }   else items.Add(s, new KeyValuePair<Dictionary<IUriNode, HashSet<INode>>, Dictionary<IUriNode, HashSet<ISubjectNode>>>(new Dictionary<IUriNode, HashSet<INode>>(){{p, new HashSet<INode>(){o}}},new Dictionary<IUriNode, HashSet<ISubjectNode>>() ));

            KeyValuePair<Dictionary<IUriNode, HashSet<INode>>, Dictionary<IUriNode, HashSet<ISubjectNode>>> byPredicate2;
            if (items.TryGetValue(o, out byPredicate2))
            {
                HashSet<ISubjectNode> subjects;
                if (byPredicate2.Value.TryGetValue(p, out subjects))
                    subjects.Add(s);
                else byPredicate2.Value.Add(p, new HashSet<ISubjectNode> {s});
            }
            else items.Add(o, new KeyValuePair<Dictionary<IUriNode, HashSet<INode>>, Dictionary<IUriNode, HashSet<ISubjectNode>>>(new Dictionary<IUriNode, HashSet<INode>>(), new Dictionary<IUriNode, HashSet<ISubjectNode>>() { { p, new HashSet<ISubjectNode>() { s } } }));
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
            KeyValuePair<Dictionary<IUriNode, HashSet<INode>>, Dictionary<IUriNode, HashSet<ISubjectNode>>> byPredicate;
            HashSet<INode> objects;
            return items.TryGetValue(subject, out byPredicate) &&
                   byPredicate.Key.TryGetValue(predicate, out objects) && objects.Contains(node);
        }

        public void Delete(IEnumerable<Triple> triples)
        {
            foreach (var triple in triples)
            {
                KeyValuePair<Dictionary<IUriNode, HashSet<INode>>, Dictionary<IUriNode, HashSet<ISubjectNode>>> byPredicate;
                if (items.TryGetValue(triple.Subject, out byPredicate))
                {
                    HashSet<INode> objects;
                    if (byPredicate.Key.TryGetValue(triple.Predicate, out objects))
                    {
                        objects.Remove(triple.Object);
                        if (objects.Count == 0)
                        {
                            byPredicate.Key.Remove(triple.Predicate);
                            //if (byPredicate.Key.Count == 0)
                            //    items.Remove(triple.Subject);
                        }
                    }
                }
                KeyValuePair<Dictionary<IUriNode, HashSet<INode>>, Dictionary<IUriNode, HashSet<ISubjectNode>>> byPredicate2;
                HashSet<ISubjectNode> subjects;
                if (!items.TryGetValue(triple.Object, out byPredicate2) ||
                    !byPredicate2.Value.TryGetValue(triple.Predicate, out subjects)) continue;
                subjects.Remove(triple.Subject);
                if (subjects.Count != 0) continue;
                byPredicate2.Value.Remove(triple.Predicate);
                //if (byPredicate2.Value.Count != 0) continue;
                //items.Remove(triple.Predicate);
            }
        }

   

        public IEnumerable<ISubjectNode> GetAllSubjects()
        {
            return items.Keys.Where(node => node is ISubjectNode).Cast<ISubjectNode>();
        }

        public long GetTriplesCount()
        {
            return items.Sum(pair => pair.Value.Key.Count);
        }

        public bool Any()
        {
            throw new NotImplementedException();
        }
    }
}