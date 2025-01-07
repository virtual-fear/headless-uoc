namespace Client.Networking.Outgoing;
using Client.Game;
public sealed class PLookAt : Packet
{
    private PLookAt() : base(0x09, 0x05) { }
    public static Packet Instantiate(IEntity entity)
    {
        Packet packet = new PLookAt();
        packet.Stream.Write((int)entity.Serial);
        return packet;
    }
}
