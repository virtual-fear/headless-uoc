namespace Client.Networking.Outgoing;
public sealed class ClientVersion : Packet
{
    /// <summary>
    ///     Requesting the client version
    /// </summary>
    private ClientVersion(NetState ns) : base(0xBD)
    {
        Version cv = Application.ClientVersion;
        base.Stream.Write(cv.ToString());
        base.Stream.Fill();
    }
    public static void SendBy(NetState ns) => ns.Send(new ClientVersion(ns));
}