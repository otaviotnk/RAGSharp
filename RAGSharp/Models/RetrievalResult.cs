namespace RAGSharp.Core.Models
{
    public record RetrievalResult(
        Chunk Chunk,
        double Score // similarity / distance (higher = better) — document the chosen convention
    );
}
