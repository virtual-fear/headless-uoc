namespace Client.Networking.Incoming;
public partial class Display
{
    public static event PacketEventHandler<DisplayContextMenuEventArgs>? UpdateContextMenu;
    public static event PacketEventHandler<DisplayEquipInfoEventArgs>? UpdateEquipmentInfo;
    public static event PacketEventHandler<DisplayProfileEventArgs>? UpdateProfile;
    public static event PacketEventHandler<HuePickerEventArgs>? UpdateHuePicker;
    public static event PacketEventHandler<PaperdollEventArgs>? UpdatePaperdoll;
    public static event PacketEventHandler<QuestionMenuEventArgs>? UpdateQuestionMenu;

    [PacketHandler(0x7C, length: -1, ingame: true)]
    protected static void ReceivedDisplay_QuestionMenu(NetState ns, PacketReader ip) => UpdateQuestionMenu?.Invoke(new(ns, ip));

    [PacketHandler(0x88, length: 66, ingame: true)]
    protected static void ReceivedDisplay_Paperdoll(NetState ns, PacketReader ip) => UpdatePaperdoll?.Invoke(new(ns, ip));

    [PacketHandler(0x95, length: 9, ingame: true)]
    protected static void ReceivedDisplay_HuePicker(NetState ns, PacketReader ip) => UpdateHuePicker?.Invoke(new(ns, ip));

    [PacketHandler(0xB8, length: -1, ingame: true)]
    protected static void ReceivedDisplay_Profile(NetState ns, PacketReader ip) => UpdateProfile?.Invoke(new(ns, ip));

    [PacketHandler(0x10, length: -1, ingame: true, extCmd: true)]
    protected static void ReceivedDisplay_EquipInfo(NetState ns, PacketReader ip) => UpdateEquipmentInfo?.Invoke(new(ns, ip));

    [PacketHandler(0x14, length: -1, ingame: true, extCmd: true)]
    protected static void ReceivedDisplay_ContextMenu(NetState ns, PacketReader ip) => UpdateContextMenu?.Invoke(new(ns, ip));
}
