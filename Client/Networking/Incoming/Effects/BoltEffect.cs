namespace Client.Networking.Incoming.Effects;
public partial class PacketHandlers
{
    public static event PacketEventHandler<BoltEffectEventArgs>? UpdateBoltEffect;
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
    protected static class BoltEffect
    {
        [PacketHandler(0xC0, length: 36, ingame: true)]
        public static void Update(NetState ns, PacketReader pvSrc)
        {
            BoltEffectEventArgs e = new(ns);

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
            UpdateBoltEffect?.Invoke(e);
        }
    }
}