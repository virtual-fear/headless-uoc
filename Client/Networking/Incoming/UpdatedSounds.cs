namespace Client.Networking.Incoming;
using System.Collections;
using Client.Game.Context.Data;
using static PacketSink;
public partial class PacketSink
{
    #region EventArgs
    
    public sealed class PlayMusicEventArgs : EventArgs
    {
        public NetState State { get; }
        public PlayMusicEventArgs(NetState state) => State = state;
        public MusicName Name { get; set; }
    }
    public sealed class PlaySoundEventArgs : EventArgs
    {
        public NetState State { get; }
        public PlaySoundEventArgs(NetState state) => State = state;
        public short SoundID { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
        public short Z { get; set; }
    }

    #endregion (done)

    public static event PacketEventHandler<PlaySoundEventArgs>? PlaySound;
    public static event PacketEventHandler<PlayMusicEventArgs>? PlayMusic;
    public static void InvokePlayMusic(PlayMusicEventArgs e) => PlayMusic?.Invoke(e);
    public static void InvokePlaySound(PlaySoundEventArgs e) => PlaySound?.Invoke(e);
}
internal static class UpdatedSounds
{
    private static readonly Queue<object> Queue = new Queue<object>();
    private static readonly object Chain = new();
    private static readonly object Items = new();
    public static void Slice()
    {
        lock (Chain)
        {
            while (Queue.Count > 0)
            {
                lock (Items)
                {
                    object e = Queue.Dequeue();
                    if (e is PlayMusicEventArgs)
                        Play((PlayMusicEventArgs)e);
                    if (e is PlaySoundEventArgs)
                        Play((PlaySoundEventArgs)e);
                }
            }
        }
    }
    private static void Play(PlayMusicEventArgs e) { }
    private static void Play(PlaySoundEventArgs e) { }
    public static void Configure()
    {
        Register(0x54, 12, true, new OnPacketReceive(PlaySound));
        Register(0x6D, 03, true, new OnPacketReceive(PlayMusic));

        // When music or sound is played, it is added to the queue to get played in the next slice.
        PacketSink.PlayMusic += (e) => { lock (Chain) Queue.Enqueue(e); };
        PacketSink.PlaySound += (e) => { lock (Chain) Queue.Enqueue(e); };
    }
    
    private static void PlayMusic(NetState ns, PacketReader pvSrc) 
        => PacketSink.InvokePlayMusic(new PlayMusicEventArgs(ns) { Name = (MusicName)pvSrc.ReadInt16() });

    private static void PlaySound(NetState ns, PacketReader pvSrc)
    {
        PlaySoundEventArgs e = new PlaySoundEventArgs(ns);
        pvSrc.ReadByte();   //  Flags
        e.SoundID = pvSrc.ReadInt16();
        pvSrc.ReadByte();   //  Volume
        e.X = pvSrc.ReadInt16();
        e.Y = pvSrc.ReadInt16();
        e.Z = pvSrc.ReadInt16();
        PacketSink.InvokePlaySound(e);
    }

    static void Register(byte packetID, int length, bool variable, OnPacketReceive onReceive) 
        => PacketHandlers.Register(packetID, length, variable, onReceive);
}
