namespace Client.Networking.Incoming;
public sealed class DeathStatusEventArgs : EventArgs
{
    public NetState State { get; }
    public DeathStatusEventArgs(NetState state) => State = state;
    public bool Dead { get; set; }
}
public partial class Player
{
    public static event PacketEventHandler<DeathStatusEventArgs>? OnDeathStatus;

    [PacketHandler(0x2C, length: 2, ingame: true)]
    protected static void Receive_DeathStatus(NetState ns, PacketReader pvSrc)
    {
        DeathStatusEventArgs e = new DeathStatusEventArgs(ns);
        e.Dead = (pvSrc.ReadByte() == 2);
        OnDeathStatus?.Invoke(e);
    }
}