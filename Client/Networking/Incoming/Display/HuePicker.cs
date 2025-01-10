namespace Client.Networking.Incoming.Display;
public partial class PacketHandlers
{
    public sealed class HuePickerEventArgs : EventArgs
    {
        public NetState State { get; }
        public HuePickerEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public short ItemID { get; set; }
    }
    public static event PacketEventHandler<HuePickerEventArgs>? DisplayHuePicker;
    protected static class HuePicker
    {
        [PacketHandler(0x95, length: 9, ingame: true)]
        internal static void Update(NetState ns, PacketReader pvSrc)
        {
            HuePickerEventArgs e = new(ns) { Serial = pvSrc.ReadInt32() };
            pvSrc.ReadInt16();
            e.ItemID = pvSrc.ReadInt16();
            DisplayHuePicker?.Invoke(e);
        }
    }
}