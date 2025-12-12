namespace RAGSharp.Core.Abstractions
{
    internal interface ILLMClient
    {
        /// <summary>Run a completion for a given prompt and return text.</summary>
        Task<string> GenerateAsync(string prompt, CancellationToken cancellationToken = default);

        /// <summary>Optional: chat-style method with system/user messages.</summary>
        Task<string> ChatAsync(IEnumerable<(string role, string content)> messages, CancellationToken cancellationToken = default);
    }
}
}
