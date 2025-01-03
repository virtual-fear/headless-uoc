using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.IO;
using System.Net;
using Client.Game;
using Client.Networking;
using static Client.Networking.Incoming.PacketSink;


namespace Client.Game
{
    public sealed class CityInfo
    {
        private byte m_Index;
        private string m_City, m_Building;
        private int m_X, m_Y, m_Z;
        private int m_MapID;
        private int m_Description;

        public byte Index { get { return m_Index; } }
        public string City { get { return m_City; } }
        public string Building { get { return m_Building; } }
        public int X { get { return m_X; } }
        public int Y { get { return m_Y; } }
        public int Z { get { return m_Z; } }
        public int MapID { get { return m_MapID; } }
        public int Description { get { return m_Description; } }

        public static CityInfo[] Instantiate(PacketReader pvSrc)
        {
            int count = pvSrc.ReadByte();
            if (count < 2)
            {
                return new CityInfo[0]; //No cities
            }

            CityInfo[] info = new CityInfo[count - 2];

            for (int i = 0; i < info.Length; i++)
                info[i] = new CityInfo(pvSrc);

            return info;
        }

        private CityInfo(PacketReader pvSrc)
        {
            m_Index = pvSrc.ReadByte();
            m_City = pvSrc.ReadString(32);
            m_Building = pvSrc.ReadString(32);
            m_X = pvSrc.ReadInt32();
            m_Y = pvSrc.ReadInt32();
            m_Z = pvSrc.ReadInt32();
            m_MapID = pvSrc.ReadInt32();
            m_Description = pvSrc.ReadInt32();

            pvSrc.ReadInt32();  //  0
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2}) {3}, {4}", this.X, this.Y, this.Z, this.City, this.Building);
        }
    }
    public sealed class CharInfo
    {
        private sealed class PlayCharacter : Packet
        {
            private PlayCharacter() : base(0x5D, 73) { }
            public static Packet Instantiate(CharInfo ci)
            {
                if (ci == null)
                    throw new ArgumentNullException("Unable to play character, slot is invalid.", "info");

                Packet p = new PlayCharacter();
                PacketWriter s = p.Stream;

                s.Write(0xEDEDEDED);
                s.WriteAsciiFixed(ci.Name, 30);
                
                s.Seek(02, SeekOrigin.Current);
                s.Write((int)31);

                s.Seek(24, SeekOrigin.Current);
                s.Write(ci.Index);

                NetState ns = ci.State;
                s.Write(ns.AuthID);
                
                Console.WriteLine("Packet:PlayCharacter AuthID: {0}", ns.AuthID);

                return p;
            }
        }

        public NetState State { get; }
        private CharInfo(NetState state, int index, string name)
        {
            State = state;
            Index = index;
            Name = name;
        }
        public int Index { get; }
        public string Name { get; }
        public void Play() => State.Send(PlayCharacter.Instantiate(this));
        public static CharInfo[] Instantiate(NetState state, PacketReader pvSrc)
        {
            List<CharInfo> list = new List<CharInfo>();
            int c = pvSrc.ReadByte();

            for (int i = 0; i < c; i++)
            {
                string un = pvSrc.ReadString(30).TrimEnd('\0');
                pvSrc.ReadString(30);

                if (string.IsNullOrEmpty(un))
                {
                    continue;
                }

                list.Add(new CharInfo(state, i, un));
            }

            return list.ToArray();
        }
    }
    public sealed class ShardInfo
    {
        private class PlayServer : Packet
        {
            public PlayServer(ushort index) : base(0xA0, 3) => base.Stream.Write((short)index);
        }
        public ushort Index { get; }
        public string Name { get; }
        public byte FullPercent { get; }
        public sbyte TimeZone { get; }
        public IPAddress Address { get; }
        private ShardInfo(PacketReader pvSrc)
        {
            Index = pvSrc.ReadUInt16();
            Name = pvSrc.ReadString(32);
            FullPercent = pvSrc.ReadByte();
            TimeZone = pvSrc.ReadSByte();
            int addr = pvSrc.ReadInt32();
            Address = new IPAddress((int)addr);
        }
        public static IEnumerable<ShardInfo> Instantiate(PacketReader pvSrc)
        {
            pvSrc.ReadByte();   //  Unknown
            ShardInfo[] info = new ShardInfo[pvSrc.ReadUInt16()];
            for (int i = 0; i < info.Length; i++)
                info[i] = new ShardInfo(pvSrc);

            return info;
        }

        public void Submit(AccountLoginEventArgs e) => e.State.Send(new PlayServer(Index));

    }
}

