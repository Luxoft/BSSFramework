using System.CodeDom;
using System.Collections.Generic;
using Framework.CodeDom;
using Framework.DomainDriven.BLL;

namespace Framework.DomainDriven.BLLCoreGenerator;

public class BLLFactoryContainerInterfaceFileFactoryBase<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public BLLFactoryContainerInterfaceFileFactoryBase(TConfiguration configuration)
            : base(configuration, null)
    {

    }


    public override FileType FileType => FileType.BLLFactoryContainerInterface;


    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration
               {
                       Name = this.Name,

                       Attributes = MemberAttributes.Public,
                       IsPartial = true,
                       IsInterface = true,

                       BaseTypes = { this.Configuration.SecurityBLLFactoryContainerType }
               };
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var domainType in this.Configuration.BLLDomainTypes)
        {
            yield return new CodeMemberProperty
                         {
                                 Name = domainType.Name,
                                 Type = this.Configuration.GetCodeTypeReference(domainType, FileType.BLLInterface),
                                 HasGet = true
                         };
        }
    }
}
