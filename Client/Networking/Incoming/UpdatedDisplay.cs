using Client.Game;
using Client.Game.Data;

namespace Client.Networking.Incoming;

using static PacketSink;
public partial class PacketSink
{
    #region EventArgs

    public sealed class DisplayPaperdollEventArgs : EventArgs
    {
        public NetState State { get; }
        public DisplayPaperdollEventArgs(NetState state) => State = state;
        public Mobile Mobile { get; set; }
        public string Text { get; set; }
        public bool Draggable { get; set; }
    }
    public sealed class DisplayProfileEventArgs : EventArgs
    {
        public NetState State { get; }
        public DisplayProfileEventArgs(NetState state) => State = state;
        public Mobile Mobile { get; set; }
        public string Header { get; set; }
        public string Footer { get; set; }
        public string Body { get; set; }
    }
    public sealed class DisplayQuestionMenuEventArgs : EventArgs
    {
        public NetState State { get; }
        public DisplayQuestionMenuEventArgs(NetState state) => State = state;
        public int MenuSerial { get; set; }
        public string Question { get; set; }
        public string[] Answers { get; set; }
    }
    public sealed class DisplayHuePickerEventArgs : EventArgs
    {
        public NetState State { get; }
        public DisplayHuePickerEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public short ItemID { get; set; }
    }
    public sealed class DisplayGumpEventArgs : EventArgs
    {
        public NetState State { get; }
        public DisplayGumpEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public int TypeID { get; set; }
        public int GumpX { get; set; }
        public int GumpY { get; set; }
        public string Layout { get; set; }
        public string[] Text { get; set; }
        public bool Packed { get; set; }
    }
    public sealed class DisplayEquipmentInfoEventArgs : EventArgs
    {
        private List<EquipInfoAttribute> m_Attributes = new List<EquipInfoAttribute>();
        public NetState State { get; }
        public DisplayEquipmentInfoEventArgs(NetState state) => State = state;
        public IEnumerable<EquipInfoAttribute> Attributes => m_Attributes;
        public int Item { get; set; }
        public int Number { get; set; }
        public bool Identified { get; set; }
        public string Name { get; set; }
        public void AddAttribute(EquipInfoAttribute attribute)
        {
            if (attribute == null)
                return;

            m_Attributes.Add(attribute);
        }
    }
    public sealed class DisplayContextMenuEventArgs : EventArgs
    {
        public NetState State { get; }
        public DisplayContextMenuEventArgs(NetState state) => State = state;
        public int MenuTarget { get; set; }
        public ContextMenuEntry[] Entries { get; set; }
    }

    #endregion (done)

    public static event PacketEventHandler<DisplayPaperdollEventArgs>? DisplayPaperdoll;
    public static event PacketEventHandler<DisplayProfileEventArgs>? DisplayProfile;
    public static event PacketEventHandler<DisplayQuestionMenuEventArgs>? DisplayQuestionMenu;
    public static event PacketEventHandler<DisplayHuePickerEventArgs>? DisplayHuePicker;
    public static event PacketEventHandler<DisplayGumpEventArgs>? DisplayGump;
    public static event PacketEventHandler<DisplayEquipmentInfoEventArgs>? DisplayEquipmentInfo;
    public static event PacketEventHandler<DisplayContextMenuEventArgs>? DisplayContextMenu;
    public static void InvokeDisplayContextMenu(DisplayContextMenuEventArgs e) => DisplayContextMenu?.Invoke(e);
    public static void InvokeDisplayEquipmentInfo(DisplayEquipmentInfoEventArgs e) => DisplayEquipmentInfo?.Invoke(e);
    public static void InvokeDisplayGump(DisplayGumpEventArgs e) => DisplayGump?.Invoke(e);
    public static void InvokeDisplayHuePicker(DisplayHuePickerEventArgs e) => DisplayHuePicker?.Invoke(e);
    public static void InvokeDisplayQuestionMenu(DisplayQuestionMenuEventArgs e) => DisplayQuestionMenu?.Invoke(e);
    public static void InvokeDisplayProfile(DisplayProfileEventArgs e) => DisplayProfile?.Invoke(e);
    public static void InvokeDisplayPaperdoll(DisplayPaperdollEventArgs e) => DisplayPaperdoll?.Invoke(e);

}


