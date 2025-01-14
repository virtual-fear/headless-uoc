namespace Client.Networking.Outgoing;

public delegate void SeedEventHandler(SeedEventArgs e);
public sealed class SeedEventArgs : EventArgs
{
    public NetState State { get; }
    public SeedEventArgs(NetState state) => State = state;
    public SeedEventArgs(NetState state, Version version)
    {
        State = state;
        Version = version;
    }
    public Version? Version { get; set; }
}

public sealed class PInitialSeed : Packet
{
    public static event SeedEventHandler Construct;
    private PInitialSeed() : base(0xEF, 21) => Encode = false;
    static PInitialSeed() => PInitialSeed.Construct += UpdateVersion;
    static void UpdateVersion(SeedEventArgs e) => e.Version = Application.ClientVersion;
    private static Packet Instantiate(NetState state)
    {
        SeedEventArgs e = new(state) { Version = Application.ClientVersion };
        Construct?.Invoke(e);
        Version ver = e.Version;
        Packet packet = new PInitialSeed();
        packet.Stream.Write((int)1);
        packet.Stream.Write((int)ver.Major);
        packet.Stream.Write((int)ver.Minor);
        packet.Stream.Write((int)ver.Build);
        packet.Stream.Write((int)ver.Revision);
        return packet;
    }
    public static void SendBy(NetState state)
    {
        if (state == null)
            throw new ArgumentNullException("state");
        state.Send(PInitialSeed.Instantiate(state));
    }
}