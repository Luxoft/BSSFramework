using System.CodeDom;
using Framework.CodeGeneration.BLLCoreGenerator.Configuration;

namespace Framework.CodeGeneration.BLLCoreGenerator.FileFactory;

public class BLLFactoryContainerInterfaceFileFactory<TConfiguration>(TConfiguration configuration) : BLLFactoryContainerInterfaceFileFactoryBase<TConfiguration>(configuration)
    where TConfiguration : class, IBLLCoreGeneratorConfiguration<IBLLCoreGenerationEnvironment>
{
    public override FileType FileType => FileType.BLLFactoryContainerInterface;


    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var member in base.GetMembers())
        {
            yield return member;
        }

        foreach (var domainType in this.Configuration.BLLDomainTypes)
        {
            yield return new CodeMemberProperty
                         {
                                 Name = domainType.Name + "Factory",
                                 Type = this.Configuration.GetCodeTypeReference(domainType, FileType.BLLFactoryInterface),
                                 HasGet = true
                         };
        }
    }
}
