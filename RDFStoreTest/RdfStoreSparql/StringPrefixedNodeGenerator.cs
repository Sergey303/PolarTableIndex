using SparqlParseRun.RdfCommon;


public class StringPrefixedNodeGenerator : StringNodeGenerator
    {
       ProloguePolar prologue=new ProloguePolar();

    public override IUriNode CreateUriNode(UriPrefixed uri)
        {
            return new UriNode(prologue.CreatePrefixed(uri)); 
        }

    public override IUriNode GetUri(string uri)
        {                                                               
            return new UriNode(prologue.GetPrefixedUriFromUndefined(uri));
        }


    public override void Clear()
    {
        base.Clear();
        prologue.Clear();
    }

    public override void Close()
        {
            base.Close();
            prologue.Close();
        }                  
    }

    

