using System.CodeDom;

using Framework.CodeGeneration.BLLCoreGenerator.Configuration;
using Framework.CodeGeneration.BLLCoreGenerator.FileFactory.__Base;

namespace Framework.CodeGeneration.BLLCoreGenerator.FileFactory;

public class BLLFactoryContainerInterfaceFileFactoryBase<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public BLLFactoryContainerInterfaceFileFactoryBase(TConfiguration configuration)
            : base(configuration, null)
    {

    }


    public override FileType.FileType FileType => BLLCoreGenerator.FileType.FileType.BLLFactoryContainerInterface;


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
                                 Type = this.Configuration.GetCodeTypeReference(domainType, BLLCoreGenerator.FileType.FileType.BLLInterface),
                                 HasGet = true
                         };
        }
    }
}
