namespace Client.Networking.Incoming;
public sealed class SeasonChangeEventArgs : EventArgs
{
    public NetState State { get; }
    public SeasonChangeEventArgs(NetState state) => State = state;
    public byte Value { get; set; }
    public bool Sound { get; set; }
}

public partial class World
{
    public static event PacketEventHandler<SeasonChangeEventArgs>? SeasonUpdate;

    [PacketHandler(0xBC, length: 3, ingame: true)]
    public static void Received_Season(NetState ns, PacketReader pvSrc)
    {
        SeasonChangeEventArgs e = new(ns)
        {
            Value = pvSrc.ReadByte(),
            Sound = pvSrc.ReadBoolean()
        };
        SeasonUpdate?.Invoke(e);
    }
}