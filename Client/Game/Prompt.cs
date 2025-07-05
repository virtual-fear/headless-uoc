namespace Client.Game;
using System;
using Client.Networking;
public partial class Prompt
{
    internal static void OnASCII(NetState state, int serial, int promptID, string? text)
    {
        throw new NotImplementedException();
    }
    internal static void OnUnicode(NetState state, int serial, int promptID)
    {
        throw new NotImplementedException();
    }
}
