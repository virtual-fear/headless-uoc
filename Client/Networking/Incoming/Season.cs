namespace Client.Networking.Incoming;
public partial class PacketHandlers
{
    public static event PacketEventHandler<SeasonChangeEventArgs>? SeasonUpdate;
    public sealed class SeasonChangeEventArgs : EventArgs
    {
        public NetState State { get; }
        public SeasonChangeEventArgs(NetState state) => State = state;
        public byte Value { get; set; }
        public bool Sound { get; set; }
    }
    protected static class Season
    {
        [PacketHandler(0xBC, length: 3, ingame: true)]
        public static void Update(NetState ns, PacketReader pvSrc)
        {
            SeasonChangeEventArgs e = new(ns)
            {
                Value = pvSrc.ReadByte(),
                Sound = pvSrc.ReadBoolean()
            };
            SeasonUpdate?.Invoke(e);
        }
    }
}
