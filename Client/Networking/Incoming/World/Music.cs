namespace Client.Networking.Incoming;
using Client.Game.Data;
public sealed class MusicEventArgs : EventArgs
{
    public NetState State { get; }
    public MusicEventArgs(NetState state) => State = state;
    public MusicName Name { get; set; }
}
public partial class World
{
    public static event PacketEventHandler<MusicEventArgs>? UpdateMusic;

    [PacketHandler(0x6D, length: 3, ingame: true)]
    protected static void Received_Music(NetState ns, PacketReader pvSrc)
        => UpdateMusic?.Invoke(
            new(ns)
            {
                Name = (MusicName)pvSrc.ReadInt16()
            });
}
