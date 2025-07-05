using System.Collections;
using Client.Networking;
namespace Client.Game;
public partial class Map
{
    internal static void InvokeChange(NetState state, byte index)
    {
    }

    internal static void InvokeCommand(NetState state, int mapItem, byte command, byte number, int x, int y)
    {
    }

    internal static void InvokePatches(NetState state, Hashtable? table)
    {
    }
}