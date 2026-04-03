namespace Framework.Infrastructure.LocalDBEvents;

public class LocalDBEventMessageSenderSettings<TPersistentDomainObjectBase>
{
    public string QueueTag { get; set; } = "default";
}
