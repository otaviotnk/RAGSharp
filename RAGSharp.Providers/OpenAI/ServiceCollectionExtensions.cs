using Microsoft.Extensions.DependencyInjection;
using RAGSharp.Core.Abstractions;
using System.Net.Http.Headers;

namespace RAGSharp.Providers.OpenAI
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOpenAI(
            this IServiceCollection services,
            Action<OpenAIOptions> configure)
        {
            var options = new OpenAIOptions();
            configure(options);

            services.AddSingleton(options);

            services.AddHttpClient<OpenAIEmbedder>(client =>
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", options.ApiKey);
            });

            services.AddHttpClient<OpenAILLMClient>(client =>
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", options.ApiKey);
            });

            services.AddSingleton<IEmbedder, OpenAIEmbedder>();
            services.AddSingleton<ILLMClient, OpenAILLMClient>();

            return services;
        }
    }
}
