using System.Net;
using System.Net.Sockets;
using Client.Game;
using Client.Game.Context;
using Client.Game.Data;
using Client.Networking.Arguments;
namespace Client.Networking;
public abstract class NetState : Network
{
    static NetState()
    {
        Shard.OnLoginConfirm += Shard_OnLoginConfirm;
        Shard.OnLoginComplete += Shard_OnLoginComplete;
    }

    private static void Shard_OnLoginComplete(LoginCompleteEventArgs e)
    {
        e.State.Ingame = true;
    }

    private static void Shard_OnLoginConfirm(LoginConfirmEventArgs e)
    {
        e.State.ConfirmedLogin = true;
    }

    public NetState() : base() => Construct(this);
    public System.Version? Version { get; private set; }
    public int AuthID { get; protected set; }
    public IAccount? Account { get; protected set; }
    public MobileContext? Mobile { get; protected set; }
    public abstract bool IsOpen { get; }
    public abstract IOStream Stream { get; }
    public abstract IPAddress Address { get; }
    public bool ConfirmedLogin { get; protected set; }
    public bool Ingame { get; protected set; }
    public abstract bool Attach(Socket socket);
    public abstract void Detach();
    public abstract bool Login(IAccount account);
    public abstract void Send(Packet? packet);
    public abstract void Slice();
}