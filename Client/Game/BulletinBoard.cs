namespace Client.Game;
using Client.Networking;
using Client.Networking.Arguments;
public static class BulletinBoard
{
    [PacketHandler(0x71, length: -1, ingame: true)]
    public static event PacketEventHandler<BulletinBoardEventArgs>? UpdateBulletinBoard;
}