/// <summary>
/// Represents a single entry of a <see cref="ContextMenu">context menu</see>.
/// <seealso cref="ContextMenu" />
/// </summary>
public class ContextMenuEntry
{
    private int m_Number;
    private int m_Color;
    private bool m_Enabled;
    private int m_Range;
    private CMEFlags m_Flags;
    private ContextMenu m_Owner;

    /// <summary>
    /// Gets or sets additional <see cref="CMEFlags">flags</see> used in client communication.
    /// </summary>
    public CMEFlags Flags
    {
        get { return m_Flags; }
        set { m_Flags = value; }
    }

    /// <summary>
    /// Gets or sets the <see cref="ContextMenu" /> that owns this entry.
    /// </summary>
    public ContextMenu Owner
    {
        get { return m_Owner; }
        set { m_Owner = value; }
    }

    /// <summary>
    /// Gets or sets the localization number containing the name of this entry.
    /// </summary>
    public int Number
    {
        get { return m_Number; }
        set { m_Number = value; }
    }

    /// <summary>
    /// Gets or sets the maximum range at which this entry may be used, in tiles. A value of -1 signifies no maximum range.
    /// </summary>
    public int Range
    {
        get { return m_Range; }
        set { m_Range = value; }
    }

    /// <summary>
    /// Gets or sets the color for this entry. Format is A1-R5-G5-B5.
    /// </summary>
    public int Color
    {
        get { return m_Color; }
        set { m_Color = value; }
    }

    /// <summary>
    /// Gets or sets whether this entry is enabled. When false, the entry will appear in a gray hue and <see cref="OnClick" /> will never be invoked.
    /// </summary>
    public bool Enabled
    {
        get { return m_Enabled; }
        set { m_Enabled = value; }
    }

    /// <summary>
    /// Gets a value indicating if non local use of this entry is permitted.
    /// </summary>
    public virtual bool NonLocalUse
    {
        get { return false; }
    }

    /// <summary>
    /// Instantiates a new ContextMenuEntry with a given <see cref="Number">localization number</see> (<paramref name="number" />). No <see cref="Range">maximum range</see> is used.
    /// </summary>
    /// <param name="number">
    /// The localization number containing the name of this entry.
    /// <seealso cref="Number" />
    /// </param>
    public ContextMenuEntry(int number)
        : this(number, -1)
    {
    }

    /// <summary>
    /// Instantiates a new ContextMenuEntry with a given <see cref="Number">localization number</see> (<paramref name="number" />) and <see cref="Range">maximum range</see> (<paramref name="range" />).
    /// </summary>
    /// <param name="number">
    /// The localization number containing the name of this entry.
    /// <seealso cref="Number" />
    /// </param>
    /// <param name="range">
    /// The maximum range at which this entry can be used.
    /// <seealso cref="Range" />
    /// </param>
    public ContextMenuEntry(int number, int range)
    {
        if (number <= 0x7FFF) // Legacy code support
            m_Number = 3000000 + number;
        else
            m_Number = number;

        m_Range = range;
        m_Enabled = true;
        m_Color = 0xFFFF;
    }

    /// <summary>
    /// Overridable. Virtual event invoked when the entry is clicked.
    /// </summary>
    public virtual void OnClick()
    {
    }
}

/// <summary>
/// Represents the state of an active context menu. This includes who opened the menu, the menu's focus object, and a list of <see cref="ContextMenuEntry">entries</see> that the menu is composed of.
/// <seealso cref="ContextMenuEntry" />
/// </summary>
public class ContextMenu
{
    private Mobile m_From;
    private object m_Target;
    private ContextMenuEntry[] m_Entries;

    /// <summary>
    /// Gets the <see cref="Mobile" /> who opened this ContextMenu.
    /// </summary>
    public Mobile From
    {
        get { return m_From; }
    }

    /// <summary>
    /// Gets an object of the <see cref="Mobile" /> or <see cref="Item" /> for which this ContextMenu is on.
    /// </summary>
    public object Target
    {
        get { return m_Target; }
    }

    /// <summary>
    /// Gets the list of <see cref="ContextMenuEntry">entries</see> contained in this ContextMenu.
    /// </summary>
    public ContextMenuEntry[] Entries
    {
        get { return m_Entries; }
    }

