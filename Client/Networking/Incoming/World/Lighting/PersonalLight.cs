namespace Client.Networking.Incoming;
public sealed class PersonalLightEventArgs : EventArgs
{
    public NetState State { get; }
    public PersonalLightEventArgs(NetState state) => State = state;
    public sbyte Level { get; set; }
    public int Serial { get; set; }
}
public partial class World
{
    public partial class Lighting
    {
        public static event PacketEventHandler<PersonalLightEventArgs>? OnPersonalChange;

        [PacketHandler(0x4E, length: 6, ingame: true)]
        protected static void PersonalLight_Receive(NetState ns, PacketReader pvSrc)
        {
            PersonalLightEventArgs e = new(ns);
            e.Serial = pvSrc.ReadInt32();
            e.Level = pvSrc.ReadSByte();
            OnPersonalChange?.Invoke(e);
        }
    }
}