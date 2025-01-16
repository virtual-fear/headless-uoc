using MusicName = Client.Game.Data.MusicName;
namespace Client.Networking.Incoming;
public sealed class PlayMusicEventArgs : EventArgs
{
    public NetState State { get; }
    public MusicName Name { get; }
    internal PlayMusicEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Name = (MusicName)ip.ReadInt16();
    }
}