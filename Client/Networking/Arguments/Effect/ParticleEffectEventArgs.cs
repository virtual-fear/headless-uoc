namespace Client.Networking.Arguments;
using Client.Game;
using Client.Game.Data;
public sealed class ParticleEffectEventArgs : EventArgs
{
    [PacketHandler(0xC7, length: 49, ingame: true)]
    public static event PacketEventHandler<ParticleEffectEventArgs>? Update;
    public NetState State { get; }
    public EffectType Type { get; }
    public Serial Source { get; }
    public Serial Target { get; }
    public short ItemID { get; }
    public IPoint3D From { get; }
    public IPoint3D To { get; }
    public byte Speed { get; }
    public byte Duration { get; }
    public bool FixedDirection { get; }
    public bool Explode { get; }
    public int Hue { get; }
    public int Render { get; }
    public short EffectID { get; }
    public short ExplodeEffect { get; }
    public short ExplodeSound { get; }
    public int Serial { get; }
    public byte Layer { get; }
    public short Unknown { get; }
    internal ParticleEffectEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Type = (EffectType)ip.ReadByte();
        Source = (Serial)ip.ReadUInt32();
        Target = (Serial)ip.ReadUInt32();
        ItemID = ip.ReadInt16();
        From = new Point3D()
        {
            X = ip.ReadInt16(),
            Y = ip.ReadInt16(),
            Z = ip.ReadSByte()
        };
        To = new Point3D()
        {
            X = ip.ReadInt16(),
            Y = ip.ReadInt16(),
            Z = ip.ReadSByte()
        };
        Speed = ip.ReadByte();
        Duration = ip.ReadByte();
        ip.ReadByte();   //  0x00
        ip.ReadByte();   //  0x00
        FixedDirection = ip.ReadBoolean();
        Explode = ip.ReadBoolean();
        Hue = ip.ReadInt32();
        Render = ip.ReadInt32();   //  RenderMode
        EffectID = ip.ReadInt16();
        ExplodeEffect = ip.ReadInt16();
        ExplodeSound = ip.ReadInt16();
        Serial = ip.ReadInt32();
        Layer = ip.ReadByte();
        Unknown = ip.ReadInt16();
    }
    static ParticleEffectEventArgs() => Update += ParticleEffectEventArgs_Update;
    private static void ParticleEffectEventArgs_Update(ParticleEffectEventArgs e)
        => Effect.Enqueue(e.State, EffectType.FixedFrom, e.Source, e.Target, e.ItemID, e.From, e.To, e.Hue, e.Speed, e.Duration, e.FixedDirection, e.Explode, e.Render, e.EffectID, e.ExplodeEffect, e.ExplodeSound, e.Serial, e.Layer, e.Unknown);
}