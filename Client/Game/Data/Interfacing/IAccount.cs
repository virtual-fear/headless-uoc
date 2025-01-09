namespace Client.Game.Data;
using Client.Game.Context;
public interface IAccount
{
    string Username { get; }
    string Password { get; set; }
    MobileContext this[int index] { get; set; }
    void Delete();
}
