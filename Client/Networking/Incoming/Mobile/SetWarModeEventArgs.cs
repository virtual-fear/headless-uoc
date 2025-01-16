namespace Client.Networking.Incoming;
public sealed class SetWarModeEventArgs : EventArgs
{
    public NetState State { get; }
    public bool Enabled { get; }
    internal SetWarModeEventArgs(NetState state, PacketReader pvSrc) {
        State = state;
        Enabled = pvSrc.ReadBoolean();
        pvSrc.ReadByte();   //  0x00
        pvSrc.ReadByte();   //  0x32
        pvSrc.ReadByte();   //  0x00
    }
}