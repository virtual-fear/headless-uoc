
namespace Client.Networking.Incoming;
public partial class Shard
{
    public static event PacketEventHandler<AccountLoginEventArgs>? OnAccountLogin;
    public static event PacketEventHandler<CharacterListEventArgs>? OnCharacterList;
    public static event PacketEventHandler<CharacterListUpdateEventArgs>? OnCharacterListUpdate;
    public static event PacketEventHandler<CurrentTimeEventArgs>? OnUpdate_CurrentTime;
    public static event PacketEventHandler<GQCountEventArgs>? UpdateGQCount;
    public static event PacketEventHandler<LaunchBrowserEventArgs>? OnLaunchBrowser;
    public static event PacketEventHandler<LoginCompleteEventArgs>? OnLoginComplete;
    public static event PacketEventHandler<LoginConfirmEventArgs>? OnLoginConfirm;
    public static event PacketEventHandler<LoginDelayEventArgs>? OnLoginDelay;
    public static event PacketEventHandler<NullFastwalkStackEventArgs>? OnUpdate_NullFastwalkStack;
    public static event PacketEventHandler<PauseEventArgs>? OnPause;
    public static event PacketEventHandler<ProtocolExtensionEventArgs>? OnUpdate_ProtocolExtension; // TODO: Move data types in EventArgs to Client.Game.Data
    public static event PacketEventHandler<RejectedLoginEventArgs>? OnRejectedLogin = (e) => Logger.LogError($"Login rejected (0x{e.Command}:X2)");
    public static event PacketEventHandler<SequenceEventArgs>? OnUpdate_Sequence;
    public static event PacketEventHandler<ServerAckEventArgs>? OnServerAck;
    public static event PacketEventHandler<ServerListReceivedEventArgs>? OnUpdate_ServerList;
    public static event PacketEventHandler<SpeedControlEventArgs>? OnSpeedControl;
    public static event PacketEventHandler<SupportedFeaturesEventArgs>? OnUpdate_SupportedFeatures;

    [PacketHandler(0xB9, length: 5, ingame: false)]
    protected static void Receive_SupportedFeatures(NetState ns, PacketReader ip) => OnUpdate_SupportedFeatures?.Invoke(new(ns, ip));

    [PacketHandler(0x26, length: 3, ingame: true, extCmd: true)]
    protected static void ReceivedExt_SpeedControl(NetState ns, PacketReader ip) => OnSpeedControl?.Invoke(new(ns, ip));

    [PacketHandler(0xA8, length: -1, ingame: false)]
    protected static void Received_ServerList(NetState state, PacketReader ip) => OnUpdate_ServerList?.Invoke(new(state, ip));

    [PacketHandler(0x8C, length: 11, ingame: false)]
    protected static void Received_ServerAck(NetState state, PacketReader ip) => OnServerAck?.Invoke(new(state, ip));
    
    [PacketHandler(0x7B, length: 2, ingame: true)]
    protected static void Received_Sequence(NetState ns, PacketReader ip) => OnUpdate_Sequence?.Invoke(new(ns, ip));

    [PacketHandler(0x53, length: -1, ingame: false)]
    protected static void Received_Rejection_0x53(NetState ns, PacketReader pvSrc) => OnRejectedLogin?.Invoke(new(ns, cmd: 0x53));

    [PacketHandler(0x82, length: -1, ingame: false)]
    protected static void Received_Rejection_0x82(NetState ns, PacketReader pvSrc) => OnRejectedLogin?.Invoke(new(ns, cmd: 0x82));

    [PacketHandler(0x85, length: -1, ingame: false)]
    protected static void Received_Rejection_0x85(NetState ns, PacketReader pvSrc) => OnRejectedLogin?.Invoke(new(ns, cmd: 0x85));

    [PacketHandler(0xF0, length: -1, ingame: true)]
    protected static void Received_ProtocolExtension(NetState ns, PacketReader ip) => OnUpdate_ProtocolExtension?.Invoke(new(ns, ip));

    [PacketHandler(0x33, length: 2, ingame: true)]
    protected static void Received_Pause(NetState ns, PacketReader ip) => OnPause?.Invoke(new(ns, ip));

    [PacketHandler(0x1D, length: 5, ingame: true, extCmd: true)]
    protected static void ReceivedExt_NullFastwalkStack(NetState ns, PacketReader ip) => OnUpdate_NullFastwalkStack?.Invoke(new(ns, ip));

    [PacketHandler(0xFD, length: -1, ingame: false)]
    protected static void Received_LoginDelay(NetState ns, PacketReader ip) => OnLoginDelay?.Invoke(new(ns));

    [PacketHandler(0x1B, length: 37, ingame: false)]
    protected static void Received_LoginConfirm(NetState ns, PacketReader ip) => OnLoginConfirm?.Invoke(new(ns, ip));

    [PacketHandler(0x55, length: 1, ingame: true)]
    protected static void Received_LoginComplete(NetState ns, PacketReader pvSrc) => OnLoginComplete?.Invoke(new(ns));

    [PacketHandler(0xA5, length: -1, ingame: true)]
    protected static void Received_LaunchBrowser(NetState ns, PacketReader ip) => OnLaunchBrowser?.Invoke(new(ns, ip));

    [PacketHandler(0xCB, length: 7, ingame: true)]
    protected static void Received_ProtocolExtension_0xF0(NetState ns, PacketReader ip) => UpdateGQCount?.Invoke(new(ns, ip));

    [PacketHandler(0x5B, length: 4, ingame: false)]
    protected static void Received_CurrentTime(NetState ns, PacketReader ip) => OnUpdate_CurrentTime?.Invoke(new(ns, ip));

    [PacketHandler(0x86, length: -1, ingame: false)]
    protected static void Received_CharacterListUpdate(NetState ns, PacketReader ip) => OnCharacterListUpdate?.Invoke(new(ns, ip));

    [PacketHandler(0xA9, length: -1, ingame: false)]
    protected static void Received_CharacterList(NetState ns, PacketReader ip) => OnCharacterList?.Invoke(new(ns, ip));
}