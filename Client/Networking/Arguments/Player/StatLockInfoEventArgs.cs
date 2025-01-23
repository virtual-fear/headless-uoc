using Client.Game;

namespace Client.Networking.Arguments;
public sealed class StatLockInfoEventArgs : EventArgs
{
    [PacketHandler(0x19, length: 12, ingame: true, extCmd: true)]
    private static event PacketEventHandler<StatLockInfoEventArgs>? Update; // (ext) packetID: 0x19 (TODO-Fix: Same as bonded status?
    public NetState State { get; }
    public int Mobile { get; }
    private StatLockInfoEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        ip.ReadByte();
        Mobile = ip.ReadInt32();
        ip.ReadByte();
        byte lockBits = ip.ReadByte();
        // TODO: Fix lockBits
    }

    static StatLockInfoEventArgs() => Update += StatLockInfoEventArgs_Update;
    private static void StatLockInfoEventArgs_Update(StatLockInfoEventArgs e) => throw new NotImplementedException();
}