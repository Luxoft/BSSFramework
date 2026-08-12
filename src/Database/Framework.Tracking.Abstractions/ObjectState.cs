using System.Linq.Expressions;

using Framework.Core;

namespace Framework.Tracking;

public record struct ObjectState(string PropertyName, object? CurrentValue, object? PreviousValue, bool IsModified)
{
    public static ObjectState Create<T>(Expression<Func<T, object>> propertyExpression, object? currentValue, object? previousValue)
    {
        var path = propertyExpression.ToPath().ToLower();
        return new ObjectState(path, currentValue, previousValue, true);
    }
}
