using System.CodeDom;
using System.Collections.ObjectModel;
using System.Reflection;

using CommonFramework;

using Framework.BLL.Domain.ClientMappingService;
using Framework.CodeDom;
using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileFactory.Base;

namespace Framework.CodeGeneration.DTOGenerator.FileFactory.ClientMapping;

public class ClientPrimitiveDTOMappingServiceBaseFileFactory<TConfiguration> : FileFactory<TConfiguration, FileType.FileType>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    private readonly ReadOnlyCollection<IClientMappingServiceExternalMethodGenerator> _externalGenerators;

    public ClientPrimitiveDTOMappingServiceBaseFileFactory(TConfiguration configuration, IEnumerable<IClientMappingServiceExternalMethodGenerator> externalGenerators)
            : base(configuration, null)
    {
        if (externalGenerators == null) throw new ArgumentNullException(nameof(externalGenerators));

        this._externalGenerators = externalGenerators.ToReadOnlyCollection();
    }


    public override FileType.FileType FileType { get; } = DTOGenerator.FileType.FileType.ClientPrimitiveDTOMappingServiceBase;

    public override CodeTypeReference BaseReference => typeof(ClientDTOMappingServiceBase).ToTypeReference();

    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        foreach (var baseType in base.GetBaseTypes())
        {
            yield return baseType;
        }

        yield return this.Configuration.GetCodeTypeReference(null, DTOGenerator.FileType.FileType.ClientDTOMappingServiceInterface);
    }

    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration(this.Name)
               {
                       Attributes = MemberAttributes.Abstract,
                       TypeAttributes = TypeAttributes.Public | TypeAttributes.Abstract,
                       IsPartial = true,
               };
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var member in base.GetMembers())
        {
            yield return member;
        }


        foreach (var fieldFileFactory in this._externalGenerators)
        {
            foreach (var method in fieldFileFactory.GetClientMappingServiceMethods())
            {
                yield return method;
            }
        }
    }
}
