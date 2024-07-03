using System.Collections.ObjectModel;
using System.Reflection;

using Framework.Core;
using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.DTOGenerator.Server;
using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.Serialization;
using Framework.Events;
using Framework.SecuritySystem;

using SampleSystem.Domain;

using ServiceModelGenerator = Framework.DomainDriven.ServiceModelGenerator;

namespace SampleSystem.CodeGenerate.ServerDTO;

public class ServerDTOGeneratorConfiguration : ServerGeneratorConfigurationBase<ServerGenerationEnvironment>
{
    public ServerDTOGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
    {
    }

    public override string DataContractNamespace => this.Environment.DTODataContractNamespace;

    protected virtual ICodeFileFactoryHeader<MainDTOFileType> FullRefDTOFileFactoryHeader { get; } = SampleSystemFileType.FullRefDTO.ToHeader();

    protected virtual ICodeFileFactoryHeader<MainDTOFileType> SimpleRefFullDetailDTOFileFactoryHeader { get; } = SampleSystemFileType.SimpleRefFullDetailDTO.ToHeader();

    public override IDomainObjectEventMetadata DomainObjectEventMetadata { get; } = new SampleSystemDomainObjectEventMetadata();

    protected override IEnumerable<KeyValuePair<Type, ReadOnlyCollection<SecurityRule>>> GetMainTypesWithSecondarySecurityRules()
    {
        foreach (var baseData in base.GetMainTypesWithSecondarySecurityRules())
        {
            yield return baseData;
        }

        yield return new(
            typeof(BusinessUnit),
            new SecurityRule[]
            {
                SampleSystemSecurityOperation.BusinessUnitView,
                SampleSystemSecurityOperation.BusinessUnitHrDepartmentView,
                SampleSystemSecurityOperation.EmployeeEdit,
                SampleSystemSecurityOperation.BusinessUnitHrDepartmentEdit
            }.ToReadOnlyCollection());
    }

    public override IEnumerable<GenerateTypeMap> GetTypeMaps()
    {
        foreach (var baseTypeMap in base.GetTypeMaps())
        {
            yield return baseTypeMap;
        }

        foreach (var type in this.DomainTypes)
        {
            yield return this.GetTypeMap(type, SampleSystemFileType.FullRefDTO);

            yield return this.GetTypeMap(type, SampleSystemFileType.SimpleRefFullDetailDTO);
        }
    }

    public override ILayerCodeTypeReferenceService GetLayerCodeTypeReferenceService(DTOFileType fileType)
    {
        if (fileType == SampleSystemFileType.SimpleRefFullDetailDTO || fileType == SampleSystemFileType.FullRefDTO)
        {
            return new FullRefCodeTypeReferenceService<ServerDTOGeneratorConfiguration>(this);
        }
        else
        {
            return base.GetLayerCodeTypeReferenceService(fileType);
        }
    }

    protected override IGeneratePolicy<RoleFileType> CreateGeneratePolicy()
    {
        var primitivePolicy = new SampleSystemExtGeneratePolicy()
                              .Or(new ServiceModelGenerator.DTOServiceGeneratePolicy<MainServiceGeneratorConfiguration>(this.Environment.MainService))
                              .Or(new ServiceModelGenerator.DTOServiceGeneratePolicy<QueryServiceGeneratorConfiguration>(this.Environment.QueryService))
                              .Or(new ServiceModelGenerator.DTOServiceGeneratePolicy<IntegrationGeneratorConfiguration>(this.Environment.IntegrationService))
                              //.Or(new ServiceModelGenerator.DTOServiceGeneratePolicy<AuditServiceGeneratorConfiguration>(this.Environment.AuditService))
                              .Or(new DTORoleGeneratePolicy(DTORole.Event))
                              .Or(new DTORoleGeneratePolicy(DTORole.Client, ClientDTORole.Projection// | ClientDTORole.Update
                                                           ))
                              .Or(new SampleSystemEventDTORoleGeneratePolicy());

        return new SampleSystemServerDependencyGeneratePolicy(primitivePolicy, this.GetTypeMaps());
    }

    protected override IEnumerable<ICodeFileFactoryHeader<FileType>> GetFileFactoryHeaders()
    {
        return base.GetFileFactoryHeaders().Concat(new[]
                                                   {
                                                           this.FullRefDTOFileFactoryHeader,
                                                           this.SimpleRefFullDetailDTOFileFactoryHeader
                                                   });
    }

    protected override IEnumerable<PropertyInfo> GetInternalDomainTypeProperties(Type domainType, DTOFileType fileType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        if (fileType == SampleSystemFileType.FullRefDTO)
        {
            return this.GetFullRefDTOProperties(domainType);
        }
        else if (fileType == SampleSystemFileType.SimpleRefFullDetailDTO)
        {
            return this.GetSimpleRefFullDetailDTOProperties(domainType);
        }
        else
        {
            return base.GetInternalDomainTypeProperties(domainType, fileType);
        }
    }
}
