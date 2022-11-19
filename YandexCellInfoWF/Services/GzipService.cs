using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexCellInfoWF.Services
{
    public static class GzipService
    {
        public static async Task<byte[]> CompressString(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var ms = new MemoryStream();
            using (GZipStream zipStream = new GZipStream(ms, CompressionMode.Compress))
            {
                await zipStream.WriteAsync(bytes, 0, bytes.Length);
                await zipStream.FlushAsync(); //Doesn't seem like Close() is available in UWP, so I changed it to Flush(). Is this the problem?
            }

            // we create the data array here once the GZIP stream has been disposed
            var data = ms.ToArray();
            ms.Dispose();
            return data;
        }

        public static async Task<byte[]> DecompressBytes(byte[] input)
        {
            var ms = new MemoryStream();
            using (GZipStream zipStream = new GZipStream(ms, CompressionMode.Compress))
            {
                await zipStream.WriteAsync(input, 0, input.Length);
                await zipStream.FlushAsync(); //Doesn't seem like Close() is available in UWP, so I changed it to Flush(). Is this the problem?
            }

            // we create the data array here once the GZIP stream has been disposed
            var data = ms.ToArray();
            ms.Dispose();
            return data;
        }
    }
}
