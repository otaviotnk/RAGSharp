using System.Text.Json;
using RAGSharp.Core.Abstractions;
using RAGSharp.Core.Models;

namespace RAGSharp.Core.Pipelines;

public class SimpleRagPipeline : IRagPipeline
{
    private readonly IChunker _chunker;
    private readonly IEmbedder _embedder;
    private readonly IVectorStore _vectorStore;
    private readonly ILLMClient _llm;

    // local in-memory mapping id->Chunk to get chunk text (for InMemory vector store POC)
    private readonly Dictionary<string, Chunk> _chunkIndex = new();

    public SimpleRagPipeline(IChunker chunker, IEmbedder embedder, IVectorStore vectorStore, ILLMClient llm)
    {
        _chunker = chunker;
        _embedder = embedder;
        _vectorStore = vectorStore;
        _llm = llm;
    }

    public async Task IngestDocumentAsync(Document doc, CancellationToken cancellationToken = default)
    {
        var chunks = await _chunker.ChunkAsync(doc, cancellationToken);
        var items = chunks.Select(c => (id: c.Id, text: c.Text)).ToList();
        var embeddings = await _embedder.EmbedBatchAsync(items, cancellationToken);

        // add metadata payload: we'll include document id and small preview
        var withPayload = embeddings.Select(e =>
        {
            var chunkId = e.Id;
            var chunk = chunks.First(c => c.Id == chunkId);
            var payload = JsonSerializer.Serialize(new { chunk.DocumentId, chunk.Index, preview = chunk.Text.Length <= 200 ? chunk.Text : chunk.Text.Substring(0, 200) });
            return new EmbeddingVector(e.Id, e.Vector, payload);
        }).ToList();

        // store chunk text locally (for the in-memory demo)
        foreach (var c in chunks) _chunkIndex[c.Id] = c;

        await _vectorStore.UpsertAsync(withPayload, cancellationToken);
    }

    public async Task<QueryResult> QueryAsync(string userQuery, int topK = 5, CancellationToken cancellationToken = default)
    {
        // 1) embed query
        var qEmbedding = await _embedder.EmbedAsync($"query_{Guid.NewGuid()}", userQuery, cancellationToken);

        // 2) retrieve top-k
        var retrieved = await _vectorStore.SearchAsync(qEmbedding.Vector, topK, cancellationToken);

        // 3) map retrievals to chunks using local index or payload
        var resolved = new List<RetrievalResult>();
        foreach (var r in retrieved)
        {
            // try to get chunk from local index
            if (_chunkIndex.TryGetValue(r.Chunk.Id, out var chunk))
            {
                resolved.Add(new RetrievalResult(chunk, r.Score));
            }
            else
            {
                // fallback: create a minimal chunk with Id (payload parse could be added here)
                resolved.Add(new RetrievalResult(new Chunk(r.Chunk.Id, "", 0, $"[chunk text not available in pipeline demo]", 0, 0), r.Score));
            }
        }

        // 4) build prompt by concatenating top chunks
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("You are an assistant. Use the following context to answer the question. If unsure, say you don't know.");
        sb.AppendLine("--- Context ---");
        foreach (var rr in resolved)
        {
            sb.AppendLine($"[score:{rr.Score:F3}] {rr.Chunk.Text}");
            sb.AppendLine("----");
        }
        sb.AppendLine($"Question: {userQuery}");
        sb.AppendLine("--- End Context ---");

        var prompt = sb.ToString();
        var answer = await _llm.GenerateAsync(prompt, cancellationToken);

        return new QueryResult(answer, resolved, prompt);
    }
}