namespace Client.Networking.Incoming
{
    using static PacketSink;
    public partial class PacketSink
    {
        #region EventArgs
        
        public sealed class AccountLoginEventArgs : EventArgs
        {
            public NetState State { get; }
            public AccountLoginEventArgs(NetState state) => State = state;
            public IEnumerable<ShardInfo> Shards { get; set; }
        }
        public sealed class ServerAckEventArgs : EventArgs
        {
            public uint Addr { get; }
            public short Port { get; }
            public int Seed { get; }
            internal ServerAckEventArgs(uint addr, short port, int seed)
            {
                Addr = addr;
                Port = port;
                Seed = seed;
            }

        }
        public sealed class ServerListReceivedEventArgs : EventArgs
        {
            internal ServerListEntry[] ServerListEntries { get; set; }
        }
        public sealed class CharacterListEventArgs : EventArgs
        {
            public NetState State { get; }
            public CharacterListEventArgs(NetState state) => State = state;
            public IEnumerable<CharInfo> Characters { get; private set; }
            public IEnumerable<CityInfo> Cities { get; private set; }
            public int Flags { get; private set; }
            public void LoadCharacters(PacketReader pvSrc)
            {
                Characters = CharInfo.Instantiate(State, pvSrc);
            }
            public void LoadCities(PacketReader pvSrc)
            {
                Cities = CityInfo.Instantiate(pvSrc);
                Flags = pvSrc.ReadInt32();  //      CharacterListFlags
                pvSrc.ReadInt16();  //      -1
            }
        }
        public sealed class SupportedFeaturesEventArgs : EventArgs
        {
            public NetState State { get; }
            internal SupportedFeaturesEventArgs(NetState state) => State = state;
            public uint Features { get; set; }
        }
        public sealed class LoginConfirmEventArgs : EventArgs
        {
            public NetState State { get; }
            public LoginConfirmEventArgs(NetState state) => State = state;
            public int Serial { get; set; } = -1;
            public short Body { get; set; }
            public short X { get; set; }
            public short Y { get; set; }
            public short Z { get; set; }
            public Direction Direction { get; set; }
            public short Width { get; set; }
            public short Height { get; set; }
        }
        public sealed class LoginCompleteEventArgs : EventArgs
        {
            public NetState State { get; }
            public LoginCompleteEventArgs(NetState state) => State = state;
        }
        public sealed class CurrentTimeEventArgs : EventArgs
        {
            public NetState State { get; }
            public CurrentTimeEventArgs(NetState state) => State = state;
            public TimeSpan Span { get; set; }
        }

        #endregion EventArgs

