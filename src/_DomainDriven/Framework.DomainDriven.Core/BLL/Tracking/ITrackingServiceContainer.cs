namespace Framework.DomainDriven.BLL.Tracking
{
    public interface ITrackingServiceContainer<in TPersistentDomainObjectBase>
    {
        ITrackingService<TPersistentDomainObjectBase> TrackingService { get; }
    }
}