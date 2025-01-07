namespace Client.Game
{
    public enum Files
    {
        SkillIdx, SkillMul, SoundIdx, SoundMul, LightIdx, LightMul, Fonts, Tiledata,
        AnimIdx, AnimMul, ArtIdx, ArtMul, TexIdx, TexMul, Hues, MultiIdx, MultiMul,
        Map0, Map2, Sta0Mul, Sta2Mul, Sta0Idx, Sta2Idx, Animdata, Verdata, GumpIdx, GumpMul
    }

    public static class Config
    {
        private static Dictionary<Files, string> m_Files;

        public static string GetFile(Files file)
        {
            if (m_Files == null)
            {
                InitializeFiles();
            }
            return m_Files[file];
        }

        private static void InitializeFiles()
        {
            m_Files = new Dictionary<Files, string>
            {
                { Files.SkillIdx, "Skills.idx" },
                { Files.SkillMul, "Skills.mul" },
                { Files.SoundIdx, "SoundIdx.mul" },
                { Files.SoundMul, "Sound.mul" },
                { Files.LightIdx, "LightIdx.mul" },
                { Files.LightMul, "Light.mul" },
                { Files.Fonts, "Fonts.mul" },
                { Files.Tiledata, "TileData.mul" },
                { Files.AnimIdx, "Anim.idx" },
                { Files.AnimMul, "Anim.mul" },
                { Files.ArtIdx, "ArtIdx.mul" },
                { Files.ArtMul, "Art.mul" },
                { Files.TexIdx, "TexIdx.mul" },
                { Files.TexMul, "TexMaps.mul" },
                { Files.Hues, "Hues.mul" },
                { Files.MultiIdx, "Multi.idx" },
                { Files.MultiMul, "Multi.mul" },
                { Files.Map0, "Map0.mul" },
                { Files.Map2, "Map2.mul" },
                { Files.Sta0Mul, "Statics0.mul" },
                { Files.Sta2Mul, "Statics2.mul" },
                { Files.Sta0Idx, "StaIdx0.mul" },
                { Files.Sta2Idx, "StaIdx2.mul" },
                { Files.Animdata, "AnimData.mul" },
                { Files.Verdata, "VerData.mul" },
                { Files.GumpIdx, "GumpIdx.mul" },
                { Files.GumpMul, "GumpArt.mul" }
            };

            if (!File.Exists(FileManager.Resolve(Files.Verdata)))
            {
                m_Files[Files.Verdata] = FileManager.Base.Resolve("data/ultima/empty-verdata.mul");
                if (!File.Exists(m_Files[Files.Verdata]))
                {
                    using (Stream stream = File.Create(m_Files[Files.Verdata], 4))
                    {
                        stream.Write(new byte[4], 0, 4);
                        stream.Flush();
                    }
                }
            }
        }
    }
}