        public static event PacketEventHandler<AccountLoginEventArgs> AccountLogin;
        public static event PacketEventHandler<ServerAckEventArgs> ServerAck;
        public static event PacketEventHandler<ServerListReceivedEventArgs> ServerList;
        public static event PacketEventHandler<CharacterListEventArgs> CharacterList;
        public static event PacketEventHandler<SupportedFeaturesEventArgs> SupportedFeatures;
        public static event PacketEventHandler<LoginConfirmEventArgs> LoginConfirm;
        public static event PacketEventHandler<LoginCompleteEventArgs> LoginComplete;
        public static event PacketEventHandler<CurrentTimeEventArgs> CurrentTime;
        public static void InvokeCurrentTime(CurrentTimeEventArgs e) => CurrentTime?.Invoke(e);
        public static void InvokeLoginComplete(LoginCompleteEventArgs e) => LoginComplete?.Invoke(e);
        public static void InvokeLoginConfirm(LoginConfirmEventArgs e) => LoginConfirm?.Invoke(e);
        public static void InvokeSupportedFeatures(SupportedFeaturesEventArgs e) => SupportedFeatures?.Invoke(e);
        public static void InvokeCharListReceived(CharacterListEventArgs e) => CharacterList?.Invoke(e);
        public static void InvokeServerListReceived(ServerListReceivedEventArgs e) => ServerList?.Invoke(e);
        public static void InvokeServerAck(ServerAckEventArgs e) => ServerAck?.Invoke(e);
        public static void InvokeAccountLogin(AccountLoginEventArgs e) => AccountLogin?.Invoke(e);
    }

