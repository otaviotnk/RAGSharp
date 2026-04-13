using Microsoft.Extensions.DependencyInjection;
using RAGSharp.Core.Abstractions;

namespace RAGSharp.Core.LLM
{
    internal sealed class LLMClientFactory : ILLMClientFactory
    {
        private readonly IServiceProvider _services;

        public LLMClientFactory(IServiceProvider services) => _services = services;

        public ILLMClient GetClient(string? provider = null)
        {
            if (provider is not null)
            {
                return _services.GetKeyedService<ILLMClient>(provider.ToLowerInvariant())
                    ?? throw new InvalidOperationException(
                        $"LLM provider '{provider}' is not registered. " +
                        $"Call AddOpenAI() or AddClaude() in Program.cs.");
            }

            return _services.GetRequiredService<ILLMClient>();
        }
    }
}
