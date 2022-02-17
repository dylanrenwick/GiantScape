﻿using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace GiantScape.Common
{
    public static class Serializer
    {
        public static byte[] Serialize(object target)
        {
            var mem = new MemoryStream();
            using (var writer = new BsonWriter(mem))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, target);
            }

            return mem.ToArray();
        }

        public static T Deserialize<T>(byte[] bson)
        {
            var mem = new MemoryStream(bson);
            T result;
            using (var reader = new BsonReader(mem))
            {
                var serializer = new JsonSerializer();
                result = serializer.Deserialize<T>(reader);
            }

            return result;
        }
    }
}
