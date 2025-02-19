namespace Framework.SecuritySystem.ExternalSystem.SecurityContextStorage;

public interface ITypedSecurityContextStorage
{
    IEnumerable<SecurityContextData> GetSecurityContexts();

    IEnumerable<SecurityContextData> GetSecurityContextsByIdents(IEnumerable<Guid> securityContextIdents);

    IEnumerable<SecurityContextData> GetSecurityContextsWithMasterExpand(Guid startSecurityContextId);

    bool IsExists (Guid securityContextId);
}
