namespace Client.Game;
using System;
using Client.Game.Data;
using Client.Networking;
public partial class Container
{
    internal static void Display(NetState ns, int containerID, short gumpID)
    {
        throw new NotImplementedException();
    }
    internal static void DisplayContent(NetState ns, ContainerItem[]? items)
    {
    }
    internal static void DisplayContentUpdate(NetState ns, int serial, ushort iD, ushort amount, short x, short y, int parent, short hue)
    {
    }
}
