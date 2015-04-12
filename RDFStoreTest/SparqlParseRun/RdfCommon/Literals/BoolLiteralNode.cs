namespace SparqlParseRun.RdfCommon.Literals
{
    public class BoolLiteralNode : TypedLiteralNode
    {
        private static BoolLiteralNode _boolLiteralNodeFalse;
        private static BoolLiteralNode _boolLiteralNodeTrue;

        public BoolLiteralNode(bool Value, IUriNode boolTypeNode)
            : base(Value, boolTypeNode)
        {
           
        }
       

        public static BoolLiteralNode TrueNode(IUriNode boolType)
        {
            return _boolLiteralNodeTrue ?? (_boolLiteralNodeTrue = new BoolLiteralNode(true, boolType));            
        }

        public static BoolLiteralNode FalseNode(IUriNode boolType)
        {
            return _boolLiteralNodeFalse ?? (_boolLiteralNodeFalse = new BoolLiteralNode(false, boolType));
        }
    }
}