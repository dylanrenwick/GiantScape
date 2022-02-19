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
        public DbCollection<UserModel> Users { get; private set; }
        public DbCollection<PlayerModel> Players { get; private set; }
        public DbCollection<MapModel> Maps { get; private set; }
        public DbCollection<TilesetModel> Tilesets { get; private set; }

        public JsonDataProvider(string jsonFilename)
        {
            string json = File.ReadAllText(jsonFilename);
            var jsonObj = JObject.Parse(json);

            Users = ParseCollection<UserModel>(jsonObj["Users"]);
            Players = ParseCollection<PlayerModel>(jsonObj["Players"]);
            Maps = ParseCollection<MapModel>(jsonObj["Maps"]);
            Tilesets = ParseCollection<TilesetModel>(jsonObj["Tilesets"]);
        }

        private DbCollection<T> ParseCollection<T>(JToken json) where T : BaseModel
        {
            if (json is JArray jsonArray)
            {
                return new DbCollection<T>(jsonArray.Select(jToken => jToken.ToObject<T>()));
            }

            throw new ArgumentException("Could not parse collection from token that is not array");
        }
    }
}
