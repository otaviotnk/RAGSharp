using RAGSharp.Core.Abstractions;
using System.Net.Http.Json;
using System.Text.Json;

namespace RAGSharp.Providers.Claude
{
    public sealed class ClaudeLLMClient : ILLMClient
    {
        private const string ApiUrl = "https://api.anthropic.com/v1/messages";
        private const string AnthropicVersion = "2023-06-01";

        private readonly HttpClient _http;
        private readonly ClaudeOptions _options;

        public ClaudeLLMClient(HttpClient http, ClaudeOptions options)
        {
            _http = http;
            _options = options;
        }

        public Task<string> GenerateAsync(string prompt, CancellationToken cancellationToken = default)
        {
            var messages = new[] { (role: "user", content: prompt) };
            return ChatAsync(messages, cancellationToken);
        }

        public async Task<string> ChatAsync(
            IEnumerable<(string role, string content)> messages,
            CancellationToken cancellationToken = default)
        {
            var messageList = messages.ToList();

            // Anthropic uses a top-level "system" field instead of a system message role
            var systemMessage = messageList
                .Where(m => m.role == "system")
                .Select(m => m.content)
                .FirstOrDefault();

            var chatMessages = messageList
                .Where(m => m.role != "system")
                .Select(m => new { role = m.role, content = m.content })
                .ToArray();

            object requestBody = systemMessage is not null
                ? new
                {
                    model = _options.ChatModel,
                    max_tokens = _options.MaxTokens,
                    system = systemMessage,
                    messages = chatMessages
                }
                : new
                {
                    model = _options.ChatModel,
                    max_tokens = _options.MaxTokens,
                    messages = chatMessages
                };

            using var response = await _http.PostAsJsonAsync(ApiUrl, requestBody, cancellationToken);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var json = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);

            return json
                .RootElement
                .GetProperty("content")[0]
                .GetProperty("text")
                .GetString()!;
        }
    }
}
