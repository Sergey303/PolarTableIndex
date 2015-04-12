using System;
using System.Collections.Generic;
using System.Linq;
using SparqlParseRun.RdfCommon;
using SparqlParseRun.SparqlClasses.GraphPattern.Triples.Node;
using SparqlParseRun.SparqlClasses.Query.Result;

namespace SparqlParseRun.SparqlClasses.GraphPattern.Triples.Path
{
    public class SparqlPathManyTriple : SparqlTriple, ISparqlGraphPattern
    {
        private readonly SparqlPathTranslator predicatePath;
        private Dictionary<INode, HashSet<INode>> bothVariablesCacheBySubject, bothVariablesCacheByObject;
        private KeyValuePair<INode, INode>[] bothVariablesChache;

        public SparqlPathManyTriple(INode subj, SparqlPathTranslator pred, INode obj, RdfQuery11Translator q) : base(subj,pred,obj,q)
        {
            predicatePath =pred;
        }

        public override IEnumerable<SparqlResult> Run(IEnumerable<SparqlResult> variableBindings)
        {
            var bindings = variableBindings as SparqlResult[] ?? variableBindings.ToArray();




            Queue<INode> newSubjects = new Queue<INode>();
            INode[] fromVariable = null;
            if (sVariableNode == null)
            {
                if (oVariableNode == null)
                {
                    newSubjects.Enqueue(subj);
                    if (TestSOConnection(subj, obj))
                        foreach (var r in bindings)
                            yield return r;
                }
                else
                {
                    SparqlVariableBinding o = null;
                    newSubjects.Enqueue(subj);
                    foreach (var binding in bindings)
                    {
                        if (binding.row.TryGetValue(oVariableNode, out o))
                        {
                            if (TestSOConnection(subj, o.Value))
                                yield return binding;
                        }
                        else
                        {
                            if (fromVariable == null)
                                fromVariable = GetAllSConnections(subj).ToArray();
                            foreach (var node in fromVariable)
                                yield return new SparqlResult(binding, node, oVariableNode);
                        }
                    }
                }
            }
            else
            {
                if (oVariableNode == null) //s variable o const
                {
                    SparqlVariableBinding s = null;
                    foreach (var binding in bindings)
                    {
                        if (binding.row.TryGetValue(sVariableNode, out s))
                        {
                            if (TestSOConnection(s.Value, obj))
                                yield return binding;
                        }
                        else
                        {
                            if (fromVariable == null)
                                fromVariable = GetAllOConnections(obj).ToArray();
                            foreach (var node in fromVariable)
                                yield return new SparqlResult(binding, node, oVariableNode);
                        }
                    }
                }
                else // both variables
                {
                    SparqlVariableBinding o = null;
                    SparqlVariableBinding s = null;
                
                    if (
                        bindings.All(
                            binding => binding.row.ContainsKey(sVariableNode) || binding.row.ContainsKey(oVariableNode)))
                        foreach (var binding in bindings)
                        {
                            if (binding.row.TryGetValue(sVariableNode, out s))
                            {
                                if (binding.row.TryGetValue(oVariableNode, out o))
                                {
                                    if (TestSOConnection(s.Value, o.Value))
                                        yield return binding;
                                }
                                else
                                {
                                    foreach (var node in  GetAllSConnections(s.Value))
                                        yield return new SparqlResult(binding, node, oVariableNode);
                                }
                            }

                            else if (binding.row.TryGetValue(oVariableNode, out o))
                            {
                                foreach (var node in  GetAllOConnections(o.Value))
                                    yield return new SparqlResult(binding, node, sVariableNode);
                            }
                            else // both unknowns
                            {
                                throw new Exception();
                            }
                        }
                    else
                    {
                        bothVariablesChache = predicatePath.CreateTriple(sVariableNode, oVariableNode, q)
                            .Aggregate(Enumerable.Repeat(new SparqlResult(), 1),
                                (enumerable, triple) => triple.Run(enumerable))
                            .Select(r => new KeyValuePair<INode, INode>(r[sVariableNode].Value, r[oVariableNode].Value))
                            .ToArray();

                        foreach (var binding in bindings)
                            if (binding.row.TryGetValue(sVariableNode, out s))
                                if (binding.row.TryGetValue(oVariableNode, out o))
                                {
                                    if (TestSOConnectionFromCache(s.Value, o.Value))
                                        yield return binding;
                                }
                                else
                                {
                                    foreach (var node in GetAllSConnectionsFromCache(s.Value))
                                        yield return new SparqlResult(binding, node, oVariableNode);
                                }
                            else if (binding.row.TryGetValue(oVariableNode, out o))
                                foreach (var node in GetAllOConnectionsFromCache(o.Value))
                                    yield return new SparqlResult(binding, node, sVariableNode);
                            else // both unknowns
                            {
                                if (bothVariablesCacheBySubject == null)
                                {
                                    bothVariablesCacheBySubject = new Dictionary<INode, HashSet<INode>>();
                                    foreach (var pair in bothVariablesChache)
                                    {
                                        HashSet<INode> nodes;
                                        if (!bothVariablesCacheBySubject.TryGetValue(pair.Key, out nodes))
                                            bothVariablesCacheBySubject.Add(pair.Key, new HashSet<INode>() {pair.Value});
                                        else nodes.Add(pair.Value);
                                    }
                                }

                                foreach (var sbj in bothVariablesCacheBySubject.Keys)
                                    foreach (var node in GetAllSConnectionsFromCache(sbj))
                                        yield return
                                            new SparqlResult(binding, sbj, sVariableNode, node, oVariableNode);
                            }
                    }
                }
            }
        }

