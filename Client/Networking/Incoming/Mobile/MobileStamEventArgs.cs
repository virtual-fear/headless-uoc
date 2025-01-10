namespace Client.Networking.Incoming;
using Client.Game.Data;
public sealed class MobileStamEventArgs : EventArgs
{
    public NetState State { get; }
    public MobileStamEventArgs(NetState state) => State = state;
    public Serial Serial { get; set; }
    public short StamMax { get; set; }
    public short Stam { get; set; }
}
public partial class Mobile
{
    public static event PacketEventHandler<MobileStamEventArgs>? OnChangedStamina;

    [PacketHandler(0xA3, length: 9, ingame: true)]
    protected static void Received_MobileStam(NetState ns, PacketReader pvSrc)
    {
        MobileStamEventArgs e = new(ns);
        e.Serial = (Serial)pvSrc.ReadUInt16();
        e.StamMax = pvSrc.ReadInt16();
        e.Stam = pvSrc.ReadInt16();
        OnChangedStamina?.Invoke(e);
    }
}
