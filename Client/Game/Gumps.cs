using System.IO;
using System;

namespace Client.Game
{
    using Compression;
    using Networking;

    public static class Gumps
    {
        public const bool MoongateConfirmation = false;

        private static readonly string[] m_Strings = new string[] {
            "Dost thou wish to step into the moongate? Continue to enter the gate, Cancel to stay here",
        };

        private static byte[] m_CompBuffer;

        public static void HandleGump(int serial, int dialog, int xOffset, int yOffset, string layout, string[] text)
        {
            TextWriter output = Console.Out;

            if ((text.Length > 0) && (text[0] == m_Strings[0]) && MoongateConfirmation)
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

        public static PacketReader GetCompressedReader(PacketReader pvSrc)
        {
            if (m_CompBuffer == null)
                m_CompBuffer = new byte[0x1000];  //  4096

            int compressedLength = pvSrc.ReadInt32();
            if (compressedLength == 0)
                return new PacketReader(m_CompBuffer, index: 0, size: 3, false, 0x00, "Gump Subset");

            int decompressedLength = pvSrc.ReadInt32();
            if (decompressedLength == 0)
                return new PacketReader(m_CompBuffer, index: 0, size: 3, false, 0x00, "Gump Subset");

            byte[] buffer = pvSrc.ReadBytes(compressedLength - 4);
            if (decompressedLength > m_CompBuffer.Length)
                m_CompBuffer = new byte[(decompressedLength + 0xFFFF) & -4096]; // 4095

            ZLib.Unpack(m_CompBuffer, ref decompressedLength, buffer, buffer.Length);
            return new PacketReader(m_CompBuffer, index: 0, size: decompressedLength, true, 0x00, "Gump Subset");
        }
    }
}