namespace Client.Game.Data;
public interface IEntity
{
    int Serial { get; }
    short X { get; }
    short Y { get; }
    sbyte Z { get; }
}