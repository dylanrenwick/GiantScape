using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json.Linq;

using GiantScape.Server.Data.Models;

namespace GiantScape.Server.Data.Json
{
    internal class JsonDataProvider : IDataProvider
    {
        public IEnumerable<User> Users { get; private set; }
        public IEnumerable<Player> Players { get; private set; }
        public IEnumerable<Map> Maps { get; private set; }
        public IEnumerable<Tileset> Tilesets { get; private set; }

        public JsonDataProvider(string jsonFilename)
        {
            string json = File.ReadAllText(jsonFilename);
            var jsonObj = JObject.Parse(json);

            Users = ParseCollection<User>(jsonObj["Users"]);
            Players = ParseCollection<Player>(jsonObj["Players"]);
            Maps = ParseCollection<Map>(jsonObj["Maps"]);
            Tilesets = ParseCollection<Tileset>(jsonObj["Tilesets"]);
        }

        private IEnumerable<T> ParseCollection<T>(JToken json)
        {
            if (json is JArray jsonArray)
            {
                return jsonArray.Select(jToken => jToken.ToObject<T>());
            }

            throw new ArgumentException("Could not parse collection from token that is not array");
        }
    }
}
