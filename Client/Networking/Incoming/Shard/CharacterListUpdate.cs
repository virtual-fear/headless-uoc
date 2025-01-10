namespace Client.Networking.Incoming.Shard;
public partial class PacketHandlers
{
    protected static class CharacterListUpdate
    {
        [PacketHandler(0x86, length: -1, ingame: false)]
        internal static void Update (NetState state, PacketReader pvSrc)
        {
            Logger.LogError("CharacterListUpdate received, not fully implemented yet.");
        }

    }
}
