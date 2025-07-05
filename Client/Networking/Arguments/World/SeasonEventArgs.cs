namespace Client.Networking.Arguments;
using Client.Game;
public sealed class SeasonChangeEventArgs : EventArgs
{

    [PacketHandler(0xBC, length: 3, ingame: true)]
    public static event PacketEventHandler<SeasonChangeEventArgs>? Update;
    public NetState State { get; }
    public byte Value { get; }
    public bool Sound { get; }
    internal SeasonChangeEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Value = ip.ReadByte();
        Sound = ip.ReadBoolean();
    }

    static SeasonChangeEventArgs() => Update += SeasonChangeEventArgs_Update;
    private static void SeasonChangeEventArgs_Update(SeasonChangeEventArgs e)
    {
        World.Season.Value = e.Value;
        World.SeasonHasAudio.Value = e.Sound;
    }
}