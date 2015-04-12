namespace SparqlParseRun.RdfCommon.Literals
{
    public abstract class LiteralNode : ILiteralNode
    {
        protected LiteralNode(dynamic content)
        {
            this.content = content;
        }

        protected readonly dynamic content;
        public  dynamic Content { get { return content; } }
        public NodeType Type { get{ return NodeType.Literal;}}
        public abstract IUriNode DataType { get; }
        public abstract LiteralType LiteralType { get; }
        public override int GetHashCode()
        {
            return content.GetHashCode();
        }
    }
}