namespace Client.Game;
using System;
using Client.Game.Data;
using Client.Networking;

public static class Message
{
    internal static void Add(NetState from, IEntity ent, short graphic, byte messageType, short hue, short font, string? name, string? text, string? arguments)
    {
        Logger.Log(from.Address, $"Serial:{ent.Serial} (gfx:{graphic}, type:{messageType}, hue:{hue}, font:{font}, name:{name}, text:{text}, args:{arguments})");
    }

    internal static void Add(NetState from, IEntity ent, short graphic, byte messageType, short hue, short font, string? name, string? text)
    {
        Logger.Log(from.Address, $"Serial:{ent.Serial} (gfx:{graphic}, type:{messageType}, hue:{hue}, font:{font}, name:{name}, text:{text}");
    }

    internal static void AddScrollEntry(byte type, int tip, string? text)
    {
        throw new NotImplementedException();
    }
}