namespace Client.Networking.Arguments;
using Client.Game;
public sealed class HuePickerEventArgs : EventArgs
{
    [PacketHandler(0x95, length: 9, ingame: true)]
    private static event PacketEventHandler<HuePickerEventArgs> Update;
    public NetState State { get; }
    public int Serial { get; }
    public short ItemID { get; }
    internal HuePickerEventArgs(NetState state, PacketReader pvSrc)
    {
        State = state;
        Serial = pvSrc.ReadInt32();
        pvSrc.ReadInt16();
        ItemID = pvSrc.ReadInt16();
    }
    static HuePickerEventArgs() => Update += HuePickerEventArgs_Update;
    private static void HuePickerEventArgs_Update(HuePickerEventArgs e)
        => Display.ShowHuePicker(e.State, e.Serial, e.ItemID);
}