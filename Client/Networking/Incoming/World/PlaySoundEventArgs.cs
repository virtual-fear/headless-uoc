namespace Client.Networking.Incoming;
public sealed class PlaySoundEventArgs : EventArgs
{
    public NetState State { get; }
    public byte Flags { get; }
    public short SoundID { get; }
    public byte Volume { get; }
    public short X { get; }
    public short Y { get; }
    public short Z { get; }
    internal PlaySoundEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Flags = ip.ReadByte();
        SoundID = ip.ReadInt16();
        Volume = ip.ReadByte();
        X = ip.ReadInt16();
        Y = ip.ReadInt16();
        Z = ip.ReadInt16();
    }
}