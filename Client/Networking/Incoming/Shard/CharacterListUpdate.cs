namespace Client.Networking.Incoming;
public partial class Shard
{
    [PacketHandler(0x86, length: -1, ingame: false)]
    protected static void Received_CharacterListUpdate(NetState state, PacketReader pvSrc)
    {
        Logger.LogError("CharacterListUpdate received, not fully implemented yet.");
    }
}