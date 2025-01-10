namespace Client.Networking.Incoming.Player;
public partial class PacketHandlers
{
    public static event PacketEventHandler<SecureTradeEventArgs>? Player_SecureTrade;
    public sealed class SecureTradeEventArgs : EventArgs
    {
        public NetState State { get; }
        public SecureTradeEventArgs(NetState state) => State = state;
        public int Them { get; set; }
        public int FirstContainer { get; set; }
        public int SecondContainer { get; set; }
        public string? Name { get; set; }
    }
    protected static class SecureTrade
    {
        [PacketHandler(0x6F, length: -1, ingame: true)]
        public static void Update(NetState ns, PacketReader pvSrc)
        {
            SecureTradeEventArgs e = new(ns);
            pvSrc.ReadByte();   //  0   :   runuo:display
            e.Them = pvSrc.ReadInt32();
            e.FirstContainer = pvSrc.ReadInt32();
            e.SecondContainer = pvSrc.ReadInt32();
            pvSrc.ReadBoolean();    //  always true
            e.Name = pvSrc.ReadString(30);
            Player_SecureTrade?.Invoke(e);
        }
    }
}
