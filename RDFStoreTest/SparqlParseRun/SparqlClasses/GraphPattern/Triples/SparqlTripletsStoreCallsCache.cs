using System.Collections.Generic;
using System.Linq;
using SparqlParseRun.RdfCommon;
using SparqlParseRun.SparqlClasses.GraphPattern.Triples.Node;
using SparqlParseRun.SparqlClasses.Query.Result;

namespace SparqlParseRun.SparqlClasses.GraphPattern.Triples
{
    public class SparqlTripletsStoreCallsCache : ISparqlTripletsStoreCalls
    {
        private readonly IStore store;
        private readonly Cache<ISubjectNode, IUriNode, IEnumerable<INode>> spOCache;
        private readonly Cache<ISubjectNode, INode, IEnumerable<IUriNode>> sPoCache;
        private readonly Cache<IUriNode, INode, IEnumerable<ISubjectNode>> SpoCache;
        private readonly Cache<ISubjectNode, IEnumerable<Triple>> sPOCache;
        private readonly Cache<IUriNode, IEnumerable<Triple>> SpOCache;
        private readonly Cache<INode, IEnumerable<Triple>> SPoCache;
        private readonly Cache<ISubjectNode, IUriNode, INode, bool> spoCache;
       // private bool hasSPOCache;
    
        private readonly Cache<ISubjectNode, IUriNode, IUriNode, IGrouping<IUriNode, INode>> spOgCache;
        private readonly Cache<ISubjectNode, INode, IUriNode, IGrouping<IUriNode, IUriNode>> sPogCache;
        private readonly Cache<IUriNode, INode, IUriNode, IGrouping<IUriNode, ISubjectNode>> SpogCache;
        private readonly Cache<ISubjectNode, IUriNode, IGrouping<IUriNode, Triple>> sPOgCache;
        private readonly Cache<IUriNode, IUriNode, IGrouping<IUriNode, Triple>> SpOgCache;
        private readonly Cache<INode, IUriNode, IGrouping<IUriNode, Triple>> SPogCache;
        private readonly Cache<ISubjectNode, IUriNode, INode, IUriNode, bool> spogCache;
      //  private Cache<IUriNode,bool> SPOgCache;

