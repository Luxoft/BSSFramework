using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Configuration.Domain.Models.Custom.Reports;
using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.DTOGenerator.Client;
using Framework.DomainDriven.FacadeServiceProxyGenerator;
using Framework.DomainDriven.Generation.Domain;
using Framework.Persistent;

using FileType = Framework.DomainDriven.DTOGenerator.FileType;

namespace Framework.Configuration.ClientGenerate
{
    public class ClientDTOGeneratorConfiguration : ClientGeneratorConfigurationBase<ClientGenerationEnvironment>
    {
        public ClientDTOGeneratorConfiguration(ClientGenerationEnvironment environment)
            : base(environment)
        {
        }

        protected override System.Collections.Generic.IEnumerable<System.Reflection.Assembly> GetReuseTypesAssemblies()
        {
            foreach (var assembly in base.GetReuseTypesAssemblies())
            {
                yield return assembly;
            }

            yield return typeof(SubscriptionType).Assembly;
            yield return typeof(NotificationExpandType).Assembly;
        }

        protected override IEnumerable<Type> GetDomainTypes()
        {
            // Exclude ReportGenerationRequestModel for Silverlight DTO Generator
            return base.GetDomainTypes().Except(new[] { typeof(ReportGenerationRequestModel) });
        }

        public override bool ForceGenerateProperties(Type domainType, DTOFileType fileType)
        {
            if (domainType == null) throw new ArgumentNullException(nameof(domainType));
            if (fileType == null) throw new ArgumentNullException(nameof(fileType));

            return fileType == FileType.StrictDTO;
        }

        public override string DataContractNamespace => this.Environment.DTODataContractNamespace;

        public override string Namespace { get; } = "Framework.Configuration.Generated.DTO";

        protected override IGeneratePolicy<RoleFileType> CreateGeneratePolicy()
        {
            var compositePolicy =
                new DTOClientServiceGeneratePolicy<MainFacadeServiceProxyConfiguration>(this.Environment.MainFacadeServiceProxy)
                    .Or(new CustomClientDTOUsedGeneratePolicy());

            return new ServiceProxyClientDependencyGeneratePolicy(compositePolicy, this.GetTypeMaps());
        }
    }
}
