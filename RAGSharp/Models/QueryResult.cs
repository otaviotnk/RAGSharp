namespace RAGSharp.Core.Models
{
    public record QueryResult(
        string Answer,
        IReadOnlyList<RetrievalResult> Retrieved,
        string? PromptUsed = null
    );
}
