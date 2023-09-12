using System.CodeDom;

namespace Framework.DomainDriven.BLLCoreGenerator;

public interface IRootSecurityServiceGenerator
{
    IDomainSecurityServiceGenerator GetDomainSecurityServiceGenerator(Type domainType);

    IEnumerable<CodeTypeMember> GetBaseMembers();

    IEnumerable<Type> GetSecurityServiceDomainTypes();



    IEnumerable<CodeTypeReference> GetBLLContextBaseTypes();


    CodeTypeReference GetGenericRootSecurityServiceType();

    CodeTypeReference GetGenericRootSecurityServiceInterfaceType();
}
