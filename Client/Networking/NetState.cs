using System.Net;
using System.Net.Sockets;
namespace Client.Networking;
using Client.Accounting;
using Client.Game.Agents;

public abstract class NetState : Network
{
    public NetState() : base() => Construct(this);
    public Version? Version { get; private set; }
    public int AuthID { get; protected set; }
    public IAccount? Account { get; protected set; }
    public MobileContext? Mobile { get; private set; }
    public abstract bool IsOpen { get; }
    public abstract IOStream Stream { get; }
    public abstract IPAddress Address { get; }
    public abstract bool Attach(Socket socket);
    public abstract void Detach();
    public abstract bool Login(IAccount account);
    public abstract void Send(Packet? packet);
    public abstract void Slice();
}