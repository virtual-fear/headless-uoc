namespace Client.Game;
using System;
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
    internal static void ClearWeaponAbility(NetState state)
    {
        throw new NotImplementedException();
    }

    internal static void EquipItem(int mobSerial, int itemSerial, object itemID, Layer layer, short hue)
    {
        throw new NotImplementedException();
    }

    internal static void LiftRej(NetState state, LRReason reason)
    {
        throw new NotImplementedException();
    }

    internal static void OnCancelArrow()
    {
        throw new NotImplementedException();
    }

    internal static void OnFight(byte flag, int attacker, int defender)
    {
        throw new NotImplementedException();
    }

    internal static void OnMultiTarget(MultiTargetEventArgs e)
    {
        throw new NotImplementedException();
    }

    internal static void OnServerChange(NetState state, IPoint3D location, short xMap, short yMap)
    {
        throw new NotImplementedException();
    }

    internal static void OnSkillUpdate(NetState state, byte type, List<Networking.Arguments.SkillInfo> skills)
    {
        throw new NotImplementedException();
    }

    internal static void OnTarget(NetState state, int targetID, TargetFlags flags)
    {
        throw new NotImplementedException();
    }

    internal static void Trade(NetState state, int firstContainer, string? name, int them, int secondContainer)
    {
        throw new NotImplementedException();
    }

    internal static void UpdateHealthbar(NetState ns, Mobile mob, HealthbarType type, byte level)
    {
        Logger.Log(ns.Address, $"Updating healthbar for mobile: {mob.Serial} ({mob.Name})");
    }

    internal static void UpdateSpellbook(NetState state, int item, short graphic, short offset, long content)
    {
        throw new NotImplementedException();
    }
}