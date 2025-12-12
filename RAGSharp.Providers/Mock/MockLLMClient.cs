using RAGSharp.Core.Abstractions;
namespace RAGSharp.Providers.Mock;

public class MockLLMClient : ILLMClient
{
    public Task<string> GenerateAsync(string prompt, CancellationToken cancellationToken = default)
    {
        // Simple deterministic reply for POC
        var preview = prompt.Length > 500 ? prompt.Substring(0, 500) + "..." : prompt;
        return Task.FromResult($"[MOCK LLM RESPONSE] Based on provided context (preview): {preview}");
    }

    public Task<string> ChatAsync(IEnumerable<(string role, string content)> messages, CancellationToken cancellationToken = default)
    {
        var joined = string.Join(" | ", messages.Select(m => $"{m.role}:{m.content}"));
        return Task.FromResult($"[MOCK CHAT] {joined}");
    }
}
