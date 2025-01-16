using EffectType = Client.Game.Data.EffectType;
using Serial = Client.Game.Data.Serial;
namespace Client.Networking.Incoming;
public sealed class HuedEffectEventArgs : EventArgs
{
    public NetState State { get; }
    public EffectType Type { get; }
    public Serial From { get; }
    public Serial To { get; }
    public short ItemID { get; }
    public short SourceX { get; }
    public short SourceY { get; }
    public sbyte SourceZ { get; }
    public short DestX { get; }
    public short DestY { get; }
    public sbyte DestZ { get; }
    public byte Speed { get; }
    public byte Duration { get; }
    public bool IsFixedDirection { get; }
    public bool IsExploding { get; }
    public int Hue { get; }
    public int RenderMode { get; }
    internal HuedEffectEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Type = (EffectType)ip.ReadByte();
        From = (Serial)ip.ReadUInt32();
        To = (Serial)ip.ReadUInt32();
        ItemID = ip.ReadInt16();
        SourceX = ip.ReadInt16();
        SourceY = ip.ReadInt16();
        SourceZ = ip.ReadSByte();
        DestX = ip.ReadInt16();
        DestY = ip.ReadInt16();
        DestZ = ip.ReadSByte();
        Speed = ip.ReadByte();
        Duration = ip.ReadByte();
        ip.ReadByte(); // 0
        ip.ReadByte(); // 0
        IsFixedDirection = ip.ReadBoolean();
        IsExploding = ip.ReadBoolean();
        Hue = ip.ReadInt32();
        RenderMode = ip.ReadInt32();
    }
}