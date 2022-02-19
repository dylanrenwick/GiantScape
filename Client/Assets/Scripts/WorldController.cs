using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using GiantScape.Common.Game;

namespace GiantScape.Client
{
    public class WorldController : MonoBehaviour
    {
        private World world;

        private void Start()
        {
            world = new World(UnityLogger.Instance.SubLogger("WORLD");
        }
    }
}
