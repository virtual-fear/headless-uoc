namespace Client.Networking.Arguments;
using Client.Game;
public sealed class PersonalLightEventArgs : EventArgs
{
    [PacketHandler(0x4E, length: 6, ingame: true)]
    private static event PacketEventHandler<PersonalLightEventArgs>? Update;
    public NetState State { get; }
    public sbyte Level { get; set; }
    public int Serial { get; set; }
    private PersonalLightEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Serial = ip.ReadInt32();
        Level = ip.ReadSByte();
    }
    static PersonalLightEventArgs() => Update += PersonalLightEventArgs_Update;
    private static void PersonalLightEventArgs_Update(PersonalLightEventArgs e) => World.PersonalLight.Value = e.Level;
}