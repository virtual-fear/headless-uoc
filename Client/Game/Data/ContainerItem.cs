namespace Client.Game.Data;
using Client.Networking;
public sealed class ContainerItem
{
    public NetState State { get; }
    public ContainerItem(NetState state) => State = state;
    public int Serial { get; set; } // Child item serial
    public ushort ID { get; set; }
    public ushort Amount { get; set; }
    public short X { get; set; }
    public short Y { get; set; }
    public int Parent { get; set; } // Beheld item serial
    public ushort Hue { get; set; }
}
