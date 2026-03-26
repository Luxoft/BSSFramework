using System.CodeDom;
using System.Collections.ObjectModel;

using CommonFramework;

using Framework.BLL.DTOMapping;
using Framework.CodeDom;
using Framework.CodeGeneration.DomainMetadata;
using Framework.CodeGeneration.DTOGenerator.FileFactory.Base;
using Framework.CodeGeneration.DTOGenerator.FileType;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.FileType;

namespace Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Mapping;

public class ServerDTOMappingServiceInterfaceFileFactory<TConfiguration> : FileFactory<TConfiguration, BaseFileType>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    private readonly ReadOnlyCollection<IServerMappingServiceExternalMethodGenerator> externalGenerators;


    public ServerDTOMappingServiceInterfaceFileFactory(TConfiguration configuration, IEnumerable<IServerMappingServiceExternalMethodGenerator> externalGenerators)
            : base(configuration, null)
    {
        if (externalGenerators == null) throw new ArgumentNullException(nameof(externalGenerators));

        this.externalGenerators = externalGenerators.ToReadOnlyCollection();
    }


    public override BaseFileType FileType { get; } = ServerFileType.ServerDTOMappingServiceInterface;


    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration(this.Name)
               {
                       Attributes = MemberAttributes.Public,
                       IsPartial = true,
                       IsInterface = true,
               };
    }

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
