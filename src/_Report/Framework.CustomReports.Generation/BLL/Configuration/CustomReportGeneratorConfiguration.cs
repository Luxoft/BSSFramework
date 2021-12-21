using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Framework.CustomReports.Domain;
using Framework.CustomReports.Services;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.CustomReports.Generation.BLL
{
    public class CustomReportBLLGeneratorConfiguration<TEnvironment> : GeneratorConfiguration<TEnvironment, FileType>, ICustomReportBLLGeneratorConfiguration<TEnvironment>
        where TEnvironment : class, IGenerationEnvironment
    {
        private readonly IList<CustomReportParameterLink> links;

        public CustomReportBLLGeneratorConfiguration(
            TEnvironment environment,
            params Assembly[] customReportAssemblies)
            : base(environment)
        {
            var customReportAssembly = new CustomReportAssembly(customReportAssemblies.SelectMany(z => z.GetExportedTypes()).ToList(), new List<Type>());

            this.links = new CustomReportParameterLinkService(customReportAssembly).GetLinks();
        }

        protected override IEnumerable<Type> GetDomainTypes()
        {
            return this.Links.Select(z => z.ParameterType);
        }


        protected override string NamespacePostfix => "CustomReports.BLL";


        public IEnumerable<CustomReportParameterLink> Links => this.links;

        protected override IEnumerable<ICodeFileFactoryHeader<FileType>> GetFileFactoryHeaders()
        {
            yield return new CodeFileFactoryHeader<FileType>(FileType.CustomReportBLL, "", domainType => $"{domainType.Name}BLL");
        }
    }
}
