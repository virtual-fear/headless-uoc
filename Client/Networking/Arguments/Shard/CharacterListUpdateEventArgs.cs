namespace Client.Networking.Arguments;
public sealed class CharacterListUpdateEventArgs : EventArgs
{
    internal CharacterListUpdateEventArgs(NetState state, PacketReader ip) : base()
    {
        Logger.LogError("CharacterListUpdate received, not fully implemented!");
    }
}