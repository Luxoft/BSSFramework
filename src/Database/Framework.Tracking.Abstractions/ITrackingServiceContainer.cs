namespace Framework.Tracking;

public interface ITrackingServiceContainer<in TPersistentDomainObjectBase>
{
    ITrackingService<TPersistentDomainObjectBase> TrackingService { get; }
}
