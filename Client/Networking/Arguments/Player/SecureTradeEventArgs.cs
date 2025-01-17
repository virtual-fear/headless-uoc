namespace Client.Networking.Arguments;
public sealed class SecureTradeEventArgs : EventArgs
{
    public NetState State { get; }
    public int Them { get; }
    public int FirstContainer { get; }
    public int SecondContainer { get; }
    public string? Name { get; }
    internal SecureTradeEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        ip.ReadByte();   //  0   :   runuo:display
        Them = ip.ReadInt32();
        FirstContainer = ip.ReadInt32();
        SecondContainer = ip.ReadInt32();
        ip.ReadBoolean();    //  always true
        Name = ip.ReadString(30);
    }
}