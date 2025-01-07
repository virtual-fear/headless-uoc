using System.Text;
using System.Xml;
namespace Client;
public static class Utility
{
    public static void FormatBuffer(TextWriter output, byte[]? buffer, ConsoleColor color = ConsoleColor.Cyan)
    {
        if (buffer == null)
            color = ConsoleColor.Red;
        var lastColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        if (buffer == null)
        {
            output.WriteLine("NO buffer to format!");
        }
        else
        {
            using Stream input = new MemoryStream(buffer);
            FormatBuffer(output, input, buffer.Length);
        }
        Console.ForegroundColor = lastColor;
    }
    public static void FormatBuffer(TextWriter output, Stream input, int length)
    {
        output.WriteLine("        0  1  2  3  4  5  6  7   8  9  A  B  C  D  E  F");
        output.WriteLine("       -- -- -- -- -- -- -- --  -- -- -- -- -- -- -- --");

        int byteIndex = 0;

        int whole = length >> 4;
        int rem = length & 0xF;

        for (int i = 0; i < whole; ++i, byteIndex += 16)
        {
            StringBuilder bytes = new StringBuilder(49);
            StringBuilder chars = new StringBuilder(16);

            for (int j = 0; j < 16; ++j)
            {
                int c = input.ReadByte();

                bytes.Append(c.ToString("X2"));

                if (j != 7)
                    bytes.Append(' ');
                else
                    bytes.Append("  ");

                if (c >= 0x20 && c < 0x7F)
                    chars.Append((char)c);
                else
                    chars.Append('.');
            }

            output.Write(byteIndex.ToString("X4"));
            output.Write("   ");
            output.Write(bytes.ToString());
            output.Write("  ");
            output.WriteLine(chars.ToString());
        }

        if (rem != 0)
        {
            StringBuilder bytes = new StringBuilder(49);
            StringBuilder chars = new StringBuilder(rem);

            for (int j = 0; j < 16; ++j)
            {
                if (j < rem)
                {
                    int c = input.ReadByte();

                    bytes.Append(c.ToString("X2"));

                    if (j != 7)
                        bytes.Append(' ');
                    else
                        bytes.Append("  ");

                    if (c >= 0x20 && c < 0x7F) // 32 - 126
                        chars.Append((char)c);
                    else
                        chars.Append('.');
                }
                else
                {
                    bytes.Append("   ");
                }
            }

            output.Write(byteIndex.ToString("X4"));
            output.Write("   ");
            output.Write(bytes.ToString());
            output.Write("  ");
            output.WriteLine(chars.ToString());
        }
    }
    public static string GetText(XmlElement node, string defaultValue) => node?.InnerText ?? defaultValue;
    public static int GetInt32(string intValue, int defaultValue)
    {
        int v;
        try
        {
            v = XmlConvert.ToInt32(intValue);
        }
        catch
        {
            if (!int.TryParse(intValue, out v))
                v = defaultValue;
        }
        return v;
    }
    public static string GetAttribute(XmlElement node, string name, string defaultValue)
    {
        if (node == null)
            return defaultValue;

        XmlAttribute attribute = node.Attributes[name];

        if (attribute == null)
            return defaultValue;

        return attribute.Value;
    }
    public static string GetText(DateTime time, string defaultValue) => time.ToBinary().ToString();
}
