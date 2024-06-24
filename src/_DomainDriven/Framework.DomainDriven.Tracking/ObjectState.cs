using System.Linq.Expressions;

using Framework.Core;

namespace Framework.DomainDriven.Tracking;

public struct ObjectState
{
    public ObjectState(string propertyName, object currentValue, object previusValue, bool isModified)
            : this()
    {
        this.PropertyName = propertyName;
        this.CurrentValue = currentValue;
        this.PreviusValue = previusValue;
        this.IsModified = isModified;
    }

    public string PropertyName { get; private set; }

    public object CurrentValue { get; private set; }

    public object PreviusValue { get; private set; }

    public bool IsModified { get; private set; }

    public static ObjectState Create<T>(Expression<Func<T, object>> propertyExpression, object currentValue, object previusValue)
    {
        var path = propertyExpression.ToPath().ToLower();
        return new ObjectState(path, currentValue, previusValue, true);
    }
}
