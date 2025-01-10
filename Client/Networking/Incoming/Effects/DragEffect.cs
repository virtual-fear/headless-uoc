namespace Client.Networking.Incoming.Effects;
public partial class PacketHandlers
{
    public static event PacketEventHandler<DragEffectEventArgs> UpdateDragEffect;
    public sealed class DragEffectEventArgs : EventArgs
    {
        public NetState State { get; }
        public DragEffectEventArgs(NetState state) => State = state;
        public int SourceSerial { get; set; }
        public int TargetSerial { get; set; }
        public short ItemID { get; set; }
        public short SourceX { get; set; }
        public short SourceY { get; set; }
        public sbyte SourceZ { get; set; }
        public short TargetX { get; set; }
        public short TargetY { get; set; }
        public sbyte TargetZ { get; set; }
        public short Hue { get; set; }
        public short Amount { get; set; }
    }
    protected static class DragEffect
    {
        [PacketHandler(0x23, length: 26, ingame: true)]
        public static void Update(NetState ns, PacketReader pvSrc)
        {
            DragEffectEventArgs e = new(ns);
            e.ItemID = pvSrc.ReadInt16();
            pvSrc.ReadByte();
            e.Hue = pvSrc.ReadInt16();
            e.Amount = pvSrc.ReadInt16();
            e.SourceSerial = pvSrc.ReadInt32();
            e.SourceX = pvSrc.ReadInt16();
            e.SourceY = pvSrc.ReadInt16();
            e.SourceZ = pvSrc.ReadSByte();
            e.TargetSerial = pvSrc.ReadInt32();
            e.TargetX = pvSrc.ReadInt16();
            e.TargetY = pvSrc.ReadInt16();
            e.TargetZ = pvSrc.ReadSByte();
            UpdateDragEffect?.Invoke(e);
        }
    }
}