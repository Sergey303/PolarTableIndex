using System;
using SparqlParseRun.RdfCommon;


    internal class CodedUriNode : IUriNode, IBlankNode, IEquatable<CodedUriNode>
    {
        public bool Equals(CodedUriNode other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Code == other.Code && Equals(creator, other.creator);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CodedUriNode) obj);
        }

        public override int GetHashCode()
         {
            unchecked
            {
                return Code; //(Code*397) ^ (creator != null ? creator.GetHashCode() : 0);
            }
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }


        public static bool operator ==(CodedUriNode left, CodedUriNode right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CodedUriNode left, CodedUriNode right)
        {
            return !Equals(left, right);
        }

        private readonly CodedNodeGenerator creator;
        public readonly int Code;
        private Uri uri;

        public CodedUriNode(CodedNodeGenerator creator, int code)
        {
            this.creator = creator;
            this.Code = code;
        }
        
        public NodeType Type { get{ return NodeType.Uri;} }
        public string UriString { get { return creator.GetString(Code); }}
        public UriPrefixed UriPrefixed { get { return creator.GetUriRefixed(Code); } }

        public string ToStringWithBraces()
        {
            return "<" + UriString + ">";
        }
        /// <summary>
        /// blank node
        /// </summary>
        public string Name { get { return UriString; } }

        public override string ToString()
        {
            return UriString;
        }
    }
