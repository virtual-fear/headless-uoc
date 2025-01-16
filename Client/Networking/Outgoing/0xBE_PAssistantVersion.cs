namespace Client.Networking.Outgoing;
public sealed class PAssistantVersion : Packet
{
    /// <summary>
    ///     Requesting Assistant
    /// </summary>
    private PAssistantVersion(NetState ns, PacketReader ip) : base(0xBE)
    {
        base.Stream.Write((int)ip.ReadInt32());
        Version cv = Application.ClientVersion;
        base.Stream.WriteAscii(cv.ToString());
        base.Stream.FillwithZeros(sizeof(byte));
    }
    public static void SendBy(NetState ns, PacketReader ip) => ns.Send(new PAssistantVersion(ns, ip));
}