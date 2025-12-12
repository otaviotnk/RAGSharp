using RAGSharp.Core.Abstractions;
using RAGSharp.Core.Models;

namespace RAGSharp.Storage.InMemory;

public class InMemoryVectorStore : IVectorStore
{
    private readonly Dictionary<string, EmbeddingVector> _store = new();
    private readonly object _lock = new();

    public Task UpsertAsync(IEnumerable<EmbeddingVector> vectors, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            foreach (var v in vectors)
            {
                _store[v.Id] = v;
            }
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            foreach (var id in ids) _store.Remove(id);
        }
        return Task.CompletedTask;
    }

    public Task<EmbeddingVector?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        _store.TryGetValue(id, out var v);
        return Task.FromResult(v);
    }

    public Task<IReadOnlyList<RetrievalResult>> SearchAsync(float[] queryVector, int k = 5, CancellationToken cancellationToken = default)
    {
        var results = new List<RetrievalResult>();
        lock (_lock)
        {
            foreach (var kv in _store.Values)
            {
                cancellationToken.ThrowIfCancellationRequested();
                double score = CosineSimilarity(queryVector, kv.Vector);
                // payloadJson expected to contain chunk metadata; we will not parse here — pipeline will map ids -> chunk if needed
                // create a dummy Chunk with minimal fields (pipeline will keep real chunk in payload when upserting)
                var maybeChunk = new Chunk(kv.Id, "", 0, "", 0, 0);
                results.Add(new RetrievalResult(maybeChunk, score));
            }
        }
        var top = results.OrderByDescending(r => r.Score).Take(k).ToList();
        return Task.FromResult((IReadOnlyList<RetrievalResult>)top);
    }

    private static double CosineSimilarity(float[] a, float[] b)
    {
        if (a == null || b == null || a.Length != b.Length) return -1;
        double dot = 0, na = 0, nb = 0;
        for (int i = 0; i < a.Length; i++)
        {
            dot += a[i] * b[i];
            na += a[i] * a[i];
            nb += b[i] * b[i];
        }
        if (na == 0 || nb == 0) return -1;
        return dot / (Math.Sqrt(na) * Math.Sqrt(nb));
    }
}
