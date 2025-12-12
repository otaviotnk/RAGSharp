using Microsoft.Extensions.DependencyInjection;
using RAGSharp.Core.Abstractions;
using RAGSharp.Core.Chunking;
using RAGSharp.Core.Pipelines;
//using RAGSharp.Providers.Mock;
//using RAGSharp.Storage.InMemory;
namespace RAGSharp.Core;

public static class RagSharpServiceCollectionExtensions
{
    public static IServiceCollection AddRagSharpCore(this IServiceCollection services)
    {
        // default/test implementations - consumer can override
        services.AddSingleton<IChunker, SimpleChunker>();
        //services.AddSingleton<IEmbedder, MockEmbedder>();
        //services.AddSingleton<IVectorStore, InMemoryVectorStore>();
        //services.AddSingleton<ILLMClient, MockLLMClient>();
        services.AddSingleton<IRagPipeline, SimpleRagPipeline>();
        return services;
    }
}