        private readonly Cache<ISubjectNode, IUriNode, List<IGrouping<IUriNode, INode>>> spOGCache;
        private readonly Cache<ISubjectNode, INode, List<IGrouping<IUriNode, IUriNode>>> sPoGCache;
        private readonly Cache<IUriNode, INode,  List<IGrouping<IUriNode, ISubjectNode>>> SpoGCache;
        private readonly Cache<ISubjectNode, List<IGrouping<IUriNode, Triple>>> sPOGCache;
        private readonly Cache<IUriNode, List<IGrouping<IUriNode, Triple>>> SpOGCache;
        private readonly Cache<INode, List<IGrouping<IUriNode, Triple>>> SPoGCache;
        private readonly Cache<ISubjectNode, IUriNode, INode, DataSet> spoGCache;
        //private bool SPOGCache;
          public SparqlTripletsStoreCallsCache(IStore store)
        {
            this.store = store;
              spOCache = new Cache<ISubjectNode, IUriNode, IEnumerable<INode>>(
                  (s, p) =>
                  {
                      var nodes = store.GetTriplesWithSubjectPredicate(s, p).ToArray();
                      foreach (var o in nodes)
                          spoCache.Add(s, p, o, true);
                      return nodes;
                  });
            sPoCache = new Cache<ISubjectNode, INode, IEnumerable<IUriNode>>(
                (s, o) =>
                {
                    var predicates = store.GetTriplesWithSubjectObject(s, o).ToArray();
                    foreach (var p in predicates)
                        spoCache.Add(s, p, o, true);
                    return predicates;
                });
            SpoCache = new Cache<IUriNode, INode, IEnumerable<ISubjectNode>>(
                (p, o) =>
                {
                    var subjectNodes = store.GetTriplesWithPredicateObject(p, o).ToArray();
                    foreach (var s in subjectNodes)
                        spoCache.Add(s, p, o, true);
                    return subjectNodes;
                });

            sPOCache = new Cache<ISubjectNode, IEnumerable<Triple>>(s => { 
                                                                             var triplesWithSubject = store.GetTriplesWithSubject(s).ToArray();
                                                                             foreach (var pg in triplesWithSubject.GroupBy(t => t.Predicate))
                                                                                 spOCache.Add(s, pg.Key,
                                                                                     pg.Select(t => t.Object));
                                                                             foreach (var pg in triplesWithSubject.GroupBy(t => t.Object))
                                                                                 sPoCache.Add(s, pg.Key,
                                                                                     pg.Select(t => t.Predicate));
                                                                             foreach (var triple in triplesWithSubject)
                                                                                 spoCache.Add(triple.Subject, triple.Predicate, triple.Object, true);
                                                                             return triplesWithSubject; });
            SpOCache = new Cache<IUriNode, IEnumerable<Triple>>(p => {
                var triples = store.GetTriplesWithPredicate(p).ToArray();
                foreach (var pg in triples.GroupBy(t => t.Object))
                    SpoCache.Add(p, pg.Key,
                        pg.Select(t => t.Subject));
                foreach (var pg in triples.GroupBy(t => t.Subject))
                    spOCache.Add(pg.Key, p,
                        pg.Select(t => t.Object));
                foreach (var triple in triples)
                    spoCache.Add(triple.Subject, triple.Predicate, triple.Object, true); 
                return triples;
            });
            SPoCache = new Cache<INode, IEnumerable<Triple>>(o => {
                var triples = store.GetTriplesWithObject(o).ToArray();
                foreach (var pg in triples.GroupBy(t => t.Predicate))
                    SpoCache.Add(pg.Key, o, 
                        pg.Select(t => t.Subject));
                foreach (var pg in triples.GroupBy(t => t.Subject))
                    sPoCache.Add(pg.Key, o,
                        pg.Select(t => t.Predicate));
                foreach (var triple in triples)
                    spoCache.Add(triple.Subject, triple.Predicate, triple.Object, true);    
                return triples;
            });
            spoCache = new Cache<ISubjectNode, IUriNode, INode, bool>(store.Contains);
           // SPOCache = new Cache<ISubjectNode, IUriNode, IEnumerable<INode>>(store.GetTriplesWithSubjectPredicate);
            spOgCache = new Cache<ISubjectNode, IUriNode, IUriNode, IGrouping<IUriNode, INode>>(null);
            sPogCache = new Cache<ISubjectNode, INode, IUriNode, IGrouping<IUriNode, IUriNode>>(null);
            SpogCache = new Cache<IUriNode, INode, IUriNode, IGrouping<IUriNode, ISubjectNode>>(null);

            sPOgCache = new Cache<ISubjectNode, IUriNode, IGrouping<IUriNode, Triple>>(null);
         
            SpOgCache = new Cache<IUriNode, IUriNode, IGrouping<IUriNode, Triple>>(null);
            SPogCache = new Cache<INode, IUriNode, IGrouping<IUriNode, Triple>>(null);

            spogCache = new Cache<ISubjectNode, IUriNode, INode, IUriNode, bool>(null);

            spOGCache = new Cache<ISubjectNode, IUriNode, List<IGrouping<IUriNode, INode>>>((node,  arg3) => new List<IGrouping<IUriNode, INode>>());
            sPoGCache = new Cache<ISubjectNode, INode, List<IGrouping<IUriNode, IUriNode>>>((node,  arg3) => new List<IGrouping<IUriNode, IUriNode>>());
            SpoGCache = new Cache<IUriNode, INode, List<IGrouping<IUriNode, ISubjectNode>>>((node,  arg3) => new List<IGrouping<IUriNode, ISubjectNode>>());

            sPOGCache = new Cache<ISubjectNode, List<IGrouping<IUriNode, Triple>>>((node) => new List<IGrouping<IUriNode, Triple>>());

            SpOGCache = new Cache<IUriNode, List<IGrouping<IUriNode, Triple>>>((node) => new List<IGrouping<IUriNode, Triple>>());
            SPoGCache = new Cache<INode, List<IGrouping<IUriNode, Triple>>>((node) => new List<IGrouping<IUriNode, Triple>>());

            spoGCache = new Cache<ISubjectNode, IUriNode, INode, DataSet>((node, p, arg3) => new DataSet());  
          // SPOgCache=new Cache<IUriNode, bool>(set => false);
        }
        public IEnumerable<SparqlResult> spO(ISubjectNode subjNode, IUriNode predicateNode,
            VariableNode obj, SparqlResult variablesBindings)
        {   
            return spOCache.Get(subjNode,predicateNode)
                    .Select(node => new SparqlResult(variablesBindings, node, obj));
        }


       


        // from merged named graphs
             public  IEnumerable<SparqlResult> spOVarGraphs( ISubjectNode subjNode, IUriNode predicateNode,
            VariableNode obj, SparqlResult variablesBindings, VariableDataSet variableDataSet)
             {
                 return spOCacheGraphs(subjNode, predicateNode, variableDataSet)
                //.GetGraphUri(variablesBindings) if graphs is empty, Gets All named graphs
                .SelectMany(grouping =>            
                        grouping.Select(node =>
                            new SparqlResult(variablesBindings, node, obj, grouping.Key, variableDataSet.Variable))); // if graphVariable is null, ctor check this.
            
            }

             private IEnumerable<IGrouping<IUriNode, INode>> spOCacheGraphs(ISubjectNode subjNode, IUriNode predicateNode, DataSet variableDataSet)
        {
            if (variableDataSet.Any() )     //search in all named graphs
                if (spOGCache.Contains(subjNode, predicateNode))
                    return spOGCache.Get(subjNode, predicateNode);
                else
                {
                    var result = store.NamedGraphs.GetTriplesWithSubjectPredicateFromGraphs(subjNode, predicateNode,
                        variableDataSet).ToList();
                    spOGCache.Add(subjNode, predicateNode, result);
                    foreach (var grouping in result)
                    {
                        spOgCache.Add(subjNode, predicateNode, grouping.Key, grouping);
                        foreach (var o in grouping)
                            spoGCache.Get(subjNode, predicateNode, o).Add(grouping.Key);
                    }
                    return result;
                }
            var res = new List<IGrouping<IUriNode, INode>>();
            var gList = new DataSet();
            foreach (var g in variableDataSet)
                if (spOgCache.Contains(subjNode, predicateNode, g))
                    res.Add(spOgCache.Get(subjNode, predicateNode, g));
                else
                    gList.Add(g);
            foreach (var gRes in store.NamedGraphs.GetTriplesWithSubjectPredicateFromGraphs(subjNode, predicateNode, gList))
            {
                spOgCache.Add(subjNode, predicateNode, gRes.Key,gRes);
                foreach (var o in gRes)
                    spogCache.Add(subjNode, predicateNode, o, gRes.Key,true);
                res.Add(gRes);
            }
            return res;
        }


