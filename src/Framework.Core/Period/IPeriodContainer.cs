// ReSharper disable once CheckNamespace
namespace Framework.Core;

public interface PeriodObject
{
    Period Period
    {
        get;
    }
}

public interface IMutablePeriodObject : PeriodObject
{
    new Period Period
    {
        get;
        set;
    }
}
