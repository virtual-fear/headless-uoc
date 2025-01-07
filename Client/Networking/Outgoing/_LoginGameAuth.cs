using System;
using Client.Accounting;
using Client.Cryptography;
using Client.Cryptography.Impl;

namespace Client.Networking.Outgoing
{
    public delegate void LoginAuthEventHandler(LoginAuth.LoginAuthEventArgs e);
    public sealed class LoginAuth : Packet
    {
        private LoginAuth()
            : base(0x80, 62) => Encode = false;

        public static event LoginAuthEventHandler Invoke;
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
            }
            public bool Allowed { get; set; }
        }
        public static Packet Instantiate(LoginAuthEventArgs e)
        {
            e.Allowed = true;

            if (Invoke != null)
                Invoke(e);

            string un, pw;
            un = e.Username;
            pw = e.Password;

            Packet packet = new LoginAuth();
            if (e.Allowed)
            {
                packet.Stream.WriteAsciiFixed(un, 30);
                packet.Stream.WriteAsciiFixed(pw, 30);
                packet.Stream.Fill();
                return packet;
            }
            return null;
        }
    }

    public delegate void SecondLoginAuthEventHandler(SecondLoginAuth.SecondLoginAuthEventArgs e);
    public sealed class SecondLoginAuth : Packet
    {
        public static event SecondLoginAuthEventHandler Invoke;
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
            }
            public bool Allowed { get; set; }
        }
        public static Packet Instantiate(SecondLoginAuthEventArgs e)
        {
            e.Allowed = true;

            Invoke?.Invoke(e);

            string un,  pw;
            un = e.Username;
            pw = e.Password;

            Packet packet = new SecondLoginAuth();
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
        private SecondLoginAuth()
            : base(0x91, 65) => Encode = true;
    }

    public delegate void GameAuthEventHandler(GameAuthEventArgs e);
    public sealed class GameAuthEventArgs : EventArgs
    {
        public NetState State { get; }
        public IAccount Account { get; }
        public Crypto Crypto { get; }
        public GameAuthEventArgs(NetState state)
        {
            State = state;
            Account = state.Account;
            Crypto = new GameCrypto();
        }
        public int Address { get; set; }
        public short Port { get; set; }
        public int AuthID { get; set; }
    }
}
