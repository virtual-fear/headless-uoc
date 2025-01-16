namespace Client.Networking.Incoming;
public sealed class HuePickerEventArgs : EventArgs
{
    public NetState State { get; }
    public int Serial { get;}
    public short ItemID { get; }
    internal HuePickerEventArgs(NetState state, PacketReader pvSrc)
    {
        State = state;
        Serial = pvSrc.ReadInt32();
        pvSrc.ReadInt16();
        ItemID = pvSrc.ReadInt16();
    }
}