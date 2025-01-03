using System;

namespace Client.Networking.Incoming
{
    using static PacketSink;
    public partial class PacketSink
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

        public static event PacketEventHandler<CustomizedHouseContentEventArgs> CustomizedHouseContent;
        public static void InvokeCustomizedHouseContent(CustomizedHouseContentEventArgs e) => CustomizedHouseContent?.Invoke(e);
    }
    public static class UpdatedCustomizedHouseContent
    {
        public static void Configure()
        {
            Register(0xD8, -1, true, new OnPacketReceive(Update));
        }
        private static void Update(NetState ns, PacketReader pvSrc)
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

            PacketSink.InvokeCustomizedHouseContent(e);
        }
        static void Register(byte packetID, int length, bool ingame, OnPacketReceive receive) => PacketHandlers.Register(packetID, length, ingame, receive);
    }
}
