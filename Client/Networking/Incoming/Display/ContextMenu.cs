namespace Client.Networking.Incoming.Display;
using Client.Game.Context;
using Client.Game.Data;
public partial class PacketHandlers
{
    public static event PacketEventHandler<DisplayContextMenuEventArgs>? DisplayContextMenu;
    public sealed class DisplayContextMenuEventArgs : EventArgs
    {
        public NetState? State { get; }
        public DisplayContextMenuEventArgs(NetState state) => State = state;
        public int MenuTarget { get; set; }
        public ContextMenuEntry[]? Entries { get; set; }
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
        public const bool ExtendedCommand = true;

        private MobileContext m_From;
        private object m_Target;
        private ContextMenuEntry[] m_Entries;

        /// <summary>
        /// Gets the <see cref="MobileContext" /> who opened this ContextMenu.
        /// </summary>
        public MobileContext From
        {
            get { return m_From; }
        }

        /// <summary>
        /// Gets an object of the <see cref="MobileContext" /> or <see cref="ItemContext" /> for which this ContextMenu is on.
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
        /// The <see cref="MobileContext" /> who opened this ContextMenu.
        /// <seealso cref="From" />
        /// </param>
        /// <param name="target">
        /// The <see cref="MobileContext" /> or <see cref="ItemContext" /> for which this ContextMenu is on.
        /// <seealso cref="Target" />
        /// </param>
        public ContextMenu(MobileContext from, object target)
        {
            m_From = from;
            m_Target = target;

            List<ContextMenuEntry> list = new List<ContextMenuEntry>();

            if (target is MobileContext)
            {
                //((Mobile)target).GetContextMenuEntries(from, list);
            }
            else if (target is ItemContext)
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

        [PacketHandler(0x14, length: -1, ingame: true, extCmd: ExtendedCommand)]
        public static void Update(NetState ns, PacketReader pvSrc)
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
            DisplayContextMenu?.Invoke(e);
        }
    }
    public class EquipInfoAttribute
    {
        public int Number { get; }
        public int Charges { get; }
        public EquipInfoAttribute(int number, int charges = -1)
        {
            Number = number;
            Charges = charges;
        }
    }
}