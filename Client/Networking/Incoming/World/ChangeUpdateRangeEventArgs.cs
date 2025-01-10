namespace Client.Networking.Incoming;
public sealed class ChangeUpdateRangeEventArgs : EventArgs
{
    public NetState State { get; }
    public ChangeUpdateRangeEventArgs(NetState state) => State = state;
    public byte Range { get; set; }
}
public partial class World
{
    public static event PacketEventHandler<ChangeUpdateRangeEventArgs>? OnChangeUpdateRange;

    [PacketHandler(0xC8, length: 2, ingame: true)]
    internal static void Receive_ChangeUpdateRange(NetState ns, PacketReader pvSrc)
    {
        ChangeUpdateRangeEventArgs e = new(ns);
        e.Range = pvSrc.ReadByte();
        OnChangeUpdateRange?.Invoke(e);
    }
}