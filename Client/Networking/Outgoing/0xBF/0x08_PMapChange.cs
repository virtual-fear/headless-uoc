using System;
namespace Client.Networking.Outgoing
{
    public class PMapChange : Packet
    {
        [Obsolete("MapIndex not introduced into Mobile", true)]
        public static void SendBy(NetState state)
        {
            state.Send(PMapChange.Instantiate(state));
        }

        private static Packet Instantiate(NetState state)
        {
            Packet packet = new PMapChange();
            //packet.Stream.Write((byte)state.Mobile.MapIndex);
            return packet;
        }

        private PMapChange()
            : base(0xBF)
        {
            base.Stream.Write((short)0x08);
        }
    }

}
