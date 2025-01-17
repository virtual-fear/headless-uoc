namespace Client.Networking.Arguments;
public sealed class GlobalLightEventArgs : EventArgs
{
    public NetState State { get; }
    public sbyte Level { get; }
    internal GlobalLightEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Level = ip.ReadSByte();
    }
}