        public  IEnumerable<SparqlResult> spOGraphs( ISubjectNode subjNode, IUriNode predicateNode,
            VariableNode obj, SparqlResult variablesBindings, DataSet graphs)
        {
            return spOCacheGraphs(subjNode, predicateNode,graphs)
                .SelectMany(grouping =>
                        grouping.Select(node => new SparqlResult(variablesBindings, node, obj)));     
        }


        public  IEnumerable<SparqlResult> Spo( VariableNode subj, IUriNode predicateNode, INode objectNode, SparqlResult variablesBindings)
        {
            return SpoCache.Get(predicateNode, objectNode)
                .Select(node => new SparqlResult(variablesBindings, node, subj));

            // from merged named graphs
        }



        public  IEnumerable<SparqlResult> SpoGraphs( VariableNode subj, IUriNode predicateNode,
            INode objectNode, SparqlResult variablesBindings, DataSet graphs)
        {
            return SpoCacheGraphs(predicateNode, objectNode, graphs)
                // if graphs is empty, Gets All named graphs
                .SelectMany(grouping =>
                        grouping.Select(node =>
                            new SparqlResult(variablesBindings, node, subj)));
                // if graphVariable is null, ctor check this.
        }
        public  IEnumerable<SparqlResult> SpoVarGraphs( VariableNode subj, IUriNode predicateNode,
            INode objectNode, SparqlResult variablesBindings, VariableDataSet variableDataSet)
        {
            return SpoCacheGraphs(predicateNode, objectNode, variableDataSet) //.GetGraphUri(variablesBindings) if graphs is empty, Gets All named graphs
                .SelectMany(grouping =>
                   grouping.Select(node =>
                       new SparqlResult(variablesBindings, node, subj, grouping.Key, variableDataSet.Variable))); // if graphVariable is null, ctor check this.

        }
   
        private IEnumerable<IGrouping<IUriNode, INode>> SpoCacheGraphs(IUriNode predicateNode, INode objectNode, DataSet variableDataSet)
        {
            if (variableDataSet.Any())     //search in all named graphs
                if (SpoGCache.Contains(predicateNode, objectNode))
                    return SpoGCache.Get(predicateNode, objectNode);
                else
                {
                    var result = store.NamedGraphs.GetTriplesWithPredicateObjectFromGraphs(predicateNode, objectNode,
                        variableDataSet).ToList();
                    SpoGCache.Add(predicateNode, objectNode, result);
                    foreach (var grouping in result)
                    {
                        SpogCache.Add(predicateNode, objectNode, grouping.Key, grouping);
                        foreach (var s in grouping)
                            spoGCache.Get(s, predicateNode, objectNode).Add(grouping.Key);
                    }
                    return result;
                }
            var res = new List<IGrouping<IUriNode, ISubjectNode>>();
            var gList = new DataSet();
            foreach (var g in variableDataSet)
                if (SpogCache.Contains(predicateNode, objectNode, g))
                    res.Add(SpogCache.Get(predicateNode, objectNode, g));
                else
                    gList.Add(g);
            foreach (var gRes in store.NamedGraphs.GetTriplesWithPredicateObjectFromGraphs(predicateNode, objectNode, gList))
            {
                SpogCache.Add(predicateNode, objectNode, gRes.Key, gRes);
                foreach (var s in gRes)
                    spoGCache.Get(s, predicateNode, objectNode).Add(gRes.Key);
                res.Add(gRes);
            }
            return res;
        }

        public  IEnumerable<SparqlResult> sPo( ISubjectNode subj, VariableNode pred, INode obj, SparqlResult variablesBindings)
        {
            return sPoCache.Get(subj, obj)
                .Select(newObj => new SparqlResult(variablesBindings, newObj, pred));

        }

