namespace Client.Game;
using System.Globalization;
public class Localization
{
    public static string Language { get; }
    static Localization() => Language = CultureInfo.CurrentUICulture.ThreeLetterWindowsLanguageName.ToUpper();
}