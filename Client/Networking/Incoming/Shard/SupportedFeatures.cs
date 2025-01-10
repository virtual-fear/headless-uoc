namespace Client.Networking.Incoming.Shard;
public partial class PacketHandlers
{
    public static event PacketEventHandler<SupportedFeaturesEventArgs>? Shard_SupportedFeatures;
    public sealed class SupportedFeaturesEventArgs : EventArgs
    {
        public NetState State { get; }
        internal SupportedFeaturesEventArgs(NetState state) => State = state;
        public uint Features { get; set; }
    }

    protected static class SupportedFeatures
    {
        [PacketHandler(0xB9, length: 5, ingame: false)]
        internal static void Update(NetState ns, PacketReader pvSrc)
        {
            SupportedFeaturesEventArgs e = new SupportedFeaturesEventArgs(ns);

            switch (pvSrc.Length)
            {
                case 5:
                    e.Features = pvSrc.ReadUInt32();
                    break;
                case 3:
                    e.Features = pvSrc.ReadUInt16();
                    break;
                default:
                    pvSrc.Trace();
                    break;
            }
            Shard_SupportedFeatures?.Invoke(e);
        }
    }
}