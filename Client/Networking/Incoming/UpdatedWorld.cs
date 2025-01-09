namespace Client.Networking.Incoming;

using Client.Game.Data;
using static PacketSink;
public partial class PacketSink
{
    #region EventArgs

    public sealed class RemoveEventArgs : EventArgs
    {
        public NetState State { get; }
        public RemoveEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
    }
    public sealed class WeatherEventArgs : EventArgs
    {
        public NetState State { get; }
        public WeatherEventArgs(NetState state) => State = state;
        public byte[] Buffer { get; set; }
        public byte V1 => Buffer[0];
        public byte V2 => Buffer[1];
        public byte V3 => Buffer[2];
    }
    public sealed class SequenceEventArgs : EventArgs
    {
        public NetState State { get; }
        public SequenceEventArgs(NetState state) => State = state;
        public int Value { get; set; }
    }
    public sealed class PauseEventArgs : EventArgs
    {
        public NetState State { get; }
        public bool Supported { get; }
        public PauseEventArgs(NetState state, bool supported)
        {
            State = state;
            Supported = supported;
        }
    }
    public sealed class ChangeUpdateRangeEventArgs : EventArgs
    {
        public NetState State { get; }
        public ChangeUpdateRangeEventArgs(NetState state) => State = state;
        public byte Range { get; set; }
    }
    public sealed class GQCountEventArgs : EventArgs
    {
        public NetState State { get; }
        public GQCountEventArgs(NetState state) => State = state;
        public short Unk { get; set; }
        public int Count { get; set; }
    }
    public sealed class CorpseEquipEventArgs : EventArgs
    {
        public NetState State { get; }
        public CorpseEquipEventArgs(NetState state) => State = state;
        public int Beheld { get; set; }
        public LayerInfo[] Layers { get; set; }
    }
    public sealed class NullFastwalkStackEventArgs : EventArgs
    {
        public NetState State { get; }
        public NullFastwalkStackEventArgs(NetState state) => State = state;
        public byte[] Buffer { get; set; }
    }
    public sealed class SpeedControlEventArgs : EventArgs
    {
        public NetState State { get; }
        public SpeedControlEventArgs(NetState state) => State = state;
        public int Value { get; set; }
    }

    #endregion (done)

    public static event PacketEventHandler<RemoveEventArgs>? Remove;
    public static event PacketEventHandler<WeatherEventArgs>? Weather;
    public static event PacketEventHandler<SequenceEventArgs>? Sequence;
    public static event PacketEventHandler<PauseEventArgs>? Pause;
    public static event PacketEventHandler<ChangeUpdateRangeEventArgs>? ChangeUpdateRange;
    public static event PacketEventHandler<GQCountEventArgs>? GQCount;
    public static event PacketEventHandler<CorpseEquipEventArgs>? CorpseEquip;
    public static event PacketEventHandler<NullFastwalkStackEventArgs>? NullFastwalkStack;
    public static event PacketEventHandler<SpeedControlEventArgs>? SpeedControl;
    public static void InvokeSpeedControl(SpeedControlEventArgs e) => SpeedControl?.Invoke(e);
    public static void InvokeNullFastwalkStack(NullFastwalkStackEventArgs e) => NullFastwalkStack?.Invoke(e);
    public static void InvokeCorpseEquip(CorpseEquipEventArgs e) => CorpseEquip?.Invoke(e);
    public static void InvokeGQCount(GQCountEventArgs e) => GQCount?.Invoke(e);
    public static void InvokeChangeUpdateRange(ChangeUpdateRangeEventArgs e) => ChangeUpdateRange?.Invoke(e);
    public static void InvokePause(PauseEventArgs e) => Pause?.Invoke(e);
    public static void InvokeSequence(SequenceEventArgs e) => Sequence?.Invoke(e);
    public static void InvokeWeather(WeatherEventArgs e) => Weather?.Invoke(e);
    public static void InvokeRemove(RemoveEventArgs e) => Remove?.Invoke(e);
}

