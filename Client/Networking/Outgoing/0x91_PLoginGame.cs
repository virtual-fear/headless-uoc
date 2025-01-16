namespace Client.Networking.Outgoing;
internal sealed class PLoginGame : Packet
{
    public static event LoginEventHandler<SecondLoginAuthEventArgs>? Instantiated;
    private PLoginGame() : base(0x91, 65) => Encode = false;
    public sealed class SecondLoginAuthEventArgs : EventArgs
    {
        public NetState? State { get; }
        public string? Username { get; }
        public string? Password { get; }
        public uint Seed { get; }
        public SecondLoginAuthEventArgs(NetState? state, string username, string password, uint seed)
        {
            State = state;
            Username = username;
            Password = password;
            Seed = seed;
            Allowed = state?.IsOpen ?? false;
        }
        public bool Allowed { get; set; }
    }
    public static Packet? Instantiate(SecondLoginAuthEventArgs? e)
    {
        if (e == null)
            throw new ArgumentNullException(nameof(e), "Invalid arguments");

        Instantiated?.Invoke(e);

        string un, pw;
        un = e.Username;
        pw = e.Password;

        Packet packet = new PLoginGame();
        if (e.Allowed)
        {
            packet.Stream.Write((uint)e.Seed);
            packet.Stream.WriteAscii(un, 30);
            packet.Stream.WriteAscii(pw, 30);
            packet.Stream.FilltoCapacity();
            return packet;
        }
        return null;
    }
}