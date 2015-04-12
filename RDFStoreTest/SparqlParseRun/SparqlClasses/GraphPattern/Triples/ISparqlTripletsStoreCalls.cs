using System.Collections.Generic;
using SparqlParseRun.RdfCommon;
using SparqlParseRun.SparqlClasses.GraphPattern.Triples.Node;
using SparqlParseRun.SparqlClasses.Query.Result;

namespace SparqlParseRun.SparqlClasses.GraphPattern.Triples
{
    public interface ISparqlTripletsStoreCalls
    {
        IEnumerable<SparqlResult> spO(ISubjectNode subjNode, IUriNode predicateNode,
            VariableNode obj, SparqlResult variablesBindings);

        IEnumerable<SparqlResult> spOVarGraphs(ISubjectNode subjNode, IUriNode predicateNode,
            VariableNode obj, SparqlResult variablesBindings, VariableDataSet variableDataSet);

        IEnumerable<SparqlResult> spOGraphs(ISubjectNode subjNode, IUriNode predicateNode,
            VariableNode obj, SparqlResult variablesBindings, DataSet graphs);

        IEnumerable<SparqlResult> Spo(VariableNode subj, IUriNode predicateNode, INode objectNode,
            SparqlResult variablesBindings);

        IEnumerable<SparqlResult> SpoGraphs(VariableNode subj, IUriNode predicateNode,
            INode objectNode, SparqlResult variablesBindings, DataSet graphs);

        IEnumerable<SparqlResult> SpoVarGraphs(VariableNode subj, IUriNode predicateNode,
            INode objectNode, SparqlResult variablesBindings, VariableDataSet variableDataSet);

        IEnumerable<SparqlResult> SpO(VariableNode s, IUriNode predicate, VariableNode o, SparqlResult variablesBindings);

        IEnumerable<SparqlResult> SpOGraphs(VariableNode s, IUriNode predicate, VariableNode o,
            SparqlResult variablesBindings, DataSet graphs);

        IEnumerable<SparqlResult> SpOVarGraphs(VariableNode s, IUriNode predicate, VariableNode o,
            SparqlResult variablesBindings, VariableDataSet graphs);

        IEnumerable<SparqlResult> sPo(ISubjectNode subj, VariableNode pred, INode obj, SparqlResult variablesBindings);

        IEnumerable<SparqlResult> sPoGraphs(ISubjectNode subj, VariableNode pred, INode obj,
            SparqlResult variablesBindings, DataSet graphs);

        IEnumerable<SparqlResult> sPoVarGraphs(ISubjectNode subj, VariableNode pred, INode obj,
            SparqlResult variablesBindings, VariableDataSet variableDataSet);

        IEnumerable<SparqlResult> sPO(ISubjectNode subj, VariableNode pred, VariableNode obj,
            SparqlResult variablesBindings);

        IEnumerable<SparqlResult> sPOGraphs(ISubjectNode subj, VariableNode pred,
            VariableNode obj, SparqlResult variablesBindings, DataSet graphs);

        IEnumerable<SparqlResult> sPOVarGraphs(ISubjectNode subj, VariableNode pred,
            VariableNode obj, SparqlResult variablesBindings, VariableDataSet variableDataSet);

        IEnumerable<SparqlResult> SPo(VariableNode subj, VariableNode predicate, INode obj,
            SparqlResult variablesBindings);

        IEnumerable<SparqlResult> SPoGraphs(VariableNode subj, VariableNode pred,
            INode obj, SparqlResult variablesBindings, DataSet graphs);

        IEnumerable<SparqlResult> SPoVarGraphs(VariableNode subj, VariableNode pred,
            INode obj, SparqlResult variablesBindings, VariableDataSet variableDataSet);

        IEnumerable<SparqlResult> SPO(VariableNode subj, VariableNode predicate, VariableNode obj,
            SparqlResult variablesBindings);

        IEnumerable<SparqlResult> SPOGraphs(VariableNode subj, VariableNode predicate, VariableNode obj,
            SparqlResult variablesBindings, DataSet graphs);

        IEnumerable<SparqlResult> SPOVarGraphs(VariableNode subj, VariableNode predicate, VariableNode obj,
            SparqlResult variablesBindings, VariableDataSet variableDataSet);

        IEnumerable<SparqlResult> spoGraphs(ISubjectNode subjectNode, IUriNode predicateNode, INode objectNode, SparqlResult variablesBindings, DataSet graphs);
        IEnumerable<SparqlResult> spoVarGraphs(ISubjectNode subjectNode, IUriNode predicateNode, INode objectNode, SparqlResult variablesBindings, VariableDataSet graphs);

        IEnumerable<SparqlResult> spo(ISubjectNode subjectNode, IUriNode predicateNode, INode objNode, SparqlResult variablesBindings);
    }
}