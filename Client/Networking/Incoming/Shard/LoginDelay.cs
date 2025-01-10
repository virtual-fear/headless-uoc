namespace Client.Networking.Incoming;
public partial class Shard
{
    [PacketHandler(0xFD, length: -1, ingame: false)]
    protected static void Received_LoginDelay(NetState state, PacketReader pvSrc)
        => Logger.LogError("LoginDelay received, not fully implemented yet.");

}
