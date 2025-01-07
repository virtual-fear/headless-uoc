using System.Xml;

namespace Client.Accounting
{
    using Client.Game.Context;
    public interface IAccount
    {
        string Username { get; }
        string Password { get; set; }
        MobileAgent this[int index] { get; set; }
        void Delete();
    }

    public interface INode
    {
        void Begin(string key);
        void End();
        string GetAttribute(string key, string def);
        void SetAttribute(string key, string value);
    }

    public class Account : IAccount, IComparable, IComparable<Account>
    {
        private string m_Username;
        private string m_Password;
        private DateTime m_Created;
        private MobileAgent[] m_Mobiles;
        public Account( string username, string password ) {
            m_Username = username;
            m_Password = password;
            m_Created = DateTime.UtcNow;
            m_Mobiles = new MobileAgent[5];
        }

        /// <summary>
        /// Gets the username of this account.
        /// </summary>
        public string Username
        {
            get { return m_Username; }
        }

        /// <summary>
        /// Gets or sets the password of this account.
        /// </summary>
        public string Password
        {
            get { return m_Password; }
            set { m_Password = value; }
        }

        /// <summary>
        /// Gets the current number of characters on this account.
        /// </summary>
        public int Count
        {
            get
            {
                int c = 0;

                for (int i = 0; i < this.Length; ++i)
                {
                    if (this[i] != null)
                        ++c;
                }

                return c;
            }
        }

        /// <summary>
        /// Gets the maximum amount of characters allowed to be created on this account.
        /// Values other than 1, 5, 7, or 7 are not supported by the client.
        /// </summary>
        public int Limit
        {
            get => 5;
            //get { return (Core.SA ? 7 : Core.AOS ? 6 : 5); }
        }

        /// <summary>
        /// Gets the maximum amount of character that this account can hold.
        /// </summary>
        public int Length
        {
            get { return m_Mobiles.Length; }
        }

        /// <summary>
        /// Gets or sets the character at the specified index for this account.
        /// Out of bound index values are handled; null returned for get, ignored for set.
        /// </summary>
        public MobileAgent this[int index]
        {
            get
            {
                if (index >= 0 && index < m_Mobiles.Length)
                {
                    MobileAgent m = m_Mobiles[index];
                    if ((m != null) && m.IsDeleted)
                    {
                        m.Account = null;
                        m = null;
                    }
                    return m_Mobiles[index] = m;
                }
                return null;
            }
            set
            {
                if (index >= 0 && index < m_Mobiles.Length)
                {
                    if (m_Mobiles[index] != null)
                        m_Mobiles[index].Account = null;

                    m_Mobiles[index] = value;

                    if (m_Mobiles[index] != null)
                        m_Mobiles[index].Account = this;
                }
            }
        }

        /// <summary>
        /// Deletes the account, all information of the account.
        /// </summary>
        public void Delete()
        {
            for (int i = 0; i < this.Length; ++i)
            {
                MobileAgent m = this[i];

                if (m == null)
                    continue;

                if (m.IsDeleted)
                {
                    m = this[i] = null;
                    return;
                }

                m.Delete();

                m.Account = null;
                m_Mobiles[i] = null;
            }

            Accounts.Remove(m_Username);
        }

        /// <summary>
        /// Serializes this account instance to XmlTextWriter.
        /// </summary>
        public void Save(XmlTextWriter xml)
        {
            xml.WriteStartElement("account");

            if (string.IsNullOrEmpty(m_Username))
                m_Username = null;

            xml.WriteAttributeString("username", m_Username);

            if (string.IsNullOrEmpty(m_Password))
                m_Password = null;

            xml.WriteAttributeString("password", m_Password);

            xml.WriteAttributeString("created", XmlConvert.ToString(m_Created, XmlDateTimeSerializationMode.Utc));
            //xml.WriteAttributeString("playedLast", XmlConvert.ToString(m_PlayedLast, XmlDateTimeSerializationMode.Utc));

            xml.WriteStartElement("mobiles");

            for (int i = 0; i < m_Mobiles.Length; ++i)
            {
                MobileAgent m = m_Mobiles[i];
                //if ((m == null) ||m.Removed) // m.Deleted
                if (m == null)
                    continue;

                xml.WriteStartElement("char");
                xml.WriteAttributeString("index", i.ToString());
                xml.WriteString(m.Serial.ToString());
                xml.WriteEndElement();
            }

            xml.WriteEndElement();

            xml.WriteEndElement();
        }

        public Account(XmlElement node)
        {
            m_Username = Utility.GetAttribute(node, "username", "empty");
            m_Password = Utility.GetAttribute(node, "password", "empty");
        
        }

        public override string ToString()
        {
            return m_Username;
        }

        public int CompareTo(object obj)
        {
            if (obj is Account)
                return this.CompareTo((Account)obj);

            throw new ArgumentException();
        }

        public int CompareTo(Account other)
        {
            if (other == null)
                return 1;

            return m_Username.CompareTo(other.Username);
        }

        public int CompareTo(IAccount other)
        {
            if (other == null)
                return 1;

            return m_Username.CompareTo(other.Username);
        }
    }
}
