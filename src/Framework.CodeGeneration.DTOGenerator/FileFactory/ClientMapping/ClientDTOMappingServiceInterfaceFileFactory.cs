using System.CodeDom;
using System.Collections.ObjectModel;

using CommonFramework;

using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileFactory.Base;
using Framework.CodeGeneration.DTOGenerator.FileType;

namespace Framework.CodeGeneration.DTOGenerator.FileFactory.ClientMapping;

public class ClientDTOMappingServiceInterfaceFileFactory<TConfiguration>(TConfiguration configuration, IEnumerable<IClientMappingServiceExternalMethodGenerator> externalGenerators)
    : FileFactory<TConfiguration, BaseFileType>(configuration, null)
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    private readonly ReadOnlyCollection<IClientMappingServiceExternalMethodGenerator> externalGenerators = externalGenerators.ToReadOnlyCollection();

    public override BaseFileType FileType { get; } = BaseFileType.ClientDTOMappingServiceInterface;


    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration(this.Name)
               {
                       Attributes = MemberAttributes.Public,
                       IsPartial = true,
                       IsInterface = true,
               };
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var member in base.GetMembers())
        {
            yield return member;
        }

        foreach (var fieldFileFactory in this.externalGenerators)
        {
            foreach (var method in fieldFileFactory.GetClientMappingServiceInterfaceMethods())
            {
                yield return method;
            }
        }
    }
}
