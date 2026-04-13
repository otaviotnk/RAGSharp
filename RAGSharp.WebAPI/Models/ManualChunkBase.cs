namespace RAGSharp.WebAPI.Models
{
    public class ManualChunkBase
    {
        public string DocumentId { get; set; } = Guid.NewGuid().ToString("n");
        public string? Id { get; set; }
        public int Index { get; set; }
        public string? Text { get; set; }
    }
}