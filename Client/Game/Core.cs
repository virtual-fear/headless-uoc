namespace Client.Game;
using Microsoft.Win32;
public static class Core
{
    public static List<string> Directories { get; } = new List<string>();
    public static string? FindDataFile(string path)
    {
        if (Directories.Count == 0)
            throw new InvalidOperationException("Attempted to FindDataFile before DataDirectories list has been filled.");

        string? fullPath = null;
        for (int i = 0; i < Directories.Count; ++i)
        {
            fullPath = Path.Combine(Directories[i], path);
            if (File.Exists(fullPath))
                break;

            fullPath = null;
        }
        return fullPath;
    }
    public static string? FindDataFile(string format, params object[] args) => FindDataFile(string.Format(format, args));
    private static string? GetPath(string subName, string keyName)
    {
        try
        {
            string keyString = @$"SOFTWARE\{(Application.Is64Bit ? "Wow6432Node" : "")}\{0}";

            if (Application.Is64Bit)
                keyString = @"SOFTWARE\Wow6432Node\{0}";
            else
                keyString = @"SOFTWARE\{0}";

            if (OperatingSystem.IsWindows())
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(string.Format(keyString, subName)))
                {
                    if (key == null)
                        return null;
                    string? v = key.GetValue(keyName) as string;
                    if (String.IsNullOrEmpty(v))
                        return null;
                    if (keyName == "InstallDir")
                        v = v + @"\";
                    v = Path.GetDirectoryName(v);
                    if (String.IsNullOrEmpty(v))
                        return null;
                    return v;
                }
            }
        }
        catch
        {
        }
        return null;
    }
    static Core()
    {
        string? pathUO = GetPath(@"Origin Worlds Online\Ultima Online\1.0", "ExePath");
        string? pathTD = GetPath(@"Origin Worlds Online\Ultima Online Third Dawn\1.0", "ExePath"); //These refer to 2D & 3D, not the Third Dawn expansion
        string? pathKR = GetPath(@"Origin Worlds Online\Ultima Online\KR Legacy Beta", "ExePath"); //After KR, This is the new registry key for the 2D client
        string? pathSA = GetPath(@"Electronic Arts\EA Games\Ultima Online Stygian Abyss Classic", "InstallDir");
        string? pathHS = GetPath(@"Electronic Arts\EA Games\Ultima Online Classic", "InstallDir");

        if (pathUO != null)
            Core.Directories.Add(pathUO);
        if (pathTD != null)
            Core.Directories.Add(pathTD);
        if (pathKR != null)
            Core.Directories.Add(pathKR);
        if (pathSA != null)
            Core.Directories.Add(pathSA);
        if (pathHS != null)
            Core.Directories.Add(pathHS);
    }
}
