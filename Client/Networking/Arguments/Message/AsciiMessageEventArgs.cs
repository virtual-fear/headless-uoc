namespace Client.Networking.Arguments;
using Client.Game;
using Client.Game.Data;

public sealed class AsciiMessageEventArgs : EventArgs
{
    [PacketHandler(0x1C, length: -1, ingame: true)]
    private static event PacketEventHandler<AsciiMessageEventArgs>? Update;
    public NetState State { get; }
    public Mobile Mobile { get; }
    public short Graphic { get; }
    public byte MessageType { get; }
    public short Hue { get; }
    public short Font { get; }
    public string? Name { get; }
    public string? Text { get; }
    private AsciiMessageEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Mobile = Mobile.Acquire((Serial)ip.ReadUInt32());
        Graphic = ip.ReadInt16();
        MessageType = ip.ReadByte();
        Hue = ip.ReadInt16();
        Font = ip.ReadInt16();
        Name = ip.ReadString(30); // AsciiFixed
        Text = ip.ReadString(); // AsciiNull
    }
    static AsciiMessageEventArgs() => Update += AsciiMessageEventArgs_Update;
    private static void AsciiMessageEventArgs_Update(AsciiMessageEventArgs e) => Message.Add(e.State, e.Mobile, e.Graphic, e.MessageType, e.Hue, e.Font, e.Name, e.Text);
}