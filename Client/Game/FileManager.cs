using System;
using System.IO;

namespace Client.Game
{
    using FileAccess = System.IO.FileAccess;
    public class FileManager
    {
        private static FileManager m_Base;
        private static FileManager m_Game;

        public static FileManager Base => m_Base;
        public static FileManager Game => m_Game;

        private string m_Path;
        private bool m_CanWrite;

        public string CurrentPath => m_Path;
        public bool CanWrite
        {
            get => m_CanWrite;
            private set => m_CanWrite = value;
        }

        protected FileManager(string path)
        {
            m_Path = path;
            m_CanWrite = false;
        }

        public Stream Create(string path, string ext)
        {
            ext = ext.Trim('.');
            path = Resolve($"{path}.{ext}");
            for (int i = 0; i < 10; ++i)
            {
                try
                {
                    return new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read);
                }
                catch
                {
                    path = $"{path}.{i}.{ext}";
                }
            }
            return Stream.Null;
        }

        //public static void LoadDialog(FileDialog fileDialog)
        //{
        //    fileDialog.FileMode = FileDialog.FileModeEnum.OpenFile;
        //    fileDialog.Access = FileDialog.AccessEnum.Filesystem;
        //    fileDialog.Filters = new string[] { "*.exe ; Client.exe" };
        //    fileDialog.Title = "Find your UO directory";
        //    fileDialog.CurrentDir = Path.GetPathRoot(m_Base.CurrentPath);
        //    fileDialog.PopupCentered();
        //}

        public static Stream OpenMUL(Files file)
        {
            return m_Game.OpenRead(file);
        }

        public Stream OpenRead(Files file)
        {
            return OpenRead(Path.Combine(m_Path, Config.GetFile(file)));
        }

        protected static Stream OpenRead(string path)
        {
            return File.OpenRead(path);
        }

        public string Resolve(string path)
        {
            return Path.Combine(m_Path, path);
        }

        public static string Resolve(Files file)
        {
            return m_Game.Resolve(Config.GetFile(file));
        }

        static FileManager()
        {
            m_Base = new FileManager(AppDomain.CurrentDomain.BaseDirectory);
            m_Base.CanWrite = true;
            m_Game = new FileManager(RegistryManager.ClientPath);
        }
    }
}
