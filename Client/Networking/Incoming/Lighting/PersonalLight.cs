namespace Client.Networking.Incoming.Lighting;
public partial class PacketHandlers
{
    public static event PacketEventHandler<PersonalLightEventArgs>? UpdatePersonalLight;
    public sealed class PersonalLightEventArgs : EventArgs
    {
        public NetState State { get; }
        public PersonalLightEventArgs(NetState state) => State = state;
        public sbyte Level { get; set; }
        public int Serial { get; set; }
    }
    protected static class PersonalLight
    {
        [PacketHandler(0x4E, length: 6, ingame: true)]
        internal static void Update(NetState ns, PacketReader pvSrc)
        {
            PersonalLightEventArgs e = new(ns);
            e.Serial = pvSrc.ReadInt32();
            e.Level = pvSrc.ReadSByte();
            UpdatePersonalLight?.Invoke(e);
        }
    }
}