using System;

namespace SparqlParseRun.RdfCommon
{
    public interface IUriNode : INode, ISubjectNode, IComparable
    {
         string UriString { get; }
        UriPrefixed UriPrefixed { get; }

        string ToStringWithBraces();

    }

    
}
