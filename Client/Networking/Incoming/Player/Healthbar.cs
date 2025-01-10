namespace Client.Networking.Incoming.Player;
using Client.Game.Data;
public partial class PacketHandlers
{
    public static event PacketEventHandler<HealthbarEventArgs>? Player_Healthbar;
    public sealed class HealthbarEventArgs : EventArgs
    {
        public NetState State { get; }
        public HealthbarEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public HealthbarType Type { get; set; }
        public byte Level { get; set; }
    }
    protected static class Healthbar
    {
        [PacketHandler(0x17, length: 12, ingame: true)]
        internal static void Update(NetState ns, PacketReader pvSrc)
        {
            HealthbarEventArgs e = new(ns);
            e.Serial = pvSrc.ReadInt32();
            pvSrc.Seek(2, SeekOrigin.Current);
            e.Type = (HealthbarType)pvSrc.ReadInt16();
            e.Level = pvSrc.ReadByte();
            Player_Healthbar?.Invoke(e);
        }
    }
}
