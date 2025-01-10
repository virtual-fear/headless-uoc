namespace Client.Networking.Incoming;
public partial class PacketHandlers
{
    public sealed class CustomizedHouseContentEventArgs : EventArgs
    {
        public NetState State { get; }
        public bool Supported { get; }
        public CustomizedHouseContentEventArgs(NetState state, bool supported)
        {
            State = state;
            Supported = supported;
        }
        public int CompressionType { get; set; }
        public int Serial { get; set; }
        public int Revision { get; set; }
        public byte[] Buffer { get; set; }
    }

    public static event PacketEventHandler<CustomizedHouseContentEventArgs>? CustomizedHouseContent;
    protected partial class Content
    {
        [PacketHandler(0xD8, length: -1, ingame: true)]
        internal static void CustomizedHouse(NetState ns, PacketReader pvSrc)
        {
            CustomizedHouseContentEventArgs e = new CustomizedHouseContentEventArgs(ns, false);

            e.CompressionType = pvSrc.ReadByte();

            pvSrc.ReadByte();

            e.Serial = pvSrc.ReadInt32();
            e.Revision = pvSrc.ReadInt32();

            pvSrc.ReadUInt16();

            int length = pvSrc.ReadUInt16();
            byte[] buffer = pvSrc.ReadBytes(length);

            e.Buffer = buffer;

            //Item item = World.FindItem(serial);
            //if (((item != null) && (item.Multi != null)) && item.IsMulti)
            //{
            //    CustomMultiLoader.SetCustomMulti(serial, revision, item.Multi, compressionType, buffer);
            //}

            CustomizedHouseContent?.Invoke(e);
        }
    }
}