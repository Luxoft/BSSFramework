using System.Collections.ObjectModel;
using System.Runtime.Serialization;

using Framework.Core;
using Framework.QueryLanguage;

namespace Framework.OData;

[DataContract]
public class SelectOperation : IDynamicSelectOperation, IEquatable<SelectOperation>
{
    public SelectOperation(LambdaExpression filter, IEnumerable<SelectOrder> orders, IEnumerable<LambdaExpression> expands, IEnumerable<LambdaExpression> selects, int skipCount, int takeCount)
    {
        if (filter == null) throw new ArgumentNullException(nameof(filter));
        if (orders == null) throw new ArgumentNullException(nameof(orders));
        if (expands == null) throw new ArgumentNullException(nameof(expands));
        if (selects == null) throw new ArgumentNullException(nameof(selects));

        this.Filter = filter;
        this.Orders = orders.CheckNotNull().ToReadOnlyCollection();
        this.Expands = expands.CheckNotNull().ToReadOnlyCollection();
        this.Selects = selects.CheckNotNull().ToReadOnlyCollection();
        this.SkipCount = skipCount;
        this.TakeCount = takeCount;
    }


    [DataMember]
    public LambdaExpression Filter { get; private set; }

    [DataMember]
    public ReadOnlyCollection<SelectOrder> Orders { get; private set; }

    [DataMember]
    public ReadOnlyCollection<LambdaExpression> Expands { get; private set; }

    [DataMember]
    public ReadOnlyCollection<LambdaExpression> Selects { get; private set; }

    [DataMember]
    public int SkipCount { get; private set; }

    [DataMember]
    public int TakeCount { get; private set; }


    public SelectOperation ToCountOperation()
    {
        return new SelectOperation(this.Filter, Default.Orders, Default.Expands, Default.Selects, Default.SkipCount, Default.TakeCount);
    }


    public override bool Equals(object obj)
    {
        return this.Equals(obj as SelectOperation);
    }

    public bool Equals(SelectOperation other)
    {
        return !ReferenceEquals(other, null)
               && this.Filter == other.Filter
               && this.Orders.SequenceEqual(other.Orders)
               && this.Expands.SequenceEqual(other.Expands)
               && this.Selects.SequenceEqual(other.Selects)
               && this.SkipCount == other.SkipCount
               && this.TakeCount == other.TakeCount;
    }

    public override int GetHashCode()
    {
        return this.Orders.Count ^ this.Expands.Count ^ this.Selects.Count ^ this.SkipCount ^ this.TakeCount;
    }



    public static SelectOperation CreateFilter<TSource>(System.Linq.Expressions.Expression<Func<TSource, bool>> filter)
    {
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        return CreateFilter((System.Linq.Expressions.LambdaExpression)filter);
    }

    public static SelectOperation CreateFilter(System.Linq.Expressions.LambdaExpression filter)
    {
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        return new SelectOperation(new LambdaExpression(filter), Default.Orders, Default.Expands, Default.Selects, Default.SkipCount, Default.TakeCount);
    }


    public static SelectOperation Parse(string text)
    {
        return SelectOperationParser.Default.Parse(text);
    }


    public static readonly SelectOperation Default = new SelectOperation(

                                                                         new LambdaExpression(BooleanConstantExpression.True, new[] { ParameterExpression.Default }),

                                                                         new SelectOrder[0],

                                                                         new LambdaExpression[0],

                                                                         new LambdaExpression[0],

                                                                         0,

                                                                         int.MaxValue);


    #region IDynamicSelectOperation Members

    IEnumerable<LambdaExpression> IDynamicSelectOperation.Expands
    {
        get { return this.Expands; }
    }

    IEnumerable<LambdaExpression> IDynamicSelectOperation.Selects
    {
        get { return this.Selects; }
    }

    #endregion
}
