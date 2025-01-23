namespace Client.Networking.Arguments;
public sealed class NullFastwalkStackEventArgs : EventArgs
{
    [PacketHandler(0x1D, length: 5, ingame: true, extCmd: true)]
    public static event PacketEventHandler<NullFastwalkStackEventArgs>? Update;
    public NetState State { get; }
    public byte[]? Buffer { get; }
    internal NullFastwalkStackEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Buffer = ip.ReadBytes(6 * sizeof(int));
    }
    static NullFastwalkStackEventArgs() => Update += NullFastwalkStackEventArgs_Update;
    private static void NullFastwalkStackEventArgs_Update(NullFastwalkStackEventArgs e)
        => Logger.Log(e.State.Address, "NullFastwalkStack");
}