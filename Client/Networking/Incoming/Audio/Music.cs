using Client.Game.Data;

namespace Client.Networking.Incoming.Audio;
public partial class PacketHandlers
{
    public static event PacketEventHandler<PlayMusicEventArgs>? OnMusic;
    public sealed class PlayMusicEventArgs : EventArgs
    {
        public NetState State { get; }
        public PlayMusicEventArgs(NetState state) => State = state;
        public MusicName Name { get; set; }
    }
    protected static class Music
    {
        [PacketHandler(0x6D, length: 3, ingame: true)]
        internal static void Receive(NetState ns, PacketReader pvSrc)
            => OnMusic?.Invoke(
                new(ns) {
                    Name = (MusicName)pvSrc.ReadInt16()
                });
    }
}
