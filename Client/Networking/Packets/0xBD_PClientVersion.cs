using System.Net.Sockets;

namespace Client.Networking.Packets;
public sealed class PClientVersion : Packet
{
    /// <summary>
    ///     Requesting the client version
    /// </summary>
    private PClientVersion(NetState ns) : base(0xBD)
    {
        Version cv = Application.ClientVersion;
        base.Stream.WriteAscii(cv.ToString());
        base.Stream.FilltoCapacity();

    }
    public static void SendBy(NetState ns) => ns.Send(new PClientVersion(ns));
}