// ReSharper disable once CheckNamespace
namespace Framework.Core;

public interface IMutablePeriodObject : IPeriodObject
{
    new Period Period
    {
        get;
        set;
    }
}
