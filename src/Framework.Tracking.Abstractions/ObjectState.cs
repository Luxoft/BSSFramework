using System.Linq.Expressions;

using Framework.Core;

namespace Framework.DomainDriven.Tracking;

public record struct ObjectState(string PropertyName, object? CurrentValue, object? PreviusValue, bool IsModified)
{
    public static ObjectState Create<T>(Expression<Func<T, object>> propertyExpression, object? currentValue, object? previusValue)
    {
        var path = propertyExpression.ToPath().ToLower();
        return new ObjectState(path, currentValue, previusValue, true);
    }
}
