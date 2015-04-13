using System.Collections.Generic;
using System.Linq;
using SparqlParseRun.RdfCommon;
using SparqlParseRun.SparqlClasses.GraphPattern.Triples.Node;

namespace SparqlParseRun.SparqlClasses.Query.Result
{
    public class SpraqlGroupOfResults : SparqlResult
    {
        public IEnumerable<SparqlResult> Group;

        public SpraqlGroupOfResults(VariableNode variable, INode value)
        {
            row.Add(variable, new SparqlVariableBinding(variable, value));
        }

        public SpraqlGroupOfResults()
        {
          
        }

        public SpraqlGroupOfResults(IEnumerable<VariableNode> variables, IEnumerable<INode> values)
        {
            int i = 0;
            var valuesArray = values.ToArray();
            foreach (var variable in variables)
            {
                i++;
                if(variable==null) continue;
                row.Add(variable,new SparqlVariableBinding(variable,valuesArray[i]));
            }
        }
    }
}