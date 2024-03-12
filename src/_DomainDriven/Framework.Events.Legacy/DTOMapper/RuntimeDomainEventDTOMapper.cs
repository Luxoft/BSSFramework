namespace Framework.Events.Legacy;

public class RuntimeDomainEventDTOMapper<TPersistentDomainObjectBase, TMappingService, TEventDTOBase> : IDomainEventDTOMapper<TPersistentDomainObjectBase>
{
    private readonly TMappingService mappingService;

    private readonly RuntimeDomainEventDTOConverter<TPersistentDomainObjectBase, TMappingService, TEventDTOBase> converter;

    public RuntimeDomainEventDTOMapper(
        TMappingService mappingService,
        RuntimeDomainEventDTOConverter<TPersistentDomainObjectBase, TMappingService, TEventDTOBase> converter)
    {
        this.mappingService = mappingService;
        this.converter = converter;
    }

    public virtual object Convert<TDomainObject>(TDomainObject domainObject, EventOperation domainObjectEvent)
        where TDomainObject : TPersistentDomainObjectBase
    {
        return this.converter.Convert(this.mappingService, domainObject, domainObjectEvent);
    }
}
