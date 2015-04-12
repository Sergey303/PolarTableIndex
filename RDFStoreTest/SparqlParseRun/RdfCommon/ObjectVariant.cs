public struct ObjectVariant
{
    private readonly int tag;
    private readonly object content;

    public ObjectVariant(int tag, object content)
    {
        this.tag = tag;
        this.content = content;
    }

    public object Content
    {
        get { return content; }
    }

    public int Tag
    {
        get { return tag; }
    }

    public object[] AsArray { get { return new[] {tag, content}; } }
}