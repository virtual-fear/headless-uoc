namespace Client.Networking.Incoming.Display;
using Client.Game;
public partial class PacketHandlers
{
    public static event PacketEventHandler<GumpEventArgs>? DisplayGump;
    public sealed class GumpEventArgs : EventArgs
    {
        public NetState? State { get; }
        public GumpEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public int TypeID { get; set; }
        public int GumpX { get; set; }
        public int GumpY { get; set; }
        public string? Layout { get; set; }
        public string[]? Text { get; set; }
        public bool Packed { get; set; }
    }
    protected partial class Gump
    {
        [PacketHandler(0xB0, length: -1, ingame: true)]
        public static void Update(NetState ns, PacketReader pvSrc)
        {
            GumpEventArgs e = new(ns);
            e.Serial = pvSrc.ReadInt32();
            e.TypeID = pvSrc.ReadInt32();
            e.GumpX = pvSrc.ReadInt32();
            e.GumpY = pvSrc.ReadInt32();
            e.Layout = pvSrc.ReadString(pvSrc.ReadUInt16());
            string[] text = new string[pvSrc.ReadUInt16()];
            for (int i = 0; i < text.Length; ++i)
            {
                int l;
                string v;

                l = pvSrc.ReadUInt16();
                v = pvSrc.ReadUnicodeString(l);

                text[i] = v;
            }
            e.Text = text;
            DisplayGump?.Invoke(e);    
        }

        [PacketHandler(0xDD, length: -1, ingame: true)]
        public static void UpdatePacked(NetState ns, PacketReader pvSrc)
        {
            GumpEventArgs e = new(ns);
            e.Serial = pvSrc.ReadInt32();
            e.TypeID = pvSrc.ReadInt32();
            e.GumpX = pvSrc.ReadInt32();
            e.GumpY = pvSrc.ReadInt32();
            PacketReader pvComp = Gumps.GetCompressedReader(pvSrc);
            e.Layout = pvComp.ReadString();
            string[] text = new string[pvSrc.ReadInt32()];
            for (int i = 0; i < text.Length; ++i)
            {
                int l;
                string v;

                l = pvSrc.ReadUInt16();
                v = pvSrc.ReadUnicodeString(l);

                text[i] = v;
            }
            e.Text = text;
            e.Packed = true;
            DisplayGump?.Invoke(e);
        }
    }
}