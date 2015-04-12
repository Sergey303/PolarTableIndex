using System;

namespace SparqlParseRun.RdfCommon.Literals
{
    public interface IStringLiteralNode : ILiteralNode, IComparable
    {
        bool ComparebleWith(ILiteralNode other);
    }
}