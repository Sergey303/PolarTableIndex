using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SparqlParseRun.RdfCommon
{
    public class PrologueFullString
    {
        public Dictionary<string, string> namspace2Prefix = new Dictionary<string, string>();
        public Dictionary<string, string> prefix2Namspace = new Dictionary<string, string>();
          
        public static Regex PrefixNSSlpit = new Regex("^([^:]*:)(.*)$");
        private string baseUri;
        public string StringRepresentationOfProlog;

        public string GetUriFromPrefixed(string p)
        {
            if (p.StartsWith("<") && p.EndsWith(">"))
                return GetFromIri(p); 
            var uriPrefixed = SplitPrefixed(p);
            // if(prefix=="_")   throw new NotImplementedException();
            string fullNamespace;
            if (!prefix2Namspace.TryGetValue(uriPrefixed.Prefix, out fullNamespace))
                throw new Exception("prefix " + uriPrefixed.Prefix);
            uriPrefixed.Namespace = fullNamespace;
            return uriPrefixed.FullName;
        }

        public static UriPrefixed SplitUndefined(string p)
        {
            if (p.StartsWith("<") && p.EndsWith(">"))
                return SplitUri(p.Substring(1, p.Length - 2));
            if (p.StartsWith("http://") || p.StartsWith("mailto:"))
                return SplitUri(p);
            return SplitPrefixed(p);
        }
        public static UriPrefixed SplitPrefixed(string p)
        {
            var match = PrefixNSSlpit.Match(p);
            var prefix = match.Groups[1].Value;
            var shortName = match.Groups[2].Value;
            return new UriPrefixed(prefix, shortName, null);
        }
                public static UriPrefixed SplitUri(string p)
        {
            var rsi = p.LastIndexOf('\\');
            var lsi = p.LastIndexOf('/');
            var ssi = p.LastIndexOf('#');
            var dot = p.LastIndexOf('.');
            var i = Math.Max(rsi, Math.Max(lsi, Math.Max(ssi, dot)));
            return new UriPrefixed(null, p.Substring(i + 1), p.Substring(0, i+1));
        }
        public string GetUriFromPrefixedNamespace(string p)
        {   
            var match = PrefixNSSlpit.Match(p);
            var prefix = match.Groups[1].Value;
           
          //  if (prefix == "_" ) throw new NotImplementedException();
            string fullNamespace;
            if (!prefix2Namspace.TryGetValue(prefix, out fullNamespace)) throw new Exception("prefix " + prefix);
            return fullNamespace;
        }

        public string GetFromString(string p)
        {
            if (p.StartsWith("<") && p.EndsWith(">"))
            {
                return GetFromIri(p);
            }
            if (p.StartsWith("http://") || p.StartsWith("mailto:"))
            {
                return baseUri == null ? p : baseUri + p;
            }
            return GetUriFromPrefixed(p);
        }

        public string GetFromIri(string p)
        {      
            if(p.StartsWith("<") && p.EndsWith(">"))           
            p = p.Substring(1, p.Length - 2);
            return baseUri == null ? p : baseUri + p;
        }




        public void SetBase(string p)
        {
            baseUri = p;
        }

        public void AddPrefix(string prefix, string ns)
        {
            ns = ns.Substring(1, ns.Length - 2);
            prefix2Namspace.Add(prefix, ns);
            namspace2Prefix.Add(ns, prefix);
        }

        public UriPrefixed SetThisPrefix(UriPrefixed uri)
        {
            string existsPrefix;
            if (namspace2Prefix.TryGetValue(uri.Namespace, out existsPrefix))
                uri.Prefix = existsPrefix;
            return uri;
        }
    }
}