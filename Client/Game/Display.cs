namespace Client.Game;
using Client.Networking;
using Client.Networking.Arguments;
public partial class Display
{
    [PacketHandler(0x14, length: -1, ingame: true, extCmd: true)]
    public static event PacketEventHandler<DisplayContextMenuEventArgs>? UpdateContextMenu;

    [PacketHandler(0x10, length: -1, ingame: true, extCmd: true)]
    public static event PacketEventHandler<DisplayEquipInfoEventArgs>? UpdateEquipmentInfo;

    [PacketHandler(0xB8, length: -1, ingame: true)]
    public static event PacketEventHandler<DisplayProfileEventArgs>? UpdateProfile;

    [PacketHandler(0x95, length: 9, ingame: true)]
    public static event PacketEventHandler<HuePickerEventArgs>? UpdateHuePicker;

    [PacketHandler(0x88, length: 66, ingame: true)]
    public static event PacketEventHandler<PaperdollEventArgs>? UpdatePaperdoll;

    [PacketHandler(0x7C, length: -1, ingame: true)]
    public static event PacketEventHandler<QuestionMenuEventArgs>? UpdateQuestionMenu;
}
