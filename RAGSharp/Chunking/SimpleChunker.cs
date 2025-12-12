using RAGSharp.Core.Abstractions;
using RAGSharp.Core.Models;

namespace RAGSharp.Core.Chunking
{
    internal class SimpleChunker : IChunker
    {
        private readonly int _chunkSize;
        private readonly int _overlap;

        public SimpleChunker(int chunkSize = 1000, int overlap = 200)
        {
            _chunkSize = Math.Max(200, chunkSize);
            _overlap = Math.Clamp(overlap, 0, _chunkSize - 1);
        }

        public Task<IReadOnlyList<Chunk>> ChunkAsync(Document document, CancellationToken cancellationToken = default)
        {
            var text = document.Content ?? string.Empty;
            var chunks = new List<Chunk>();
            int index = 0;
            for (int start = 0; start < text.Length; start += (_chunkSize - _overlap))
            {
                cancellationToken.ThrowIfCancellationRequested();
                int end = Math.Min(start + _chunkSize, text.Length);
                var chunkText = text[start..end];
                var chunkId = $"{document.Id}_chunk_{index}";
                chunks.Add(new Chunk(chunkId, document.Id, index, chunkText, start, end));
                index++;
                if (end == text.Length) break;
            }
            return Task.FromResult((IReadOnlyList<Chunk>)chunks);
        }
    }
}