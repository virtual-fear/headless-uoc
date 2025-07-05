namespace Client.Networking.Arguments;
using Client.Game;
public sealed class SecureTradeEventArgs : EventArgs
{
    [PacketHandler(0x6F, length: -1, ingame: true)]
    private static event PacketEventHandler<SecureTradeEventArgs>? Update;
    public NetState State { get; }
    public int Them { get; }
    public int FirstContainer { get; }
    public int SecondContainer { get; }
    public string? Name { get; }
    private SecureTradeEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        ip.ReadByte();   //  0   :   runuo:display
        Them = ip.ReadInt32();
        FirstContainer = ip.ReadInt32();
        SecondContainer = ip.ReadInt32();
        ip.ReadBoolean();    //  always true
        Name = ip.ReadString(30);
    }
    static SecureTradeEventArgs() => Update += SecureTradeEventArgs_Update;
    private static void SecureTradeEventArgs_Update(SecureTradeEventArgs e) => Player.Trade(e.State, e.FirstContainer, e.Name, e.Them, e.SecondContainer);
}