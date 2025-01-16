using ScreenEffectType = Client.Game.Data.ScreenEffectType;
namespace Client.Networking.Incoming;
public sealed class ScreenEffectEventArgs : EventArgs
{
    public NetState State { get; }
    public ScreenEffectType Type { get; }
    internal ScreenEffectEventArgs(NetState state, PacketReader pvSrc)
    {
        State = state;
        if (pvSrc.ReadByte() != 0x04)
            pvSrc.Trace();
        pvSrc.ReadBytes(8);
        Type = (ScreenEffectType)pvSrc.ReadInt16();
        pvSrc.ReadBytes(16);
    }
}