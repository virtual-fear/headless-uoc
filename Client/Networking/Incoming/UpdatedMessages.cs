using System;
using System.IO;

namespace Client.Networking.Incoming
{
    using static PacketSink;
    public partial class PacketSink
    {
        #region EventArgs
        
        public sealed class AsciiMessageEventArgs : EventArgs
        {
            public NetState State { get; }
            public AsciiMessageEventArgs(NetState state) => State = state;
            public int Serial { get; set; }
            public short Graphic { get; set; }
            public byte MessageType { get; set; }
            public short Hue { get; set; }
            public short Font { get; set; }
            public string Name { get; set; }
            public string Text { get; set; }
        }
        public sealed class UnicodeMessageEventArgs : EventArgs
        {
            public NetState State { get; }
            public UnicodeMessageEventArgs(NetState state) => State = state;
            public int Serial { get; set; }
            public short Graphic { get; set; }
            public byte MessageType { get; set; }
            public short Hue { get; set; }
            public short Font { get; set; }
            public string Language { get; set; }
            public string Name { get; set; }
            public string Text { get; set; }
        }
        public sealed class LocalizedMessageEventArgs : EventArgs
        {
            public NetState State { get; }
            public LocalizedMessageEventArgs(NetState state) => State = state;
            public int Serial { get; set; }
            public short Graphic { get; set; }
            public byte MessageType { get; set; }
            public short Hue { get; set; }
            public short Font { get; set; }
            public string Name { get; set; }
            public string Text { get; set; }
        }
        public sealed class LocalizedMessageAffixEventArgs : EventArgs
        {
            public NetState State { get; }
            public LocalizedMessageAffixEventArgs(NetState state) => State = state;
            public int Serial { get; set; }
            public short Graphic { get; set; }
            public byte MessageType { get; set; }
            public short Hue { get; set; }
            public short Font { get; set; }
            public byte AffixType { get; set; }
            public string Name { get; set; }
            public string Text { get; set; }
            public string Arguments { get; set; }
        }
        public sealed class ScrollMessageEventArgs : EventArgs
        {
            public NetState State { get; }
            public ScrollMessageEventArgs(NetState state) => State = state;
            public byte Type { get; set; }
            public int Tip { get; set; }
            public string Text { get; set; }
        }
        
        #endregion

        public static event PacketEventHandler<AsciiMessageEventArgs> AsciiMessage;
        public static event PacketEventHandler<UnicodeMessageEventArgs> UnicodeMessage;
        public static event PacketEventHandler<LocalizedMessageEventArgs> LocalizedMessage;
        public static event PacketEventHandler<LocalizedMessageAffixEventArgs> LocalizedMessageAffix;
        public static event PacketEventHandler<ScrollMessageEventArgs> ScrollMessage;

        public static void InvokeScrollMessage(ScrollMessageEventArgs e) => ScrollMessage?.Invoke(e);
        public static void InvokeLocalizedMessageAffix(LocalizedMessageAffixEventArgs e) => LocalizedMessageAffix?.Invoke(e);
        public static void InvokeLocalizedMessage(LocalizedMessageEventArgs e) => LocalizedMessage?.Invoke(e);
        public static void InvokeUnicodeMessage(UnicodeMessageEventArgs e) => UnicodeMessage?.Invoke(e);
        public static void InvokeAsciiMessage(AsciiMessageEventArgs e) => AsciiMessage?.Invoke(e);
    }
    public static class UpdatedMessages
    {
        public static void Configure()
        {
            Register(0x1C, -1, true, new OnPacketReceive(AsciiMessage));
            Register(0xAE, -1, true, new OnPacketReceive(UnicodeMessage));
            Register(0xC1, -1, true, new OnPacketReceive(LocalizedMessage));
            Register(0xCC, -1, true, new OnPacketReceive(LocalizedMessageAffix));
            Register(0xA6, -1, true, new OnPacketReceive(ScrollMessage));
        }
        private static void ScrollMessage(NetState ns, PacketReader pvSrc)
        {
            ScrollMessageEventArgs e = new ScrollMessageEventArgs(ns);

            e.Type = pvSrc.ReadByte();
            e.Tip = pvSrc.ReadInt32();

            e.Text = pvSrc.ReadString(pvSrc.ReadUInt16());

            PacketSink.InvokeScrollMessage(e);
        }
        private static void LocalizedMessageAffix(NetState ns, PacketReader pvSrc)
        {
            LocalizedMessageAffixEventArgs e = new LocalizedMessageAffixEventArgs(ns);

            e.Serial = pvSrc.ReadInt32();
            e.Graphic = pvSrc.ReadInt16();
            e.MessageType = pvSrc.ReadByte();
            e.Hue = pvSrc.ReadInt16();
            e.Font = pvSrc.ReadInt16();

            pvSrc.ReadInt32();  //  e.Number

            //e.Lang = Localization.GetString(pvSrc.ReadInt32());

            e.AffixType = pvSrc.ReadByte();
            e.Name = pvSrc.ReadString(30);
            e.Text = pvSrc.ReadString();
            e.Arguments = pvSrc.ReadUnicodeString();

            PacketSink.InvokeLocalizedMessageAffix(e);
        }
        private static void LocalizedMessage(NetState ns, PacketReader pvSrc)
        {
            LocalizedMessageEventArgs e = new LocalizedMessageEventArgs(ns);

            e.Serial = pvSrc.ReadInt32();
            e.Graphic = pvSrc.ReadInt16();
            e.MessageType = pvSrc.ReadByte();
            e.Hue = pvSrc.ReadInt16();
            e.Font = pvSrc.ReadInt16();

            pvSrc.ReadInt32();  //  e.Number

            e.Name = pvSrc.ReadString(30);
            e.Text = pvSrc.ReadUnicodeStringLE();

            PacketSink.InvokeLocalizedMessage(e);
        }
        private static void UnicodeMessage(NetState ns, PacketReader pvSrc)
        {
            UnicodeMessageEventArgs e = new UnicodeMessageEventArgs(ns);

            pvSrc.Trace(true);
            pvSrc.Seek(0, SeekOrigin.Begin);

            e.Serial = pvSrc.ReadInt32();
            e.Graphic = pvSrc.ReadInt16();
            e.MessageType = pvSrc.ReadByte();
            e.Hue = pvSrc.ReadInt16();
            e.Font = pvSrc.ReadInt16();
            e.Language = pvSrc.ReadString(4);
            e.Name = pvSrc.ReadString(30);
            e.Text = pvSrc.ReadUnicodeString();

            PacketSink.InvokeUnicodeMessage(e);
        }
        private static void AsciiMessage(NetState ns, PacketReader pvSrc)
        {
            AsciiMessageEventArgs e = new AsciiMessageEventArgs(ns);

            e.Serial = pvSrc.ReadInt32();
            e.Graphic = pvSrc.ReadInt16();
            e.MessageType = pvSrc.ReadByte();
            e.Hue = pvSrc.ReadInt16();
            e.Font = pvSrc.ReadInt16();
            e.Name = pvSrc.ReadString(30); // AsciiFixed
            e.Text = pvSrc.ReadString(); // AsciiNull

            PacketSink.InvokeAsciiMessage(e);
        }
        private static void Register(byte id, int length, bool variable, OnPacketReceive onReceive)
        {
            PacketHandlers.Register(id, length, variable, onReceive);
        }
    }
}
