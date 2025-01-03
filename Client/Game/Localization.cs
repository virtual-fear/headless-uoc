using System.Globalization;
using System.Collections.Generic;
using System.IO;

namespace Client.Game
{
    public class Localization
    {
        private static string m_Language;
        public static string Language
        {
            get { return m_Language; }
        }

        static Localization()
        {
            m_Language = CultureInfo.CurrentUICulture.ThreeLetterWindowsLanguageName.ToUpper();
        }
    }
}