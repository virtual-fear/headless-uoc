namespace Client.Game.Data;
public sealed class BulletinBoardBody
{
    public BulletinBoardAppearance Appearance { get; }
    public string[] Lines { get; }
    public BulletinBoardBody(BulletinBoardAppearance appearance, string[] lines)
    {
        Appearance = appearance;
        Lines = lines;
    }
}
