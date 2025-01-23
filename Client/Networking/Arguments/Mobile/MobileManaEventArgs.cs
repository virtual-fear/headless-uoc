namespace Client.Networking.Arguments;
using Client.Game;
using Client.Game.Data;
public sealed class MobileManaEventArgs : EventArgs
{

    [PacketHandler(0xA2, length: 9, ingame: true)]
    private static event PacketEventHandler<MobileManaEventArgs>? Update;
    public NetState State { get; }
    public Serial Serial { get; set; }
    public short ManaMax { get; set; }
    public short Mana { get; set; }
    private MobileManaEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Serial = (Serial)ip.ReadUInt32();
        ManaMax = ip.ReadInt16();
        Mana = ip.ReadInt16();
    }
    static MobileManaEventArgs() => Update += MobileManaEventArgs_Update;
    private static void MobileManaEventArgs_Update(MobileManaEventArgs e)
        => Mobile.Acquire(e.Serial).UpdateMana(e.Mana, e.ManaMax);
}