namespace Client.Game;
using Client.Game.Compression;
using Client.Networking;
public partial class Gumps
{
    public const bool MoongateConfirmation = false;

    private static readonly string[] m_Strings = new string[] {
        "Dost thou wish to step into the moongate? Continue to enter the gate, Cancel to stay here",
    };

    public static void HandleGump(int serial, int dialog, int xOffset, int yOffset, string layout, string[] text)
    {
        TextWriter output = Console.Out;

        if (text.Length > 0 && text[0] == m_Strings[0] && MoongateConfirmation)
        {
            //if (World.State != null)
            //{
            //    World.State.Send(GumpButton.Instantiate(serial, dialog, 1));
            //}
            throw new NotImplementedException();
        }
        else
        {
            output.WriteLine("HandleGump serial:{0} dialog:{1}", serial, dialog);

            for (int i = 0; i < text.Length; i++)
                output.WriteLine("{0}. {1}", i, text[i]);

            output.WriteLine();
            //GServerGump.GetCachedLocation( dialog, ref xOffset, ref yOffset );
            //GServerGump toAdd = new GServerGump( serial, dialog, xOffset, yOffset, layout, text );
            //Gumps.Desktop.Children.Add( toAdd );
        }
    }

}