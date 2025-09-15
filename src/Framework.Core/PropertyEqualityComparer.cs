namespace Framework.Core;

public class PropertyEqualityComparer<T, TProperty> : EqualityComparer<T>
{
    private readonly Func<T, TProperty> getPropertyFunc;

    public PropertyEqualityComparer(Func<T, TProperty> getPropertyFunc)
    {
        this.getPropertyFunc = getPropertyFunc;
    }

    public override bool Equals(T x, T y)
    {
        var xValue = this.getPropertyFunc(x);
        var yValue = this.getPropertyFunc(y);

        return Equals(xValue, yValue);
    }

    public override int GetHashCode(T obj)
    {
        return 0;
    }
}
