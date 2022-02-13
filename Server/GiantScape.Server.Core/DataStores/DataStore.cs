using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;

namespace GiantScape.Server.DataStores
{
    internal partial class DataStore
    {
        public void SaveToFile(string filepath)
        {
            using (var fs = new FileStream(filepath, FileMode.OpenOrCreate))
            using (var writer = new BsonDataWriter(fs))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, this);
            }
        }

        public static DataStore FromJson(string json)
        {
            return JsonConvert.DeserializeObject<DataStore>(json);
        }
        public static DataStore FromJson(JToken json)
        {
            return json.ToObject<DataStore>();
        }
    }
}
