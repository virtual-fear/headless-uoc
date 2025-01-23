namespace Client.Networking.Arguments;
using Client.Game;
public sealed class CreateWorldEntityEventArgs
{
    [PacketHandler(0xF3, length: 26, ingame: true)]
    public static event PacketEventHandler<CreateWorldEntityEventArgs>? Update;
    public NetState State { get; }
    public int Type { get; }
    public int GraphicsID { get; }
    public ushort Amount { get; }
    public ushort Hue { get; }
    public byte Light { get; }
    public int Flags { get; }
    public ushort ItemID { get; }
    private CreateWorldEntityEventArgs(NetState state, PacketReader pvSrc, bool isHS = false)
    {
        State = state;
        if (pvSrc.ReadInt16() != 0x1)
        {
            pvSrc.Trace();
            return;
        }
        Type = pvSrc.ReadInt32();
        GraphicsID = pvSrc.ReadInt32(); // Body or ItemID graphic
        Amount = (ushort)pvSrc.ReadInt32();
        Hue = (ushort)pvSrc.ReadInt32();
        Light = pvSrc.ReadByte();
        Flags = pvSrc.ReadInt32();
        ItemID = Type switch { 0 or 1 or 2 => (ushort)(GraphicsID & (isHS ? 0xFFFF : 0x7FFF)), _ => 0 };
    }

    static CreateWorldEntityEventArgs() => Update += CreateWorldEntityEventArgs_Update;
    private static void CreateWorldEntityEventArgs_Update(CreateWorldEntityEventArgs e) => World.CreateEntity(e);
}