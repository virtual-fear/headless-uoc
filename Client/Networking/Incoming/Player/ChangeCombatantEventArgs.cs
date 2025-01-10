namespace Client.Networking.Incoming;
public sealed class ChangeCombatantEventArgs : EventArgs
{
    public NetState State { get; }
    public ChangeCombatantEventArgs(NetState state) => State = state;
    public int Serial { get; set; }
}
public partial class Player
{
    public static event PacketEventHandler<ChangeCombatantEventArgs>? OnChangeCombatant;

    [PacketHandler(0xAA, length: 5, ingame: true)]
    protected static void Receive_ChangeCombatant(NetState ns, PacketReader pvSrc)
    {
        ChangeCombatantEventArgs e = new(ns);
        e.Serial = pvSrc.ReadInt32();
        OnChangeCombatant?.Invoke(e);
    }
}