        private IEnumerable<INode> GetAllSConnections(INode subj)
        {
            HashSet<INode> history = new HashSet<INode>(){subj};
              Queue<INode> subjects =new Queue<INode>();
                    subjects.Enqueue(subj);
            while (subjects.Count > 0)
                foreach (var objt in RunTriple(subjects.Dequeue(), oVariableNode)
                    .Select(sparqlResult => sparqlResult[oVariableNode].Value)
                    .Where(objt =>
                    {
                        var isNewS = !history.Contains(objt);
                        if (isNewS)
                        {
                            history.Add(objt);
                            subjects.Enqueue(objt);
                        }
                        return isNewS;
                    }))
                    yield return objt;
        }

        private IEnumerable<INode> GetAllSConnectionsFromCache(INode subj)
        {
            if (bothVariablesCacheBySubject == null)
            {
                bothVariablesCacheBySubject = new Dictionary<INode, HashSet<INode>>();
                foreach (var pair in bothVariablesChache)
                {
                    HashSet<INode> nodes;
                    if (!bothVariablesCacheBySubject.TryGetValue(pair.Key, out nodes))
                        bothVariablesCacheBySubject.Add(pair.Key, new HashSet<INode>() { pair.Value });
                    else nodes.Add(pair.Value);
                }
            }
            HashSet<INode> history = new HashSet<INode>() { subj };
            Queue<INode> subjects = new Queue<INode>();
            subjects.Enqueue(subj);
                HashSet<INode> objects;
            while (subjects.Count > 0)
                if(bothVariablesCacheBySubject.TryGetValue(subjects.Dequeue(), out objects))
                foreach (var objt in objects
                    .Where(objt =>
                    {
                        var isNewS = !history.Contains(objt);
                        if (isNewS)
                        {
                            history.Add(objt);
                            subjects.Enqueue(objt);
                        }
                        return isNewS;
                    }))
                    yield return objt;
        }

        private IEnumerable<INode> GetAllOConnections(INode objt)
        {
             HashSet<INode> history=new HashSet<INode>(){objt};
            Queue<INode> objects = new Queue<INode>();
                    objects.Enqueue(objt);                      

            while (objects.Count > 0)
                foreach (var subjt in RunTriple(sVariableNode, objects.Dequeue())
                    .Select(sparqlResult => sparqlResult[sVariableNode].Value)
                    .Where(subjt =>
                    {
                        var isNewS = !history.Contains(subjt);
                        if (isNewS)
                        {
                            history.Add(subjt);
                            objects.Enqueue(subjt);
                        }
                        return isNewS;
                    }))
                    yield return subjt;
        }

