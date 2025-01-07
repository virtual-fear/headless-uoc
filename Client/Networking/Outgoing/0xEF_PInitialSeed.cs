using System;

namespace Client.Networking.Outgoing
{
    public delegate void SeedEventHandler(SeedEventArgs e);
    public sealed class PInitialSeed : Packet
    {
        private static readonly Version ClientVersion
            = new Version(7, 0, 106, 21);
        //= new Version(7, 0, 34, 15);

        public static event SeedEventHandler Version;

        private PInitialSeed()
            : base(0xEF, 21) => Encode = false;
        static PInitialSeed()
        {
            PInitialSeed.Version += UpdateVersion;
        }
        static void UpdateVersion(SeedEventArgs e) => e.Version = PInitialSeed.ClientVersion;
        private static Packet Instantiate(NetState state)
        {
            var e = new SeedEventArgs(state) { Version = PInitialSeed.ClientVersion };

            if (Version != null)
                Version(e);

            Version ver = e.Version;
            Packet packet = new PInitialSeed();
            packet.Stream.Write((int)1);
            packet.Stream.Write((int)ver.Major);
            packet.Stream.Write((int)ver.Minor);
            packet.Stream.Write((int)ver.Build);
            packet.Stream.Write((int)ver.Revision);
            return packet;
        }
        public static void SendBy(NetState state)
        {
            if (state == null)
                throw new ArgumentNullException("state");
            state.Send(PInitialSeed.Instantiate(state));
        }
    }
    public sealed class SeedEventArgs : EventArgs
    {
        public NetState State { get; }
        public SeedEventArgs(NetState state) => State = state;
        public Version Version { get; set; }

    }
}
