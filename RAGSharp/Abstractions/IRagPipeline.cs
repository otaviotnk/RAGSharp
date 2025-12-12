using RAGSharp.Core.Models;

namespace RAGSharp.Core.Abstractions
{
    public interface IRagPipeline
    {
        /// <summary>Ingest a document: chunk -> embed -> store</summary>
        Task IngestDocumentAsync(Document doc, CancellationToken cancellationToken = default);

        /// <summary>Query: embed query, retrieve top-k, and call LLM to generate an answer.</summary>
        Task<QueryResult> QueryAsync(string userQuery, int topK = 5, CancellationToken cancellationToken = default);
    }
}
