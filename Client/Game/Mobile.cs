using Client.Game.Context;
using Client.Game.Data;
using Client.Networking.Incoming;
using static Client.Networking.Incoming.Mobile;
public sealed class Mobile : MobileContext
{
    public Mobile(Serial serial) : base(serial)
    {
    }

    static Mobile()
    {
    }
}
