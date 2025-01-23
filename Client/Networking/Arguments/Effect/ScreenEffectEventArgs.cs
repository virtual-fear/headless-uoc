namespace Client.Networking.Arguments;
using Client.Game;
using Client.Game.Data;
public sealed class ScreenEffectEventArgs : EventArgs
{
    [PacketHandler(0x70, length: 28, ingame: true)]
    private static event PacketEventHandler<ScreenEffectEventArgs>? Update;
    public NetState State { get; }
    public ScreenEffectType Type { get; }
    private ScreenEffectEventArgs(NetState state, PacketReader pvSrc)
    {
        State = state;
        if (pvSrc.ReadByte() != 0x04)
            pvSrc.Trace();
        pvSrc.ReadBytes(8);
        Type = (ScreenEffectType)pvSrc.ReadInt16();
        pvSrc.ReadBytes(16);
    }
    static ScreenEffectEventArgs() => Update += ScreenEffectEventArgs_Update;
    private static void ScreenEffectEventArgs_Update(ScreenEffectEventArgs e)
        => Effect.Enqueue(e.State, e.Type);
}