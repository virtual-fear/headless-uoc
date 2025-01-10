using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Client.Networking
{
    public delegate void OnPacketReceive(NetState state, PacketReader pvSrc);
    public delegate void PacketEventHandler<T>(T e);

    [CompilerGenerated]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PacketHandlerAttribute : Attribute
    {
        public int PacketID { get; }
        public int Length { get; }
        public bool Ingame { get; }
        public bool ExtendedCommand { get; }
        public PacketHandlerAttribute(int packetID, int length, bool ingame, bool extCmd = false)
        {
            PacketID = packetID;
            Length = length;
            Ingame = ingame;
            ExtendedCommand = extCmd;
        }
    }

    public class PacketHandler
    {
        protected Dictionary<int, PacketHandler> Table { get; } = new Dictionary<int, PacketHandler>();
        public bool IsExtended => Table.Count > 0;
        public int ReceivedHits { get; private set;  }
        public int PacketID { get; }
        public int Length { get; }
        public bool Ingame { get; }
        private OnPacketReceive OnReceive { get; }
        public string Name => OnReceive != null ? OnReceive.Method.Name : GetType().Name;
        public PacketHandler(int packetID, int length, bool ingame, OnPacketReceive receive)
        {
            PacketID = packetID;
            Length = length;
            Ingame = ingame;
            OnReceive = receive;
        }
        public PacketHandler? this[int packetID] 
        {
            get => Table.ContainsKey(packetID) ? Table[packetID] : null;
            set
            {
                if (this[packetID] == null)
                {
                    if (value == null)
                    {
                        Table.Remove(packetID);
                        return;
                    }
                    Table[packetID] = value;
                }
                else if (value != null)
                {
                    Table.Add(packetID, value);
                }
            }
        }
        public void Receive(NetState state, PacketReader pvSrc)
        {
            ReceivedHits += 1;
            OnReceive.Invoke(state, pvSrc);

        }
        public override string ToString() => $"0x{PacketID:X2} ({Length}) {Name} ({(Ingame ? "in-game" : "out-of-game")})";
    }
}
