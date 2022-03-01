using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Framework.Core;
using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.DTOGenerator.Client;
using Framework.DomainDriven.DTOGenerator.Server;
using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.Serialization;

using ServiceModelGenerator = Framework.DomainDriven.ServiceModelGenerator;


namespace WorkflowSampleSystem.CodeGenerate.ClientDTO
{
    public class ClientDTOGeneratorConfiguration : ClientGeneratorConfigurationBase<ServerGenerationEnvironment>
    {
        public ClientDTOGeneratorConfiguration(ServerGenerationEnvironment environment)
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
                return new FullRefCodeTypeReferenceService<ClientDTOGeneratorConfiguration>(this);
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
                .Or(new DTORoleGeneratePolicy(DTORole.Client, ClientDTORole.Projection// | ClientDTORole.Update
                                                              ));

            return new WorkflowSampleSystemClientDependencyGeneratePolicy(primitivePolicy, this.GetTypeMaps());
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
