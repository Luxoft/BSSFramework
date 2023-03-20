using System;
using System.CodeDom;
using System.Collections.Generic;

namespace Framework.DomainDriven.BLLCoreGenerator;

public interface IRootSecurityServiceGenerator
{
    IDomainSecurityServiceGenerator GetDomainSecurityServiceGenerator(Type domainType);

    IEnumerable<CodeTypeMember> GetBaseMembers();

    IEnumerable<Type> GetSecurityServiceDomainTypes();



    IEnumerable<CodeTypeReference> GetBLLContextBaseTypes();

    IEnumerable<CodeTypeMember> GetBLLContextMembers();


    CodeTypeReference GetGenericRootSecurityServiceType();

    CodeTypeReference GetGenericRootSecurityServiceInterfaceType();


    CodeTypeReference GetDomainInterfaceBaseServiceType();
}
