using System.Threading.Tasks;

internal static class Configs
{
    private static ConfigFile userConfig;

    public static async Task Preload()
    {
        await Task.Run(() => UserConfig);
    }

    public static ConfigFile UserConfig
    {
        get
        {
            if (userConfig != null)
                return userConfig;

            userConfig = new ConfigFile();

            // Load data from a file.
            Error err = userConfig.LoadEncryptedPass("user://userConfig.cfg", System.Environment.MachineName);

            // If the file loaded we can return, otherwise continue to create base data.
            if (err == Error.Ok)
            {
                return userConfig;
            }

            userConfig.SetValue("Main", "addr", "127.0.0.1:2593");
            userConfig.SetValue("Main", "account", string.Empty);
            userConfig.SetValue("Main", "password", string.Empty);

            return userConfig;
        }
    }

    public static async Task SaveUserConfig()
    {
        await Task.Run(() => userConfig.SaveEncryptedPass("user://userConfig.cfg", System.Environment.MachineName));
    }
}