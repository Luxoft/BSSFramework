namespace Framework.Core;

// ReSharper disable once CheckNamespace
public interface IMutablePeriodObject : IPeriodObject
{
    new Period Period
    {
        get;
        set;
    }
}
