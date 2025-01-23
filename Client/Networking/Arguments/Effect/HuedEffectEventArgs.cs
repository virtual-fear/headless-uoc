namespace Client.Networking.Arguments;
using Client.Game;
using Client.Game.Data;
public sealed class HuedEffectEventArgs : EventArgs
{
    [PacketHandler(0xC0, length: 36, ingame: true)]
    private static event PacketEventHandler<HuedEffectEventArgs>? Update;
    public NetState State { get; }
    public EffectType Type { get; }
    public Serial Source { get; }
    public Serial Target { get; }
    public short ItemID { get; }
    public IPoint3D From { get; }
    public IPoint3D To { get; }
    public byte Speed { get; }
    public byte Duration { get; }
    public bool IsFixedDirection { get; }
    public bool IsExploding { get; }
    public int Hue { get; }
    public int RenderMode { get; }
    private HuedEffectEventArgs(NetState state, PacketReader ip)
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
        ip.ReadByte(); // 0
        ip.ReadByte(); // 0
        IsFixedDirection = ip.ReadBoolean();
        IsExploding = ip.ReadBoolean();
        Hue = ip.ReadInt32();
        RenderMode = ip.ReadInt32();
    }
    static HuedEffectEventArgs() => Update += HuedEffectEventArgs_Update;
    private static void HuedEffectEventArgs_Update(HuedEffectEventArgs e)
        => Effect.Enqueue(e.State, e.Type, e.Source, e.Target, e.ItemID, e.From, e.To, e.Hue, e.Speed, e.Duration, e.IsFixedDirection, e.IsExploding, e.RenderMode);
}