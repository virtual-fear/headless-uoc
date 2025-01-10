using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Networking.Incoming.Shard
{
    public partial class PacketHandlers
    {
        protected static class RejectedLogin
        {
            private static void Rejected()
                => Logger.LogError("ReceiveLoginRejection received, not fully implemented yet.");

            [PacketHandler(0x53, length: -1, ingame: false)]
            public static void Rej_0x53(NetState ns, PacketReader pvSrc) => Rejected();

            [PacketHandler(0x82, length: -1, ingame: false)]
            public static void Rej_0x82(NetState ns, PacketReader pvSrc) => Rejected();

            [PacketHandler(0x85, length: -1, ingame: false)]
            public static void Rej_0x85(NetState ns, PacketReader pvSrc) => Rejected();

        }
    }
}
