using System;

namespace GiantScape.Common.Game
{
    public abstract class GameEntity
    {
        public string ID { get; private set; }
        public string Name { get; private set; }

        public GameEntity(string name = "")
        {
            Name = name;
            ID = Guid.NewGuid().ToString();
        }
    }
}
