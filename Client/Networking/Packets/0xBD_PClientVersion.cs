namespace Client.Networking.Packets;
using Client.Networking.Arguments;
public sealed class PClientVersion : Packet
{
    /// <summary>
    ///     Requesting the client version
    /// </summary>
    internal PClientVersion(ClientVersionEventArgs e) : base(0xBD)
    {
        base.Stream.WriteAscii(e.Text);
        base.Stream.FilltoCapacity();
    }
}