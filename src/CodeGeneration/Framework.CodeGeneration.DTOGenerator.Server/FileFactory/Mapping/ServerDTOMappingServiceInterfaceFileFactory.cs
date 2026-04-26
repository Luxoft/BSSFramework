using System.CodeDom;
using System.Collections.ObjectModel;

using Anch.Core;
using Framework.BLL.DTOMapping.Services;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.DTOGenerator.FileFactory.Base;
using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.FileGeneration.Configuration;

namespace Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Mapping;

public class ServerDTOMappingServiceInterfaceFileFactory<TConfiguration> : FileFactory<TConfiguration, BaseFileType>
        where TConfiguration : class, IServerDTOGeneratorConfiguration<IServerDTOGenerationEnvironment>
{
    private readonly ReadOnlyCollection<IServerMappingServiceExternalMethodGenerator> externalGenerators;


    public ServerDTOMappingServiceInterfaceFileFactory(TConfiguration configuration, IEnumerable<IServerMappingServiceExternalMethodGenerator> externalGenerators)
            : base(configuration, null)
    {
        if (externalGenerators == null) throw new ArgumentNullException(nameof(externalGenerators));

        this.externalGenerators = externalGenerators.ToReadOnlyCollection();
    }


    public override BaseFileType FileType { get; } = ServerFileType.ServerDTOMappingServiceInterface;


    protected override CodeTypeDeclaration GetCodeTypeDeclaration() =>
        new(this.Name)
        {
            Attributes = MemberAttributes.Public,
            IsPartial = true,
            IsInterface = true,
        };

    public override CodeTypeReference BaseReference => typeof(IDTOMappingService<,>).ToTypeReference(this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference(), this.Configuration.Environment.GetIdentityType().ToTypeReference());


    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var member in base.GetMembers())
        {
            yield return member;
        }

        foreach (var fieldFileFactory in this.externalGenerators)
        {
            foreach (var method in fieldFileFactory.GetServerMappingServiceInterfaceMethods())
            {
                yield return method;
            }
        }
    }
}
