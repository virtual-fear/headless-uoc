namespace Client.Networking.Arguments;
public sealed class CharacterListUpdateEventArgs : EventArgs
{
    [PacketHandler(0x86, length: -1, ingame: false)]
    public static event PacketEventHandler<CharacterListUpdateEventArgs>? Update;
    public NetState State { get; }
    
    // TODO: Add PacketReader instructions to the ctor
    internal CharacterListUpdateEventArgs(NetState state, PacketReader ip)
    {
        State = state;
    }
    static CharacterListUpdateEventArgs() => Update += CharacterListUpdateEventArgs_Update;
    private static void CharacterListUpdateEventArgs_Update(CharacterListUpdateEventArgs e)
    {
        Logger.LogError("CharacterListUpdate received, not fully implemented!");
    }
}