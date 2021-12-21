namespace Framework.DomainDriven
{
    public interface IDateTimeServiceContainer
    {
        IDateTimeService DateTimeService { get; }
    }
}