namespace Client.Game
{
    public enum Notoriety : byte
    {
        Invalid = 0,
        Innocent = 1,
        Ally = 2,
        Attackable = 3,
        Criminal = 4,
        Enemy = 5,
        Murderer = 6,
        Vendor = 7
    }

    public enum CMEFlags
    {
        None = 0x00,
        Disabled = 0x01,
        Arrow = 0x02,
        Highlighted = 0x04,
        Colored = 0x20
    }
}
