namespace Framework.DomainDriven.ApplicationCore.ExternalSource;

public interface ISecurityEntitySource
{
    ITypedSecurityEntitySource GetTyped(Guid securityContextTypeId);

    ITypedSecurityEntitySource GetTyped(Type securityContextType);
}
