namespace Client.Networking.Outgoing;
internal sealed class PLoginGame : Packet
{
    public static event LoginEventHandler<SecondLoginAuthEventArgs>? Instantiated;
    private PLoginGame() : base(0x91, 65) => Encode = true;
    public sealed class SecondLoginAuthEventArgs : EventArgs
    {
        public NetState State { get; }
        public string Username { get; }
        public string Password { get; }
        public uint Seed { get; }
        public SecondLoginAuthEventArgs(NetState state, string username, string password, uint seed)
        {
            State = state;
            Username = username;
            Password = password;
            Seed = seed;
            Allowed = state.IsOpen;
        }
        public bool Allowed { get; set; }
    }
    public static Packet? Instantiate(SecondLoginAuthEventArgs e)
    {
        Instantiated?.Invoke(e);

        string un, pw;
        un = e.Username;
        pw = e.Password;

        Packet packet = new PLoginGame();
        if (e.Allowed)
        {
            packet.Stream.WriteUInt32_BE(e.Seed);
            packet.Stream.WriteAsciiFixed(un, 30);
            packet.Stream.WriteAsciiFixed(pw, 30);
            packet.Stream.Fill();
            return packet;
        }
        return null;
    }
}