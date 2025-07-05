namespace Client.Networking.Arguments;
using Client.Game;
using Client.Game.Data;
public sealed class DragEffectEventArgs : EventArgs
{
    [PacketHandler(0x23, length: 26, ingame: true)]
    private static event PacketEventHandler<DragEffectEventArgs>? Update;
    public NetState State { get; }
    public short ItemID { get; }
    public short Hue { get; }
    public short Amount { get; }
    public Serial Source { get; }
    public Serial Target { get; }
    public IPoint3D From { get; }
    public IPoint3D To { get; }
    private DragEffectEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        ItemID = ip.ReadInt16();
        ip.ReadByte();
        Hue = ip.ReadInt16();
        Amount = ip.ReadInt16();
        Source = (Serial)ip.ReadUInt32();
        From = new Point3D()
        {
            X = ip.ReadInt16(),
            Y = ip.ReadInt16(),
            Z = ip.ReadSByte()
        };
        Target = (Serial)ip.ReadUInt32();
        To = new Point3D()
        {
            X = ip.ReadInt16(),
            Y = ip.ReadInt16(),
            Z = ip.ReadSByte()
        };
    }

    static DragEffectEventArgs() => Update += DragEffectEventArgs_Update;
    private static void DragEffectEventArgs_Update(DragEffectEventArgs e)
        => Effect.Enqueue(e.State, EffectType.Moving, e.Source, e.Target, e.ItemID, e.From, e.To, e.Hue, e.Amount);
}