        public  IEnumerable<SparqlResult> sPoGraphs( ISubjectNode subj, VariableNode pred, INode obj, SparqlResult variablesBindings, DataSet graphs)
        {
            return sPoCacheGraphs(subj, obj, graphs) 
                .SelectMany(grouping =>
                        grouping.Select(node =>
                            new SparqlResult(variablesBindings, node, pred)));
        }
        public  IEnumerable<SparqlResult> sPoVarGraphs( ISubjectNode subj, VariableNode pred, INode obj, SparqlResult variablesBindings, VariableDataSet variableDataSet)
        {
            return sPoCacheGraphs(subj, obj, variableDataSet) //.GetGraphUri(variablesBindings)
               .SelectMany(grouping =>
                       grouping.Select(node =>
                           new SparqlResult(variablesBindings, node, pred, grouping.Key, variableDataSet.Variable)));
        }
        private IEnumerable<IGrouping<IUriNode, INode>> sPoCacheGraphs(ISubjectNode subjectNode, INode objectNode, DataSet variableDataSet)
        {
            if (variableDataSet.Any())     //search in all named graphs
                if (sPoGCache.Contains(subjectNode, objectNode))
                    return sPoGCache.Get(subjectNode, objectNode);
                else
                {
                    var result = store.NamedGraphs.GetTriplesWithSubjectObjectFromGraphs(subjectNode, objectNode,
                        variableDataSet).ToList();
                    sPoGCache.Add(subjectNode, objectNode, result);
                    foreach (var grouping in result)
                    {
                        sPogCache.Add(subjectNode, objectNode, grouping.Key, grouping);
                        foreach (var p in grouping)
                            spoGCache.Get(subjectNode, p, objectNode).Add(grouping.Key);
                    }
                    return result;
                }
            var res = new List<IGrouping<IUriNode, ISubjectNode>>();
            var gList = new DataSet();
            foreach (var g in variableDataSet)
                if (sPogCache.Contains(subjectNode, objectNode, g))
                    res.Add(sPogCache.Get(subjectNode, objectNode, g));
                else
                    gList.Add(g);
            foreach (var gRes in store.NamedGraphs.GetTriplesWithSubjectObjectFromGraphs(subjectNode, objectNode, gList))
            {
                sPogCache.Add(subjectNode, objectNode, gRes.Key, gRes);
                foreach (var p in gRes)
                    spoGCache.Get(subjectNode, p, objectNode).Add(gRes.Key);
                res.Add(gRes);
            }
            return res;
        }

        public IEnumerable<SparqlResult> SpO(VariableNode s, IUriNode predicate, VariableNode o, SparqlResult variablesBindings)
        {
            return SpOCache.Get(predicate)
                .Select(triple => new SparqlResult(new Dictionary<VariableNode, SparqlVariableBinding>(variablesBindings.row)
                {
                    {s, new SparqlVariableBinding(s,triple.Subject)},
                    {o, new SparqlVariableBinding(o,triple.Object)}
                }));
        }

        public IEnumerable<SparqlResult> SpOGraphs(VariableNode s, IUriNode predicate, VariableNode o, SparqlResult variablesBindings, DataSet graphs)
        {
            return SpOCacheGraphs(predicate, graphs)
             .SelectMany(grouping =>
                     grouping
                        .Select(triple => new SparqlResult(new Dictionary<VariableNode, SparqlVariableBinding>(variablesBindings.row)
              {
                  {s, new SparqlVariableBinding(s,triple.Subject)},
                  {o, new SparqlVariableBinding(o,triple.Object)}
              })));
        }

        public IEnumerable<SparqlResult> SpOVarGraphs(VariableNode s, IUriNode predicate, VariableNode o, SparqlResult variablesBindings, VariableDataSet graphs)
        {
            return SpOCacheGraphs(predicate, graphs) //.GetGraphUri(variablesBindings))
             .SelectMany(grouping =>
                     grouping
                        .Select(triple => new SparqlResult(new Dictionary<VariableNode, SparqlVariableBinding>(variablesBindings.row)
              {
                  {s, new SparqlVariableBinding(s,triple.Subject)},
                  {o, new SparqlVariableBinding(o,triple.Object)}, 
                  {graphs.Variable, new SparqlVariableBinding(graphs.Variable,grouping.Key)}
              })));
        }
        private IEnumerable<IGrouping<IUriNode, Triple>> SpOCacheGraphs(IUriNode predicateNode, DataSet variableDataSet)
        {
            if (variableDataSet.Any())
                if (SpOGCache.Contains(predicateNode))
                    return SpOGCache.Get(predicateNode);
                else
                {
                    var result = store.NamedGraphs.GetTriplesWithPredicateFromGraphs(predicateNode, variableDataSet).ToList();
                    SpOGCache.Add(predicateNode, result);
                    foreach (var grouping in result)
                    {
                        SpOgCache.Add(predicateNode, grouping.Key,  grouping);
                        foreach (var tripleGroup in grouping.GroupBy(triple => triple.Subject))
                        {
                            var oGroup = new Grouping<IUriNode, INode>(grouping.Key,
                                tripleGroup.Select(triple => triple.Object));
                            spOgCache.Add(tripleGroup.Key, predicateNode, grouping.Key,oGroup);
                         
                            spOGCache.Get(tripleGroup.Key, predicateNode).Add(oGroup);
                            foreach (var o in oGroup)
                            {
                                spoGCache.Get(tripleGroup.Key, predicateNode, o).Add(grouping.Key);
                                spogCache.Add(tripleGroup.Key, predicateNode, o, grouping.Key, true);
                            }
                        }
                        foreach (var tripleGroup in grouping.GroupBy(triple => triple.Object))
                        {
                            var sGroup = new Grouping<IUriNode, ISubjectNode>(grouping.Key,
                                tripleGroup.Select(triple => triple.Subject));
                            SpogCache.Add(predicateNode, tripleGroup.Key, grouping.Key, sGroup);
                            SpoGCache.Get(predicateNode, tripleGroup.Key).Add(sGroup); 
                        }
                    }       
                    return result;
                }
            var res = new List<IGrouping<IUriNode, Triple>>();
            var gList = new DataSet();
            foreach (var g in variableDataSet)
                if (SpOgCache.Contains(predicateNode, g))
                    res.Add(SpOgCache.Get(predicateNode, g));
                else
                    gList.Add(g);
            foreach (var gRes in store.NamedGraphs.GetTriplesWithPredicateFromGraphs(predicateNode,  gList))
            {
                SpOgCache.Add(predicateNode,  gRes.Key, gRes);
                res.Add(gRes);
                foreach (var tripleGroup in gRes.GroupBy(triple => triple.Subject))
                {
                    var oGroup = new Grouping<IUriNode, INode>(gRes.Key,
                        tripleGroup.Select(triple => triple.Object));
                    spOgCache.Add(tripleGroup.Key, predicateNode, gRes.Key, oGroup);

                    spOGCache.Get(tripleGroup.Key, predicateNode).Add(oGroup);
                    foreach (var o in oGroup)
                    {
                        spoGCache.Get(tripleGroup.Key, predicateNode, o).Add(gRes.Key);
                        spogCache.Add(tripleGroup.Key, predicateNode, o, gRes.Key, true);
                    }
                }
                foreach (var tripleGroup in gRes.GroupBy(triple => triple.Object))
                {
                    var sGroup = new Grouping<IUriNode, ISubjectNode>(gRes.Key,
                        tripleGroup.Select(triple => triple.Subject));
                    SpogCache.Add(predicateNode, tripleGroup.Key, gRes.Key, sGroup);
                    SpoGCache.Get(predicateNode, tripleGroup.Key).Add(sGroup);
                }  
            }
            return res;
        }

