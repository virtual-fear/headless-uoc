namespace Client.Networking.Incoming.Player;
public partial class PacketHandlers
{
    public static event PacketEventHandler<FightingEventArgs>? Player_Fighting;
    public sealed class FightingEventArgs : EventArgs
    {
        public NetState State { get; }
        public FightingEventArgs(NetState state) => State = state;
        public byte Flag { get; set; }
        public int Attacker { get; set; }
        public int Defender { get; set; }
    }

    protected static class Fighting
    {
        [PacketHandler(0x2F, length: 10, ingame: true)]
        internal static void Update(NetState ns, PacketReader pvSrc)
        {
            FightingEventArgs e = new FightingEventArgs(ns);
            e.Flag = pvSrc.ReadByte();
            e.Attacker = pvSrc.ReadInt32();
            e.Defender = pvSrc.ReadInt32();
            Player_Fighting?.Invoke(e);
        } // RunUO: Swing

    }
}
