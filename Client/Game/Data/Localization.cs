namespace Client.Game.Data;
using System.Globalization;
public static class Localization
{
    public static string Language { get; }
    static Localization() => Language = CultureInfo.CurrentUICulture.ThreeLetterWindowsLanguageName.ToUpper();
}