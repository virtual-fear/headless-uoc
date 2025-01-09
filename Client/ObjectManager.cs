namespace Client;
using Client.Game.Context;
public class ObjectManager
{
    private static readonly Type _typeMobile = typeof(MobileContext);
    private static readonly Type _typeItem = typeof(Item);

    private static Dictionary<Type, Dictionary<int, object>> m_Types;
    private static Dictionary<int, object> Find(Type type)
    {
        if (m_Types == null)
            m_Types = new Dictionary<Type, Dictionary<int, object>>();

        if (m_Types[type] == null)
            m_Types[type] = new Dictionary<int, object>();

        return m_Types[type];
    }

    private static object Find(Type type, int serial)
    {
        Dictionary<int, object> types = Find(type);

        object o = null;

        if (types.ContainsKey(serial))
            o = types[serial];

        return o;
    }

    public object this[Type type, int serial]
    {
        get { return Find(type, serial); }
        set
        {
            Dictionary<int, object> types = Find(type);

            if (value == null)
                return;

            if (!types.ContainsKey(serial))
                types.Add(serial, null);

            types[serial] = value;
        }
    }

    static ObjectManager()
    {
        m_Types = new Dictionary<Type, Dictionary<int, object>>();

        if (Find(_typeMobile) == null)
            Logger.Log("Warning: ObjectManager could not find type: Mobile");
        if (Find(_typeItem) == null)
            Logger.Log("Warning: ObjectManager could not find type: Item");
    }
}