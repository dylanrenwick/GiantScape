namespace GiantScape.Common.Game
{
    public class PlayerEntity : GameEntity
    {
        public PlayerEntity(string mapID, string name = "")
            : base(mapID, name)
        { }

        public Vector2Int Position { get; set; }
    }
}
