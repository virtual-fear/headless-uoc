namespace Client.Networking.Arguments;
public sealed class PersonalLightEventArgs : EventArgs
{
    public NetState State { get; }
    public sbyte Level { get; set; }
    public int Serial { get; set; }
    internal PersonalLightEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Serial = ip.ReadInt32();
        Level = ip.ReadSByte();
    }
}