using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using GiantScape.Common.Game;
using GiantScape.Common.Game.Tilemaps;

namespace GiantScape.Client
{
    public class WorldController : MonoBehaviour
    {
        private World world;

        public void RegisterTileset(TilesetData tileset) => world.RegisterTileset(tileset.TilesetName, tileset);

        private void Start()
        {
            world = new World(UnityLogger.Instance.SubLogger("WORLD");
        }
    }
}
