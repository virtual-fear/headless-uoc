namespace Client.Networking.Incoming.Mobiles;
public partial class PacketHandlers
{
    public static event PacketEventHandler<SetWarModeEventArgs>? OnSetWarMode;
    public sealed class SetWarModeEventArgs : EventArgs
    {
        public NetState State { get; }
        public SetWarModeEventArgs(NetState state) => State = state;
        public bool Enabled { get; set; }
    }

    protected static class SetWarMode
    {
        [PacketHandler(0x72, length: 5, ingame: true)] // NOTE: Maybe this should be elsewhere?
        public static void Update(NetState ns, PacketReader pvSrc)
        {
            SetWarModeEventArgs e = new(ns);
            e.Enabled = pvSrc.ReadBoolean();
            pvSrc.ReadByte();   //  0x00
            pvSrc.ReadByte();   //  0x32
            pvSrc.ReadByte();   //  0x00
            OnSetWarMode?.Invoke(e);
        }
    }
}