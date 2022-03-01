using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.DTOGenerator.Server;
using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.Serialization;

using WorkflowSampleSystem.CodeGenerate.Configurations.Services.Audit;

using ServiceModelGenerator = Framework.DomainDriven.ServiceModelGenerator;

namespace WorkflowSampleSystem.CodeGenerate.ServerDTO
{
    public class ServerDTOGeneratorConfiguration : ServerGeneratorConfigurationBase<ServerGenerationEnvironment>
    {
        public ServerDTOGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
        {
        }

        public override string DataContractNamespace => this.Environment.DTODataContractNamespace;

        protected virtual ICodeFileFactoryHeader<MainDTOFileType> FullRefDTOFileFactoryHeader { get; } = WorkflowSampleSystemFileType.FullRefDTO.ToHeader();

        protected virtual ICodeFileFactoryHeader<MainDTOFileType> SimpleRefFullDetailDTOFileFactoryHeader { get; } = WorkflowSampleSystemFileType.SimpleRefFullDetailDTO.ToHeader();

        public override IEnumerable<GenerateTypeMap> GetTypeMaps()
        {
            foreach (var baseTypeMap in base.GetTypeMaps())
            {
                yield return baseTypeMap;
            }

            foreach (var type in this.DomainTypes)
            {
                yield return this.GetTypeMap(type, WorkflowSampleSystemFileType.FullRefDTO);

                yield return this.GetTypeMap(type, WorkflowSampleSystemFileType.SimpleRefFullDetailDTO);
            }
        }

        public override ILayerCodeTypeReferenceService GetLayerCodeTypeReferenceService(DTOFileType fileType)
        {
            if (fileType == WorkflowSampleSystemFileType.SimpleRefFullDetailDTO || fileType == WorkflowSampleSystemFileType.FullRefDTO)
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
            var primitivePolicy = new WorkflowSampleSystemExtGeneratePolicy()
                    .Or(new ServiceModelGenerator.DTOServiceGeneratePolicy<MainServiceGeneratorConfiguration>(this.Environment.MainService))
                    .Or(new ServiceModelGenerator.DTOServiceGeneratePolicy<QueryServiceGeneratorConfiguration>(this.Environment.QueryService))
                    .Or(new ServiceModelGenerator.DTOServiceGeneratePolicy<IntegrationGeneratorConfiguration>(this.Environment.IntegrationService))
                    //.Or(new ServiceModelGenerator.DTOServiceGeneratePolicy<AuditServiceGeneratorConfiguration>(this.Environment.AuditService))
                    .Or(new DTORoleGeneratePolicy(DTORole.Event))
                    .Or(new DTORoleGeneratePolicy(DTORole.Client, ClientDTORole.Projection// | ClientDTORole.Update
                                                                  ))
                    .Or(new WorkflowSampleSystemEventDTORoleGeneratePolicy())
                    .Or(new ServiceModelGenerator.DTOServiceGeneratePolicy<CustomReportServiceGeneratorConfiguration>(this.Environment.CustomReportService));

            return new WorkflowSampleSystemServerDependencyGeneratePolicy(primitivePolicy, this.GetTypeMaps());
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

            if (fileType == WorkflowSampleSystemFileType.FullRefDTO)
            {
                return this.GetFullRefDTOProperties(domainType);
            }
            else if (fileType == WorkflowSampleSystemFileType.SimpleRefFullDetailDTO)
            {
                return this.GetSimpleRefFullDetailDTOProperties(domainType);
            }
            else
            {
                return base.GetInternalDomainTypeProperties(domainType, fileType);
            }
        }
    }
}
