namespace Framework.DomainDriven.ServiceModel.IAD;

public class LocalDBEventMessageSenderSettings<TPersistentDomainObjectBase>
{
    public string QueueTag { get; set; } = "default";
}
