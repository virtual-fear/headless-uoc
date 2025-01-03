using System;
using System.IO;
using System.Reflection;
using Client.Diagnostics;

namespace Client.Networking
{
    [Obsolete]
    public sealed class TicketAuth : Packet
    {
        [Obsolete("Special login system used for UOGamers: Hybrid")]
        private TicketAuth(ulong ticket)
            : base(240)
        {
            base.Stream.Write((byte)0xdd);
            base.Stream.Write((int)(ticket >> 0x20));
            base.Stream.Write((int)ticket);
            byte[] toWrite = null;
            try
            {
                toWrite = Assembly.GetExecutingAssembly().GetName().GetPublicKeyToken();
            }
            catch
            {
            }
            if (toWrite == null)
            {
                toWrite = new byte[0];
            }
            base.Stream.Write(toWrite.Length);
            base.Stream.Write(toWrite, 0, toWrite.Length);
        }
    }

    public class Packet
    {
        public byte ID { get; }
        public bool Fixed { get; }
        public bool Encode { get; set; } = true;
        public Int64 Length => Stream == null ? 0 : Stream.Count;
        public PacketWriter Stream { get; }
        public Packet(byte packetID, int length = -1)
        {
            // 0x00 : packetID
            // 0x01 :   length << 8 (short #2)
            // 0x02 :   length << 0 (short #1)

            ID = packetID;
            if (Fixed = (length == -1))
                length = 32;
            
            Stream = new PacketWriter(length);
            Stream.Write(packetID);
            if (Fixed)
                Stream.Write((short)0);

            Type t = this.GetType();
            PacketSendProfile.Acquire(t).Increment();
        }
        public byte[] Compile()
        {
            if (Fixed)
            {
                Stream.Seek(1L, SeekOrigin.Begin);
                Stream.Write((ushort)Stream.Count);
            }
            Stream.Flush();
            return Stream.Compile();
        }
        public override string ToString() => $"{GetType().Name} (0x{ID:X2}, {Length})";
    }
}
