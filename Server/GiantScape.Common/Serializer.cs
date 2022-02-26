using System.IO;
using System.Text.RegularExpressions;

using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace GiantScape.Common
{
    public static class Serializer
    {
        private static Regex commentRegex = new Regex("\\/\\*.*?\\*\\/");

        public static byte[] Serialize(object target)
        {
            var mem = new MemoryStream();
            using (var writer = new BsonDataWriter(mem))
            {
                var serializer = JsonSerializer.CreateDefault(new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
                serializer.Serialize(writer, target);
            }

            return mem.ToArray();
        }

        public static T Deserialize<T>(byte[] bson)
        {
            var mem = new MemoryStream(bson);
            T result;
            using (var reader = new BsonDataReader(mem))
            {
                var serializer = new JsonSerializer();
                result = serializer.Deserialize<T>(reader);
            }

            return result;
        }

        public static T Deserialize<T>(string json)
        {
            json = StripComments(json);
            return JsonConvert.DeserializeObject<T>(json);
        }

        private static string StripComments(string json)
        {
            return commentRegex.Replace(json, string.Empty);
        }
    }
}
