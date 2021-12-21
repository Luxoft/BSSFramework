using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.Metadata;

using SampleSystem.Domain;

namespace SampleSystem.CodeGenerate
{
    public class DALGeneratorConfiguration : Framework.DomainDriven.NHibernate.DALGenerator.GeneratorConfigurationBase<ServerGenerationEnvironment>
    {
        public DALGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
        {
        }

        protected override AssemblyMetadata CreateAssemblyMetadata(IAssemblyInfo assembly)
        {
            var baseResult = base.CreateAssemblyMetadata(assembly);

            var ignoreLinks = this.GetIgnoreFields();

            var groupJoin = baseResult.DomainTypes.GroupJoin(ignoreLinks, z => z.DomainType, z => z.fromType, (d, i) => (domainTypeMetadata: d, ignoreLinks: i.ToArray()));

            var partialResult = groupJoin.Partial(z => z.ignoreLinks.Length == 0, (originalDomainTypes, forCorrected) => (originalDomainTypes: originalDomainTypes.Select(q => q.domainTypeMetadata), forCorrected: forCorrected));

            var corrected = partialResult.forCorrected
                                         .Select(z => (source: z, next: new DomainTypeMetadata(z.domainTypeMetadata.DomainType, z.domainTypeMetadata.AssemblyMetadata)))
                                         .Select(z => z.next.Self(s => s.AddFields(z.source.domainTypeMetadata.Fields.Where(field => !z.source.ignoreLinks.Any(i => string.Equals(i.propertyName, field.Name, StringComparison.InvariantCultureIgnoreCase))))))
                                         .Select(z => z.Self(q => q.EndDeclaration()))
                                         .ToArray();

            baseResult.DomainTypes = partialResult.originalDomainTypes.Concat(corrected).ToList();

            return baseResult;
        }

        private (Type fromType, string propertyName)[] GetIgnoreFields()
        {
            return new[]
                   {
                       (fromType: typeof(SampleSystemMessageTemplate), propertyName: nameof(SampleSystemMessageTemplate.Version))
                   };
        }
    }
}
