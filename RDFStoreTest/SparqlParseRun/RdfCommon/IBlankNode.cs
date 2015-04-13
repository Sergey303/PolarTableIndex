namespace SparqlParseRun.RdfCommon
{
    public interface IBlankNode : ISubjectNode ,INode
    {
        string Name { get; }
    }
}