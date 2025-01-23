namespace Client.Networking.Arguments;
using Client.Game;
public sealed class GlobalLightEventArgs : EventArgs
{
    [PacketHandler(0x4F, length: 2, ingame: true)]
    private static event PacketEventHandler<GlobalLightEventArgs>? Update;
    public NetState State { get; }
    public sbyte Level { get; }
    private GlobalLightEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Level = ip.ReadSByte();
    }
    static GlobalLightEventArgs() => Update += GlobalLightEventArgs_Update;

    private static void GlobalLightEventArgs_Update(GlobalLightEventArgs e) => World.LightGlobalValue = e.Level;
}