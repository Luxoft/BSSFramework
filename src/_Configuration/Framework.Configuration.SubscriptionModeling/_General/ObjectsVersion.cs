using Framework.Core;
using Framework.Notification;

namespace Framework.Configuration;

public class ObjectsVersion : IObjectsVersion
{
    public ObjectsVersion(object previous, object current, IEnumerable<IEmployee> recipients)
    {
        if (previous == null && current == null)
        {
            throw new ArgumentException("both arguments (previous and current) can't be null");
        }

        this.Previous = previous;
        this.Current = current;
        this.Recipients = recipients.EmptyIfNull().ToList();
    }

    public ObjectsVersion(object previous, object current, params IEmployee[] recipients)
            : this(previous, current, (IEnumerable<IEmployee>)recipients)
    {
    }


    public object Previous { get; private set; }

    public object Current { get; private set; }

    public IEnumerable<IEmployee> Recipients { get; private set; }


    public IEnumerable<ObjectsVersion> SplitByRecipient()
    {
        if (this.Recipients.Any())
        {
            return from recipient in this.Recipients

                   select this.OverrideRecipient(recipient);
        }
        else
        {
            return new[] { this };
        }
    }


    protected virtual ObjectsVersion OverrideRecipient(IEmployee recipient)
    {
        return new ObjectsVersion(this.Previous, this.Current, recipient);
    }

    public static ObjectsVersion CreateDynamic<T>(T previous, T current, params IEmployee[] recipients)
    {
        return CreateDynamic(previous, current, (IEnumerable<IEmployee>)recipients);
    }

    public static ObjectsVersion CreateDynamic<T>(T previous, T current, IEnumerable<IEmployee> recipients)
    {
        if (previous == null && current == null)
        {
            throw new ArgumentException("both arguments (previous and current) can't be null");
        }

        if (typeof(T) == typeof(object))
        {
            var superType = (previous == null ? current.GetType()
                             : current == null ? previous.GetType()
                             : current.GetType().GetSuperSet(previous.GetType(), true));

            var ctor = typeof(ObjectsVersion<>).MakeGenericType(superType).GetConstructor(new[] { superType, superType, typeof(IEnumerable<IEmployee>) });

            return (ObjectsVersion)ctor.Invoke(new object[] { previous, current, recipients });
        }
        else
        {
            return new ObjectsVersion<T>(previous, current, recipients);
        }
    }

    public static ObjectsVersion<T> Create<T>(T previous, T current, params IEmployee[] recipients)
    {
        return Create(previous, current, (IEnumerable<IEmployee>)recipients);
    }

    public static ObjectsVersion<T> Create<T>(T previous, T current, IEnumerable<IEmployee> recipients)
    {
        if (previous == null && current == null)
        {
            throw new ArgumentException("both arguments (previous and current) can't be null");
        }

        return new ObjectsVersion<T>(previous, current, recipients);
    }
}

public class ObjectsVersion<T> : ObjectsVersion, IObjectsVersion<T>
{
    public ObjectsVersion(T previous, T current, IEnumerable<IEmployee> recipients)
            : base(previous, current, recipients)
    {
    }

    public ObjectsVersion(T previous, T current, params IEmployee[] recipients)
            : this(previous, current, (IEnumerable<IEmployee>)recipients)
    {
    }


    public new T Previous
    {
        get { return (T)base.Previous; }
    }

    public new T Current
    {
        get { return (T)base.Current; }
    }


    protected override ObjectsVersion OverrideRecipient(IEmployee recipient)
    {
        return new ObjectsVersion<T>(this.Previous, this.Current, recipient);
    }
}

public interface IObjectsVersion
{
    object Previous { get; }

    object Current { get; }

    IEnumerable<IEmployee> Recipients { get; }
}

public interface IObjectsVersion<out T> : IObjectsVersion
{
    new T Previous { get; }

    new T Current { get; }
}
