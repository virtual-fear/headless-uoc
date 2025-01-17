namespace Client.Game;
using Client.Game.Compression;
using Client.Networking;
using Client.Networking.Arguments;
public class Gump
{
    public const bool MoongateConfirmation = false;
    private static readonly string[] m_Strings = new string[] {
        "Dost thou wish to step into the moongate? Continue to enter the gate, Cancel to stay here",
    };
    #region Networking Events

    [PacketHandler(0x04, length: 13, ingame: true, extCmd: true)]
    public static event PacketEventHandler<ClosedGumpEventArgs>? OnClose;

    [PacketHandler(0xB0, length: -1, ingame: true)]
    public static event PacketEventHandler<DisplayGumpEventArgs>? OnDisplayGump;

    [PacketHandler(0xDD, length: -1, ingame: true)]
    public static event PacketEventHandler<DisplayPackedGumpEventArgs>? OnDisplayPackedGump;

    #endregion

    private static byte[]? _compressedBuffer;
    internal static PacketReader GetCompressedReader(PacketReader pvSrc)
    {
        _compressedBuffer ??= new byte[0x1000]; //  4096
        int compressedLength = pvSrc.ReadInt32();
        if (compressedLength == 0)
            return new PacketReader(
                   buffer: _compressedBuffer.AsSpan(),
                fixedSize: false,
                      cmd: 0x00,
                     name: "Gump Subset");

        int decompressedLength = pvSrc.ReadInt32();
        if (decompressedLength == 0)
            return new PacketReader(
                 buffer: _compressedBuffer.AsSpan(start: 0, length: 3),
              fixedSize: false,
                    cmd: 0x00,
                   name: "Gump Subset");

        byte[] buffer = pvSrc.ReadBytes(compressedLength - 4);
        if (decompressedLength > _compressedBuffer.Length)
            _compressedBuffer = new byte[decompressedLength + 0xFFFF & -4096]; // 4095
        ZLib.Unpack(_compressedBuffer, ref decompressedLength, buffer, buffer.Length);
        return new PacketReader(
            buffer: _compressedBuffer.AsSpan(start: 0, length: decompressedLength),
            fixedSize: true,
            cmd: 0x00,
            name: "Gump Subset");
    }

    [Obsolete]
    public static void Handle(int serial, int dialog, int xOffset, int yOffset, string layout, string[] text)
    {
        TextWriter output = Console.Out;

        if (text.Length > 0 && text[0] == m_Strings[0] && MoongateConfirmation)
        {
            //if (World.State != null)
            //{
            //    World.State.Send(GumpButton.Instantiate(serial, dialog, 1));
            //}
            throw new NotImplementedException();
        }
        else
        {
            output.WriteLine("HandleGump serial:{0} dialog:{1}", serial, dialog);

            for (int i = 0; i < text.Length; i++)
                output.WriteLine("{0}. {1}", i, text[i]);

            output.WriteLine();
            //GServerGump.GetCachedLocation( dialog, ref xOffset, ref yOffset );
            //GServerGump toAdd = new GServerGump( serial, dialog, xOffset, yOffset, layout, text );
            //Gumps.Desktop.Children.Add( toAdd );
        }
    }
}