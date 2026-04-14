using System.CodeDom;
using Framework.CodeGeneration.DTOGenerator.FileFactory;
using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Main.Base;

namespace Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Main;

public class DefaultVisualDTOFileFactory<TConfiguration>(TConfiguration configuration, Type domainType) : MainDTOFileFactory<TConfiguration>(configuration, domainType)
    where TConfiguration : class, IServerDTOGeneratorConfiguration<IServerDTOGenerationEnvironment>
{
    public override MainDTOFileType FileType { get; } = BaseFileType.VisualDTO;

    protected override bool ConvertToStrict { get; } = false;


    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        foreach (var baseType in base.GetBaseTypes())
        {
            yield return baseType;
        }

        if (this.Configuration.GeneratePolicy.Used(this.DomainType, BaseFileType.IdentityDTO))
        {
            yield return this.GetIdentityObjectContainerTypeReference();
        }
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var baseMember in base.GetMembers())
        {
            yield return baseMember;
        }

        if (this.Configuration.GeneratePolicy.Used(this.DomainType, BaseFileType.IdentityDTO))
        {
            yield return this.GetIdentityObjectContainerImplementation();
        }
    }
}
