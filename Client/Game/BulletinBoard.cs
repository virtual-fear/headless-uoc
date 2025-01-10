namespace Client.Game;
using System.Text;
using Client.Game.Context;
using Client.Game.Data;
using Client.Game.Data.BulletinBoard;
using Client.Networking;
public partial class PacketHandlers
{
    public static event PacketEventHandler<BulletinBoardEventArgs>? UpdateBulletinBoard;
    public sealed class BulletinBoardEventArgs : EventArgs
    {
        public NetState State { get; }
        public bool Supported { get; }
        public BulletinBoardEventArgs(NetState state, bool supported)
        {
            State = state;
            Supported = supported;
        }
        public BulletinBoardType? Type { get; set; }
        public ItemContext? Item { get; set; }
        public BulletinBoardHeader? Header { get; set; }
        public BulletinBoardBody? Body { get; set; }
    }
    protected static class BulletinBoard
    {
        [PacketHandler(0x71, length: -1, ingame: true)]
        internal static void Update(NetState ns, PacketReader pvSrc)
        {
            BulletinBoardEventArgs e = new BulletinBoardEventArgs(ns, false);
            BulletinBoardType type = (BulletinBoardType)pvSrc.ReadByte();
            var boardReader = delegate (PacketReader reader)
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

            ItemContext? board, temp, thread;
            Serial serial;
            Action act = delegate
            {
                board = ItemContext.Acquire((Serial)pvSrc.ReadUInt32());
                if (board == null)
                    return;

                temp = ItemContext.Acquire((Serial)pvSrc.ReadUInt32());
                if (temp == null)
                    return;

                thread = null;
                serial = (Serial)pvSrc.ReadUInt32();
                if (serial >= World.ItemOffset)
                    thread = ItemContext.Acquire(serial);

                string p, s, t;

                //  p   :   poster
                //  s   :   subject
                //  t   :   time

                p = boardReader.Invoke(pvSrc);
                s = boardReader.Invoke(pvSrc);
                t = boardReader.Invoke(pvSrc);

                e.Header = new BulletinBoardHeader(board, thread, p, s, t);
            };

            e.Type = type;
            switch (type)
            {
                case BulletinBoardType.Display:
                    e.Item = ItemContext.Acquire((Serial)pvSrc.ReadUInt32());
                    break;
                case BulletinBoardType.SetBody:
                    act.Invoke();
                    short body = pvSrc.ReadInt16();
                    short hue = pvSrc.ReadInt16();
                    BulletinBoardItem[] items = new BulletinBoardItem[pvSrc.ReadByte()];
                    for (int i = 0; i < items.Length; ++i)
                    {
                        items[i].ItemID = pvSrc.ReadUInt16();
                        items[i].Hue = pvSrc.ReadUInt16();
                    }

                    string[] lines = new string[pvSrc.ReadByte()];
                    for (int i = 0; i < lines.Length; ++i)
                        lines[i] = boardReader.Invoke(pvSrc);
                    e.Body = new BulletinBoardBody(new BulletinBoardAppearance(body, hue, items), lines);
                    break;
                case BulletinBoardType.SetHeader:
                    act.Invoke();
                    break;
                default:
                    pvSrc.Trace();
                    return;
            }
            UpdateBulletinBoard?.Invoke(e);
        }
    }
}