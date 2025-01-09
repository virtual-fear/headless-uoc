using Client.Game.Context;

namespace Client.Game;
public sealed class Mobile : MobileContext
{
    public Mobile(int serial) : base(serial) { }
}
