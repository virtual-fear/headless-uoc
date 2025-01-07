namespace Client.Networking.Incoming;
using static PacketSink;
public partial class PacketSink
{
    public sealed class SeasonChangeEventArgs : EventArgs
    {
        public NetState State { get; }
        public SeasonChangeEventArgs(NetState state) => State = state;
        public byte Value { get; set; }
        public bool Sound { get; set; }
    }

    public static event PacketEventHandler<SeasonChangeEventArgs>? SeasonChange;
    public static void InvokeSeasonChange(SeasonChangeEventArgs e) => SeasonChange?.Invoke(e);
}
public static class UpdatedSeasons
{
    public static void Configure()
    {
        PacketHandlers.Register(0xBC, 03, true, new OnPacketReceive(Update));
    }

    private static void Update(NetState ns, PacketReader pvSrc)
    {
        SeasonChangeEventArgs e = new SeasonChangeEventArgs(ns);

        e.Value = pvSrc.ReadByte();
        e.Sound = pvSrc.ReadBoolean();

        PacketSink.InvokeSeasonChange(e);
    }
}
