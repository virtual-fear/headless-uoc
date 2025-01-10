namespace Client.Networking.Incoming;
public partial class Shard
{
    private static void RejectedLogin()
        => Logger.LogError("Login rejection received, not fully implemented yet!");

    [PacketHandler(0x53, length: -1, ingame: false)]
    public static void Received_Rejection_0x53(NetState ns, PacketReader pvSrc) => RejectedLogin();

    [PacketHandler(0x82, length: -1, ingame: false)]
    public static void Received_Rejection_0x82(NetState ns, PacketReader pvSrc) => RejectedLogin();

    [PacketHandler(0x85, length: -1, ingame: false)]
    public static void Received_Rejection_0x85(NetState ns, PacketReader pvSrc) => RejectedLogin();
}
