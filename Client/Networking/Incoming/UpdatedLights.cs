using System;

namespace Client.Networking.Incoming
{
    using static PacketSink;
    public partial class PacketSink
    {
        #region EventArgs

        public sealed class PersonalLightEventArgs : EventArgs
        {
            public NetState State { get; }
            public PersonalLightEventArgs(NetState state) => State = state;
            public sbyte Level { get; set; }
            public int Serial { get; set; }
        }
        public sealed class GlobalLightEventArgs : EventArgs
        {
            public NetState State { get; }
            public GlobalLightEventArgs(NetState state) => State = state;
            public sbyte Level { get; set; }
        }

        #endregion (done)

        public static event PacketEventHandler<GlobalLightEventArgs> GlobalLight;
        public static event PacketEventHandler<PersonalLightEventArgs> PersonalLight;
        public static void InvokePersonalLight(PersonalLightEventArgs e) => PersonalLight?.Invoke(e);
        public static void InvokeGlobalLight(GlobalLightEventArgs e) => GlobalLight?.Invoke(e);
    }
 
    public static class UpdatedLights
    {
        public static void Configure()
        {
            Register(0x4F, 02, true, new OnPacketReceive(GlobalLightLevel));
            Register(0x4E, 06, true, new OnPacketReceive(PersonalLightLevel));
        }
        private static void PersonalLightLevel(NetState ns, PacketReader pvSrc)
        {
            PersonalLightEventArgs e = new PersonalLightEventArgs(ns);

            e.Serial = pvSrc.ReadInt32();
            e.Level = pvSrc.ReadSByte();

            PacketSink.InvokePersonalLight(e);

        }
        private static void GlobalLightLevel(NetState ns, PacketReader pvSrc)
        {
            GlobalLightEventArgs e = new GlobalLightEventArgs(ns);

            e.Level = pvSrc.ReadSByte();

            PacketSink.InvokeGlobalLight(e);
        }
        public static void Register(int packetID, int length, bool ingame, OnPacketReceive receive)
        {
            PacketHandlers.Register(packetID, length, ingame, receive);
        }
    }
}
