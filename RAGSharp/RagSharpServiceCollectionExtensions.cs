using Microsoft.Extensions.DependencyInjection;
using RAGSharp.Core.Abstractions;
using RAGSharp.Core.Chunking;
using RAGSharp.Core.Pipelines;

namespace RAGSharp.Core;

public static class RagSharpServiceCollectionExtensions
{
    public static IServiceCollection AddRagSharpCore(this IServiceCollection services)
    {
        services.AddSingleton<IChunker, SimpleChunker>();
        services.AddSingleton<IRagPipeline, SimpleRagPipeline>();      
        return services;
    }
}
