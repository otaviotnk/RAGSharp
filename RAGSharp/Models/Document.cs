namespace RAGSharp.Core.Models
{
    public record Document(
        string Id,
        string Title,
        string? Source,
        string Content,
        DateTimeOffset CreatedAt
    );
}
