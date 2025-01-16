using Client.Game.Compression;

namespace Client.Networking.Incoming;
public partial class Gumps
{
    public static event PacketEventHandler<ClosedGumpEventArgs>? OnClose;
    public static event PacketEventHandler<DisplayGumpEventArgs>? OnDisplay;
    [PacketHandler(0xB0, length: -1, ingame: true)]
    protected static void ReceivedGump_Display(NetState ns, PacketReader ip) => OnDisplay?.Invoke(new(ns, ip));

    [PacketHandler(0xDD, length: -1, ingame: true)]
    protected static void ReceivedGump_PackedDisplay(NetState ns, PacketReader ip) => OnDisplay?.Invoke(new(ns, ip, packed: true));

    [PacketHandler(0x04, length: 13, ingame: true, extCmd: true)]
    protected static void ReceivedGump_Close(NetState ns, PacketReader ip) => OnClose?.Invoke(new(ns, ip));

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
}
