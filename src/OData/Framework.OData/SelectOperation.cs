using System.Collections.Immutable;

using Framework.QueryLanguage;

namespace Framework.OData;

public record SelectOperation(
    LambdaExpression Filter,
    ImmutableArray<SelectOrder> Orders,
    int SkipCount,
    int TakeCount)
    : IDynamicSelectOperation
{
    public ImmutableArray<LambdaExpression> Expands { get; init; } = [];

    public ImmutableArray<LambdaExpression> Selects { get; init; } = [];

    public SelectOperation ToCountOperation() => Default with { Filter = this.Filter };

    public virtual bool Equals(SelectOperation? other) =>
        ReferenceEquals(this, other)

        || (other is not null

            && this.Filter == other.Filter
            && this.Orders.SequenceEqual(other.Orders)
            && this.Expands.SequenceEqual(other.Expands)
            && this.Selects.SequenceEqual(other.Selects)
            && this.SkipCount == other.SkipCount
            && this.TakeCount == other.TakeCount);

    public override int GetHashCode()
    {
        return this.Orders.Length ^ this.Expands.Length ^ this.Selects.Length ^ this.SkipCount ^ this.TakeCount;
    }

    public static SelectOperation CreateFilter<TSource>(System.Linq.Expressions.Expression<Func<TSource, bool>> filter)
    {
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        return CreateFilter((System.Linq.Expressions.LambdaExpression)filter);
    }

    public static SelectOperation CreateFilter(System.Linq.Expressions.LambdaExpression filter)
    {
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        return new SelectOperation(new LambdaExpression(filter), Default.Orders, Default.SkipCount, Default.TakeCount);
    }


    public static SelectOperation Parse(string text)
    {
        return SelectOperationParser.Default.Parse(text);
    }


    public static readonly SelectOperation Default = new(

        new LambdaExpression(BooleanConstantExpression.True, [ParameterExpression.Default]),

        [],

        0,

        int.MaxValue);
}
