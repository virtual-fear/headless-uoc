namespace Client.Networking.Outgoing;

internal delegate void LoginEventHandler<T>(T e);
internal sealed class PLoginAccount : Packet
{
    public static event LoginEventHandler<LoginAuthEventArgs>? Instantiated;
    private PLoginAccount() : base(0x80, 62) => Encode = false;
    public sealed class LoginAuthEventArgs : EventArgs
    {
        public NetState State { get; }
        public string Username { get; }
        public string Password { get; }
        public LoginAuthEventArgs(NetState state, string username, string password)
        {
            State = state;
            Username = username;
            Password = password;
            Allowed = state.IsOpen;
        }
        public bool Allowed { get; set; }
    }
    public static Packet? Instantiate(LoginAuthEventArgs e)
    {
        Instantiated?.Invoke(e);
        Packet packet = new PLoginAccount();
        if (e.Allowed)
        {
            packet.Stream.WriteAscii(e.Username, 30);
            packet.Stream.WriteAscii(e.Password, 30);
            packet.Stream.FilltoCapacity();
            return packet;
        }
        return null;
    }
}