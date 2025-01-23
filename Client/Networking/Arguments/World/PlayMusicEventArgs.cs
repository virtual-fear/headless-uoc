namespace Client.Networking.Arguments;
using Client.Game;
using Client.Game.Data;
public sealed class PlayMusicEventArgs : EventArgs
{
    [PacketHandler(0x6D, length: 3, ingame: true)]
    private static event PacketEventHandler<PlayMusicEventArgs>? Update;
    public NetState State { get; }
    public MusicName Name { get; }
    private PlayMusicEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Name = (MusicName)ip.ReadInt16();
    }

    static PlayMusicEventArgs() => Update += PlayMusicEventArgs_Update;
    private static void PlayMusicEventArgs_Update(PlayMusicEventArgs e) => World.PlayAudio(from: e.State, e.Name);
}