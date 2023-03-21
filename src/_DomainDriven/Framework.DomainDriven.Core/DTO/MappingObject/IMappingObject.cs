using Framework.Persistent;

namespace Framework.DomainDriven;

public interface IMappingObject<in TMappingService, in TDomainObject>
{
    void MapToDomainObject(TMappingService mappingService, TDomainObject domainObject);
}

public interface IMappingObject<in TMappingService, in TDomainObject, out TIdent> : IMappingObject<TMappingService, TDomainObject>, IIdentityObject<TIdent>
{

}

public interface IConvertMappingObject<in TMappingService, out TDomainObject>
{
    TDomainObject ToDomainObject(TMappingService mappingService);
}
