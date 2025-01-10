namespace Client.Networking.Incoming;
public sealed class FightingEventArgs : EventArgs
{
    public NetState State { get; }
    public FightingEventArgs(NetState state) => State = state;
    public byte Flag { get; set; }
    public int Attacker { get; set; }
    public int Defender { get; set; }
}

public partial class Player
{
    public static event PacketEventHandler<FightingEventArgs>? OnFighting;

    [PacketHandler(0x2F, length: 10, ingame: true)]
    protected static void Receive_Fighting(NetState ns, PacketReader pvSrc)
    {
        FightingEventArgs e = new FightingEventArgs(ns);
        e.Flag = pvSrc.ReadByte();
        e.Attacker = pvSrc.ReadInt32();
        e.Defender = pvSrc.ReadInt32();
        OnFighting?.Invoke(e);
    } // RunUO: Swing
}