using RAGSharp.Core.Abstractions;
using RAGSharp.Core.Models;
using System.Security.Cryptography;
using System.Text;

namespace RAGSharp.Providers.Mock
{
    public class MockEmbedder : IEmbedder
    {
        private readonly int _dimension;
        public MockEmbedder(int dimension = 128) => _dimension = dimension;

        public Task<EmbeddingVector> EmbedAsync(string id, string text, CancellationToken cancellationToken = default)
        {
            var vec = DeterministicVectorFromString(text, _dimension);
            return Task.FromResult(new EmbeddingVector(id, vec));
        }

        public Task<IReadOnlyList<EmbeddingVector>> EmbedBatchAsync(IReadOnlyList<(string id, string text)> items, CancellationToken cancellationToken = default)
        {
            var list = items.Select(i => new EmbeddingVector(i.id, DeterministicVectorFromString(i.text, _dimension))).ToList();
            return Task.FromResult((IReadOnlyList<EmbeddingVector>)list);
        }

        private static float[] DeterministicVectorFromString(string text, int dim)
        {
            // SHA256 -> use bytes to build floats in [-1,1]
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(text));
            var vec = new float[dim];
            for (int i = 0; i < dim; i++)
            {
                int bIdx = (i * 4) % hash.Length;
                uint val = BitConverter.ToUInt32(new byte[] { hash[bIdx], hash[(bIdx + 1) % hash.Length], hash[(bIdx + 2) % hash.Length], hash[(bIdx + 3) % hash.Length] }, 0);
                // normalize to [-1,1]
                vec[i] = (float)((val / (double)uint.MaxValue) * 2.0 - 1.0);
            }
            // optional: normalize to unit length
            var norm = Math.Sqrt(vec.Select(v => v * v).Sum());
            if (norm > 0)
            {
                for (int i = 0; i < vec.Length; i++) vec[i] /= (float)norm;
            }
            return vec;
        }
    }
}
