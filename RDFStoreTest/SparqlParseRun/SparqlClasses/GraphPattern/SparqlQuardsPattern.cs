﻿using System.Collections.Generic;
using System.Linq;
using SparqlParseRun.SparqlClasses.GraphPattern;
using SparqlParseRun.SparqlClasses.Query.Result;

namespace SparqlParseRun.SparqlClasses
{
    public class SparqlQuardsPattern : List<ISparqlGraphPattern>, ISparqlGraphPattern
    {
        public virtual IEnumerable<SparqlResult> Run(IEnumerable<SparqlResult> variableBindings)
        {
             return this.Aggregate(variableBindings, (current, pattern) => pattern.Run(current).ToArray()).ToArray();
        }

        public virtual SparqlGraphPatternType PatternType { get{return SparqlGraphPatternType.ListOfPatterns;} }
    
    }
}
