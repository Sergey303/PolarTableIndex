namespace SparqlParseRun.RdfCommon.Literals
{
    public class TypedLiteralNode : ILiteralNode
    {
        private readonly dynamic content;
        public readonly IUriNode UriType;

        public TypedLiteralNode(dynamic content, IUriNode uriType)
        {
            this.content = content;
            UriType = uriType;
        }

     

        public virtual IUriNode DataType
        {
            get { return UriType; }
        }

        public dynamic Content { get { return content; } }


        public virtual LiteralType LiteralType{get { return LiteralType.TypedObject; }}
        public NodeType Type { get{return NodeType.Literal;} }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (!(obj is TypedLiteralNode)) return false;
            return Equals((TypedLiteralNode)obj);
        }

        protected bool Equals(TypedLiteralNode other)
        {
            return Equals(content, other.Content) && Equals(UriType, other.DataType);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = content.GetHashCode();
                hashCode = hashCode *(11) ^ (UriType != null ? UriType.GetHashCode() : 1);
                return hashCode;
            }
        }

        public override string ToString()
        {
            //if(content is DateTimeOffset)
            //    return string.Format("\"{0}\"^^{1}", content.ToString().Replace("+", "%2B"), DataType).ToString();
            return string.Format("\"{0}\"^^{1}", content, DataType.ToStringWithBraces()).ToString();
        }
    }

    
}

