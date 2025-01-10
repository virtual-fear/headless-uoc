namespace Client.Networking.Incoming.Effects;
using Client.Game.Data;
public partial class Effect
{
    public static event PacketEventHandler<ParticleEffectEventArgs>? OnParticleUpdate;
    public sealed class ParticleEffectEventArgs : EventArgs
    {
        public NetState State { get; }
        public ParticleEffectEventArgs(NetState state) => State = state;
        public EffectType Type { get; set; }
        public int Source { get; set; }
        public int Target { get; set; }
        public short ItemID { get; set; }
        public short SourceX { get; set; }
        public short SourceY { get; set; }
        public sbyte SourceZ { get; set; }
        public short TargetX { get; set; }
        public short TargetY { get; set; }
        public sbyte TargetZ { get; set; }
        public byte Speed { get; set; }
        public byte Duration { get; set; }
        public bool FixedDirection { get; set; }
        public bool Explode { get; set; }
        public int Hue { get; set; }
        public int Render { get; set; }
        public short Effect { get; set; }
        public short ExplodeEffect { get; set; }
        public short ExplodeSound { get; set; }
        public int Serial { get; set; }
        public byte Layer { get; set; }
        public short Unknown { get; set; }
    }

    [PacketHandler(0xC7, length: 49, ingame: true)]
    protected static void Received_ParticleEffect(NetState ns, PacketReader pvSrc)
    {
        ParticleEffectEventArgs e = new(ns);
        e.Type = (EffectType)pvSrc.ReadByte();
        e.Source = pvSrc.ReadInt32();
        e.Target = pvSrc.ReadInt32();
        e.ItemID = pvSrc.ReadInt16();
        e.SourceX = pvSrc.ReadInt16();
        e.SourceY = pvSrc.ReadInt16();
        e.SourceZ = pvSrc.ReadSByte();
        e.TargetX = pvSrc.ReadInt16();
        e.TargetY = pvSrc.ReadInt16();
        e.TargetZ = pvSrc.ReadSByte();
        e.Speed = pvSrc.ReadByte();
        e.Duration = pvSrc.ReadByte();
        pvSrc.ReadByte();   //  0x00
        pvSrc.ReadByte();   //  0x00
        e.FixedDirection = pvSrc.ReadBoolean();
        e.Explode = pvSrc.ReadBoolean();
        e.Hue = pvSrc.ReadInt32();
        e.Render = pvSrc.ReadInt32();   //  RenderMode
        e.Effect = pvSrc.ReadInt16();
        e.ExplodeEffect = pvSrc.ReadInt16();
        e.ExplodeSound = pvSrc.ReadInt16();
        e.Serial = pvSrc.ReadInt32();
        e.Layer = pvSrc.ReadByte();
        e.Unknown = pvSrc.ReadInt16();
        OnParticleUpdate?.Invoke(e);
    }
}