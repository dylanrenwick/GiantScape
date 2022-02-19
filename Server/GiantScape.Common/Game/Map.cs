using GiantScape.Common.Game.Tilemaps;

namespace GiantScape.Common.Game
{
    public class Map
    {
        public string Name => Tilemap.Name;

        public TilemapData Tilemap;
        public TilesetData Tileset;

        public Map(TilemapData tilemap, TilesetData tileset)
        {
            this.Tilemap = tilemap;
            this.Tileset = tileset;
        }
    }
}
