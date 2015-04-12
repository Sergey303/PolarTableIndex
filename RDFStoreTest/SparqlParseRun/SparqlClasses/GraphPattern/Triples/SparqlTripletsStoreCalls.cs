using System.Collections.Generic;
using System.Linq;
using SparqlParseRun.RdfCommon;
using SparqlParseRun.SparqlClasses.GraphPattern.Triples.Node;
using SparqlParseRun.SparqlClasses.Query.Result;

namespace SparqlParseRun.SparqlClasses.GraphPattern.Triples
{
    public  class SparqlTripletsStoreCalls : ISparqlTripletsStoreCalls
    {
        private IStore store;

        public SparqlTripletsStoreCalls(IStore store)
        {
            this.store = store;
        }

        public IEnumerable<SparqlResult> spO(ISubjectNode subjNode, IUriNode predicateNode,
            VariableNode obj, SparqlResult variablesBindings)
        {
                return store
                    .GetTriplesWithSubjectPredicate(subjNode, predicateNode)
                    .Select(node => new SparqlResult(variablesBindings, node, obj));
        }

        // from merged named graphs
        public IEnumerable<SparqlResult> spOVarGraphs( ISubjectNode subjNode, IUriNode predicateNode,
            VariableNode obj, SparqlResult variablesBindings, VariableDataSet variableDataSet)
             {   return store
                 .NamedGraphs
                .GetTriplesWithSubjectPredicateFromGraphs(subjNode, predicateNode, variableDataSet) //.GetGraphUri(variablesBindings) if graphs is empty, Gets All named graphs
                .SelectMany(grouping =>
                        grouping.Select(node =>
                            new SparqlResult(variablesBindings, node, obj, grouping.Key, variableDataSet.Variable))); // if graphVariable is null, ctor check this.
            
            }

        public IEnumerable<SparqlResult> spOGraphs( ISubjectNode subjNode, IUriNode predicateNode,
            VariableNode obj, SparqlResult variablesBindings, DataSet graphs)
        {
            return store
                 .NamedGraphs
                .GetTriplesWithSubjectPredicateFromGraphs(subjNode, predicateNode, graphs) // if graphs is empty, Gets All named graphs
                .SelectMany(grouping => grouping.Select(node => new SparqlResult(variablesBindings, node, obj)));     
        }


        public IEnumerable<SparqlResult> Spo( VariableNode subj, IUriNode predicateNode, INode objectNode, SparqlResult variablesBindings)
        {
            return store
                .GetTriplesWithPredicateObject(predicateNode, objectNode)
                .Select(node => new SparqlResult(variablesBindings, node, subj));

            // from merged named graphs
        }


        public IEnumerable<SparqlResult> SpoGraphs( VariableNode subj, IUriNode predicateNode,
            INode objectNode, SparqlResult variablesBindings, DataSet graphs)
        {
            return store
                 .NamedGraphs
                .GetTriplesWithPredicateObjectFromGraphs(predicateNode, objectNode, graphs)
                // if graphs is empty, Gets All named graphs
                .SelectMany(grouping =>
                   
                        grouping.Select(node =>
                            new SparqlResult(variablesBindings, node, subj)));
                // if graphVariable is null, ctor check this.
        }

        public IEnumerable<SparqlResult> SpoVarGraphs( VariableNode subj, IUriNode predicateNode,
            INode objectNode, SparqlResult variablesBindings, VariableDataSet variableDataSet)
        {
            return store
                 .NamedGraphs
                .GetTriplesWithPredicateObjectFromGraphs(predicateNode, objectNode, variableDataSet) //.GetGraphUri(variablesBindings) if graphs is empty, Gets All named graphs
                .SelectMany(grouping => 
                   grouping.Select(node =>
                       new SparqlResult(variablesBindings, node, subj, grouping.Key, variableDataSet.Variable))); // if graphVariable is null, ctor check this.

        }

        public IEnumerable<SparqlResult> SpO( VariableNode s, IUriNode predicate, VariableNode o, SparqlResult variablesBindings)
        {
            return store
                .GetTriplesWithPredicate(predicate)
                .Select(triple => new SparqlResult(new Dictionary<VariableNode, SparqlVariableBinding>(variablesBindings.row)
                {
                    {s, new SparqlVariableBinding(s,triple.Subject)},
                    {o, new SparqlVariableBinding(o,triple.Object)}
                }));
        }

