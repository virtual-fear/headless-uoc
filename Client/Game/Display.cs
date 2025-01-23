namespace Client.Game;
using System;
using System.Collections.Generic;
using Client.Networking;
using Client.Networking.Arguments;
public partial class Display
{
    internal static void ShowBook(NetState state, string title, string author)
    {
        throw new NotImplementedException();
    }

    internal static void ShowContextMenu(NetState ns, int menuSerial, ContextMenuEntry[] entries)
    {
        throw new NotImplementedException();
    }

    internal static void ShowEquipInfo(NetState ns, int itemID, int number, bool identified, string? name, List<EquipInfoAttribute> attributes)
    {
        throw new NotImplementedException();
    }

    internal static void ShowHuePicker(NetState state, int serial, short itemID)
    {
        throw new NotImplementedException();
    }

    internal static void ShowPaperdoll(NetState state, Mobile mobile, string text, bool draggable)
    {
        throw new NotImplementedException();
    }

    internal static void ShowProfile(NetState ns, Mobile mob, string header, string footer, string body)
    {
        throw new NotImplementedException();
    }

    internal static void ShowQuestionMenu(NetState ns, int menuSerial, string? question, string[]? answers)
    {
        throw new NotImplementedException();
    }
}
