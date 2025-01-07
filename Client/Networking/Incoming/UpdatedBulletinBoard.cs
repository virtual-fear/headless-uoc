using System.Text;
using Client.Game;

namespace Client.Networking.Incoming;

using static PacketSink;
public partial class PacketSink
{
    public sealed class BulletinBoardEventArgs : EventArgs
    {
        public NetState State { get; }
        public BulletinBoardEventArgs(NetState state, bool supported)
        {
            State = state;
            Supported = supported;
        }
        public bool Supported { get; }
        public BulletinBoardType Type { get; set; }
        public Item Item { get; set; }
        public BulletinBoardHeader Header { get; set; }
        public BulletinBoardBody Body { get; set; }
    }
     
    public static event PacketEventHandler<BulletinBoardEventArgs>? BulletinBoard;
    public static void InvokeBulletinBoard(BulletinBoardEventArgs e) => BulletinBoard?.Invoke(e);
}

// TODO: Move this data types into the Client.Game namespace

public enum BulletinBoardType
{
    Display = 0,
    SetHeader = 1,
    SetBody = 2,
}

public struct BulletinBoardItem
{
    public int ItemID { get; set; }
    public int Hue { get; set; }
    public BulletinBoardItem(int itemID, int hue)
    {
        ItemID = itemID;
        Hue = hue;
    }
}

public sealed class BulletinBoardAppearance
{
    public int Body { get; }
    public int Hue { get; }
    public BulletinBoardItem[] Items { get; }
    public BulletinBoardAppearance(int body, int hue, params BulletinBoardItem[] items)
    {
        Body = body;
        Hue = hue;
        Items = items;
    }
}

public sealed class BulletinBoardBody
{
    public BulletinBoardAppearance Appearance { get; }
    public string[] Lines { get; }
    public BulletinBoardBody(BulletinBoardAppearance appearance, string[] lines)
    {
        Appearance = appearance;
        Lines = lines;
    }
}

public sealed class BulletinBoardHeader
{
    public Item Board { get; }
    public Item Thread { get; }
    public string Poster { get; }
    public string Subject { get; }
    public string Time { get; }
    public BulletinBoardHeader(Item board, Item thread, string poster, string subject, string time)
    {
        Board = board;
        Thread = thread;
        Poster = poster;
        Subject = subject;
        Time = time;
    }
}

public static class UpdatedBulletinBoard
{
    private delegate string BoardReader(PacketReader pvSrc);
    public static void Configure() => Register(0x71, -1, true, new OnPacketReceive(Update));
    private static void Update(NetState ns, PacketReader pvSrc)
    {
        BulletinBoardEventArgs e = new BulletinBoardEventArgs(ns, false);
        BulletinBoardType type = (BulletinBoardType)pvSrc.ReadByte();
        BoardReader str = delegate (PacketReader reader)
        {
            int length = pvSrc.ReadByte();
            byte[] buffer = pvSrc.ReadBytes(length);
            for (int i = 0; i < buffer.Length; ++i)
            {
                if (buffer[i] == 0x00)
                {
                    length = i;
                    break;
                }
            }
            return Encoding.UTF8.GetString(buffer, 0, length);
        };

        Item board, temp, thread;
        Int32 serial;
        Action act = delegate
        {
            board = Item.Acquire(pvSrc.ReadInt32());
            if (board == null)
                return;

            temp = Item.Acquire(pvSrc.ReadInt32());
            if (temp == null)
                return;

            thread = null;
            serial = pvSrc.ReadInt32();
            if (serial >= 0x4000000)
            {
                thread = Item.Acquire(serial);
            }

            string p, s, t;

            //  p   :   poster
            //  s   :   subject
            //  t   :   time

            p = str.Invoke(pvSrc);
            s = str.Invoke(pvSrc);
            t = str.Invoke(pvSrc);

            e.Header = new BulletinBoardHeader(board, thread, p, s, t);
        };

        e.Type = type;
        switch (type)
        {
            case BulletinBoardType.Display:
                e.Item = Item.Acquire(pvSrc.ReadInt32());
                break;
            case BulletinBoardType.SetBody:
                act.Invoke();

                short b, h;

                //  b   :   body
                //  h   :   hue

                b = pvSrc.ReadInt16();
                h = pvSrc.ReadInt16();

                BulletinBoardItem[] items = new BulletinBoardItem[pvSrc.ReadByte()];
                for (int i = 0; i < items.Length; ++i)
                {
                    items[i].ItemID = pvSrc.ReadUInt16();
                    items[i].Hue = pvSrc.ReadUInt16();
                }

                string[] lines = new string[pvSrc.ReadByte()];
                for (int i = 0; i < lines.Length; ++i)
                {
                    lines[i] = str.Invoke(pvSrc);
                }

                e.Body = new BulletinBoardBody(new BulletinBoardAppearance(b, h, items), lines);
                break;
            case BulletinBoardType.SetHeader:
                act.Invoke();
                break;
            default:
                pvSrc.Trace();
                return;
        }
        PacketSink.InvokeBulletinBoard(e);
    }
    private static void Register(byte packetID, int length, bool variable, OnPacketReceive handler) => PacketHandlers.Register(packetID, length, variable, handler);
}
