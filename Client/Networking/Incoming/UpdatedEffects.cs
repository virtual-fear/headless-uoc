using System;

namespace Client.Networking.Incoming
{
    using static PacketSink;
    public partial class PacketSink
    {
        #region EventArgs

        public sealed class BoltEffectEventArgs : EventArgs
        {
            public NetState State { get; }
            public BoltEffectEventArgs(NetState state) => State = state;
            public int Serial { get; set; }
            public short X { get; set; }
            public short Y { get; set; }
            public sbyte Z { get; set; }
            public int Hue { get; set; }
        }
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
        public sealed class ScreenEffectEventArgs : EventArgs
        {
            public NetState State { get; }
            public ScreenEffectEventArgs(NetState state) => State = state;
            public ScreenEffectType Type { get; set; }
        }

        #endregion (done)

        public static event PacketEventHandler<BoltEffectEventArgs> BoltEffect;
        public static event PacketEventHandler<DragEffectEventArgs> DragEffect;
        public static event PacketEventHandler<ParticleEffectEventArgs> ParticleEffect;
        public static event PacketEventHandler<ScreenEffectEventArgs> ScreenEffect;
        public static void InvokeScreenEffect(ScreenEffectEventArgs e) => ScreenEffect?.Invoke(e);
        public static void InvokeParticleEffect(ParticleEffectEventArgs e) => ParticleEffect?.Invoke(e);
        public static void InvokeDragEffect(DragEffectEventArgs e) => DragEffect?.Invoke(e);
        public static void InvokeBoltEffect(BoltEffectEventArgs e) => BoltEffect?.Invoke(e);

    }

    public enum EffectType
    {
        Moving = 0x00,
        Lightning = 0x01,
        FixedXYZ = 0x02,
        FixedFrom = 0x03
    }
    public enum ScreenEffectType
    {
        FadeOut = 0x00,
        FadeIn = 0x01,
        LightFlash = 0x02,
        FadeInOut = 0x03,
        DarkFlash = 0x04
    }

    public static class UpdatedEffects
    {
        public static void Configure()
        {
            Register(0xC0, 36, true, new OnPacketReceive(BoltEffect));
            Register(0x23, 26, true, new OnPacketReceive(DragEffect));
            Register(0xC7, 49, true, new OnPacketReceive(ParticleEffect));
            Register(0x70, 28, true, new OnPacketReceive(ScreenEffect));
        }

        private static void ScreenEffect(NetState ns, PacketReader pvSrc)
        {
            ScreenEffectEventArgs e = new ScreenEffectEventArgs(ns);

            if (pvSrc.ReadByte() != 0x04)
                pvSrc.Trace();

            pvSrc.ReadBytes(8);

            e.Type = (ScreenEffectType)pvSrc.ReadInt16();

            pvSrc.ReadBytes(16);

            PacketSink.InvokeScreenEffect(e);
        }
        private static void ParticleEffect(NetState ns, PacketReader pvSrc)
        {
            ParticleEffectEventArgs e = new ParticleEffectEventArgs(ns);
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
            PacketSink.InvokeParticleEffect(e);
        }
        private static void DragEffect(NetState ns, PacketReader pvSrc)
        {
            DragEffectEventArgs e = new DragEffectEventArgs(ns);

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

            PacketSink.InvokeDragEffect(e);
        }
        private static void BoltEffect(NetState ns, PacketReader pvSrc)
        {
            BoltEffectEventArgs e = new BoltEffectEventArgs(ns);

            pvSrc.ReadByte();   //  0x01   :   type

            e.Serial = pvSrc.ReadInt32();

            pvSrc.ReadInt32();  //  Serial.Zero
            pvSrc.ReadInt16();  //  0x00    :   itemID

            e.X = pvSrc.ReadInt16();
            e.Y = pvSrc.ReadInt16();
            e.Z = pvSrc.ReadSByte();

            pvSrc.ReadInt16();  //  e.X copy
            pvSrc.ReadInt16();  //  e.Y copy
            pvSrc.ReadSByte();  //  e.Z copy

            pvSrc.ReadByte();   //  0    :   speed
            pvSrc.ReadByte();   //  0    :   duration
            pvSrc.ReadInt16();  //  0    :   unk   

            pvSrc.ReadBoolean();    //  F   :   fixed direction
            pvSrc.ReadBoolean();    //  F   :   explode

            e.Hue = pvSrc.ReadInt32();

            pvSrc.ReadInt32();  //  0   :   render mode

            PacketSink.InvokeBoltEffect(e);
        }
        private static void Register(byte packetID, int length, bool variable, OnPacketReceive onReceive) => PacketHandlers.Register(packetID, length, variable, onReceive);
    }
}
