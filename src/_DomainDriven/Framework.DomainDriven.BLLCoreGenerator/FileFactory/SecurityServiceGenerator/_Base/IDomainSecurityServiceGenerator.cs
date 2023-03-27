using System.CodeDom;

namespace Framework.DomainDriven.BLLCoreGenerator;

public interface IDomainSecurityServiceGenerator
{
    CodeTypeReference BaseServiceType { get; }

    IEnumerable<CodeTypeMember> GetMembers();

    IEnumerable<CodeTypeReference> GetBaseTypes();

    CodeConstructor GetConstructor();
}
