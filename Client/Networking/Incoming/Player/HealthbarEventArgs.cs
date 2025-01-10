namespace Client.Networking.Incoming;
using Client.Game.Data;
public sealed class HealthbarEventArgs : EventArgs
{
    public NetState State { get; }
    public HealthbarEventArgs(NetState state) => State = state;
    public int Serial { get; set; }
    public HealthbarType Type { get; set; }
    public byte Level { get; set; }
}
public partial class Player
{
    public static event PacketEventHandler<HealthbarEventArgs>? UpdateHealthbar;

    [PacketHandler(0x17, length: 12, ingame: true)]
    protected static void Received_Healthbar(NetState ns, PacketReader pvSrc)
    {
        HealthbarEventArgs e = new(ns);
        e.Serial = pvSrc.ReadInt32();
        pvSrc.Seek(2, SeekOrigin.Current);
        e.Type = (HealthbarType)pvSrc.ReadInt16();
        e.Level = pvSrc.ReadByte();
        UpdateHealthbar?.Invoke(e);
    }
}
