//using System.Collections.Immutable;

//using CommonFramework;

//using Framework.Subscriptions.Domain;

//namespace Framework.Subscriptions.Metadata;

//public class RecipientObjectsVersion : IObjectsVersion<object>
//{
//    public RecipientObjectsVersion(object? previous, object? current, IEnumerable<IEmployee> recipients)
//    {
//        if (previous == null && current == null)
//        {
//            throw new ArgumentException("both arguments (previous and current) can't be null");
//        }

//        this.Previous = previous;
//        this.Current = current;
//        this.Recipients = [..recipients];
//    }

//    public RecipientObjectsVersion(object? previous, object? current, params IEmployee[] recipients)
//        : this(previous, current, (IEnumerable<IEmployee>)recipients)
//    {
//    }


//    public object? Previous { get; }

//    public object? Current { get; }

//    public ImmutableArray<IEmployee> Recipients { get; }


//    public IEnumerable<RecipientObjectsVersion> SplitByRecipient()
//    {
//        if (this.Recipients.Any())
//        {
//            return from recipient in this.Recipients

//                   select this.OverrideRecipient(recipient);
//        }
//        else
//        {
//            return [this];
//        }
//    }


//    protected virtual RecipientObjectsVersion OverrideRecipient(IEmployee recipient) => new(this.Previous, this.Current, recipient);

//    public static RecipientObjectsVersion CreateDynamic<T>(T previous, T current, params IEmployee[] recipients) => CreateDynamic(previous, current, (IEnumerable<IEmployee>)recipients);

//    public static RecipientObjectsVersion<T> CreateDynamic<T>(T previous, T current, IEnumerable<IEmployee> recipients)
//    {
//        if (previous == null && current == null)
//        {
//            throw new ArgumentException("both arguments (previous and current) can't be null");
//        }

//        if (typeof(T) == typeof(object))
//        {
//            var superType = (previous == null ? current!.GetType()
//                             : current == null ? previous.GetType()
//                             : current.GetType().GetSuperSet(previous.GetType(), true)!);

//            var ctor = typeof(RecipientObjectsVersion<>).MakeGenericType(superType).GetConstructor([superType, superType, typeof(IEnumerable<IEmployee>)])!;

//            return (RecipientObjectsVersion<T>)ctor.Invoke([previous, current, recipients]);
//        }
//        else
//        {
//            return new RecipientObjectsVersion<T>(previous, current, recipients);
//        }
//    }
//}

//public class RecipientObjectsVersion<T>(T? previous, T? current, IEnumerable<IEmployee> recipients) : RecipientObjectsVersion(previous, current, recipients), IObjectsVersion<T>
//{
//    public RecipientObjectsVersion(T? previous, T? current, params IEmployee[] recipients)
//            : this(previous, current, (IEnumerable<IEmployee>)recipients)
//    {
//    }


//    public new T? Previous => (T?)base.Previous;

//    public new T? Current => (T?)base.Current;

//    protected override RecipientObjectsVersion OverrideRecipient(IEmployee recipient) => new RecipientObjectsVersion<T>(this.Previous, this.Current, recipient);
//}
