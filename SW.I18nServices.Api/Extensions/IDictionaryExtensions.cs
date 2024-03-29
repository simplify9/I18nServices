﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

namespace SW.I18nService
{
    public static class IDictionaryExtensions
    {
        public static async Task ToStreamAsync<TValue>(this IDictionary<string, TValue> dictionary, Stream destinationStream)
        {
            using (var memoryStream = new MemoryStream())
            using (var zips = new GZipStream(memoryStream, CompressionLevel.Optimal))
            using (var writer = new BinaryWriter(zips))
            {
                writer.Write(dictionary.Count);
                foreach (var kvp in dictionary)
                {
                    writer.Write(kvp.Key);
                    writer.Write(kvp.Value.ToString());
                }
                writer.Flush();
                memoryStream.Position = 0;
                await memoryStream.CopyToAsync(destinationStream);
            }

        }
    }
}