    public static class UpdatedServer
    {
        // TODO: Minimize this - write something to handle all static void Configure() methods in the Network.Incoming namespace with 1 invoke
        public static void RegisterHandlers()
        {
            Register(0xA8, -1, false, new OnPacketReceive(ServerList));
            Register(0x8C, 11, false, new OnPacketReceive(ReceiveServerAck));
            Register(0x86, -1, false, new OnPacketReceive(CharacterListUpdate));
            Register(0xA9, -1, false, new OnPacketReceive(CharacterList));
            Register(0x82, -1, false, new OnPacketReceive(RejectedLogin));
            Register(0x85, -1, false, new OnPacketReceive(RejectedLogin));
            Register(0x53, -1, false, new OnPacketReceive(RejectedLogin));
            Register(0xFD, -1, false, new OnPacketReceive(LoginDelay));
            Register(0xB9, 05, false, new OnPacketReceive(SupportedFeatures));
            Register(0x1B, 37, true, new OnPacketReceive(LoginConfirm));
            Register(0x55, 01, true, new OnPacketReceive(LoginComplete));
            Register(0x5B, 04, false, new OnPacketReceive(CurrentTime));

            UpdatedAnimations.Configure();
            UpdatedBooks.Configure();
            UpdatedBulletinBoard.Configure();
            UpdatedContainer.Configure();
            UpdatedCustomizedHouseContent.Configure();
            UpdatedDisplay.Configure();
            UpdatedEffects.Configure();
            UpdatedGump.Configure();
            UpdatedItem.Configure();
            UpdatedLights.Configure();
            UpdatedMap.Configure();
            UpdatedMessages.Configure();
            UpdatedMobile.Configure();
            UpdatedMovement.Configure();
            UpdatedPlayer.Configure();
            UpdatedPrompt.Configure();
            UpdatedProtocolExtension.Configure();
            UpdatedRequests.Configure();
            UpdatedSeasons.Configure();
            // UpdatedServer.cs
            UpdatedSounds.Configure();
            UpdatedVendor.Configure();
            UpdatedVersion.Configure();
            UpdatedWorld.Configure();

            // Setup mobiles/items
            Mobile.Configure();
            Item.Configure();
        }
        private static void CurrentTime(NetState ns, PacketReader pvSrc)
        {
            CurrentTimeEventArgs e = new CurrentTimeEventArgs(ns);

            byte h, m, s;

            h = pvSrc.ReadByte();
            m = pvSrc.ReadByte();
            s = pvSrc.ReadByte();

            e.Span = new TimeSpan(h, m, s);

            PacketSink.InvokeCurrentTime(e);
        }
        private static void LoginComplete(NetState ns, PacketReader pvSrc)
        {
            LoginCompleteEventArgs e = new LoginCompleteEventArgs(ns);

            PacketSink.InvokeLoginComplete(e);
        }
        private static void LoginConfirm(NetState ns, PacketReader pvSrc)
        {
            LoginConfirmEventArgs e = new LoginConfirmEventArgs(ns);
            e.Serial = pvSrc.ReadInt32();
            pvSrc.Seek(4, SeekOrigin.Current);
            e.Body = pvSrc.ReadInt16();
            e.X = pvSrc.ReadInt16();
            e.Y = pvSrc.ReadInt16();
            e.Z = pvSrc.ReadInt16();
            e.Direction = (Direction)pvSrc.ReadInt16();
            pvSrc.Seek(9, SeekOrigin.Current);
            e.Width = pvSrc.ReadInt16();
            e.Height = pvSrc.ReadInt16();
            PacketSink.InvokeLoginConfirm(e);
        }
        private static void SupportedFeatures(NetState ns, PacketReader pvSrc)
        {
            SupportedFeaturesEventArgs e = new SupportedFeaturesEventArgs(ns);

            switch (pvSrc.Length)
            {
                case 5:
                    e.Features = pvSrc.ReadUInt32();
                    break;
                case 3:
                    e.Features = pvSrc.ReadUInt16();
                    break;
                default:
                    pvSrc.Trace();
                    break;
            }

            PacketSink.InvokeSupportedFeatures(e);
        }
        private static void LoginDelay(NetState state, PacketReader pvSrc)
        {
            Logger.LogError("LoginDelay received, not fully implemented yet.");
        }
        private static void RejectedLogin(NetState state, PacketReader pvSrc)
        {
            Logger.LogError("ReceiveLoginRejection received, not fully implemented yet.");
        }
        private static void CharacterList(NetState ns, PacketReader pvSrc)
        {
            CharacterListEventArgs e = new CharacterListEventArgs(ns);

            e.LoadCharacters(pvSrc);
            e.LoadCities(pvSrc);

            ServerData.Instance.CharInfo = (CharInfo[])e.Characters;
            ServerData.Instance.CityInfo = (CityInfo[])e.Cities;

            PacketSink.InvokeCharListReceived(e);
        }
        private static void CharacterListUpdate(NetState state, PacketReader pvSrc)
        {
            Logger.LogError("CharacterListUpdate received, not fully implemented yet.");
        }
        private static void ServerList(NetState state, PacketReader pvSrc)
        {
            // OutgoingAccountPackets.cs : SendAccountLoginAck(this NetState ns);
            //writer.Write((byte)0x5D);
            //writer.Write((ushort)info.Length);
            byte flags = pvSrc.ReadByte(); // 0x5D (Unknown)
            ushort count = pvSrc.ReadUInt16(); // info.Length
            List<ServerListEntry> entries = new List<ServerListEntry>();
            for (ushort i = 0; i < count; i++)
            {
                entries.Add(new ServerListEntry(
                    (uint)pvSrc.ReadInt16(),    // i
                    pvSrc.ReadStringSafe(32), // name
                    pvSrc.ReadByte(), // full percent
                    pvSrc.ReadByte(), // time zone
                    pvSrc.ReadUInt32() // raw ip
                ));
            }
            ServerData.Instance.ServerEntries = entries.ToArray();
            Logger.Log("Received the list of available servers");
            int entryIdx = 1;
            foreach(var entry in ServerData.Instance.ServerEntries)
                Logger.Log($"  {entryIdx++}) {entry.Name} ({entry.PercentFull}%)", color: ConsoleColor.DarkCyan);
            PacketSink.InvokeServerListReceived(new ServerListReceivedEventArgs() { ServerListEntries = ServerData.Instance.ServerEntries });
        }

        /**
         * Packet: 0x8C
         * Length: 11 bytes
         * 
         * Receives play server acknowledgement
         */
        private static void ReceiveServerAck(NetState state, PacketReader pvSrc)
        { 
            uint rawAddress = pvSrc.ReadUInt32LE();
            short port = pvSrc.ReadInt16();
            int seed = pvSrc.ReadInt32();
            PacketSink.InvokeServerAck(new ServerAckEventArgs(rawAddress, port, seed));
        }
        public static void Register(int packetID, int length, bool ingame, OnPacketReceive receive)
        {
            PacketHandlers.Register(packetID, length, ingame, receive);
        }
    }
}
