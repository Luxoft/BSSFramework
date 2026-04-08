namespace Framework.Configuration.BLL;

public interface IDomainObjectVersionsResolverFactory
{
    IDomainObjectVersionsResolver Create(Type domainObjectType);
}
