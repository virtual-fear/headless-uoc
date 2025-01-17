namespace Client.Game.Data;
public struct BulletinBoardItem
{
    public int ItemID { get; set; }
    public int Hue { get; set; }
    public BulletinBoardItem(int itemID, int hue)
    {
        ItemID = itemID;
        Hue = hue;
    }
}