public static class UpdatedWorld
{
    public static void Configure()
    {
        Register(0x1D, 05, true, new OnPacketReceive(Remove));
        Register(0x65, 04, true, new OnPacketReceive(Weather));
        Register(0x7B, 02, true, new OnPacketReceive(Sequence));
        //Register(0x33, 02, true, new OnPacketReceive(Pause));
        Register(0xC8, 02, true, new OnPacketReceive(ChangeUpdateRange));
        Register(0xCB, 07, true, new OnPacketReceive(GQCount));
        Register(0x89, -1, true, new OnPacketReceive(CorpseEquip));
        RegisterExtended(0x01, -1, true, new OnPacketReceive(Extended_NullFastwalkStack));
        RegisterExtended(0x26, 03, true, new OnPacketReceive(Extended_SpeedControl));
    }

    private static void Extended_NullFastwalkStack(NetState ns, PacketReader pvSrc)
    {
              NullFastwalkStackEventArgs e = new NullFastwalkStackEventArgs(ns);

        e.Buffer = pvSrc.ReadBytes(6 * sizeof(int));

        PacketSink.InvokeNullFastwalkStack(e);
    }
    private static void Extended_SpeedControl(NetState ns, PacketReader pvSrc)
    {
        SpeedControlEventArgs e = new SpeedControlEventArgs(ns);

        e.Value = pvSrc.ReadByte();

        PacketSink.InvokeSpeedControl(e);
    }
    private static void CorpseEquip(NetState ns, PacketReader pvSrc)
    {
        CorpseEquipEventArgs e = new CorpseEquipEventArgs(ns);

        e.Beheld = pvSrc.ReadInt32();

        Layer layer;
        List<LayerInfo> l = new List<LayerInfo>();
        while ((layer = (Layer)pvSrc.ReadByte()) != Layer.Invalid)
            l.Add(new LayerInfo(layer, pvSrc));

        e.Layers = l.ToArray();

        PacketSink.InvokeCorpseEquip(e);
    }
    private static void GQCount(NetState ns, PacketReader pvSrc)
    {
        GQCountEventArgs e = new GQCountEventArgs(ns);

        e.Unk = pvSrc.ReadInt16();
        e.Count = pvSrc.ReadInt32();

        PacketSink.InvokeGQCount(e);
    }
    private static void ChangeUpdateRange(NetState ns, PacketReader pvSrc)
    {
        ChangeUpdateRangeEventArgs e = new ChangeUpdateRangeEventArgs(ns);

        e.Range = pvSrc.ReadByte();

        PacketSink.InvokeChangeUpdateRange(e);
    }
    private static void Sequence(NetState ns, PacketReader pvSrc)
    {
        SequenceEventArgs e = new SequenceEventArgs(ns);

        e.Value = pvSrc.ReadByte();

        PacketSink.InvokeSequence(e);
    }
    private static void Weather(NetState ns, PacketReader pvSrc)
    {
        WeatherEventArgs e = new WeatherEventArgs(ns);

        e.Buffer = pvSrc.ReadBytes(3);

        PacketSink.InvokeWeather(e);
    }
    private static void Remove(NetState ns, PacketReader pvSrc)
    {
        RemoveEventArgs e = new RemoveEventArgs(ns);

        e.Serial = pvSrc.ReadInt32();

        PacketSink.InvokeRemove(e);
    }
    private static void WorldItem(NetState ns, PacketReader pvSrc)
    {
        WorldItemEventArgs e = new WorldItemEventArgs(ns);

        e.Serial = pvSrc.ReadInt32();
        e.ItemID = pvSrc.ReadInt16();
        e.Amount = pvSrc.ReadInt16();
        e.X = pvSrc.ReadInt16();
        e.Y = pvSrc.ReadInt16();
        e.Z = pvSrc.ReadSByte();
        e.Hue = pvSrc.ReadInt16();
        e.Flags = pvSrc.ReadByte();

        PacketSink.InvokeWorldItem(e);
    }
    private static void RegisterExtended(byte packetID, int length, bool variable, OnPacketReceive onReceive)
    {
        PacketHandlers.RegisterExtended(packetID, length, variable, onReceive);
    }
    private static void Register(byte packetID, int length, bool variable, OnPacketReceive onReceive)
    {
        PacketHandlers.Register(packetID, length, variable, onReceive);
    }
}
