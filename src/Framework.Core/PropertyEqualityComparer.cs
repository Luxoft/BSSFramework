namespace Framework.Core;

public class PropertyEqualityComparer<T, TProperty>(Func<T, TProperty> getPropertyFunc) : EqualityComparer<T>
{
    public override bool Equals(T x, T y)
    {
        var xValue = getPropertyFunc(x);
        var yValue = getPropertyFunc(y);

        return Equals(xValue, yValue);
    }

    public override int GetHashCode(T obj) => 0;
}
