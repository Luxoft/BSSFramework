using System.CodeDom;

using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator.Server;

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
            if (this.Configuration.GeneratePolicy.Used(this.DomainType, DTOGenerator.FileType.IdentityDTO))
            {
                yield return this.GetIdentityObjectContainerImplementation();
            }

            yield return this.GetIdentityObjectImplementation(true);
        }
    }
}