    /// <summary>
    /// Instantiates a new ContextMenu instance.
    /// </summary>
    /// <param name="from">
    /// The <see cref="Mobile" /> who opened this ContextMenu.
    /// <seealso cref="From" />
    /// </param>
    /// <param name="target">
    /// The <see cref="Mobile" /> or <see cref="Item" /> for which this ContextMenu is on.
    /// <seealso cref="Target" />
    /// </param>
    public ContextMenu(Mobile from, object target)
    {
        m_From = from;
        m_Target = target;

        List<ContextMenuEntry> list = new List<ContextMenuEntry>();

        if (target is Mobile)
        {
            //((Mobile)target).GetContextMenuEntries(from, list);
        }
        else if (target is Item)
        {
            //((Item)target).GetContextMenuEntries(from, list);
        }

        //m_Entries = (ContextMenuEntry[])list.ToArray( typeof( ContextMenuEntry ) );

        m_Entries = list.ToArray();

        for (int i = 0; i < m_Entries.Length; ++i)
        {
            m_Entries[i].Owner = this;
        }
    }

    /// <summary>
    /// Returns true if this ContextMenu requires packet version 2.
    /// </summary>
    public bool RequiresNewPacket
    {
        get
        {
            for (int i = 0; i < m_Entries.Length; ++i)
            {
                if (m_Entries[i].Number < 3000000 || m_Entries[i].Number > 3032767)
                    return true;
            }

            return false;
        }
    }
}
public class EquipInfoAttribute
{
    private int m_Number;
    private int m_Charges;

    public int Number { get { return m_Number; } }
    public int Charges { get { return m_Charges; } }

    public EquipInfoAttribute(int number)
        : this(number, -1)
    {
    }

    public EquipInfoAttribute(int number, int charges)
    {
        m_Number = number;
        m_Charges = charges;
    }
}


public static class UpdatedDisplay
{
    public static void Configure()
    {
        Register(0x88, 66, true, new OnPacketReceive(DisplayPaperdoll));
        Register(0xB8, -1, true, new OnPacketReceive(DisplayProfile));
        Register(0x7C, -1, true, new OnPacketReceive(DisplayQuestionMenu));
        Register(0x95, 09, true, new OnPacketReceive(DisplayHuePicker));
        Register(0xB0, -1, true, new OnPacketReceive(DisplayGump));
        Register(0xDD, -1, true, new OnPacketReceive(DisplayGumpPacked));
        RegisterExtended(0x10, -1, true, new OnPacketReceive(Extended_DisplayEquipmentInfo));
        RegisterExtended(0x14, -1, true, new OnPacketReceive(Extended_DisplayContextMenu));
    }

    private static void Extended_DisplayContextMenu(NetState ns, PacketReader pvSrc)
    {
        DisplayContextMenuEventArgs e = new DisplayContextMenuEventArgs(ns);

        pvSrc.ReadInt16();

        e.MenuTarget = pvSrc.ReadInt32();
        ContextMenuEntry[] entries = new ContextMenuEntry[pvSrc.ReadByte()];
        for (int i = 0; i < entries.Length; ++i)
        {
            ContextMenuEntry ent = new ContextMenuEntry(pvSrc.ReadInt32());

            entries[pvSrc.ReadInt16()] = ent;

            ent.Flags = (CMEFlags)pvSrc.ReadInt16();

        }
        e.Entries = entries;

        PacketSink.InvokeDisplayContextMenu(e);
    }

