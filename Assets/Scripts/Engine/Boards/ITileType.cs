namespace  Zongband.Engine.Boards
{
    public interface ITileType
    {
        bool BlocksGround { get; }
        bool BlocksAir { get; }
        object? Visuals { get; }
    }
}