        public IEnumerable<SparqlResult> SpOGraphs(VariableNode s, IUriNode predicate, VariableNode o, SparqlResult variablesBindings, DataSet graphs)
        {
            return store
                 .NamedGraphs
            .GetTriplesWithPredicateFromGraphs(predicate, graphs)
             .SelectMany(grouping =>
                     grouping
                        .Select(triple => new SparqlResult(new Dictionary<VariableNode, SparqlVariableBinding>(variablesBindings.row)
              {
                  {s, new SparqlVariableBinding(s,triple.Subject)},
                  {o, new SparqlVariableBinding(o,triple.Object)}
              })));
        }

        public IEnumerable<SparqlResult> SpOVarGraphs( VariableNode s, IUriNode predicate, VariableNode o, SparqlResult variablesBindings, VariableDataSet graphs)
        {
            return store
                 .NamedGraphs
            .GetTriplesWithPredicateFromGraphs(predicate, graphs) //.GetGraphUri(variablesBindings))
             .SelectMany(grouping =>
                     grouping
                        .Select(triple => new SparqlResult(new Dictionary<VariableNode, SparqlVariableBinding>(variablesBindings.row)
              {
                  {s, new SparqlVariableBinding(s,triple.Subject)},
                  {o, new SparqlVariableBinding(o,triple.Object)}, 
                  {graphs.Variable, new SparqlVariableBinding(graphs.Variable,grouping.Key)}
              })));
        }


        public IEnumerable<SparqlResult> sPo( ISubjectNode subj, VariableNode pred, INode obj, SparqlResult variablesBindings)
        {
            return store
                .GetTriplesWithSubjectObject(subj, obj)
                .Select(newObj => new SparqlResult(variablesBindings, newObj, pred));

        }

        public IEnumerable<SparqlResult> sPoGraphs(ISubjectNode subj, VariableNode pred, INode obj, SparqlResult variablesBindings, DataSet graphs)
        {
            return store
                 .NamedGraphs
                .GetTriplesWithSubjectObjectFromGraphs(subj, obj, graphs) 
                .SelectMany(grouping =>
                        grouping.Select(node =>
                            new SparqlResult(variablesBindings, node, pred)));
        }

        public IEnumerable<SparqlResult> sPoVarGraphs( ISubjectNode subj, VariableNode pred, INode obj, SparqlResult variablesBindings, VariableDataSet variableDataSet)
        {
            return store
                 .NamedGraphs
               .GetTriplesWithSubjectObjectFromGraphs(subj, obj, variableDataSet) //.GetGraphUri(variablesBindings)
               .SelectMany(grouping =>           
                   grouping.Select(node =>
                           new SparqlResult(variablesBindings, node, pred, grouping.Key, variableDataSet.Variable)));
        }


        public IEnumerable<SparqlResult> sPO( ISubjectNode subj, VariableNode pred, VariableNode obj, SparqlResult variablesBindings)
        {
            return store
              .GetTriplesWithSubject(subj)
              .Select(triple => new SparqlResult(new Dictionary<VariableNode, SparqlVariableBinding>(variablesBindings.row)
              {
                  {pred, new SparqlVariableBinding(pred,triple.Predicate)},
                  {obj, new SparqlVariableBinding(obj,triple.Object)}
              }));
        }

        public IEnumerable<SparqlResult> sPOGraphs( ISubjectNode subj, VariableNode pred,
            VariableNode obj, SparqlResult variablesBindings, DataSet graphs)
        {
              return store
                 .NamedGraphs
              .GetTriplesWithSubjectFromGraphs(subj, graphs)
               .SelectMany(grouping =>
                       grouping
                          .Select(triple => new SparqlResult(new Dictionary<VariableNode, SparqlVariableBinding>(variablesBindings.row)
              {
                  {pred, new SparqlVariableBinding(pred,triple.Predicate)},
                  {obj, new SparqlVariableBinding(obj,triple.Object)}
              })));
        }

        public IEnumerable<SparqlResult> sPOVarGraphs( ISubjectNode subj, VariableNode pred,
           VariableNode obj, SparqlResult variablesBindings, VariableDataSet variableDataSet)
        {
            return store
                 .NamedGraphs
            .GetTriplesWithSubjectFromGraphs(subj, variableDataSet)//.GetGraphUri(variablesBindings)
             .SelectMany(grouping =>
                     grouping
                        .Select(triple => new SparqlResult(new Dictionary<VariableNode, SparqlVariableBinding>(variablesBindings.row)
              {
                  {pred, new SparqlVariableBinding(pred,triple.Predicate)},
                  {obj, new SparqlVariableBinding(obj,triple.Object)},
                  {variableDataSet.Variable, new SparqlVariableBinding(variableDataSet.Variable,grouping.Key)},
              })));
        }

