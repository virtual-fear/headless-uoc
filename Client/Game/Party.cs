using System.Collections.Generic;

namespace Client.Game
{
    using Client.Networking;

    public enum PartyState
    {
        Alone,
        Joining,
        Joined
    }

    public delegate void PartyHandler(Packet packet);

    public sealed class Party
    {

        public const int Capacity = 10;

        private PartyHandler m_Handler;
        private List<Mobile> m_Members;
        private PartyState m_State;

        public PartyHandler Handler
        {
            get { return m_Handler; }
            set { m_Handler = value; }
        }
        public IEnumerable<Mobile> Members
        {
            get { return m_Members; }
        }
        public PartyState State
        {
            get { return m_State; }
            set { m_State = value; }
        }

        public static Party Create()
        {
            return new Party();
        }

        private Party()
        {
            m_Members = new List<Mobile>(Party.Capacity);
            m_State = PartyState.Alone;
        }

        public Mobile Leader
        {
            get { return m_Members[0]; }
        }

        public void AddMembers(IEnumerable<Mobile> mobiles)
        {
            foreach (Mobile m in mobiles)
            {
                if (m == null)
                    continue;
                
                m_Members.Add(m);
            }
        }

        public void SendMessage(string text)
        {
            PartyState s = m_State;
            PartyHandler c = m_Handler;

            if (s == PartyState.Joined)
            {
                if (c == null)
                {
                    Logger.Log("Party: (Joined): Invalid party handler, failed to send message.");
                    return;
                }
                c.Invoke(PartyMessage.Instantiate(text));
            }
        }

        private sealed class PartyMessage : Packet
        {
            public static Packet Instantiate(string text)
            {
                Packet packet = new PartyMessage();
                packet.Stream.Write((short)0x06);
                packet.Stream.Write((byte) 0x04);
                packet.Stream.WriteUnicode(text);
                packet.Stream.Write((short)0x00);
                return packet;
            }

            private PartyMessage()
                : base(0xBF)
            {
            }
        }
    }
}