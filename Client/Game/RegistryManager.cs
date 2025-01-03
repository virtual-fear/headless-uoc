using System;
using System.IO;
using System.Security;
using System.Security.Principal;
using Microsoft.Win32;

namespace Client.Game
{
    public static class RegistryManager
    {
        private static string[] m_Keys = new string[]
        {
            @"Software\Electronic Arts\EA Games\Ultima Online Classic",
            @"Software\Electronic Arts\EA Games\Ultima Online Stygian Abyss Classic",
            @"Software\EA Games",
            @"Software\Origin Worlds Online",
            @"SOFTWARE\Wow6432Node\Electronic Arts\EA Games\Ultima Online Classic",
            @"SOFTWARE\Wow6432Node\Electronic Arts\EA Games\Ultima Online Stygian Abyss Classic",
            @"Software\Wow6432Node\EA Games",
            @"Software\Wow6432Node\Origin Worlds Online"
        };

        private static string m_Path;

        public static string ClientPath
        {
            get
            {
                if (m_Path == null)
                    m_Path = Load();
                return m_Path;
            }
        }

        private static string Load()
        {
            if (OperatingSystem.IsWindows())
            {
                foreach (string k in m_Keys)
                {
                    try
                    {
                        using (RegistryKey key = Registry.LocalMachine.OpenSubKey(k))
                        {
                            if (Find(key, out string v))
                                return v;
                        }
                    }
                    catch (SecurityException) { }
                }
            }
            return string.Empty;
        }

        private static bool Find(RegistryKey root, out string value)
        {
            value = null;
            if (OperatingSystem.IsWindows() && (root != null))
            {
                foreach (string str in root.GetSubKeyNames())
                {
                    try
                    {
                        using (RegistryKey key = Registry.LocalMachine.OpenSubKey(str))
                        {
                            if (FindValue("InstallDir", key, out value) ||
                                FindValue("ExePath", key, out value) ||
                                Find(key, out value))
                                return true;
                        }
                    }
                    catch (SecurityException) { }
                }
            }
            return false;
        }

        private static bool FindValue(string name, RegistryKey key, out string value)
        {
            return FindValue(name, key, Path.GetFullPath, Directory.Exists, out value);
        }

        private static bool FindValue(string name, RegistryKey key, Func<string, string> method, Predicate<string> match, out string value)
        {
            value = null;
            if (key != null)
            {
                value = key.GetValue(name) as string;
                if (!string.IsNullOrEmpty(value))
                {
                    value = method(value);
                    if (match(value))
                        return true;
                }
            }
            return false;
        }
    }
}
