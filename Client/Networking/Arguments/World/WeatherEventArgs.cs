namespace Client.Networking.Arguments;
public sealed class WeatherEventArgs : EventArgs
{
    public NetState State { get; }
    public byte[]? Buffer { get; }
    internal WeatherEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Buffer = ip.ReadBytes(3);
    }
    public byte V1 => Buffer?.Length >= 1 ? Buffer[0] : (byte)0;
    public byte V2 => Buffer?.Length >= 2 ? Buffer[1] : (byte)0;
    public byte V3 => Buffer?.Length >= 3 ? Buffer[2] : (byte)0;
}