namespace Client.Game.Context;

using Client.Accounting;
using Client.Game.Data;

public class ContextEntity : IEntity
{
    private ContextEntity? m_Parent;
    public int Serial { get; }
    public short X { get; private set; }
    public short Y { get; private set; }
    public sbyte Z { get; private set; }
    public ContextEntity Parent
    {
        get => m_Parent;
        private set
        {
            m_Parent = value;
            OnParentChanged();
        }
    }
    public Dictionary<int, ContextEntity> Children { get; } = new();
    public ContextEntity(int serial) => Serial = serial;
    public ContextEntity? WorldRoot
    {
        get
        {
            for (ContextEntity e = this; e.Parent != null; e = e.Parent)
            {
                if (e is ContextEntity && e.InWorld)
                    return e as ContextEntity;
            }
            return null;
        }
    }
    public bool InWorld => m_Parent is WorldContext;
    protected virtual void Attached(ContextEntity child) { }
    protected virtual void Detached(ContextEntity child) { }
    protected virtual void OnChangedLocation() { }
    protected virtual void OnParentChanged() { }
    protected virtual void OnDelete() { }
    private void Attach(ContextEntity child)
    {
        if (child.Parent != this)
        {
            if (child.Parent != null)
                child.Parent.Detach(child);

            if (child is Item ||
                child is MobileContext)
            {
                Children[child.Serial] = child;
            }

            Attached(child);
            child.Parent = this;
        }
    }
    private void Detach(ContextEntity child)
    {
        if (child.Parent == this)
        {
            if (Children.ContainsKey(child.Serial))
                Children.Remove(child.Serial);

            Detach(child);
            child.Parent = null;
        }
    }
    public int DistanceTo(int x, int y)
    {
        x = X - x;
        y = Y - y;

        return (int)Math.Sqrt(x * x + y * y);
    }
    public bool IsChildOf(ContextEntity agent)
    {
        if (agent != null)
        {
            for (ContextEntity i = m_Parent; i != null; i = i.Parent)
                if (i == agent)
                    return true;
        }
        return false;
    }
    public IAccount? Account { get; set; } = null;
    public bool IsDeleted { get; set; } = false;
    public void Delete()
    {
        OnDelete();

        List<ContextEntity> entities = new List<ContextEntity>(Children.Values);

        while (entities.Count > 0)
            entities[entities.Count - 1].Delete();

        Children.Clear();
        IsDeleted = true;
    }
    public void SetLocation(short x, short y, sbyte z)
    {
        X = x;
        Y = y;
        Z = z;

        OnChangedLocation();
    }
    public void SetParent(ContextEntity parent)
    {
        if (Parent != parent)
        {
            if (Parent != null)
                Parent.Detach(this);

            if (parent != null)
                parent.Attach(this);
        }
    }
}