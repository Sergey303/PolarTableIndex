using System;
using System.Collections.Generic;
using System.Linq;
using SparqlParseRun.RdfCommon;
using SparqlParseRun.SparqlClasses.GraphPattern.Triples.Node;
using SparqlParseRun.SparqlClasses.Query.Result;

namespace SparqlParseRun.SparqlClasses.GraphPattern.Triples
{
    public class SparqlTriple : Triple, ISparqlGraphPattern
    {
        protected readonly DataSet graphs;
        private bool isSKnown = true;
        private bool isPKnown = true;
        private bool isOKnown = true;
        private SparqlVariableBinding sBinding;
        private SparqlVariableBinding pBinding;
        private SparqlVariableBinding oBinding;
        protected readonly VariableNode sVariableNode;
        private readonly VariableNode pVariableNode;
        protected readonly VariableNode oVariableNode;
        private readonly VariableDataSet variableDataSet;
        protected readonly RdfQuery11Translator q;

        public SparqlTriple(Triple t, RdfQuery11Translator q)  :base(t.Subject,t.Predicate, t.Object)
        {
            this.q = q;
            sVariableNode = Subject as VariableNode;
            pVariableNode = Predicate as VariableNode;
            oVariableNode = Object as VariableNode;
            variableDataSet = (q.ActiveGraphs as VariableDataSet);
            graphs = q.ActiveGraphs;
        }

        public SparqlTriple(INode subj, IUriNode pred, INode obj, RdfQuery11Translator q)
            : base((ISubjectNode) subj, pred, obj)
        {
            this.q = q;
            //if(!(subj is ISubjectNode)) throw new ArgumentException();
            graphs = q.ActiveGraphs;
             //this.Graph = graph;
            sVariableNode = Subject as VariableNode;
            pVariableNode = Predicate as VariableNode;
            oVariableNode = Object as VariableNode;
            variableDataSet = (q.ActiveGraphs as VariableDataSet);
        }


        public virtual IEnumerable<SparqlResult> Run(IEnumerable<SparqlResult> variableBindings)
        {
                return variableBindings.SelectMany(CreateBindings);
        }

        public SparqlGraphPatternType PatternType { get{return SparqlGraphPatternType.SparqlTriple;} }

        private IEnumerable<SparqlResult> CreateBindings(SparqlResult variableBinding)
        {
            ISubjectNode sValue = Subject;
            IUriNode pValue = Predicate;
            INode oValue = Object;


            TryGetSpoVariablesValues(variableBinding, ref sValue, ref pValue, ref oValue);
            SparqlVariableBinding varGraphValue;

            if (variableDataSet == null)
                if (graphs.Count == 0) return SetVariablesValues(variableBinding, oValue, pValue, sValue);   //todo
                else return SetVariablesValuesFromGraphs(variableBinding,  oValue, pValue, sValue, graphs);
            else if (variableBinding.row.TryGetValue(variableDataSet.Variable, out varGraphValue))
                return SetVariablesValuesFromGraphs(variableBinding,  oValue, pValue, sValue, new DataSet() { (IUriNode) varGraphValue.Value});
            else return SetVariablesValuesVarGraphs(variableBinding,  oValue, pValue, sValue);
        }

      

        private IEnumerable<SparqlResult> SetVariablesValues(SparqlResult variableBinding, INode oValue, IUriNode pValue,
            ISubjectNode sValue)
        {
            if (!isSKnown)
                if (!isPKnown)
                    if (!isOKnown)
                        return
                            q.StoreCalls.SPO(sVariableNode, pVariableNode, oVariableNode,
                                variableBinding);
                    else
                        return
                            q.StoreCalls.SPo(sVariableNode, pVariableNode, oValue,
                                variableBinding);
                else
            if (!isOKnown)

                return
                    q.StoreCalls.SpO(sVariableNode, pValue, oVariableNode,
                        variableBinding);

                else
            return
                q.StoreCalls.Spo(sVariableNode, pValue, oValue,
                    variableBinding);
            else
            {
                if (!isPKnown)
                    if (!isOKnown)
                        return
                            q.StoreCalls.sPO(sValue, pVariableNode, oVariableNode,
                                variableBinding);
                    else
                return
                    q.StoreCalls.sPo(sValue, pVariableNode, oValue,
                        variableBinding);
                           
                else
                {
                    if (!isOKnown)
                        return
                            q.StoreCalls.spO(sValue, pValue, oVariableNode,
                                variableBinding);
                    else
                        return q.StoreCalls.spo(sValue, pValue, oValue, variableBinding);
                }
            }
        }

        private IEnumerable<SparqlResult> SetVariablesValuesFromGraphs(SparqlResult variableBinding, INode oValue, IUriNode pValue, ISubjectNode sValue, DataSet namedGraphs)
        {
            if (!isSKnown)
                if (!isPKnown)
                    if (!isOKnown)
                        return
                            q.StoreCalls.SPOGraphs(sVariableNode, pVariableNode, oVariableNode,
                                variableBinding, namedGraphs);
                    else
                        return
                            q.StoreCalls.SPoGraphs(sVariableNode, pVariableNode, oValue,
                                variableBinding, namedGraphs);
                else if (!isOKnown)

                    return
                        q.StoreCalls.SpOGraphs(sVariableNode, pValue, oVariableNode,
                            variableBinding, namedGraphs);

                else
                    return
                        q.StoreCalls.SpoGraphs(sVariableNode, pValue, oValue,
                            variableBinding, namedGraphs);
            else
            {
                if (!isPKnown)
                    if (!isOKnown)
                        return
                            q.StoreCalls.sPOGraphs(sValue, pVariableNode, oVariableNode,
                                variableBinding, namedGraphs);
                    else
                        return
                            q.StoreCalls.sPoGraphs(sValue, pVariableNode, oValue,
                                variableBinding, namedGraphs);
                else
                {
                    if (!isOKnown)
                        return
                            q.StoreCalls.spOGraphs(sValue, pValue, oVariableNode,
                                variableBinding, namedGraphs);

                    else
                        return q.StoreCalls.spoGraphs(sValue, pValue, oValue, variableBinding, namedGraphs);
                }
            }
        }

