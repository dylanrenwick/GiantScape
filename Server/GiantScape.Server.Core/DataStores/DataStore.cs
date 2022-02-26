using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using GiantScape.Common;
using GiantScape.Common.Game;

namespace GiantScape.Server.DataStores
{
    internal partial class DataStore
    {
        private const string filepathFormat = "Resources/DataStores/{0}.dat";

        public void SaveToFile(string filename)
        {
            string filepath = string.Format(filepathFormat, filename);

            using (var fs = new FileStream(filepath, FileMode.OpenOrCreate))
            {
                byte[] bson = Serializer.Serialize(this);
                fs.Write(bson, 0, bson.Length);
            }
        }

        public static DataStore FromFile(string filename)
        {
            DataStore dataStore;
            string filepath = string.Format(filepathFormat, filename);

            if (File.Exists(filepath))
            {
                byte[] fileContents = File.ReadAllBytes(filepath);
                dataStore = Serializer.Deserialize<DataStore>(fileContents);
            }
            else dataStore = Default();

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

        private static DataStore Default()
        {
            DataStore dataStore = new DataStore();

            dataStore.MapID = "13a3afd9-7408-45b1-97bc-87f13bff024d";
            dataStore.Position = new Vector2Int(8, 5);

            return dataStore;
        }
    }
}
