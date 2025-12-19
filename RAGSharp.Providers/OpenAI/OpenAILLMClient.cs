using RAGSharp.Core.Abstractions;
using System.Net.Http.Json;
using System.Text.Json;

namespace RAGSharp.Providers.OpenAI
{
    public sealed class OpenAILLMClient : ILLMClient
    {
        private readonly HttpClient _http;
        private readonly OpenAIOptions _options;

        public OpenAILLMClient(HttpClient http, OpenAIOptions options)
        {
            _http = http;
            _options = options;
        }

        public Task<string> ChatAsync(IEnumerable<(string role, string content)> messages, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GenerateAsync(string prompt, CancellationToken ct = default)
        {
            var request = new
            {
                model = _options.ChatModel,
                messages = new[]
                {
                new { role = "user", content = prompt }
            }
            };

            using var response = await _http.PostAsJsonAsync(
                "https://api.openai.com/v1/chat/completions",
                request,
                ct
            );

            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync(ct);
            using var json = await JsonDocument.ParseAsync(stream, cancellationToken: ct);

            return json
                .RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString()!;
        }
    }
}
