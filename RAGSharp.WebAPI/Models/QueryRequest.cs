using RAGSharp.Core.Enums;

namespace RAGSharp.WebAPI.Models
{
    public class QueryRequest
    {
        public string Query { get; set; } = "";
        public int? TopK { get; set; }

        /// <summary>
        /// Optional LLM provider to use for this request.
        /// When null, uses the default provider registered in Program.cs.
        /// </summary>
        public LlmProvider? Provider { get; set; }
    }
}
