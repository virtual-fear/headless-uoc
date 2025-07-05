namespace Client.Networking.Packets;

using Client.Game.Data;

public sealed class PLookAt : Packet
{
    private PLookAt() : base(0x09, 0x05) { }
    public static Packet Instantiate(IEntity entity)
    {
        Packet packet = new PLookAt();
        packet.Stream.Write((uint)entity.Serial);
        return packet;
    }
}
