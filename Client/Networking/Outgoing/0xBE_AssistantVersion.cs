namespace Client.Networking.Outgoing;
public sealed class AssistantVersion : Packet
{
    /// <summary>
    ///     Requesting Assistant
    /// </summary>
    private AssistantVersion(NetState ns, PacketReader ip) : base(0xBE)
    {
        base.Stream.Write((int)ip.ReadInt32());
        base.Stream.Write(ns.Version.ToString());
        base.Stream.Fill(sizeof(byte));
    }
    public static void SendBy(NetState ns, PacketReader ip) => ns.Send(new AssistantVersion(ns, ip));
}