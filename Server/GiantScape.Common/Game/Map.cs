using GiantScape.Common.Game.Tilemaps;

namespace GiantScape.Common.Game
{
    public class Map
    {
        public string Name => tilemap.Name;

        public TilemapData tilemap;
        public TilesetData tileset;

        public Map(TilemapData tilemap, TilesetData tileset)
        {
            this.tilemap = tilemap;
            this.tileset = tileset;
        }
    }
}
