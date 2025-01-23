namespace Client.Game;

using System;
using Client.Networking;
using Client.Networking.Arguments;
public partial class Shard
{
    public static TimeSpan CurrentTime { get; internal set; }
    public static int GQCount { get; internal set; }
    public static int Sequence { get; internal set; }
    internal static void LaunchBrowser(string? url)
        => Logger.LogError($"Launch browser: {url}");
    internal static void Reject(NetState state, byte command) => state.Detach();
}