        private IEnumerable<SparqlResult> SetVariablesValuesVarGraphs(SparqlResult variableBinding,  INode oValue, IUriNode pValue,
            ISubjectNode sValue)
        {
            if (!isSKnown)
                if (!isPKnown)
                    if (!isOKnown)
                        return
                            q.StoreCalls.SPOVarGraphs(sVariableNode, pVariableNode, oVariableNode,
                                variableBinding, variableDataSet);
                    else
                        return
                            q.StoreCalls.SPoVarGraphs(sVariableNode, pVariableNode, oValue,
                                variableBinding, variableDataSet);
                else if (!isOKnown)

                    return
                        q.StoreCalls.SpOVarGraphs(sVariableNode, pValue, oVariableNode,
                            variableBinding, variableDataSet);

                else
                    return
                        q.StoreCalls.SpoVarGraphs(sVariableNode, pValue, oValue,
                            variableBinding, variableDataSet);
            else
            {
                if (!isPKnown)
                    if (!isOKnown)
                        return
                            q.StoreCalls.sPOVarGraphs(sValue, pVariableNode, oVariableNode,
                                variableBinding, variableDataSet);
                    else
                        return
                            q.StoreCalls.sPoVarGraphs(sValue, pVariableNode, oValue,
                                variableBinding, variableDataSet);
                else
                {
                    if (!isOKnown)
                        return
                            q.StoreCalls.spOVarGraphs(sValue, pValue, oVariableNode,
                                variableBinding, variableDataSet);
                    else
                    {
                        return
                            q.StoreCalls.spoVarGraphs(sValue, pValue, oValue, variableBinding, variableDataSet);
                    }
                }
            }
        }

        private void TryGetSpoVariablesValues(SparqlResult variableBinding, ref ISubjectNode sValue, ref IUriNode pValue,
            ref INode oValue)
        {

            isSKnown = true;
            isPKnown = true;
            isOKnown = true;
            if (sVariableNode != null)
                if (variableBinding.row.TryGetValue(sVariableNode, out sBinding))
                {
                    sValue = sBinding.Value as ISubjectNode;
                    if (sValue == null) throw new Exception("subject is not ISubjectNode");
                }
                else
                    isSKnown = false;
            if (pVariableNode != null)
                if (variableBinding.row.TryGetValue(pVariableNode, out pBinding))
                {
                    pValue = pBinding.Value as IUriNode;
                    if (pValue == null) throw new Exception("predicate is not uri");
                }
                else
                    isPKnown = false;
            if (oVariableNode != null)
                if (variableBinding.row.TryGetValue(oVariableNode, out oBinding))
                    oValue = oBinding.Value;
                else
                    isOKnown = false;
        }

        public Triple Substitution(SparqlResult variableBinding, string graphName)
        {
       
           ISubjectNode sValue;
           IUriNode pValue;
           INode oValue;

           IsSKnown(variableBinding, out sValue, out pValue, out oValue);
           if (!isSKnown && sVariableNode is IBlankNode) sValue =q.Store.NodeGenerator.CreateBlankNode(graphName, ((SparqlBlankNode)sVariableNode).Name);
           if (!isOKnown && oVariableNode is IBlankNode) oValue = q.Store.NodeGenerator.CreateBlankNode(graphName, ((SparqlBlankNode)oVariableNode).Name);
          // if (!isPKnown && pVariableNode is IBlankNode) pValue = ((SparqlBlankNode)pVariableNode).RdfBlankNode;
          // if ((isSKnown || sVariableNode is SparqlBlankNode) && (isPKnown || pVariableNode is SparqlBlankNode) && (isOKnown || oVariableNode is SparqlBlankNode))
               return new Triple(sValue, pValue, oValue);
         //  throw new Exception();
        }

        private void IsSKnown(SparqlResult variableBinding, out ISubjectNode sValue, out IUriNode pValue, out INode oValue)
        {
            sValue = Subject;
            pValue = Predicate;
            oValue = Object;
            isSKnown = true;
            isPKnown = true;
            isOKnown = true;



            if (sVariableNode != null)
                if (variableBinding.row.TryGetValue(sVariableNode, out sBinding))
                {
                    sValue = sBinding.Value as ISubjectNode;
                    if (sValue == null) throw new Exception("subject is not ISubjectNode");
                }
                else
                    isSKnown = false;
            if (pVariableNode != null)
                if (variableBinding.row.TryGetValue(pVariableNode, out pBinding))
                {
                    pValue = pBinding.Value as IUriNode;
                    if (pValue == null) throw new Exception("predicate is not uri");
                }
                else
                    isPKnown = false;
            if (oVariableNode != null)
                if (variableBinding.row.TryGetValue(oVariableNode, out oBinding))
                    oValue = oBinding.Value;
                else
                    isOKnown = false;
        }
    }
}