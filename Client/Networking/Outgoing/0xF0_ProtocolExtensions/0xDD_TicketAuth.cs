namespace Client.Networking.Outgoing;
using System.Reflection;

[Obsolete]
public sealed class TicketAuth : Packet
{
    [Obsolete("Special login system used for UOGamers: Hybrid")]
    private TicketAuth(ulong ticket)
        : base(0xF0)
    {
        base.Stream.Write((byte)0xDD);
        base.Stream.Write((int)(ticket >> 0x20));
        base.Stream.Write((int)ticket);
        byte[]? toWrite = null;
        try
        {
            toWrite = Assembly.GetExecutingAssembly().GetName().GetPublicKeyToken();
        }
        catch
        {
        }
        toWrite ??= new byte[0];
        base.Stream.Write(toWrite.Length);
        base.Stream.Write(toWrite, 0, toWrite.Length);
    }
}
