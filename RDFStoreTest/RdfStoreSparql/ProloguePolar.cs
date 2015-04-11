using System;
using PolarDB;
using RdfStoreSparqlNamespace;
using SparqlParseRun.RdfCommon;

public class ProloguePolar:Prologue
{

   private int nextPrefixCount;
    readonly PaCell prefixesCell;

    public ProloguePolar()
    {
        prefixesCell =
            new PaCell(new PTypeSequence(
                new PTypeRecord(new NamedType("prefix", new PType(PTypeEnumeration.sstring)),
                    new NamedType("ns", new PType(PTypeEnumeration.sstring)))),
                NameTableSettings.Default.path + "prefixes namespaces", false);
        if (prefixesCell.IsEmpty) prefixesCell.Fill(new object[0]);
        else
            prefixesCell.Root.Scan(o =>
            {
                var pr_ns = ((object[]) o);
                prefix2Namspace.Add((string) pr_ns[0], (string) pr_ns[1]);
                namspace2Prefix.Add((string) pr_ns[1], (string) pr_ns[2]);
                return true;
            });
    }

  
     
        public string CreatePrefixed(UriPrefixed uri)
        {
            string existsPrefix;
            if (namspace2Prefix.TryGetValue(uri.Namespace, out existsPrefix))
            {
                return existsPrefix+uri.LocalName;
            }
            if (uri.Prefix == null)
            {
                uri.Prefix = "ns" + (nextPrefixCount++)+":";
            }

            string existsNs;
            if (prefix2Namspace.TryGetValue(uri.Prefix, out existsNs))
            {
                if(existsNs.Equals(uri.Namespace)) throw new Exception("xsdfs");
                
                string newPrefix = "ns" + (nextPrefixCount++)+":";
                namspace2Prefix.Add(uri.Namespace, newPrefix);
                prefix2Namspace.Add(newPrefix, uri.Namespace);
                prefixesCell.Root.AppendElement(new object[] { newPrefix, uri.Namespace});
                return newPrefix + uri.LocalName;
            }
            namspace2Prefix.Add(uri.Namespace, uri.Prefix);
            prefix2Namspace.Add(uri.Prefix, uri.Namespace);
            prefixesCell.Root.AppendElement(new object[] { uri.Prefix, uri.Namespace });
            return uri.Prefix + uri.LocalName; 
        }

        public string GetFullUriFromUndefined(string uri)
        {
            UriPrefixed up = Prologue.SplitUndefined(uri);
            string ns;
            if (up.Namespace == null && up.Prefix != null && prefix2Namspace.TryGetValue(up.Prefix, out ns))
                up.Namespace = ns;
            return up.Namespace+up.LocalName;
        }

        public string GetPrefixedUriFromUndefined(string uri)
        {
            UriPrefixed up = Prologue.SplitUndefined(uri);
            string pref;
            if (up.Namespace != null && up.Prefix == null && namspace2Prefix.TryGetValue(up.Namespace, out pref))
                up.Namespace = pref;
            return up.Prefix + up.LocalName;
        }
        //public string CreateString(int code)
        //{
           
        //}
      
       
      
        public void Clear()
        {

            prefixesCell.Clear();
            prefixesCell.Fill(new object[0]);
            prefix2Namspace.Clear();
            namspace2Prefix.Clear();
        }

        public void Close()
        {                                     
            prefixesCell.Close();
        }


        public UriPrefixed GetFullUriFromPrefixed(string prefixed)
        {         
            var uriPrefixed = Prologue.SplitPrefixed(prefixed);
            string ns;
            if(!prefix2Namspace.TryGetValue(uriPrefixed.Prefix,out ns)) throw new Exception("prefix ");
            uriPrefixed.Namespace = ns;
            return uriPrefixed;
        }
    
    }