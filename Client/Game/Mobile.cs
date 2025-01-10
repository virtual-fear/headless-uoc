using Client.Game.Context;
using Client.Game.Data;
public sealed class Mobile : MobileContext
{
    public Mobile(Serial serial) : base(serial) { }
}