    private static void Extended_DisplayEquipmentInfo(NetState ns, PacketReader pvSrc)
    {
        DisplayEquipmentInfoEventArgs e = new DisplayEquipmentInfoEventArgs(ns);

        e.Item = pvSrc.ReadInt32();
        e.Number = pvSrc.ReadInt32();
        e.Identified = (pvSrc.ReadInt32() != -3);

        if (e.Identified)
            e.Name = pvSrc.ReadString(pvSrc.ReadUInt16());

        int n, c;

        while (pvSrc.ReadInt32() != -1)
        {
            pvSrc.Seek(-4, SeekOrigin.Current);

            n = pvSrc.ReadInt32();
            c = pvSrc.ReadInt16();

            e.AddAttribute(new EquipInfoAttribute(n, c));
        }

        PacketSink.InvokeDisplayEquipmentInfo(e);
    }
    private static void DisplayGumpPacked(NetState ns, PacketReader pvSrc)
    {
        DisplayGumpEventArgs e = new DisplayGumpEventArgs(ns);
        e.Serial = pvSrc.ReadInt32();
        e.TypeID = pvSrc.ReadInt32();
        e.GumpX = pvSrc.ReadInt32();
        e.GumpY = pvSrc.ReadInt32();
        PacketReader pvComp = Game.Gumps.GetCompressedReader(pvSrc);
        e.Layout = pvComp.ReadString();
        string[] text = new string[pvSrc.ReadInt32()];
        for (int i = 0; i < text.Length; ++i)
        {
            int l;
            string v;

            l = pvSrc.ReadUInt16();
            v = pvSrc.ReadUnicodeString(l);

            text[i] = v;
        }
        e.Text = text;
        e.Packed = true;
        PacketSink.InvokeDisplayGump(e);
    }
    private static void DisplayGump(NetState ns, PacketReader pvSrc)
    {
        DisplayGumpEventArgs e = new DisplayGumpEventArgs(ns);
        e.Serial = pvSrc.ReadInt32();
        e.TypeID = pvSrc.ReadInt32();
        e.GumpX = pvSrc.ReadInt32();
        e.GumpY = pvSrc.ReadInt32();
        e.Layout = pvSrc.ReadString(pvSrc.ReadUInt16());
        string[] text = new string[pvSrc.ReadUInt16()];
        for (int i = 0; i < text.Length; ++i)
        {
            int l;
            string v;

            l = pvSrc.ReadUInt16();
            v = pvSrc.ReadUnicodeString(l);

            text[i] = v;
        }
        e.Text = text;
        PacketSink.InvokeDisplayGump(e);
    }
    private static void DisplayHuePicker(NetState ns, PacketReader pvSrc)
    {
        DisplayHuePickerEventArgs e = new DisplayHuePickerEventArgs(ns);

        e.Serial = pvSrc.ReadInt32();

        pvSrc.ReadInt16();

        e.ItemID = pvSrc.ReadInt16();

        PacketSink.InvokeDisplayHuePicker(e);
    }
    private static void DisplayQuestionMenu(NetState ns, PacketReader pvSrc)
    {
        DisplayQuestionMenuEventArgs e = new DisplayQuestionMenuEventArgs(ns);

        // EnsureCapacity( 256 )

        e.MenuSerial = pvSrc.ReadInt32();
        pvSrc.ReadInt16();  //  0

        byte length;
        string question;
        string[] answers;

        length = pvSrc.ReadByte();
        if (length != 0)
            question = pvSrc.ReadString(length);
        else
            question = string.Empty;

        length = pvSrc.ReadByte();
        if (length > 0)
        {
            answers = new string[length];
            for (int i = 0; i < answers.Length; ++i)
            {
                pvSrc.ReadInt32();
                length = pvSrc.ReadByte();
                answers[i] = pvSrc.ReadString(length);
            }
        }
        else
        {
            answers = new string[0];
        }

        e.Question = question;
        e.Answers = answers;

        PacketSink.InvokeDisplayQuestionMenu(e);
    }
    private static void DisplayProfile(NetState ns, PacketReader pvSrc)
    {
        DisplayProfileEventArgs e = new DisplayProfileEventArgs(ns);

        e.Mobile = Mobile.Acquire(pvSrc.ReadInt32());
        e.Header = pvSrc.ReadString();
        e.Footer = pvSrc.ReadUnicodeString();
        e.Body = pvSrc.ReadUnicodeString();

        PacketSink.InvokeDisplayProfile(e);
    }
    private static void DisplayPaperdoll(NetState ns, PacketReader pvSrc)
    {
        DisplayPaperdollEventArgs e = new DisplayPaperdollEventArgs(ns);

        e.Mobile = Mobile.Acquire(pvSrc.ReadInt32());
        e.Text = pvSrc.ReadString(60);

        bool canLift = (pvSrc.ReadByte() & 2) != 0;

        e.Draggable = canLift;

        PacketSink.InvokeDisplayPaperdoll(e);
    }
    private static void RegisterExtended(int packetID, int length, bool ingame, OnPacketReceive receive)
    {
        PacketHandlers.RegisterExtended(packetID, length, ingame, receive);
    }
    private static void Register(int packetID, int length, bool ingame, OnPacketReceive receive)
    {
        PacketHandlers.Register(packetID, length, ingame, receive);
    }
}
