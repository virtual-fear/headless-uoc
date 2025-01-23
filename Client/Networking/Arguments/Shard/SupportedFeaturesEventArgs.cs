namespace Client.Networking.Arguments;
public sealed class SupportedFeaturesEventArgs : EventArgs
{
    [PacketHandler(0xB9, length: 5, ingame: false)]
    public static event PacketEventHandler<SupportedFeaturesEventArgs>? Update;
    public NetState State { get; }
    public uint Features { get; }
    internal SupportedFeaturesEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Features = ip.ReadUInt32();
    }
    static SupportedFeaturesEventArgs() => Update += SupportedFeaturesEventArgs_Update;
    private static void SupportedFeaturesEventArgs_Update(SupportedFeaturesEventArgs e)
    {
        // Server.Features = e.Features; ?
    }
}