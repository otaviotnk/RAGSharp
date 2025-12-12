namespace RAGSharp.Core.Models
{
    public record Chunk(
        string Id,
        string DocumentId,
        int Index,
        string Text,
        int StartChar,
        int EndChar
    );
}
