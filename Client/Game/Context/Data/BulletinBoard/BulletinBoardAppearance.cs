namespace Client.Game.Context.Data.BulletinBoard;
public sealed class BulletinBoardAppearance
{
    public int Body { get; }
    public int Hue { get; }
    public BulletinBoardItem[] Items { get; }
    public BulletinBoardAppearance(int body, int hue, params BulletinBoardItem[] items)
    {
        Body = body;
        Hue = hue;
        Items = items;
    }
}
