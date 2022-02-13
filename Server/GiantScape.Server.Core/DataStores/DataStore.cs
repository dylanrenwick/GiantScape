using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;

namespace GiantScape.Server.DataStores
{
    internal partial class DataStore
    {
        private const string filepathFormat = "Resources/DataStores/{0}.dat";

        public void SaveToFile(string filename)
        {
            string filepath = string.Format(filepathFormat, filename);

            using (var fs = new FileStream(filepath, FileMode.OpenOrCreate))
            using (var writer = new BsonDataWriter(fs))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, this);
            }
        }

        public static DataStore FromFile(string filename)
        {
            DataStore dataStore = null;
            string filepath = string.Format(filepathFormat, filename);

            using (var fs = new FileStream(filepath, FileMode.OpenOrCreate))
            using (var reader = new BsonDataReader(fs))
            {
                var serializer = new JsonSerializer();

                dataStore = serializer.Deserialize<DataStore>(reader);
            }

            return dataStore;
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
