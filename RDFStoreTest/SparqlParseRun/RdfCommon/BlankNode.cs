namespace SparqlParseRun.RdfCommon
{
    public class BlankNode    :IBlankNode
    {
        public NodeType Type { get{return NodeType.Blank;} }

        public BlankNode()
        {
            //Name = "blank" + (long) (random.NextDouble()*1000*1000*1000*1000);
        }

        public BlankNode(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
        public override string ToString()
        {
            return Name;
        }

    }
}
