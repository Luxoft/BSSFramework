using System.CodeDom;
using Framework.CodeGeneration.BLLCoreGenerator.Configuration;
using Framework.CodeGeneration.BLLCoreGenerator.FileFactory.__Base;

namespace Framework.CodeGeneration.BLLCoreGenerator.FileFactory;

public class BLLFactoryContainerInterfaceFileFactoryBase<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IBLLCoreGeneratorConfiguration<IBLLCoreGenerationEnvironment>
{
    public BLLFactoryContainerInterfaceFileFactoryBase(TConfiguration configuration)
            : base(configuration, null)
    {

    }


    public override FileType FileType => FileType.BLLFactoryContainerInterface;


    protected override CodeTypeDeclaration GetCodeTypeDeclaration() =>
        new()
        {
            Name = this.Name,

            Attributes = MemberAttributes.Public,
            IsPartial = true,
            IsInterface = true,

            BaseTypes = { this.Configuration.SecurityBLLFactoryContainerType }
        };

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
