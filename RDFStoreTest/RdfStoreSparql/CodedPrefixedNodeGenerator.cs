using System;
using SparqlParseRun.RdfCommon;


public class CodedPrefixedNodeGenerator : CodedNodeGenerator
{


    public new static CodedPrefixedNodeGenerator Create()
    {
        CodedPrefixedNodeGenerator nodeGenerator = new CodedPrefixedNodeGenerator();

        nodeGenerator.GenerateLiteralTypes();
        return nodeGenerator;
    }

    private ProloguePolar prologue = new ProloguePolar();




    public override IUriNode CreateUriNode(UriPrefixed uri)
    {
        return new CodedUriNode(this, CreateCode(prologue.CreatePrefixed(uri)));
    }

    public override IUriNode GetUri(string uri)
    {
        UriPrefixed up = Prologue.SplitUndefined(uri);

        int code = GetCode(prologue.GetPrefixedUriFromUndefined(uri));
        return code == -1 ? null : new CodedUriNode(this, code);
    }

    public override void Clear()
    {
        base.Clear();
        prologue.Clear();
    }

    public override void Close()
    {
        base.Clear();
        prologue.Close();
    }


    public override UriPrefixed GetUriRefixed(int code)
    {
        if (code < 0 || code >= nameTable.Root.Count()) throw new ArgumentOutOfRangeException();
        var prefixed = (string) nameTable.Root.Element(code).Field(1).Get();
        var uriPrefixed = Prologue.SplitPrefixed(prefixed);
        return uriPrefixed;
    }
}

    

