using System.Net;
using System.Net.Sockets;
namespace Client.Networking
{
    using Accounting;
    using Cryptography;
    using IO;

    public abstract class NetState : Network
    {
        public Version? Version { get; private set; }
        public int AuthID { get; protected set; }
        public IAccount? Account { get; protected set; }
        public Game.Mobile Mobile { get; private set; }
        public abstract bool IsOpen { get; }
        public abstract Crypto Crypto { get; set; }
        public abstract IPAddress Address { get; }
        public abstract BaseQueue Input { get; }
        public abstract bool Attach(Socket socket);
        public abstract void Detach();
        public abstract bool Login(IAccount account);
        public abstract void Send(Packet packet);
        public abstract void Slice();
    }
}