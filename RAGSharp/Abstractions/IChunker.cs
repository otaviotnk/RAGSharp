using RAGSharp.Core.Models;

namespace RAGSharp.Core.Abstractions
{
    public interface IChunker
    {
        /// <summary>Splits a document into chunks ready to embed and store.</summary>
        Task<IReadOnlyList<Chunk>> ChunkAsync(Document document, CancellationToken cancellationToken = default);
    }
}
