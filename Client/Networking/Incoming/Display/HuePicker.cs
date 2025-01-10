namespace Client.Networking.Incoming;
public sealed class HuePickerEventArgs : EventArgs
    {
        public NetState State { get; }
        public HuePickerEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public short ItemID { get; set; }
    }
public partial class Display
{
    public static event PacketEventHandler<HuePickerEventArgs>? UpdateHuePicker;

    [PacketHandler(0x95, length: 9, ingame: true)]
    protected static void ReceivedDisplay_HuePicker(NetState ns, PacketReader pvSrc)
    {
        HuePickerEventArgs e = new(ns) { Serial = pvSrc.ReadInt32() };
        pvSrc.ReadInt16();
        e.ItemID = pvSrc.ReadInt16();
        UpdateHuePicker?.Invoke(e);
    }
}