        public  IEnumerable<SparqlResult> sPO( ISubjectNode subj, VariableNode pred, VariableNode obj, SparqlResult variablesBindings)
        {
            return sPOCache.Get(subj)
              .Select(triple => new SparqlResult(new Dictionary<VariableNode, SparqlVariableBinding>(variablesBindings.row)
              {
                  {pred, new SparqlVariableBinding(pred,triple.Predicate)},
                  {obj, new SparqlVariableBinding(obj,triple.Object)}
              }));
        }

        public  IEnumerable<SparqlResult> sPOGraphs( ISubjectNode subj, VariableNode pred,
            VariableNode obj, SparqlResult variablesBindings, DataSet graphs)
        {
            return sPOCacheGraphs(subj, graphs)
               .SelectMany(grouping =>
                       grouping
                          .Select(triple => new SparqlResult(new Dictionary<VariableNode, SparqlVariableBinding>(variablesBindings.row)
              {
                  {pred, new SparqlVariableBinding(pred,triple.Predicate)},
                  {obj, new SparqlVariableBinding(obj,triple.Object)}
              })));
        }
        public  IEnumerable<SparqlResult> sPOVarGraphs( ISubjectNode subj, VariableNode pred,
           VariableNode obj, SparqlResult variablesBindings, VariableDataSet variableDataSet)
        {
            return sPOCacheGraphs(subj, variableDataSet)//.GetGraphUri(variablesBindings)
             .SelectMany(grouping =>
                     grouping
                        .Select(triple => new SparqlResult(new Dictionary<VariableNode, SparqlVariableBinding>(variablesBindings.row)
              {
                  {pred, new SparqlVariableBinding(pred,triple.Predicate)},
                  {obj, new SparqlVariableBinding(obj,triple.Object)},
                  {variableDataSet.Variable, new SparqlVariableBinding(variableDataSet.Variable,grouping.Key)},
              })));
        }
        private IEnumerable<IGrouping<IUriNode, Triple>> sPOCacheGraphs(ISubjectNode subjectNode, DataSet variableDataSet)
        {
            if (variableDataSet.Any())
                if (sPOGCache.Contains(subjectNode))
                    return sPOGCache.Get(subjectNode);
                else
                {
                    var result = store.NamedGraphs.GetTriplesWithSubjectFromGraphs(subjectNode, variableDataSet).ToList();
                    sPOGCache.Add(subjectNode, result);
                    foreach (var grouping in result)
                    {
                        sPOgCache.Add(subjectNode, grouping.Key, grouping);
                        foreach (var tripleGroup in grouping.GroupBy(triple => triple.Predicate))
                        {
                            var oGroup = new Grouping<IUriNode, INode>(grouping.Key,
                                tripleGroup.Select(triple => triple.Object));
                            spOgCache.Add(subjectNode, tripleGroup.Key, grouping.Key, oGroup);

                            spOGCache.Get(subjectNode, tripleGroup.Key).Add(oGroup);
                            foreach (var o in oGroup)
                            {
                                spoGCache.Get(subjectNode, tripleGroup.Key, o).Add(grouping.Key);
                                spogCache.Add(subjectNode, tripleGroup.Key, o, grouping.Key, true);
                            }
                        }
                        foreach (var tripleGroup in grouping.GroupBy(triple => triple.Object))
                        {
                            var pGroup = new Grouping<IUriNode, IUriNode>(grouping.Key,
                                tripleGroup.Select(triple => triple.Predicate));
                            sPogCache.Add(subjectNode, tripleGroup.Key, grouping.Key, pGroup);
                            sPoGCache.Get(subjectNode, tripleGroup.Key).Add(pGroup);
                        }
                    }
                    return result;
                }
            var res = new List<IGrouping<IUriNode, Triple>>();
            var gList = new DataSet();
            foreach (var g in variableDataSet)
                if (sPOgCache.Contains(subjectNode, g))
                    res.Add(sPOgCache.Get(subjectNode, g));
                else
                    gList.Add(g);
            foreach (var gRes in store.NamedGraphs.GetTriplesWithSubjectFromGraphs(subjectNode, gList))
            {
                sPOgCache.Add(subjectNode, gRes.Key, gRes);
                res.Add(gRes);
                foreach (var tripleGroup in gRes.GroupBy(triple => triple.Predicate))
                {
                    var oGroup = new Grouping<IUriNode, INode>(gRes.Key,
                        tripleGroup.Select(triple => triple.Object));
                    spOgCache.Add(subjectNode, tripleGroup.Key, gRes.Key, oGroup);

                    spOGCache.Get(subjectNode, tripleGroup.Key).Add(oGroup);
                    foreach (var o in oGroup)
                    {
                        spoGCache.Get(subjectNode, tripleGroup.Key, o).Add(gRes.Key);
                        spogCache.Add(subjectNode, tripleGroup.Key, o, gRes.Key, true);
                    }
                }
                foreach (var tripleGroup in gRes.GroupBy(triple => triple.Object))
                {
                    var pGroup = new Grouping<IUriNode, IUriNode>(gRes.Key,
                        tripleGroup.Select(triple => triple.Predicate));
                    sPogCache.Add(subjectNode, tripleGroup.Key, gRes.Key, pGroup);
                    sPoGCache.Get(subjectNode, tripleGroup.Key).Add(pGroup);
                }
            }
            return res;
        }
        public  IEnumerable<SparqlResult> SPo( VariableNode subj, VariableNode predicate, INode obj, SparqlResult variablesBindings)
        {
            return SPoCache.Get(obj)
             .Select(triple => new SparqlResult(new Dictionary<VariableNode, SparqlVariableBinding>(variablesBindings.row)
              {
                  {predicate, new SparqlVariableBinding(predicate,triple.Predicate)},
                  {subj, new SparqlVariableBinding(subj,triple.Subject)}
              }));
        }

