namespace Client.Game;
using System.Collections.Generic;
using Client.Game.Data;
using Client.Networking;
using Client.Networking.Arguments;
public static class Player
{
    public static int Combatant { get; internal set; }
    public static bool IsDead { get; internal set; }
    public static ToggleSpecialAbilityEventArgs? SpecialAbility { get; internal set; }
    public static bool Warmode { get; internal set; }
    public static Mobile? Mobile { get; internal set; }

    internal static void ClearWeaponAbility(NetState state)
    {
        Logger.Log(state.Address, "Clearing weapon ability.");
    }

    internal static void EquipItem(int mobSerial, int itemSerial, object itemID, Layer layer, short hue)
    {
        Logger.Log($"Equipping item {itemSerial} (ID: {itemID}) on mobile {mobSerial} at layer {layer} with hue {hue}.");
    }

    internal static void LiftRej(NetState state, LRReason reason)
    {
        Logger.Log(state.Address, $"Lift rej requested. Reason: {reason}");
    }

    internal static void OnCancelArrow()
    {
        Logger.Log("Arrow targeting cancelled.");
    }

    internal static void OnFight(byte flag, int attacker, int defender)
    {
        Logger.Log($"Fight event received. Flag: {flag}, Attacker: {attacker}, Defender: {defender}");
    }

    internal static void OnMultiTarget(MultiTargetEventArgs e)
    {
        Logger.Log(e.State.Address, $"Multi-target event received. Target ID: {e.TargetID}, Flags: {e.Flags}, Multi ID: {e.MultiID}, Offsets: ({e.OffsetX}, {e.OffsetY}, {e.OffsetZ})");
    }

    internal static void OnServerChange(NetState state, IPoint3D location, short xMap, short yMap)
    {
        Logger.Log(state.Address, $"Server change requested. Location: {location}, Map: ({xMap}, {yMap})");
    }

    internal static void OnSkillUpdate(NetState state, byte type, List<Networking.Arguments.SkillInfo> skills)
    {
        Logger.Log(state.Address, $"Skill update received. Type: {type}, Skills Count: {skills.Count}");
    }

    internal static void OnTarget(NetState state, int targetID, TargetFlags flags)
    {
        Logger.Log(state.Address, $"Targeting ID: {targetID}, Flags: {flags}");
    }

    internal static void Trade(NetState state, int firstContainer, string? name, int them, int secondContainer)
    {
        Logger.Log(them, $"Trade initiated with {name} (ID: {them}). 1st Container: {firstContainer}, 2nd Container: {secondContainer}");
    }

    internal static void UpdateHealthbar(NetState ns, Mobile mob, HealthbarType type, byte level)
    {
        Logger.Log(ns.Address, $"Updating healthbar for mobile: {mob.Serial} ({mob.Name})");
    }

    internal static void UpdateSpellbook(NetState state, int item, short graphic, short offset, long content)
    {
        Logger.Log(state.Address, $"Updating spellbook for item: {item}, Graphic: {graphic}, Offset: {offset}, Content: {content:X}");
    }
}