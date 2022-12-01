using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexCellInfoWF.Extensions
{
    static class ChunkExtension
    {
        public static IEnumerable<List<T>> ToChunks<T>(this IEnumerable<T> items, int chunkSize)
        {
            List<T> chunk = new List<T>(chunkSize);
            foreach (var item in items)
            {
                chunk.Add(item);
                if (chunk.Count == chunkSize)
                {
                    yield return chunk;
                    chunk = new List<T>(chunkSize);
                }
            }
            if (chunk.Any())
                yield return chunk;
        }
    }
}
