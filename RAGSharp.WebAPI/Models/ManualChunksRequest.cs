namespace RAGSharp.WebAPI.Models
{
    public class ManualChunksRequest
    {
        public List<ManualChunk> Chunks { get; set; } = new();
    }
}