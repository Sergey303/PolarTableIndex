using System;
using System.Collections.Generic;
using System.Linq;
using SparqlParseRun.SparqlClasses.GraphPattern;
using SparqlParseRun.SparqlClasses.GraphPattern.Triples.Node;
using SparqlParseRun.SparqlClasses.Query.Result;

namespace SparqlParseRun.SparqlClasses.Query
{
    public class SparqlSelect : List<IVariableNode>
    {

        private bool isAll;
        internal bool IsReduced;


        internal bool IsDistinct { get; set; }
    

        internal void IsAll()
        {
            isAll = true;
        }


        public IEnumerable<SparqlResult> Run(IEnumerable<SparqlResult> variableBindings, SparqlResultSet resultSet = null)
        {
            var bindings = variableBindings as SparqlResult[] ?? variableBindings.ToArray();
            if (bindings.Length == 0) return bindings;
            if (isAll)
            {
                var listForRemove = new List<VariableNode>();
                foreach (var sparqlResult in bindings)
                {
                    listForRemove.AddRange(sparqlResult.row.Keys.Where(v => string.IsNullOrWhiteSpace(v.VariableName)));
                    foreach (var variableNode in listForRemove)
                        sparqlResult.row.Remove(variableNode);
                    listForRemove.Clear();
                }
            }
            else
            {
                if (resultSet != null)
                    resultSet.Variables.Clear();
                SparqlResult[] newList = bindings.Select(result => new SparqlResult()).ToArray();
                foreach (var v in this)
                {
                    var expressionAsVariable = v as SparqlExpressionAsVariable;
                    var variableNode = (v as VariableNode);
                    if (expressionAsVariable != null)
                    {
                        if (resultSet != null)
                            resultSet.Variables.Add(expressionAsVariable.variableNode.VariableName,
                                expressionAsVariable.variableNode);
                        if (!(bindings.First() is SpraqlGroupOfResults) &&
                            expressionAsVariable.sparqlExpression.IsAggragate)
                        {
                            bindings = new SparqlResult[] {new SpraqlGroupOfResults() {Group = bindings}};
                            newList = new SparqlResult[] {new SpraqlGroupOfResults() {Group = newList}};
                        }

                        for (int i = 0; i < newList.Length; i++)
                        {
                            var newVariable = expressionAsVariable.RunExpressionCreateBind(bindings[i]);
                            newList[i].row.Add(expressionAsVariable.variableNode, newVariable);
                            bindings[i].row.Add(expressionAsVariable.variableNode, newVariable);
                        }
                    }
                    else if (variableNode != null)
                    {
                        if (resultSet != null)
                            resultSet.Variables.Add(variableNode.VariableName, variableNode);
                        for (int i = 0; i < newList.Length; i++)
                            if (bindings[i].row.ContainsKey(variableNode))
                                newList[i].row.Add(variableNode, bindings[i][variableNode]);
                    }
                    else
                        throw new ArgumentNullException("variableNode");
                }

                bindings = newList;
            }
            if (IsDistinct)
                bindings = bindings.Distinct(new BindingsComparer()).ToArray();
            if (IsReduced)
            {
                var counts = new Dictionary<SparqlResult, int>();
                foreach (var variableBinding in bindings)
                    if (counts.ContainsKey(variableBinding)) counts[variableBinding]++;
                    else counts.Add(variableBinding, 1);
                bindings = Reduce(counts).ToArray();
            }

            return bindings;
        }

        private static IEnumerable<SparqlResult> Reduce(Dictionary<SparqlResult, int> counts)
        {
            foreach (var count in counts)
            {
                yield return count.Key;
                if (count.Value > 1)
                    yield return count.Key;
            }
        }

        class BindingsComparer :  IEqualityComparer<SparqlResult>
        {
            public bool Equals(SparqlResult x, SparqlResult y)
            {
                if (x.row.Count != y.row.Count) return false;
                SparqlVariableBinding v2;
                return x.row.Keys.All(key => y.row.TryGetValue(key, out v2) && x[key] .Value.Equals(v2.Value));
            }

            public int GetHashCode(SparqlResult obj)
            {
                unchecked
                {
                    int sum = 0;
                    foreach (SparqlVariableBinding b in obj.row.Values)
                        sum += b.Value.GetHashCode()^2;
                    return sum;
                }
            }
        }

        

    }
}
