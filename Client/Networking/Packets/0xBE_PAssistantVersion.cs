namespace Client.Networking.Packets;
using Client.Networking.Arguments;
public sealed class PAssistantVersion : Packet
{
    internal PAssistantVersion(AssistVersionEventArgs args) : base(0xBE)
    {
        base.Stream.Write(args.Version);
        base.Stream.WriteAscii(args.Text);
        base.Stream.FillwithZeros(sizeof(byte));
    }
}