        public  IEnumerable<SparqlResult> SPoGraphs( VariableNode subj, VariableNode pred,
    INode obj, SparqlResult variablesBindings, DataSet graphs)
        {
            return SPoCacheGraphs(obj, graphs)
             .SelectMany(grouping =>
                     grouping
                        .Select(triple => new SparqlResult(new Dictionary<VariableNode, SparqlVariableBinding>(variablesBindings.row)
              {
                  {pred, new SparqlVariableBinding(pred,triple.Predicate)},
                  {subj, new SparqlVariableBinding(subj,triple.Subject)}
              })));
        }
        public  IEnumerable<SparqlResult> SPoVarGraphs( VariableNode subj, VariableNode pred,
           INode obj, SparqlResult variablesBindings, VariableDataSet variableDataSet)
        {
            return SPoCacheGraphs(obj, variableDataSet) //.GetGraphUri(variablesBindings)
             .SelectMany(grouping =>
                     grouping
                        .Select(triple => new SparqlResult(new Dictionary<VariableNode, SparqlVariableBinding>(variablesBindings.row)
              {
                  {pred, new SparqlVariableBinding(pred,triple.Predicate)},
                  {subj, new SparqlVariableBinding(subj,triple.Subject)},
                  {variableDataSet.Variable, new SparqlVariableBinding(variableDataSet.Variable,grouping.Key)},
              })));
        }
        private IEnumerable<IGrouping<IUriNode, Triple>> SPoCacheGraphs(INode objectNode, DataSet variableDataSet)
        {
            if (variableDataSet.Any())
                if (SPoGCache.Contains(objectNode))
                    return SPoGCache.Get(objectNode);
                else
                {
                    var result = store.NamedGraphs.GetTriplesWithObjectFromGraphs(objectNode, variableDataSet).ToList();
                    SPoGCache.Add(objectNode, result);
                    foreach (var grouping in result)
                    {
                        SPogCache.Add(objectNode, grouping.Key, grouping);
                        foreach (var tripleGroup in grouping.GroupBy(triple => triple.Subject))
                        {
                            var pGroup = new Grouping<IUriNode, IUriNode>(grouping.Key,
                                tripleGroup.Select(triple => triple.Predicate));
                            sPogCache.Add(tripleGroup.Key, objectNode, grouping.Key, pGroup);

                            sPoGCache.Get(tripleGroup.Key, objectNode).Add(pGroup);
                            foreach (var p in pGroup)
                            {
                                spoGCache.Get(tripleGroup.Key, p, objectNode).Add(grouping.Key);
                                spogCache.Add(tripleGroup.Key, p, objectNode, grouping.Key, true);
                            }
                        }
                        foreach (var tripleGroup in grouping.GroupBy(triple => triple.Predicate))
                        {
                            var sGroup = new Grouping<IUriNode, ISubjectNode>(grouping.Key,
                                tripleGroup.Select(triple => triple.Subject));
                            SpogCache.Add(tripleGroup.Key, objectNode, grouping.Key, sGroup);
                            SpoGCache.Get(tripleGroup.Key, objectNode).Add(sGroup);
                        }
                    }
                    return result;
                }
            var res = new List<IGrouping<IUriNode, Triple>>();
            var gList = new DataSet();
            foreach (var g in variableDataSet)
                if (SPogCache.Contains(objectNode, g))
                    res.Add(SPogCache.Get(objectNode, g));
                else
                    gList.Add(g);
            foreach (var gRes in store.NamedGraphs.GetTriplesWithObjectFromGraphs(objectNode, gList))
            {
                SPogCache.Add(objectNode, gRes.Key, gRes);
                res.Add(gRes);
                foreach (var tripleGroup in gRes.GroupBy(triple => triple.Subject))
                {
                    var pGroup = new Grouping<IUriNode, IUriNode>(gRes.Key,
                        tripleGroup.Select(triple => triple.Predicate));
                    sPogCache.Add(tripleGroup.Key, objectNode, gRes.Key, pGroup);

                    sPoGCache.Get(tripleGroup.Key, objectNode).Add(pGroup);
                    foreach (var p in pGroup)
                    {
                        spoGCache.Get(tripleGroup.Key, p, objectNode).Add(gRes.Key);
                        spogCache.Add(tripleGroup.Key, p, objectNode, gRes.Key, true);
                    }
                }
                foreach (var tripleGroup in gRes.GroupBy(triple => triple.Predicate))
                {
                    var sGroup = new Grouping<IUriNode, ISubjectNode>(gRes.Key,
                        tripleGroup.Select(triple => triple.Subject));
                    SpogCache.Add(tripleGroup.Key, objectNode, gRes.Key, sGroup);
                    SpoGCache.Get(tripleGroup.Key, objectNode).Add(sGroup);
                }
            }
            return res;
        }
     
