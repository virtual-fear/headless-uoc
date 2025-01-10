namespace Client.Networking.Incoming.Player;
public partial class PacketHandlers
{
    public static event PacketEventHandler<ChangeCombatantEventArgs>? Player_ChangeCombatant;
    public sealed class ChangeCombatantEventArgs : EventArgs
    {
        public NetState State { get; }
        public ChangeCombatantEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
    }

    protected static class ChangeCombatant
    {
        [PacketHandler(0xAA, length: 5, ingame: true)]
        public static void Update(NetState ns, PacketReader pvSrc)
        {
            ChangeCombatantEventArgs e = new(ns);
            e.Serial = pvSrc.ReadInt32();
            Player_ChangeCombatant?.Invoke(e);
        }
    }
}
