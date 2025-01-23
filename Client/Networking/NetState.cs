using System.Net;
using System.Net.Sockets;
using Client.Game;
using Client.Game.Data;
using Client.Networking.Arguments;
namespace Client.Networking;
public abstract class NetState : Network
{
    static NetState()
    {
        LoginConfirmEventArgs.Update += LoginConfirmEventArgs_Update;
        LoginCompleteEventArgs.Update += LoginCompleteEventArgs_Update;
    }

    private static void LoginCompleteEventArgs_Update(LoginCompleteEventArgs e)
    {
        e.State.CompletedLogin = true;
    }
    private static void LoginConfirmEventArgs_Update(LoginConfirmEventArgs e)
    {
        e.State.ConfirmedLogin = true;
    }
    public NetState() : base() => Construct(this);
    public System.Version? Version { get; private set; }
    public int AuthID { get; protected set; }
    public IAccount? Account { get; protected set; }
    public Game.Mobile? Mobile { get; internal set; }
    public abstract bool IsOpen { get; }
    public abstract IOStream Stream { get; }
    public abstract IPAddress Address { get; }
    public bool CompletedLogin { get; protected set; }
    public bool ConfirmedLogin { get; protected set; }
    public abstract bool Attach(Socket socket);
    public abstract void Detach();
    public abstract bool Login(IAccount account);
    public abstract void Send(Packet? packet);
    public abstract void Slice();
}