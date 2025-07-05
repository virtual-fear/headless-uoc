namespace Client.Networking.Arguments;
using Client.Game;
public sealed class DeathStatusEventArgs : EventArgs
{
    [PacketHandler(0x2C, length: 2, ingame: true)]
    private static event PacketEventHandler<DeathStatusEventArgs>? Update;
    public NetState State { get; }
    public bool Dead { get; }
    private DeathStatusEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Dead = ip.ReadByte() == 2;
    }
    static DeathStatusEventArgs() => Update += DeathStatusEventArgs_Update;
    private static void DeathStatusEventArgs_Update(DeathStatusEventArgs e) => Player.IsDead = e.Dead;
}