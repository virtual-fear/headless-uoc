using EffectType = Client.Game.Data.EffectType;
namespace Client.Networking.Incoming.Effects;
public sealed class ParticleEffectEventArgs : EventArgs
{
    public NetState State { get; }
    public EffectType Type { get; }
    public int Source { get; }
    public int Target { get; }
    public short ItemID { get; }
    public short SourceX { get; }
    public short SourceY { get; }
    public sbyte SourceZ { get; }
    public short TargetX { get; }
    public short TargetY { get; }
    public sbyte TargetZ { get; }
    public byte Speed { get; }
    public byte Duration { get; }
    public bool FixedDirection { get; }
    public bool Explode { get; }
    public int Hue { get; }
    public int Render { get; }
    public short Effect { get; }
    public short ExplodeEffect { get; }
    public short ExplodeSound { get; }
    public int Serial { get; }
    public byte Layer { get; }
    public short Unknown { get; }
    internal ParticleEffectEventArgs(NetState state, PacketReader pvSrc)
    {
        State = state;
        Type = (EffectType)pvSrc.ReadByte();
        Source = pvSrc.ReadInt32();
        Target = pvSrc.ReadInt32();
        ItemID = pvSrc.ReadInt16();
        SourceX = pvSrc.ReadInt16();
        SourceY = pvSrc.ReadInt16();
        SourceZ = pvSrc.ReadSByte();
        TargetX = pvSrc.ReadInt16();
        TargetY = pvSrc.ReadInt16();
        TargetZ = pvSrc.ReadSByte();
        Speed = pvSrc.ReadByte();
        Duration = pvSrc.ReadByte();
        pvSrc.ReadByte();   //  0x00
        pvSrc.ReadByte();   //  0x00
        FixedDirection = pvSrc.ReadBoolean();
        Explode = pvSrc.ReadBoolean();
        Hue = pvSrc.ReadInt32();
        Render = pvSrc.ReadInt32();   //  RenderMode
        Effect = pvSrc.ReadInt16();
        ExplodeEffect = pvSrc.ReadInt16();
        ExplodeSound = pvSrc.ReadInt16();
        Serial = pvSrc.ReadInt32();
        Layer = pvSrc.ReadByte();
        Unknown = pvSrc.ReadInt16();
    }
}