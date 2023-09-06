namespace Framework.SecuritySystem;

public interface IDomainObjectIdentResolver
{
    object TryGetIdent(object domainObject);

    bool HasDefaultIdent(object domainObject);
}
