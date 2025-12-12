using RAGSharp.Core.Models;

namespace RAGSharp.Core.Abstractions
{
    public interface IVectorStore
    {
        Task UpsertAsync(IEnumerable<EmbeddingVector> vectors, CancellationToken cancellationToken = default);
        Task DeleteAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default);
        /// <summary>Search returns top-k similar vectors. Score convention: higher = more similar.</summary>
        Task<IReadOnlyList<RetrievalResult>> SearchAsync(float[] queryVector, int k = 5, CancellationToken cancellationToken = default);

        /// <summary>Optional: load vector by id to retrieve metadata/payload.</summary>
        Task<EmbeddingVector?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    }
}
