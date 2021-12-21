using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

namespace Framework.DomainDriven.ServiceModelGenerator
{
    public class ImplementFileFactory<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        public ImplementFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {

        }


        public override FileType FileType { get; } = FileType.Implement;


        protected override CodeTypeDeclaration GetCodeTypeDeclaration()
        {
            return new CodeTypeDeclaration
            {
                Name = this.Configuration.ImplementClassName,
                Attributes = MemberAttributes.Public,
                IsPartial = true,
                IsClass = true
            };
        }

        protected override IEnumerable<CodeTypeMember> GetMembers()
        {
            return from methodGenerator in this.GetMethodGenerators()

                   from method in methodGenerator.GetFacadeMethods()

                   select method;
        }
    }
}
