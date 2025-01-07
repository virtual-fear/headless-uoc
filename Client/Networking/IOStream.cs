namespace Client.Networking;

using Client.Cryptography;
using Client.Cryptography.Impl;
using Client.IO;
using Client.Networking.Arguments;
using Client.Networking.Outgoing;
using static Client.Networking.Outgoing.PLoginGame;
public abstract class IOStream
{
    public static IOStream Construct() => new DefaultIOStream();
    public abstract InputQueue Input { get; }
    public abstract OutputQueue Output { get; }
    public abstract Crypto Crypto { get; protected set; }
    public void Reset()
    {
        Input.Clear();
        Output.Clear();
    }
    private sealed class DefaultIOStream : IOStream
    {
        private Crypto _crypto = Crypto.UseDefault();
        public override InputQueue Input { get; } = new InputQueue();
        public override OutputQueue Output { get; } = new OutputQueue();
        public override Crypto Crypto {
            get => _crypto;
            protected set => _crypto = value;
        }
        static DefaultIOStream() => Network.OnAttach += Network_OnAttach;
        private static void Network_OnAttach(ConnectionEventArgs e)
        {
            ConnectInfo info = Network.Info;
            if (info.Stage == ConnectionAck.SecondLogin)
            {
                if (!e.IsReady) return;
                if (!e.IsAllowed) return;

                Logger.Log($"{e.State.Address}: Sending ack response.", LogColor.Magenta);
                e.State.Stream.Crypto = new GameCrypto();
                var un = info.Username ?? string.Empty;
                var pw = info.Password ?? string.Empty;
                UInt32 v = info.Seed;
                Span<byte> b = stackalloc byte[4] {
                        (byte)(v >> 0x18),
                        (byte)(v >> 0x10),
                        (byte)(v >> 0x08),
                        (byte)(v >> 0x00)
                    };
                e.Socket.Send(b); // Seed needs to be sent after GameCrypto
                e.State.Send(PLoginGame.Instantiate(e: new SecondLoginAuthEventArgs(e.State, un, pw, info.Seed)));
            }
        }
    }
}