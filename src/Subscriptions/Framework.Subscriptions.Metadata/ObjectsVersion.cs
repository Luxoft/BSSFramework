using System.Collections.Immutable;

using CommonFramework;

using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions.Metadata;

public class ObjectsVersion : IObjectsVersion<object>
{
    public ObjectsVersion(object? previous, object? current, IEnumerable<IEmployee> recipients)
    {
        if (previous == null && current == null)
        {
            throw new ArgumentException("both arguments (previous and current) can't be null");
        }

        this.Previous = previous;
        this.Current = current;
        this.Recipients = [..recipients];
    }

    public ObjectsVersion(object? previous, object? current, params IEmployee[] recipients)
        : this(previous, current, (IEnumerable<IEmployee>)recipients)
    {
    }


    public object? Previous { get; }

    public object? Current { get; }

    public ImmutableArray<IEmployee> Recipients { get; }


    public IEnumerable<ObjectsVersion> SplitByRecipient()
    {
        if (this.Recipients.Any())
        {
            return from recipient in this.Recipients

                   select this.OverrideRecipient(recipient);
        }
        else
        {
            return [this];
        }
    }


    protected virtual ObjectsVersion OverrideRecipient(IEmployee recipient) => new(this.Previous, this.Current, recipient);

    public static ObjectsVersion CreateDynamic<T>(T previous, T current, params IEmployee[] recipients) => CreateDynamic(previous, current, (IEnumerable<IEmployee>)recipients);

    public static ObjectsVersion<T> CreateDynamic<T>(T previous, T current, IEnumerable<IEmployee> recipients)
    {
        if (previous == null && current == null)
        {
            throw new ArgumentException("both arguments (previous and current) can't be null");
        }

        if (typeof(T) == typeof(object))
        {
            var superType = (previous == null ? current!.GetType()
                             : current == null ? previous.GetType()
                             : current.GetType().GetSuperSet(previous.GetType(), true)!);

            var ctor = typeof(ObjectsVersion<>).MakeGenericType(superType).GetConstructor([superType, superType, typeof(IEnumerable<IEmployee>)])!;

            return (ObjectsVersion<T>)ctor.Invoke([previous, current, recipients]);
        }
        else
        {
            return new ObjectsVersion<T>(previous, current, recipients);
        }
    }
}

public class ObjectsVersion<T>(T? previous, T? current, IEnumerable<IEmployee> recipients) : ObjectsVersion(previous, current, recipients), IObjectsVersion<T>
{
    public ObjectsVersion(T? previous, T? current, params IEmployee[] recipients)
            : this(previous, current, (IEnumerable<IEmployee>)recipients)
    {
    }


    public new T? Previous => (T?)base.Previous;

    public new T? Current => (T?)base.Current;

    protected override ObjectsVersion OverrideRecipient(IEmployee recipient) => new ObjectsVersion<T>(this.Previous, this.Current, recipient);
}
