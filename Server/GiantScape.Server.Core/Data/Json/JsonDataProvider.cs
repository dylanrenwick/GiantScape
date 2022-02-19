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
        public IEnumerable<UserModel> Users { get; private set; }
        public IEnumerable<PlayerModel> Players { get; private set; }
        public IEnumerable<MapModel> Maps { get; private set; }
        public IEnumerable<TilesetModel> Tilesets { get; private set; }

        public JsonDataProvider(string jsonFilename)
        {
            string json = File.ReadAllText(jsonFilename);
            var jsonObj = JObject.Parse(json);

            Users = ParseCollection<UserModel>(jsonObj["Users"]);
            Players = ParseCollection<PlayerModel>(jsonObj["Players"]);
            Maps = ParseCollection<MapModel>(jsonObj["Maps"]);
            Tilesets = ParseCollection<TilesetModel>(jsonObj["Tilesets"]);
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
