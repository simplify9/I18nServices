using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

namespace SW.I18nService
{
    public static class StreamExtensions
    {

        async public static Task<IReadOnlyDictionary<string, object>> AsDictionaryAsync(this Stream sourceStream, Type valueType)
        {
            using (var memoryStream = new MemoryStream())
            {
                await sourceStream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                return memoryStream.AsDictionary(valueType);
            }

        }

        async public static Task<IDictionary<string, TValue>> AsDictionaryAsync<TValue>(this Stream sourceStream)
        {
            using (var memoryStream = new MemoryStream())
            {
                await sourceStream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                return memoryStream.AsDictionary<TValue>();
            }

        }

        public static IReadOnlyDictionary<string, object> AsDictionary(this Stream sourceStream, Type valueType)
        {

            using (var zips = new GZipStream(sourceStream, CompressionMode.Decompress))
            using (var reader = new BinaryReader(zips))
            {
                int count = reader.ReadInt32();

                var dictionary = new Dictionary<string, object>(count, StringComparer.OrdinalIgnoreCase);
                for (int n = 0; n < count; n++)
                {
                    var key = reader.ReadString();
                    var vals = reader.ReadString();

                    var val = Activator.CreateInstance(valueType, vals);

                    dictionary.Add(key, val);
                }
                return dictionary;
            }

        }

        public static IDictionary<string, TValue> AsDictionary<TValue>(this Stream sourceStream)
        {

            using (var zips = new GZipStream(sourceStream, CompressionMode.Decompress))
            using (var reader = new BinaryReader(zips))
            {
                int count = reader.ReadInt32();
                var dictionary = new Dictionary<string, TValue>(count, StringComparer.OrdinalIgnoreCase);
                var valueType = typeof(TValue);
                for (int n = 0; n < count; n++)
                {
                    var key = reader.ReadString();
                    var vals = reader.ReadString();

                    var val = (TValue)Activator.CreateInstance(valueType, vals);

                    dictionary.Add(key, val);
                }
                return dictionary;
            }

        }

        async public static Task<ICollection<TValue>> AsCollectionAsync<TValue>(this Stream sourceStream)
        {
            using (var memoryStream = new MemoryStream())
            {
                await sourceStream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                return memoryStream.AsCollection<TValue>();
            }

        }

        public static ICollection<TValue> AsCollection<TValue>(this Stream sourceStream)
        {

            using (var zips = new GZipStream(sourceStream, CompressionMode.Decompress))
            using (var reader = new BinaryReader(zips))
            {
                int count = reader.ReadInt32();
                var list = new List<TValue>(count);
                for (int n = 0; n < count; n++)
                {
                    var key = reader.ReadString();
                    var vals = reader.ReadString();

                    var val = (TValue)Activator.CreateInstance(typeof(TValue), vals);

                    list.Add(val);
                }
                return list;
            }

        }
    }
}
