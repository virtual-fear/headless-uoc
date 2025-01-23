namespace Client.Networking.Arguments;
using Client.Game;
public sealed class PlaySoundEventArgs : EventArgs
{
    [PacketHandler(0x54, length: 12, ingame: true)]
    public static event PacketEventHandler<PlaySoundEventArgs>? Update;
    public NetState State { get; }
    public byte Flags { get; }
    public short SoundID { get; }
    public byte Volume { get; }
    public short X { get; }
    public short Y { get; }
    public short Z { get; }
    private PlaySoundEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Flags = ip.ReadByte();
        SoundID = ip.ReadInt16();
        Volume = ip.ReadByte();
        X = ip.ReadInt16();
        Y = ip.ReadInt16();
        Z = ip.ReadInt16();
    }

    static PlaySoundEventArgs() => Update += PlaySoundEventArgs_Update;
    private static void PlaySoundEventArgs_Update(PlaySoundEventArgs e) => World.PlayAudio(e);
}