namespace Client.Networking.Incoming.Effects;
using Client.Game.Data;
public partial class PacketHandlers
{
    public static event PacketEventHandler<HuedEffectEventArgs>? Effects_OnHuedEffect;
    public sealed class HuedEffectEventArgs : EventArgs
    {
        public NetState State { get; }
        public EffectType Type { get; }
        public Serial From { get; }
        public Serial To { get; }
        public short ItemID { get; }
        public short SourceX { get; }
        public short SourceY { get; }
        public sbyte SourceZ { get; }
        public short DestX { get; }
        public short DestY { get; }
        public sbyte DestZ { get; }
        public byte Speed { get; }
        public byte Duration { get; }
        public bool IsFixedDirection { get; }
        public bool IsExploding { get; }
        public int Hue { get; }
        public int RenderMode { get; }
        public HuedEffectEventArgs(NetState state, PacketReader pvSrc)
        {
            State = state;
            Type = (EffectType)pvSrc.ReadByte();
            From = (Serial)pvSrc.ReadUInt32();
            To = (Serial)pvSrc.ReadUInt32();
            ItemID = pvSrc.ReadInt16();
            SourceX = pvSrc.ReadInt16();
            SourceY = pvSrc.ReadInt16();
            SourceZ = pvSrc.ReadSByte();
            DestX = pvSrc.ReadInt16();
            DestY = pvSrc.ReadInt16();
            DestZ = pvSrc.ReadSByte();
            Speed = pvSrc.ReadByte();
            Duration = pvSrc.ReadByte();
            pvSrc.ReadByte(); // 0
            pvSrc.ReadByte(); // 0
            IsFixedDirection = pvSrc.ReadBoolean();
            IsExploding = pvSrc.ReadBoolean();
            Hue = pvSrc.ReadInt32();
            RenderMode = pvSrc.ReadInt32();
        }
    }
    protected static class HuedEffect
    {
        [PacketHandler(0xC0, length: 36, ingame: true)]
        public static void Update(NetState ns, PacketReader pvSrc) => Effects_OnHuedEffect?.Invoke(new(ns, pvSrc));
    }
}