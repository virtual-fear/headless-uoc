namespace Client.Networking.Arguments;
public sealed class ScrollMessageEventArgs : EventArgs
{
    public NetState State { get; }
    public byte Type { get; }
    public int Tip { get; }
    public string? Text { get; }
    internal ScrollMessageEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Type = ip.ReadByte();
        Tip = ip.ReadInt32();
        Text = ip.ReadString(ip.ReadUInt16());
    }
}