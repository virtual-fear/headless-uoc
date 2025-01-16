namespace Client.Networking.Incoming;
public sealed class CharacterListUpdateEventArgs : EventArgs
{
    public CharacterListUpdateEventArgs(NetState state, PacketReader ip) : base()
    {
        Logger.LogError("CharacterListUpdate received, not fully implemented!");
    }
}