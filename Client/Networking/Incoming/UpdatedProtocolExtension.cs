using System;
using System.Collections.Generic;
using System.IO;

namespace Client.Networking.Incoming
{
    using static PacketSink;
    public partial class PacketSink
    {
        public sealed class ProtocolExtensionEventArgs : EventArgs
        {
            public NetState State { get; }
            public ProtocolExtensionEventArgs(NetState state) => State = state;
            public ProtocolExtensionType Type { get; set; }
            public PartyMemberInfo[] Party { get; set; }
            public GuildMemberInfo[] Guild { get; set; }
        }
        
        public static event PacketEventHandler<ProtocolExtensionEventArgs> Custom;
        public static void InvokeCustom(ProtocolExtensionEventArgs e) => Custom?.Invoke(e);
    }

    public sealed class GuildMemberInfo
    {
        public int Serial { get; }
        public short X { get; }
        public short Y { get; }
        public byte MapID { get; }
        public byte Health { get; }
        private GuildMemberInfo(PacketReader pvSrc)
        {
            Serial = pvSrc.ReadInt32();
            X = pvSrc.ReadInt16();
            Y = pvSrc.ReadInt16();
            MapID = pvSrc.ReadByte();
            Health = pvSrc.ReadByte();
        }
        public static GuildMemberInfo Instantiate(PacketReader pvSrc) => new GuildMemberInfo(pvSrc);
    }
    public sealed class PartyMemberInfo
    {
        public int Serial { get; }
        public short X { get; }
        public short Y { get; }
        public byte MapID { get; }
        private PartyMemberInfo(PacketReader pvSrc)
        {
            Serial = pvSrc.ReadInt32();
            X = pvSrc.ReadInt16();
            Y = pvSrc.ReadInt16();
            MapID = pvSrc.ReadByte();
        }
        public static PartyMemberInfo Instantiate(PacketReader pvSrc) => new PartyMemberInfo(pvSrc);
    }
    public enum ProtocolExtensionType
    {
        Accept = 0,
        PartyTrack = 1,
        GuildTrack = 2,
        Runebooks = 3,
        Guardline = 4,
    }
    public static class UpdatedProtocolExtension
    {
        public static void Configure()
        {
            Register(0xF0, -1, true, new OnPacketReceive(Update));
        }
        private static void Update(NetState ns, PacketReader pvSrc)
        {
            ProtocolExtensionEventArgs e = new ProtocolExtensionEventArgs(ns);
            ProtocolExtensionType type = (ProtocolExtensionType)pvSrc.ReadByte();

            e.Type = type;
            switch (type)
            {
                case ProtocolExtensionType.Accept:
                    pvSrc.ReadByte();
                    break;

                case ProtocolExtensionType.PartyTrack:
                    List<PartyMemberInfo> party = new List<PartyMemberInfo>();
                    while (pvSrc.ReadInt32() != 0x00)
                    {
                        pvSrc.Seek(-4, SeekOrigin.Current);
                        party.Add(PartyMemberInfo.Instantiate(pvSrc));
                    }
                    e.Party = party.ToArray();
                    break;

                case ProtocolExtensionType.GuildTrack:
                    List<GuildMemberInfo> guild = new List<GuildMemberInfo>();
                    while (pvSrc.ReadInt32() != 0x00)
                    {
                        pvSrc.Seek(-4, SeekOrigin.Current);
                        guild.Add(GuildMemberInfo.Instantiate(pvSrc));
                    }
                    e.Guild = guild.ToArray();
                    break;

                default:
                    pvSrc.Trace();
                    return;
            }

            PacketSink.InvokeCustom(e);
        }
        private static void Register(byte packetID, int length, bool variable, OnPacketReceive onReceive)
        {
            PacketHandlers.Register(packetID, length, variable, onReceive);
        }
    }
}
