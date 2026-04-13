namespace RAGSharp.Core.Abstractions
{
    public interface ILLMClientFactory
    {
        /// <summary>
        /// Returns the LLM client for the given provider key (e.g. "openai", "claude").
        /// When <paramref name="provider"/> is null, returns the default registered client.
        /// </summary>
        ILLMClient GetClient(string? provider = null);
    }
}
