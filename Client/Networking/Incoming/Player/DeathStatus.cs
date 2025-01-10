namespace Client.Networking.Incoming.Player;
public partial class PacketHandlers
{
    public static event PacketEventHandler<DeathStatusEventArgs>? Player_DeathStatus;
    public sealed class DeathStatusEventArgs : EventArgs
    {
        public NetState State { get; }
        public DeathStatusEventArgs(NetState state) => State = state;
        public bool Dead { get; set; }
    }

    protected static class DeathStatus
    {
        [PacketHandler(0x2C, length: 2, ingame: true)]
        internal static void Update(NetState ns, PacketReader pvSrc)
        {
            DeathStatusEventArgs e = new DeathStatusEventArgs(ns);
            e.Dead = (pvSrc.ReadByte() == 2);
            Player_DeathStatus?.Invoke(e);
        }
    }
}
