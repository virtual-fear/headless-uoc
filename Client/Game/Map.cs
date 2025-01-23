namespace Client.Game;
using System.Collections;
using Client.Networking;
public class Map
{
    internal static void Change(NetState ns, byte index)
    {
        Logger.Log(ns.Address, $"Changed map index to {index}");
    }

    internal static void Command(NetState ns, int mapItem, byte cmd, byte number, int x, int y)
    {
        Logger.Log(ns.Address, $"Map item {mapItem} command {cmd} number {number} x {x} y {y}");
    }

    internal static void Patches(NetState ns, Hashtable? table)
    {
        Logger.Log(ns.Address, "Received map patches");
    }
}