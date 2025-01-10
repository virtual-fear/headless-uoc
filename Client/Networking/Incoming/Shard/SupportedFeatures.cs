namespace Client.Networking.Incoming;
public sealed class SupportedFeaturesEventArgs : EventArgs
{
    public NetState State { get; }
    internal SupportedFeaturesEventArgs(NetState state) => State = state;
    public uint Features { get; set; }
}

public partial class Shard
{
    public static event PacketEventHandler<SupportedFeaturesEventArgs>? UpdateSupportedFeatures;

    [PacketHandler(0xB9, length: 5, ingame: false)]
    protected static void Receive_SupportedFeatuers(NetState ns, PacketReader pvSrc)
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
        UpdateSupportedFeatures?.Invoke(e);
    }
}