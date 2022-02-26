using System;

namespace GiantScape.Common.Game
{
    public abstract class GameEntity
    {
        public string ID { get; private set; }
        public string Name { get; private set; }
        public string MapID { get; private set; }

        public GameEntity(string mapID, string name = "")
        {
            Name = name;
            ID = Guid.NewGuid().ToString();
            MapID = mapID;
        }

        public override string ToString()
        {
            return $"{{{ID}}}";
        }
    }
}
