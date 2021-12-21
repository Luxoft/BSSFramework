namespace Framework.Core
{
    public interface IPeriodObject
    {
        Period Period
        {
            get;
        }
    }

    public interface IMutablePeriodObject : IPeriodObject
    {
        new Period Period
        {
            get;
            set;
        }
    }
}