namespace Client.Game;
using Client.Networking;
using Client.Networking.Arguments;
public partial class Book
{
    [PacketHandler(0x66, length: -1, ingame: true)]
    public static event PacketEventHandler<BookInfoEventArgs>? Info;

    [PacketHandler(0xD4, length: -1, ingame: true)]
    public static event PacketEventHandler<BookOpenEventArgs>? Open;
}