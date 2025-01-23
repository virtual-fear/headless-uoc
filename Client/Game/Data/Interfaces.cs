namespace Client.Game.Data;
public interface IPoint2D
{
    short X { get; }
    short Y { get; }
}
public interface IPoint3D : IPoint2D
{
    sbyte Z { get; }
}
public interface IEntity
{
    Serial Serial { get; }
    IPoint3D Location { get; }
}
public interface IAccount
{
    string Username { get; }
    string Password { get; set; }
    Mobile this[int index] { get; set; }
    void Delete();
}
