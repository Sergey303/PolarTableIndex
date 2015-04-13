using SparqlParseRun.RdfCommon.Literals;

namespace SparqlParseRun.RdfCommon
{
    public interface INodeGenerator
    {
       // IUriNode CreateUriNode(string p);
        IUriNode CreateUriNode(UriPrefixed uri);
        ILiteralNode CreateLiteralNode(string p);
        ILiteralNode CreateLiteralNode(string s, string lang);
        ILiteralNode CreateLiteralNode(int parse);
        ILiteralNode CreateLiteralNode(decimal p);
        ILiteralNode CreateLiteralNode(double p);
        ILiteralNode CreateLiteralNode(bool p);
        ILiteralNode CreateLiteralNode(string p, IUriNode sparqlUriNode);
        IBlankNode CreateBlankNode(string graphName, string blankNodeString = null);
        IUriNode GetUri(string uri);      
    }
}