namespace Client.Networking.Arguments;
using BulletinBoardType = Game.Data.BulletinBoardType;
using BulletinBoardBody = Game.Data.BulletinBoardBody;
using BulletinBoardHeader = Game.Data.BulletinBoardHeader;
using ItemContext = Game.Context.ItemContext;
using System.Text;
using Client.Game.Data;
using Client.Game;

public sealed class BulletinBoardEventArgs : EventArgs
{
    public NetState State { get; }
    public bool Supported { get; } = false;
    public BulletinBoardEventArgs(NetState state, PacketReader pvSrc)
    {
        State = state;
        Type = (BulletinBoardType)pvSrc.ReadByte();

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

            Header = new BulletinBoardHeader(board, thread, p, s, t);
        };

        switch (Type)
        {
            case BulletinBoardType.Display:
                Item = ItemContext.Acquire((Serial)pvSrc.ReadUInt32());
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
                Body = new BulletinBoardBody(new BulletinBoardAppearance(body, hue, items), lines);
                break;
            case BulletinBoardType.SetHeader:
                act.Invoke();
                break;
            default:
                pvSrc.Trace();
                return;
        }
    }
    public BulletinBoardType? Type { get; set; }
    public ItemContext? Item { get; set; }
    public BulletinBoardHeader? Header { get; set; }
    public BulletinBoardBody? Body { get; set; }
}
