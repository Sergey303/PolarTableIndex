namespace SparqlParseRun.RdfCommon
{
    public class UriPrefixed
    {
        public string Prefix;
        public string LocalName;
        public string Namespace;

        public UriPrefixed(string prefix, string localName, string ns)
        {
            Prefix = prefix;
            LocalName = localName;
            Namespace = ns;
        }

        private string fullName;
        public string FullName { get { return  fullName ?? (fullName=Namespace + LocalName); } }
    }
}