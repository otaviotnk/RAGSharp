using Microsoft.AspNetCore.Mvc;
using RAGSharp.Core.Abstractions;
using RAGSharp.Core.Models;
using RAGSharp.WebAPI.Models;

namespace RAGSharp.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RagController : ControllerBase
    {
        private readonly IRagPipeline _pipeline;
        private readonly IVectorStore _vectorStore;
        private readonly IChunker _chunker;
        private readonly ILogger<RagController> _logger;

        public RagController(
            IRagPipeline pipeline,
            IVectorStore vectorStore,
            IChunker chunker,
            ILogger<RagController> logger)
        {
            _pipeline = pipeline;
            _vectorStore = vectorStore;
            _chunker = chunker;
            _logger = logger;
        }
       
        [HttpPost("index")]
        public async Task<IActionResult> IndexDocument([FromBody] IndexRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Content))
                return BadRequest(new { Error = "Content is required." });

            var doc = new Document(
                Id: string.IsNullOrWhiteSpace(request.Id) ? Guid.NewGuid().ToString("n") : request.Id,
                Title: request.Title ?? string.Empty,
                Source: request.Source,
                Content: request.Content,
                CreatedAt: DateTimeOffset.UtcNow
            );

            _logger.LogInformation("Indexing document {DocId} (title: {Title})", doc.Id, doc.Title);

            await _pipeline.IngestDocumentAsync(doc, cancellationToken);

            return Ok(new { Message = "Document ingested.", DocumentId = doc.Id });
        }
       
        [HttpPost("query")]
        public async Task<IActionResult> Query([FromBody] QueryRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Query))
                return BadRequest(new { Error = "Query is required." });

            _logger.LogInformation("Running query: {Query}", request.Query);

            var result = await _pipeline.QueryAsync(request.Query, request.TopK ?? 5, request.Provider, cancellationToken);

            return Ok(result);
        }

        [HttpPost("index/manual-chunks")]
        public async Task<IActionResult> IndexManualChunks([FromBody] ManualChunksRequest request, CancellationToken cancellationToken)
        {
            if (request.Chunks == null || request.Chunks.Count == 0)
                return BadRequest(new { Error = "Chunks are required." });
       
            var vectors = request.Chunks.Select((c, i) =>
                new EmbeddingVector(
                    Id: c.Id ?? Guid.NewGuid().ToString("n"),
                    Vector: Array.Empty<float>(),
                    PayloadJson: System.Text.Json.JsonSerializer.Serialize(new { c.DocumentId, c.Index, preview = c.Text?.Substring(0, Math.Min(80, c.Text?.Length ?? 0)) })
                )
            ).ToList();

            await _vectorStore.UpsertAsync(vectors, cancellationToken);

            return Ok(new { Message = "Manual chunks upserted.", Count = vectors.Count });
        }
    }
}