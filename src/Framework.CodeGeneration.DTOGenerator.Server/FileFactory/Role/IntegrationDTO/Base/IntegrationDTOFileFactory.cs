using System.CodeDom;

using Framework.CodeGeneration.Configuration;
using Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService.Base;
using Framework.CodeGeneration.DTOGenerator.Extensions;
using Framework.CodeGeneration.DTOGenerator.FileFactory._Helpers;
using Framework.CodeGeneration.DTOGenerator.FileType;
using Framework.CodeGeneration.DTOGenerator.PropertyAssigner.__Base;
using Framework.CodeGeneration.DTOGenerator.Server.CodeTypeReferenceService;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Role._Base;
using Framework.CodeGeneration.DTOGenerator.Server.FileType;
using Framework.CodeGeneration.DTOGenerator.Server.Members.MapToDomainObject;
using Framework.CodeGeneration.DTOGenerator.Server.PropertyAssigner;

namespace Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Role.IntegrationDTO.Base;

public abstract class IntegrationDTOFileFactory<TConfiguration> : RoleDTOFileFactory<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    protected IntegrationDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
        this.CodeTypeReferenceService = new CryptCodeTypeReferenceService<TConfiguration>(this.Configuration, ServerFileType.SimpleIntegrationDTO, ServerFileType.RichIntegrationDTO);

        this.MapMappingObjectToDomainObjectPropertyAssigner = new DTOToDomainObjectPropertyAssigner<TConfiguration>(this);
    }


    public override IPropertyCodeTypeReferenceService CodeTypeReferenceService { get; }


    protected override IPropertyAssigner MapMappingObjectToDomainObjectPropertyAssigner { get; }

    protected override string DataContractNamespace => this.Configuration.IntegrationDataContractNamespace;


    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        foreach (var baseType in base.GetBaseTypes())
        {
            yield return baseType;
        }

        if (this.IsPersistent())
        {
            yield return this.Configuration.GetIdentityObjectCodeTypeReference();
        }
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var baseMember in base.GetMembers())
        {
            yield return baseMember;
        }

        yield return new BaseMapToDomainObjectMethodFactory<TConfiguration, IntegrationDTOFileFactory<TConfiguration>, DTOFileType>(this).GetMethod();

        if (this.IsPersistent())
        {
            if (this.Configuration.GeneratePolicy.Used(this.DomainType, BaseFileType.IdentityDTO))
            {
                yield return this.GetIdentityObjectContainerImplementation();
            }

            yield return this.GetIdentityObjectImplementation(true);
        }
    }
}
