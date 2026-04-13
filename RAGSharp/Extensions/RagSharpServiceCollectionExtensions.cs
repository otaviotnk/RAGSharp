using Microsoft.Extensions.DependencyInjection;
using RAGSharp.Core.Abstractions;
using RAGSharp.Core.Chunking;
using RAGSharp.Core.LLM;
using RAGSharp.Core.Pipelines;

namespace RAGSharp.Core.Extensions;

public static class RagSharpServiceCollectionExtensions
{
    public static IServiceCollection AddRagSharpCore(this IServiceCollection services)
    {
        services.AddSingleton<IChunker, SimpleChunker>();
        services.AddSingleton<ILLMClientFactory, LLMClientFactory>();
        services.AddSingleton<IRagPipeline, SimpleRagPipeline>();      
        return services;
    }
}
