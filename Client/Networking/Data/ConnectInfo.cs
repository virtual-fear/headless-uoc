namespace Client.Networking.Data;
using System.Net;
public struct ConnectInfo
{
    public ConnectionAck Stage;
    public ConnectInfo() => Stage = ConnectionAck.FirstLogin;
    public IPEndPoint EndPoint = new IPEndPoint(IPAddress.None, 0);
    public String? Username = string.Empty;
    public String? Password = string.Empty;
    public UInt32 Seed = 0;
}
