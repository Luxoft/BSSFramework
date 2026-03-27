namespace Framework.Application.DALExceptions;

public class StaleDomainObjectStateException(
    Type domainObjectType,
    object domainObjectIdent,
    Exception innerException) : Exception(
    $"Object '{domainObjectType.Name}' was updated or deleted by another transaction",
    innerException)
{
    public Type DomainObjectType { get; } = domainObjectType;

    public object DomainObjectIdent { get; } = domainObjectIdent;
}
