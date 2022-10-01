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
        public static byte[] compressString(string input)
        {
            var strBytes = Encoding.UTF8.GetBytes(input);
            var result = new MemoryStream();
            var lengthBytes = BitConverter.GetBytes(input.Length);
            result.Write(lengthBytes, 0, 4);

            var compressionStream = new GZipStream(result, CompressionMode.Compress);
            compressionStream.Write(strBytes, 0, input.Length);
            compressionStream.Flush();
            return result.ToArray();
        }

        public static byte[] decompressBytes(byte[] input)
        {
            var source = new MemoryStream(input);
            byte[] lengthBytes = new byte[4];
            source.Read(lengthBytes, 0, 4);

            var length = BitConverter.ToInt32(lengthBytes, 0);
            var decompressionStream = new GZipStream(source, CompressionMode.Decompress);
            var result = new byte[length];
            decompressionStream.Read(result, 0, length);
            return result;
        }
    }
}
