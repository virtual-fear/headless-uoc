using System;

namespace Client.Networking.Incoming
{
    using static PacketSink;
    public partial class PacketSink
    {
        public class CloseGumpEventArgs : EventArgs
        {
            public NetState State { get; }
            public CloseGumpEventArgs(NetState state) => State = state;
            public int TypeID { get; set; }
            public int ButtonID { get; set; }
        }
        public static event PacketEventHandler<CloseGumpEventArgs> CloseGump;
        public static void InvokeCloseGump(CloseGumpEventArgs e) => CloseGump?.Invoke(e);
    }
    
    public static class UpdatedGump
    {
        public static void Configure()
        {
            RegisterExtended(0x04, 13, true, new OnPacketReceive(CloseGump));

        }
        private static void CloseGump(NetState ns, PacketReader pvSrc)
        {
            CloseGumpEventArgs e = new CloseGumpEventArgs(ns);

            e.TypeID = pvSrc.ReadInt32();
            e.ButtonID = pvSrc.ReadInt32();

            PacketSink.InvokeCloseGump(e);
        }
        static void RegisterExtended(byte packetID, int length, bool variable, OnPacketReceive onReceive) => PacketHandlers.RegisterExtended(packetID, length, variable, onReceive);
    }
}
