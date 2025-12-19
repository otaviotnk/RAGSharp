using RAGSharp.Core.Abstractions;
using RAGSharp.Core.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace RAGSharp.Providers.OpenAI
{
    public sealed class OpenAIEmbedder : IEmbedder
    {
        private readonly HttpClient _http;
        private readonly OpenAIOptions _options;

        public OpenAIEmbedder(HttpClient http, OpenAIOptions options)
        {
            _http = http;
            _options = options;
        }

        public async Task<EmbeddingVector> EmbedAsync(string id, string text, CancellationToken ct = default)
        {
            var request = new
            {
                model = _options.EmbeddingModel,
                input = text
            };

            using var response = await _http.PostAsJsonAsync(
                "https://api.openai.com/v1/embeddings",
                request,
                ct
            );

            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync(ct);
            using var json = await JsonDocument.ParseAsync(stream, cancellationToken: ct);

            var embedding = json
                .RootElement
                .GetProperty("data")[0]
                .GetProperty("embedding");

            var vector = embedding.EnumerateArray().Select(x => x.GetSingle()).ToArray();

            return new EmbeddingVector(id, vector, JsonSerializer.Serialize(new { text }));
        }

        public Task<IReadOnlyList<EmbeddingVector>> EmbedBatchAsync(IReadOnlyList<(string id, string text)> items, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
