namespace Client.Game;
using Client.Game.Data;
public sealed class Point3D : IPoint3D
{
    private short _x, _y;
    private sbyte _z;
    public short X
    {
        get => _x;
        internal set => _x = value;
    }
    public short Y
    {
        get => _y;
        internal set => _y = value;
    }
    public sbyte Z
    {
        get => _z;
        internal set => _z = value;
    }
}

public delegate void EntityEventHandler<T>(IEntity e, T from, T to);
public class Entity : IEntity
{
    public static event EntityEventHandler<Serial>? ChangedSerial;
    public static event EntityEventHandler<IPoint3D>? ChangedLocation;

    private Entity? _owner;
    private Serial _serial;
    private IPoint3D _location = new Point3D();
    public Entity(Serial serial) => _serial = serial;
    public Serial Serial
    {
        get => _serial;
        set
        {
            var from = _serial;
            _serial = value;
            ChangedSerial?.Invoke(this, from, to: value);
        }
    }
    public IPoint3D Location
    {
        get => _location;
        internal set
        {
            if (_location.X == value.X &&
                _location.Y == value.Y &&
                _location.Z == value.Z) 
                return;

            IPoint3D from = _location;
            IPoint3D to = _location = new Point3D()
            {
                X = value.X,
                Y = value.Y,
                Z = value.Z
            };

            OnChangedLocation(from, to);
            ChangedLocation?.Invoke(this, from, to);
        }
    }
    public IAccount? Account { get; set; } = null;
    public Entity? Parent
    {
        get => _owner;
        private set
        {
            _owner = value;
            OnParentChanged();
        }
    }
    public Dictionary<Serial, Entity> Children { get; } = new();
    public Entity? WorldRoot
    {
        get
        {
            for (Entity e = this; e.Parent != null; e = e.Parent)
            {
                if (e is Entity && e.InWorld)
                    return e as Entity;
            }
            return null;
        }
    }
    public bool InWorld => _owner is World;
    protected virtual void Attached(Entity child) { }
    protected virtual void Detached(Entity child) { }
    protected virtual void OnChangedLocation(IPoint3D from, IPoint3D to) { }
    protected virtual void OnParentChanged() { }
    protected virtual void OnDelete() { }
    private void Attach(Entity child)
    {
        if (child.Parent != this)
        {
            if (child.Parent != null)
                child.Parent.Detach(child);

            if (child is Item || child is Mobile)
            {
                Children[child.Serial] = child;
            }

            Attached(child);
            child.Parent = this;
        }
    }
    private void Detach(Entity child)
    {
        if (child.Parent == this)
        {
            if (Children.ContainsKey(child.Serial))
                Children.Remove(child.Serial);

            Detach(child);
            child.Parent = null;
        }
    }
    public bool IsChildOf(Entity agent)
    {
        if (agent != null)
        {
            for (Entity? i = _owner; i != null; i = i.Parent)
                if (i == agent)
                    return true;
        }
        return false;
    }
    public int DistanceTo(int x, int y)
    {
        x = Location.X - x;
        y = Location.Y - y;

        return (int)Math.Sqrt(x * x + y * y);
    }
    public bool IsDeleted { get; set; } = false;
    public void Delete()
    {
        OnDelete();

        List<Entity> entities = new List<Entity>(Children.Values);

        while (entities.Count > 0)
            entities[entities.Count - 1].Delete();

        Children.Clear();
        IsDeleted = true;
    }
    public void SetParent(Entity parent)
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