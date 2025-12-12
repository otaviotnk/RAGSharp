using RAGSharp.Core.Models;

namespace RAGSharp.Core.Abstractions
{
    public interface IEmbedder
    {
        /// <summary>Returns embedding vector for a single text.</summary>
        Task<EmbeddingVector> EmbedAsync(string id, string text, CancellationToken cancellationToken = default);

        /// <summary>Batch embed multiple texts (IDs must align with texts).</summary>
        Task<IReadOnlyList<EmbeddingVector>> EmbedBatchAsync(IReadOnlyList<(string id, string text)> items, CancellationToken cancellationToken = default);
    }
}
