namespace Client.Networking.Incoming;
public sealed class GlobalLightEventArgs : EventArgs
{
    public NetState State { get; }
    public GlobalLightEventArgs(NetState state) => State = state;
    public sbyte Level { get; set; }
}
public partial class World
{
    public static event PacketEventHandler<GlobalLightEventArgs>? OnGlobalChange;
    public partial class Lighting
    {
        [PacketHandler(0x4F, length: 2, ingame: true)]
        protected static void Received_GlobalLight(NetState ns, PacketReader pvSrc)
        {
            GlobalLightEventArgs e = new(ns);
            e.Level = pvSrc.ReadSByte();
            OnGlobalChange?.Invoke(e);
        }
    }
}