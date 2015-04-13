namespace SparqlParseRun.RdfCommon.Literals
{
    public interface ILiteralNode : INode
    {
        LiteralType LiteralType { get; }
        IUriNode DataType { get; }
        dynamic Content { get;  }
    }
    public enum LiteralType
    {
        TypedObject,
        LanguageType,
        Simple
    }
}