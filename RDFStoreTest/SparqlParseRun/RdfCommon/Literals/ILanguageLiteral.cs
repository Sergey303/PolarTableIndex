namespace SparqlParseRun.RdfCommon.Literals
{
    public interface ILanguageLiteral : IStringLiteralNode
    {
        string Lang { get; }
    }
}