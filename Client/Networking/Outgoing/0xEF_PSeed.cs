using System;

namespace Client.Networking.Outgoing
{
    public delegate void SeedEventHandler(SeedEventArgs e);
    public sealed class PSeed : Packet
    {
        private static readonly Version ClientVersion
            = new Version(7, 0, 106, 21);
        //= new Version(7, 0, 34, 15);

        public static event SeedEventHandler Version;
        
        private PSeed()
            : base(0xEF, 21) { }
        static PSeed()
        {
            PSeed.Version += UpdateVersion;
        }
        static void UpdateVersion(SeedEventArgs e) => e.Version = PSeed.ClientVersion;
        private static Packet Instantiate(NetState state, uint seed = 1)
        {
            var e = new SeedEventArgs(state) { Version = PSeed.ClientVersion };

            if (Version != null)
                Version(e);

            Version ver = e.Version;
            Packet packet = new PSeed();
            packet.Stream.Write((int)seed);
            packet.Stream.Write((uint)ver.Major);
            packet.Stream.Write((uint)ver.Minor);
            packet.Stream.Write((uint)ver.Build);
            packet.Stream.Write((uint)ver.Revision);
            return packet;
        }
        public static void SendBy(NetState state, uint seed = 1)
        {
            if (state == null)
                throw new ArgumentNullException("state");
            state.Send(PSeed.Instantiate(state, seed));
        }
    }
    public sealed class SeedEventArgs : EventArgs
    {
        public NetState State { get; }
        public SeedEventArgs(NetState state) => State = state;
        public Version Version { get; set; }

    }
}
