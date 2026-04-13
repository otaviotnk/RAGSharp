using System.Text.Json.Serialization;

namespace RAGSharp.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum LlmProvider
    {
        OpenAI,
        Claude
    }
}
