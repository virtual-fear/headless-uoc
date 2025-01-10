namespace Client.Networking.Incoming.Audio;
public partial class PacketHandlers
{
    public static event PacketEventHandler<SoundEventArgs>? OnSound;
    public sealed class SoundEventArgs : EventArgs
    {
        public NetState State { get; }
        public SoundEventArgs(NetState state) => State = state;
        public short SoundID { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
        public short Z { get; set; }
    }
    protected static class Sound
    {

        [PacketHandler(0x54, length: 12, ingame: true)]
        internal static void Receive(NetState ns, PacketReader pvSrc)
        {
            SoundEventArgs e = new SoundEventArgs(ns);
            pvSrc.ReadByte();   //  Flags
            e.SoundID = pvSrc.ReadInt16();
            pvSrc.ReadByte();   //  Volume
            e.X = pvSrc.ReadInt16();
            e.Y = pvSrc.ReadInt16();
            e.Z = pvSrc.ReadInt16();
            OnSound?.Invoke(e);
        }
    }
}