        public  IEnumerable<SparqlResult> SPO( VariableNode subj, VariableNode predicate, VariableNode obj, SparqlResult variablesBindings)
        {
            return store
             .GetTriples()
             .Select(triple => new SparqlResult(new Dictionary<VariableNode, SparqlVariableBinding>(variablesBindings.row)
              {
                  {subj, new SparqlVariableBinding(subj,triple.Subject)} ,
                  {predicate, new SparqlVariableBinding(predicate,triple.Predicate)},
                  {obj, new SparqlVariableBinding(obj,triple.Object)}
              }));

        }
        public IEnumerable<SparqlResult> SPOGraphs(VariableNode subj, VariableNode predicate, VariableNode obj, SparqlResult variablesBindings, DataSet graphs)
        {
            return store
                 .NamedGraphs
             .GetTriplesFromGraphs(graphs)
                .SelectMany(grouping =>
                     grouping
             .Select(triple => new SparqlResult(new Dictionary<VariableNode, SparqlVariableBinding>(variablesBindings.row)
              {
                  {subj, new SparqlVariableBinding(subj,triple.Subject)} ,
                  {predicate, new SparqlVariableBinding(predicate,triple.Predicate)},
                  {obj, new SparqlVariableBinding(obj,triple.Object)}
              })));

        }

        public IEnumerable<SparqlResult> SPOVarGraphs(VariableNode subj, VariableNode predicate, VariableNode obj,
            SparqlResult variablesBindings, VariableDataSet variableDataSet)
        {
            return store
                .NamedGraphs
                .GetTriplesFromGraphs(variableDataSet) //.GetGraphUri(variablesBindings)
                .SelectMany(grouping =>
                    grouping
                        .Select(
                            triple =>
                                new SparqlResult(
                                    new Dictionary<VariableNode, SparqlVariableBinding>(variablesBindings.row)
                                    {
                                        {subj, new SparqlVariableBinding(subj, triple.Subject)},
                                        {predicate, new SparqlVariableBinding(predicate, triple.Predicate)},
                                        {obj, new SparqlVariableBinding(obj, triple.Object)}
                                    })));
        }


        public IEnumerable<SparqlResult> spoGraphs(ISubjectNode subjectNode, IUriNode predicateNode, INode objectNode,
              SparqlResult variablesBindings, DataSet graphs)
        {
            if (spoGraphsCache(subjectNode, predicateNode, objectNode, graphs).Any())
              yield return  variablesBindings;

        }

        public IEnumerable<SparqlResult> spoVarGraphs(ISubjectNode subjectNode, IUriNode predicateNode, INode objectNode,
            SparqlResult variablesBindings, VariableDataSet graphs)
        {
            return spoGraphsCache(subjectNode, predicateNode, objectNode, graphs)  
              .Select(g => new SparqlResult(variablesBindings, g, graphs.Variable)) ;
        }

        public IEnumerable<SparqlResult> spo(ISubjectNode subjectNode, IUriNode predicateNode, INode objNode, SparqlResult variablesBindings)
        {
            if (spoCache.Get(subjectNode, predicateNode, objNode))
                yield return variablesBindings;
        }