        public IEnumerable<SparqlResult> SPo( VariableNode subj, VariableNode predicate, INode obj, SparqlResult variablesBindings)
        {
            return store
             .GetTriplesWithObject(obj)
             .Select(triple => new SparqlResult(new Dictionary<VariableNode, SparqlVariableBinding>(variablesBindings.row)
              {
                  {predicate, new SparqlVariableBinding(predicate,triple.Predicate)},
                  {subj, new SparqlVariableBinding(subj,triple.Subject)}
              }));
        }

        public IEnumerable<SparqlResult> SPoGraphs( VariableNode subj, VariableNode pred,
    INode obj, SparqlResult variablesBindings, DataSet graphs)
        {
            return store
                 .NamedGraphs
            .GetTriplesWithObjectFromGraphs(obj, graphs)
             .SelectMany(grouping =>
                     grouping
                        .Select(triple => new SparqlResult(new Dictionary<VariableNode, SparqlVariableBinding>(variablesBindings.row)
              {
                  {pred, new SparqlVariableBinding(pred,triple.Predicate)},
                  {subj, new SparqlVariableBinding(subj,triple.Subject)}
              })));
        }

        public IEnumerable<SparqlResult> SPoVarGraphs( VariableNode subj, VariableNode pred,
           INode obj, SparqlResult variablesBindings, VariableDataSet variableDataSet)
        {
            return store
                 .NamedGraphs
            .GetTriplesWithObjectFromGraphs(obj, variableDataSet) //.GetGraphUri(variablesBindings)
             .SelectMany(grouping =>
                     grouping
                        .Select(triple => new SparqlResult(new Dictionary<VariableNode, SparqlVariableBinding>(variablesBindings.row)
              {
                  {pred, new SparqlVariableBinding(pred,triple.Predicate)},
                  {subj, new SparqlVariableBinding(subj,triple.Subject)},
                  {variableDataSet.Variable, new SparqlVariableBinding(variableDataSet.Variable,grouping.Key)},
              })));
        }


        public IEnumerable<SparqlResult> SPO( VariableNode subj, VariableNode predicate, VariableNode obj, SparqlResult variablesBindings)
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

        public IEnumerable<SparqlResult> SPOVarGraphs( VariableNode subj, VariableNode predicate, VariableNode obj, SparqlResult variablesBindings, VariableDataSet variableDataSet)
        {
            return store
                 .NamedGraphs
             .GetTriplesFromGraphs(variableDataSet) //.GetGraphUri(variablesBindings)
                .SelectMany(grouping =>
                     grouping
             .Select(triple => new SparqlResult(new Dictionary<VariableNode, SparqlVariableBinding>(variablesBindings.row)
              {
                  {subj, new SparqlVariableBinding(subj,triple.Subject)} ,
                  {predicate, new SparqlVariableBinding(predicate,triple.Predicate)},
                  {obj, new SparqlVariableBinding(obj,triple.Object)}
              })));

        }

        public IEnumerable<SparqlResult> spoGraphs(ISubjectNode subjectNode, IUriNode predicateNode, INode objectNode, SparqlResult variablesBindings,
            DataSet graphs)
        {                                                                       
            if (store.NamedGraphs.Contains(subjectNode, predicateNode, objectNode, graphs))
              yield return  variablesBindings;
        }

    

        public IEnumerable<SparqlResult> spoVarGraphs(ISubjectNode subjectNode, IUriNode predicateNode, INode objectNode,
            SparqlResult variablesBindings, VariableDataSet graphs)
        {
            return store.NamedGraphs.GetGraphs(subjectNode, predicateNode, objectNode, graphs)
                .Select(g => new SparqlResult(variablesBindings, g, graphs.Variable));
        }

        public IEnumerable<SparqlResult> spo(ISubjectNode subjectNode, IUriNode predicateNode, INode objNode, SparqlResult variablesBindings)
        {
            if (store.Contains(subjectNode, predicateNode, objNode))
                yield return variablesBindings;
        }
    }
}