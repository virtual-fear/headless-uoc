namespace Client.Networking.Arguments;
using Client.Game;
using Client.Game.Data;
public sealed class RemoveEventArgs : EventArgs
{
    [PacketHandler(0x1D, length: 5, ingame: true)]
    private static event PacketEventHandler<RemoveEventArgs>? Update;
    public NetState State { get; }
    public Serial Serial { get; }
    private RemoveEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Serial = (Serial)ip.ReadUInt32();
    }
    static RemoveEventArgs() => Update += RemoveEventArgs_Update;
    private static void RemoveEventArgs_Update(RemoveEventArgs e) => World.Remove(e.Serial);
}