        private IEnumerable<INode> GetAllOConnectionsFromCache(INode objt)
        {
            if (bothVariablesCacheByObject == null)
            {
                bothVariablesCacheByObject = new Dictionary<INode, HashSet<INode>>();
                foreach (var pair in bothVariablesChache)
                {
                    HashSet<INode> nodes;
                    if (!bothVariablesCacheByObject.TryGetValue(pair.Value, out nodes))
                        bothVariablesCacheByObject.Add(pair.Value, new HashSet<INode>() { pair.Key});
                    else nodes.Add(pair.Key);
                }
            }
            HashSet<INode> history = new HashSet<INode>() { objt };
            Queue<INode> objects = new Queue<INode>();
            objects.Enqueue(objt);

            HashSet<INode> subjects=new HashSet<INode>();
            while (objects.Count > 0)
                if(bothVariablesCacheByObject.TryGetValue(objects.Dequeue(), out subjects))
                foreach (var subjt in subjects
                    .Where(subjt =>
                    {
                        var isNewS = !history.Contains(subjt);
                        if (isNewS)
                        {
                            history.Add(subjt);
                            objects.Enqueue(subjt);
                        }
                        return isNewS;
                    }))
                    yield return subjt;
        }


        private bool TestSOConnection(INode sbj, INode objct)
        {
            HashSet<INode> history = new HashSet<INode>() { subj };
            Queue<INode> newSubjects = new Queue<INode>();
            newSubjects.Enqueue(sbj);
            var subject = newSubjects.Peek();
            if (RunTriple(subject, objct).Any())  
                return true;
            var newVariable = (SparqlBlankNode)q.CreateBlankNode();
            while (newSubjects.Count > 0)
                if (RunTriple(newSubjects.Dequeue(), newVariable)
                    .Select(sparqlResult => sparqlResult[newVariable].Value)
                    .Where(o => !history.Contains(o))
                    .Any(o =>
                    {
                        history.Add(o);
                        newSubjects.Enqueue(o);
                        return RunTriple(o, objct).Any();
                    }))    
                    return true;
            return false;
        }

        private bool TestSOConnectionFromCache(INode sbj, INode objct)
        {
            if (bothVariablesCacheBySubject==null)
            {
                bothVariablesCacheBySubject=new Dictionary<INode, HashSet<INode>>();
                foreach (var pair in bothVariablesChache)
            {
                HashSet<INode> nodes;
                if (!bothVariablesCacheBySubject.TryGetValue(pair.Key, out nodes))
                    bothVariablesCacheBySubject.Add(pair.Key, new HashSet<INode>() { pair.Value });
                else nodes.Add(pair.Value);    
            }}

            HashSet<INode> history = new HashSet<INode>() { subj };
            HashSet<INode> objects;
            if (bothVariablesCacheBySubject.TryGetValue(sbj, out objects) && objects.Contains(objct))
                return true;
            
            Queue<INode> newSubjects = new Queue<INode>();
            newSubjects.Enqueue(sbj);

            while (newSubjects.Count > 0)
                if (bothVariablesCacheBySubject.TryGetValue(newSubjects.Dequeue(), out objects)
                    && objects
                        .Where(o => !history.Contains(o))
                        .Any(o =>
                        {
                            history.Add(o);
                            newSubjects.Enqueue(o);
                            return bothVariablesCacheBySubject.TryGetValue(o, out objects) && objects.Contains(objct);
                        }))
                    return true;
            return false;
        }

        private IEnumerable<SparqlResult> RunTriple(INode subject, INode objct)
        {                                     
                return predicatePath.CreateTriple(subject, objct, q).Aggregate(Enumerable.Repeat(new SparqlResult(), 1),
                    (enumerable, triple) => triple.Run(enumerable));
           
        }

        public new SparqlGraphPatternType PatternType { get{return SparqlGraphPatternType.PathTranslator;} }
    }
}