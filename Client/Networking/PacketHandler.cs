using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Client.Networking
{
    public delegate void OnPacketReceive(NetState state, PacketReader pvSrc);
    public delegate void PacketEventHandler<T>(T e);
    public sealed class PacketHandlerInfo
    {
        public byte PacketID { get; }

        /// <summary>
        /// out-of-game:0,
        /// in-game:1
        /// </summary>
        public PacketHandler[] Handlers { get; }
        public PacketHandlerInfo(byte packetID)
        {
            PacketID = packetID;
            Handlers = new PacketHandler[2];
        }

        /// <summary>
        ///     The first handler should be out-of-game
        /// </summary>
        /// <param name="child"></param>
        public void Add(PacketHandler child)
        {
            // PacketHandler children may have different length i.e ns.ExtendedSupportedFeatures ? 5 : 3

            if (Handlers[0] == null)
            {
                Handlers[0] = child;
                return;
            }

            if (Handlers[1] == null)
                Handlers[1] = child;
        }
    }

    [CompilerGenerated]
    [AttributeUsage(AttributeTargets.Event | AttributeTargets.Method, AllowMultiple = true)]
    public class PacketHandlerAttribute : Attribute
    {
        public byte PacketID { get; }
        public short Length { get; }
        public bool Ingame { get; }
        public bool ExtendedCommand { get; }
        public PacketHandlerAttribute(byte packetID, short length, bool ingame, bool extCmd = false)
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
        public byte PacketID { get; }
        public short Length { get; }
        public bool Ingame { get; }
        private OnPacketReceive OnReceive { get; }
        public string Name { get; }
        public PacketHandler(byte packetID, short length, bool ingame, OnPacketReceive receive, string? name = default)
        {
            PacketID = packetID;
            Length = length;
            Ingame = ingame;
            OnReceive = receive;
            name ??= receive.Method.Name;
            Name = name ?? $"(unknown:{packetID}:X2";

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
