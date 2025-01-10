namespace Client.Networking.Incoming;
public sealed class WeatherEventArgs : EventArgs
{
    public NetState State { get; }
    public WeatherEventArgs(NetState state) => State = state;
    public byte[]? Buffer { get; set; }
    public byte V1 => Buffer?.Length >= 1 ? Buffer[0] : (byte)0;
    public byte V2 => Buffer?.Length >= 2 ? Buffer[1] : (byte)0;
    public byte V3 => Buffer?.Length >= 3 ? Buffer[2] : (byte)0;
}
public partial class World
{
    public static event PacketEventHandler<WeatherEventArgs>? WeatherUpdate;

    [PacketHandler(0x65, length: 4, ingame: true)]
    protected static void Received_Weather(NetState ns, PacketReader pvSrc)
    {
        WeatherEventArgs e = new(ns)
        {
            Buffer = pvSrc.ReadBytes(3)
        };
        WeatherUpdate?.Invoke(e);
    }
}