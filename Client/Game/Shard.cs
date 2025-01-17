namespace Client.Game;
using Client.Networking;
using Client.Networking.Arguments;
public partial class Shard
{
    public static event PacketEventHandler<AccountLoginEventArgs>? OnAccountLogin;

    [PacketHandler(0xA9, length: -1, ingame: false)]
    public static event PacketEventHandler<CharacterListEventArgs>? OnCharacterList;

    [PacketHandler(0x86, length: -1, ingame: false)]
    public static event PacketEventHandler<CharacterListUpdateEventArgs>? OnCharacterListUpdate;

    [PacketHandler(0x5B, length: 4, ingame: false)]
    public static event PacketEventHandler<CurrentTimeEventArgs>? OnUpdate_CurrentTime;

    [PacketHandler(0xCB, length: 7, ingame: true)]
    public static event PacketEventHandler<GQCountEventArgs>? UpdateGQCount;

    [PacketHandler(0xA5, length: -1, ingame: true)]
    public static event PacketEventHandler<LaunchBrowserEventArgs>? OnLaunchBrowser;

    [PacketHandler(0x55, length: 1, ingame: true)]
    public static event PacketEventHandler<LoginCompleteEventArgs>? OnLoginComplete;

    [PacketHandler(0x1B, length: 37, ingame: false)]
    public static event PacketEventHandler<LoginConfirmEventArgs>? OnLoginConfirm;

    [PacketHandler(0xFD, length: -1, ingame: false)]
    public static event PacketEventHandler<LoginDelayEventArgs>? OnLoginDelay;

    [PacketHandler(0x1D, length: 5, ingame: true, extCmd: true)]
    public static event PacketEventHandler<NullFastwalkStackEventArgs>? OnUpdate_NullFastwalkStack;

    [PacketHandler(0x33, length: 2, ingame: true)]
    public static event PacketEventHandler<PauseEventArgs>? OnPause;

    [PacketHandler(0xF0, length: -1, ingame: true)]
    public static event PacketEventHandler<ProtocolExtensionEventArgs>? OnUpdate_ProtocolExtension; // TODO: Move data types in EventArgs to Client.Game.Data

    [PacketHandler(0x53, length: -1, ingame: false)]
    [PacketHandler(0x82, length: -1, ingame: false)]
    [PacketHandler(0x85, length: -1, ingame: false)]
    public static event PacketEventHandler<RejectedLoginEventArgs>? OnRejectedLogin = (e) => Logger.LogError($"Login rejected (0x{e.Command}:X2)");

    [PacketHandler(0x7B, length: 2, ingame: true)]
    public static event PacketEventHandler<SequenceEventArgs>? OnUpdate_Sequence;

    [PacketHandler(0x8C, length: 11, ingame: false)]
    public static event PacketEventHandler<ServerAckEventArgs>? OnServerAck;

    [PacketHandler(0xA8, length: -1, ingame: false)]
    public static event PacketEventHandler<ServerListEventArgs>? OnUpdate_ServerList;

    [PacketHandler(0x26, length: 3, ingame: true, extCmd: true)]
    public static event PacketEventHandler<SpeedControlEventArgs>? OnSpeedControl;

    [PacketHandler(0xB9, length: 5, ingame: false)]
    public static event PacketEventHandler<SupportedFeaturesEventArgs>? OnUpdate_SupportedFeatures;
}