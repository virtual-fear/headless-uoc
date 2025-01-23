namespace Client.Networking.Arguments;
using Client.Game;
public sealed class SetWarModeEventArgs : EventArgs
{
    [PacketHandler(0x72, length: 5, ingame: true)] // NOTE: Maybe this should be elsewhere?
    private static event PacketEventHandler<SetWarModeEventArgs>? Update;
    public NetState State { get; }
    public bool Enabled { get; }
    private SetWarModeEventArgs(NetState state, PacketReader pvSrc)
    {
        State = state;
        Enabled = pvSrc.ReadBoolean();
        pvSrc.ReadByte();   //  0x00
        pvSrc.ReadByte();   //  0x32
        pvSrc.ReadByte();   //  0x00
    }

    static SetWarModeEventArgs() => Update += SetWarModeEventArgs_Update;
    private static void SetWarModeEventArgs_Update(SetWarModeEventArgs e) => Player.Warmode = e.Enabled;
}