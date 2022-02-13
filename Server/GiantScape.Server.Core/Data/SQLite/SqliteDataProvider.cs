using System;

using Microsoft.Data.Sqlite;

using GiantScape.Common.Game;
using GiantScape.Common.Game.Tilemaps;

namespace GiantScape.Server.Data.SQLite
{
    internal class SqliteDataProvider : IDataProvider
    {
        private readonly SqliteConnection connection;

        public SqliteDataProvider(string connectionString = "Data Source=:memory:")
        {
            connection = new SqliteConnection(connectionString);
            connection.Open();
        }

        public TilemapData GetMap()
        {
            throw new NotImplementedException();
        }

        public void GetPlayerAsync(string username, Action<PlayerEntity> callback)
        {
            throw new NotImplementedException();
        }
    }
}
