namespace Client.Game.Data;
using Client.Game.Context;

public interface IPoint2D
{
    short X { get; }
    short Y { get; }
}
public interface IPoint3D : IPoint2D
{
    sbyte Z { get; }
}
public interface IEntity : IPoint3D
{
    Serial Serial { get; }
}
public interface IAccount
{
    string Username { get; }
    string Password { get; set; }
    MobileContext this[int index] { get; set; }
    void Delete();
}
