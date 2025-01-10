namespace Client.Networking.Incoming;
public sealed class SetWarModeEventArgs : EventArgs
{
    public NetState State { get; }
    public SetWarModeEventArgs(NetState state) => State = state;
    public bool Enabled { get; set; }
}
public partial class Mobile
{
    public static event PacketEventHandler<SetWarModeEventArgs>? OnWarmode;

    [PacketHandler(0x72, length: 5, ingame: true)] // NOTE: Maybe this should be elsewhere?
    protected static void Receive_SetWarmode(NetState ns, PacketReader pvSrc)
    {
        SetWarModeEventArgs e = new(ns);
        e.Enabled = pvSrc.ReadBoolean();
        pvSrc.ReadByte();   //  0x00
        pvSrc.ReadByte();   //  0x32
        pvSrc.ReadByte();   //  0x00
        OnWarmode?.Invoke(e);
    }
}