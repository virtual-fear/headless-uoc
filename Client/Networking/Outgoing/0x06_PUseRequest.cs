using Client.Game;

namespace Client.Networking.Outgoing
{
    public sealed class PUseRequest : Packet
    {
        private PUseRequest()
            : base(0x06, 5) { }
        
        public static Packet Instantiate(IEntity entity)
        {
            Packet packet = new PUseRequest();
            packet.Stream.Write((int)entity.Serial);
            return packet;
        }
    }
}
