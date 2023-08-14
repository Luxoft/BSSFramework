using Framework.Authorization.Domain;
using Framework.DomainDriven.Generation.Domain.ExternalMetadataSource;

namespace Framework.Authorization.TestGenerate;

public class AuthorizationDomainObjectExternalMetadataSource : DomainObjectExternalMetadataSource<PersistentDomainObjectBase>
{
    public AuthorizationDomainObjectExternalMetadataSource()
    {
        //[DomainType("{fa27cd64-c5e6-4356-9efa-a35b00ff69dd}")]
        //[AuthorizationViewDomainObject(AuthorizationSecurityOperationCode.PrincipalView)]
        //[AuthorizationEditDomainObject(AuthorizationSecurityOperationCode.PrincipalEdit)]
        //[BLLViewRole]
        //[BLLSaveRole]
        //[BLLRemoveRole]

        this.AddDomainType<Principal>()
            .AddTypeAttributes()
    }
}
