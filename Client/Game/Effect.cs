namespace Client.Game;

using System;
using Client.Game.Data;
using Client.Networking;
public partial class Effect
{
    internal static void Enqueue(NetState ns, EffectType type, 
        Serial src, Serial target, short itemID, IPoint3D from, IPoint3D to, 
        short hue, short amount)
    {
        throw new NotImplementedException();
    }

    internal static void Enqueue(NetState ns, EffectType type, 
        Serial src, Serial target, short itemID, IPoint3D from, IPoint3D to, 
        int hue, byte speed, byte duration, bool fixedDirection, bool isExploding, int renderMode)
    {
        throw new NotImplementedException();
    }

    internal static void Enqueue(NetState ns, EffectType type, 
        Serial src, Serial target, short itemID, IPoint3D from, IPoint3D to, 
        int hue, byte speed, byte duration, bool fixedDirection, bool isExploding, int renderMode,
        short effectID, short explodeEffect, short explodeSound, int serial, byte layer, short unknown)
    {
        throw new NotImplementedException();
    }

    internal static void Enqueue(NetState ns, ScreenEffectType type)
    {
        throw new NotImplementedException();
    }
}
