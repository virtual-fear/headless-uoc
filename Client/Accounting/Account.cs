namespace Client.Accounting;
using System.Xml;
using Client.Game;
using Client.Game.Data;

public sealed class Account : IAccount, IComparable, IComparable<Account>
{
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public string Username { get; }
    public string Password { get; set; }

    /// <summary>
    ///     Mobile characters held on this account.
    ///     <c>
    ///         <para>DEFAULT   = 5</para>
    ///         <para>AOS       = 6</para>
    ///         <para>SA        = 7</para>
    ///     </c>
    /// </summary>
    public Mobile[]? Mobiles { get; } = new Mobile[5]; // 1, 5, or 7 (all other values are not supported)
    public Account(string username, string password)
    {
        Username = username;
        Password = password;
    }

    /// <summary>
    ///     Gets the maximum value of characters this account can hold.
    /// </summary>
    public int MobileCapacity => Mobiles?.Length ?? 0;

    public int MobileCount
    {
        get
        {
            int c = 0;

            for (int i = 0; i < MobileCapacity; ++i)
            {
                if (this[i] != null)
                    ++c;
            }

            return c;
        }
    }

    /// <summary>
    ///     Gets the maximum a of characters allowed to be created on this account.
    ///     Values other than 1, 5, 7, or 7 are not supported by the client.
    /// </summary>
    public int Limit => 5;
    //get { return (Core.SA ? 7 : Core.AOS ? 6 : 5); }

    /// <summary>
    /// Gets or sets the character at the specified index for this account.
    /// Out of bound index values are handled; null returned for get, ignored for set.
    /// </summary>
    public Mobile? this[int index]
    {
        get
        {
            if (index >= 0 && index < Mobiles?.Length)
            {
                Mobile? m = Mobiles[index];
                if (m != null && m.IsDeleted)
                {
                    m.Account = null;
                    m = null;
                }
                return Mobiles[index] = m;
            }
            return null;
        }
        set
        {
            if (index >= 0 && index < Mobiles?.Length)
            {
                if (Mobiles[index] != null)
                    Mobiles[index].Account = null;

                Mobiles.SetValue(value, index);

                if (Mobiles[index] != null)
                    Mobiles[index].Account = this;
            }
        }
    }

    /// <summary>
    /// Deletes the account, all information of the account.
    /// </summary>
    public void Delete()
    {
        Mobile[]? mobs = Mobiles;
        for (int i = 0; i < mobs?.Length; ++i)
        {
            Mobile? m = mobs?[i];
            if (m == null)
                continue;

            if (m.IsDeleted)
                this[i] = null;
            else
            {
                m.Delete();
                m.Account = null;
            }
        }
        Accounts.Remove(Username);
    }

    /// <summary>
    /// Serializes this account instance to XmlTextWriter.
    /// </summary>
    public void Save(XmlTextWriter xml)
    {
        string un = string.IsNullOrEmpty(Username) ? string.Empty : Username;
        string pw = string.IsNullOrEmpty(Password) ? string.Empty : Password;

        xml.WriteStartElement("account");
        xml.WriteAttributeString("username", un);
        xml.WriteAttributeString("password", pw);
        xml.WriteAttributeString("created", XmlConvert.ToString(CreatedAt, XmlDateTimeSerializationMode.Utc));
        xml.WriteStartElement("mobiles");
        for (int i = 0; i < Mobiles?.Length; ++i)
        {
            Mobile? m = Mobiles[i];
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
        Username = Utility.GetAttribute(node, "username", "empty");
        Password = Utility.GetAttribute(node, "password", "empty");
    }
    public override string ToString() => Username;
    public int CompareTo(object? obj)
    {
        if (obj is Account)
            return CompareTo((Account)obj);

        throw new ArgumentException();
    }
    public int CompareTo(Account? other) => other == null ? 1 : Username.CompareTo(other.Username);
    public int CompareTo(IAccount other) => other == null ? 1 : Username.CompareTo(other.Username);
}
