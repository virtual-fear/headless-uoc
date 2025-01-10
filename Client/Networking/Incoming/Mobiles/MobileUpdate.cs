namespace Client.Networking.Incoming.Mobiles;

using Client.Game.Data;
public partial class PacketHandlers
{
    public static event PacketEventHandler<MobileUpdateEventArgs>? OnMobileUpdate;
    public sealed class MobileUpdateEventArgs : EventArgs
    {
        public NetState State { get; }
        public MobileUpdateEventArgs(NetState state) => State = state;
        public Serial Serial { get; set; }
        public short Body { get; set; }
        public short Hue { get; set; }
        public byte PacketFlags { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
        public Direction Direction { get; set; }
        public sbyte Z { get; set; }
    }
    protected static class MobileUpdate
    {
        [PacketHandler(0x20, length: 19, ingame: true)]
        public static void Update(NetState ns, PacketReader pvSrc)
        {
            MobileUpdateEventArgs e = new MobileUpdateEventArgs(ns);
            e.Serial = (Serial)pvSrc.ReadUInt32();
            e.Body = (short)pvSrc.ReadUInt16();
            pvSrc.ReadByte();   //  0
            e.Hue = pvSrc.ReadInt16();
            e.PacketFlags = pvSrc.ReadByte();
            e.X = pvSrc.ReadInt16();
            e.Y = pvSrc.ReadInt16();
            pvSrc.ReadInt16();  //  0
            e.Direction = (Direction)pvSrc.ReadByte();
            e.Z = pvSrc.ReadSByte();
            //ns.Send(Warmode.Instantiate(false));
            OnMobileUpdate?.Invoke(e);
        }
    }
}