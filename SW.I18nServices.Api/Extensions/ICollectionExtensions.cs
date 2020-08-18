using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

namespace SW.I18n
{
    public static class ICollectionExtensions
    {


        async public static Task ToStreamAsync<TVlaue>(this ICollection<TVlaue> collection, Stream destinationStream)
        {
            using (var memoryStream = new MemoryStream())
            using (var zips = new GZipStream(memoryStream, CompressionLevel.Optimal))
            using (var writer = new BinaryWriter(zips))
            {
                writer.Write(collection.Count);
                foreach (var item in collection)
                {
                    writer.Write(item.ToString());
                }
                writer.Flush();
                memoryStream.Position = 0;
                await memoryStream.CopyToAsync(destinationStream);
            }

        }

    }
}
