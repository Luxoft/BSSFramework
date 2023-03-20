using System;
using System.CodeDom;
using System.Collections.Generic;

namespace Framework.DomainDriven.BLLCoreGenerator;

public interface IDomainSecurityServiceGenerator
{
    CodeTypeReference BaseServiceType { get; }

    IEnumerable<CodeTypeMember> GetMembers();

    IEnumerable<CodeTypeReference> GetBaseTypes();

    CodeConstructor GetConstructor();
}
