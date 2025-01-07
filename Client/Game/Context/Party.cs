namespace Client.Game.Context;

using Client.Game.Context.Agents;
using Client.Game.Context.Data;
using Client.Networking;
using Client.Networking.Outgoing;

public delegate void PartyHandler(Packet packet);
public sealed class Party
{
    public const int MaxCapacity = 10;
    public PartyHandler? Handler { get; set; }
    public List<MobileAgent> Members { get; } = new(MaxCapacity);
    public PartyState State { get; set; } = PartyState.Alone;
    public static Party Create() => new();
    public MobileAgent? Leader => Members.Count > 0 ? Members[0] : null;
    public void AddMembers(IEnumerable<MobileAgent> mobiles)
    {
        foreach (MobileAgent m in mobiles)
        {
            if (m == null)
                continue;

            Members.Add(m);
        }
    }
    public void SendMessage(string text)
    {
        if (Handler == null)
            throw new ArgumentNullException(nameof(Handler));

        PartyState s = State;
        PartyHandler c = Handler;
        if (s == PartyState.Joined)
        {
            if (c == null)
            {
                Logger.Log("Party: (Joined): Invalid party handler, failed to send message.");
                return;
            }
            c.Invoke(new PPartyMessage(text));
        }
    }
}