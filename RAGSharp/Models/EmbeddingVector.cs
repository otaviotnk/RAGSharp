namespace RAGSharp.Core.Models
{
    public record EmbeddingVector(
    string Id,
    float[] Vector,
    string PayloadJson = "{}" // store metadata (e.g. chunk id, doc id) as JSON
    );
}
