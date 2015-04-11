using SparqlParseRun.RdfCommon;

public static class CodedExtensions
{
    public static int GetCode(this ISubjectNode uriNode)
    {
        return ((CodedUriNode) uriNode).Code;
    }   
}