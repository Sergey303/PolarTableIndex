using System;

namespace SparqlParseRun.RdfCommon
{
    public class UriNode :IUriNode  ,IBlankNode
    {
        private readonly string uriString;         

        public override int GetHashCode()
        {
            return uriString.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            var node = obj as UriNode;
            if (node != null)
                return System.String.Compare(uriString, node.UriString, System.StringComparison.Ordinal);
            throw new ArgumentException();
        }

        public UriNode(string p1)
        {
            // TODO: Complete member initialization
            uriString= p1;
        }
         
        public NodeType Type { get { return NodeType.Uri; } }

        public override bool Equals(object obj)
        {
            var uriNode = obj as IUriNode;
            return uriNode != null && UriString.Equals(uriNode.UriString);
        }

        public override string ToString()
        {
            return UriString;
        }

        public string UriString
        {
            get { return uriString; }
        }

        public UriPrefixed UriPrefixed { get { return Prologue.SplitUri(uriString); } }

        public string ToStringWithBraces()
        {
            return "<" + UriString + ">";
        }

        public string Name { get { return uriString; } }
    }
}