        private IEnumerable<IUriNode> spoGraphsCache(ISubjectNode subjectNode, IUriNode predicateNode, INode objectNode, DataSet graphs)
        {
            if (graphs.Count == 0)
            {
                if (spoGCache.Contains(subjectNode, predicateNode, objectNode))
                    foreach (var g in spoGCache.Get(subjectNode, predicateNode, objectNode))
                        yield return g;
                else
                {
                    var resultGraphs = store.NamedGraphs.GetGraphs(subjectNode, predicateNode, objectNode, graphs);

                    spoGCache.Add(subjectNode, predicateNode, objectNode, resultGraphs);
                    foreach (var resultGraph in resultGraphs)
                        spogCache.Add(subjectNode, predicateNode, objectNode, resultGraph, true);
                }
            }
            else
            {
                DataSet ask = new DataSet();
                DataSet trues = new DataSet();
                foreach (var graph in graphs)
                    if (spogCache.Contains(subjectNode, predicateNode, objectNode, graph))
                    {
                        if (spogCache.Get(subjectNode, predicateNode, objectNode, graph))
                        {
                            trues.Add(graph);
                            yield return graph;
                        }       
                    }
                    else
                        ask.Add(graph);
                var resultGraphs = store.NamedGraphs.GetGraphs(subjectNode, predicateNode, objectNode, ask);

                trues.AddRange(resultGraphs);
                spoGCache.Add(subjectNode, predicateNode, objectNode, trues);
                foreach (var g in ask) 
                    spogCache.Add(subjectNode, predicateNode, objectNode, g, resultGraphs.Contains(g));

            }


        }

        private IEnumerable<IGrouping<IUriNode, Triple>> SPOCacheGraphs(DataSet variableDataSet)
        {
          //   if (variableDataSet.Any())
                //if (SPOGCache.Contains(objectNode))
                //    return SPOGCache.Get(objectNode);
                //else
                {
                    var result = store.NamedGraphs.GetTriplesFromGraphs(variableDataSet);
                    //SPOGCache.Add(objectNode, result);                            .ToList()
                    //foreach (IGrouping<IUriNode, Triple> grouping in result)
                    //{
                    //    SPOgCache.Add(grouping.Key, true);
                    //    foreach (var tripleGroup in grouping.GroupBy(triple => triple.Subject))
                    //    {
                    //        var pGroup = new Grouping<IUriNode, IUriNode>(grouping.Key,
                    //           tripleGroup.Select(triple => triple.Predicate));
                    //        sPOgCache.Add(tripleGroup.Key,grouping.Key,tripleGroup);
                           
                    //        sPogCache.Add(tripleGroup.Key, objectNode, grouping.Key, pGroup);

                    //        sPoGCache.Get(tripleGroup.Key, objectNode).Add(pGroup);
                    //        foreach (var p in pGroup)
                    //        {
                    //            spoGCache.Get(tripleGroup.Key, p, objectNode).Add(grouping.Key);
                    //            spogCache.Add(tripleGroup.Key, p, objectNode, grouping.Key, true);
                    //        }
                    //    }
                    //    foreach (var tripleGroup in grouping.GroupBy(triple => triple.Predicate))
                    //    {
                    //        var sGroup = new Grouping<IUriNode, ISubjectNode>(grouping.Key,
                    //            tripleGroup.Select(triple => triple.Subject));
                    //        SpogCache.Add(tripleGroup.Key, objectNode, grouping.Key, sGroup);
                    //        SpoGCache.Get(tripleGroup.Key, objectNode).Add(sGroup);
                    //    }
                    //}
                    return result;
                }
            //var res = new List<IGrouping<IUriNode, Triple>>();
            //var gList = new List<IUriNode>();
            //foreach (var g in variableDataSet)
            //    if (SPogCache.Contains(objectNode, g))
            //        res.Add(SPogCache.Get(objectNode, g));
            //    else
            //        gList.Add(g);
            //foreach (var gRes in store.NamedGraphs.GetTriplesWithObjectFromGraphs(objectNode, gList))
            //{
            //    SPogCache.Add(objectNode, gRes.Key, gRes);
            //    res.Add(gRes);
            //    foreach (var tripleGroup in gRes.GroupBy(triple => triple.Subject))
            //    {
            //        var pGroup = new Grouping<IUriNode, IUriNode>(gRes.Key,
            //            tripleGroup.Select(triple => triple.Predicate));
            //        sPogCache.Add(tripleGroup.Key, objectNode, gRes.Key, pGroup);

            //        sPoGCache.Get(tripleGroup.Key, objectNode).Add(pGroup);
            //        foreach (var p in pGroup)
            //        {
            //            spoGCache.Get(tripleGroup.Key, p, objectNode).Add(gRes.Key);
            //            spogCache.Add(tripleGroup.Key, p, objectNode, gRes.Key, true);
            //        }
            //    }
            //    foreach (var tripleGroup in gRes.GroupBy(triple => triple.Predicate))
            //    {
            //        var sGroup = new Grouping<IUriNode, ISubjectNode>(gRes.Key,
            //            tripleGroup.Select(triple => triple.Subject));
            //        SpogCache.Add(tripleGroup.Key, objectNode, gRes.Key, sGroup);
            //        SpoGCache.Get(tripleGroup.Key, objectNode).Add(sGroup);
            //    }
            //}
            //return res;
        }
    }

}