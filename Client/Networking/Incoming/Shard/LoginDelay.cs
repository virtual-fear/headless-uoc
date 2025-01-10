namespace Client.Networking.Incoming.Shard;
public partial class PacketHandlers
{
    protected static class LoginDelay
    {
        [PacketHandler(0xFD, length: -1, ingame: false)]
        internal static void Receive(NetState state, PacketReader pvSrc)
            => Logger.LogError("LoginDelay received, not fully implemented yet.");

    }
}
