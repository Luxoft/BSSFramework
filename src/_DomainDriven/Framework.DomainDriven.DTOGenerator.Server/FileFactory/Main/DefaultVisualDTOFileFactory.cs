using System;
using System.CodeDom;

namespace Framework.DomainDriven.DTOGenerator.Server
{
    public class DefaultVisualDTOFileFactory<TConfiguration> : MainDTOFileFactory<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
    {
        public DefaultVisualDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
        }


        public override MainDTOFileType FileType { get; } = DTOGenerator.FileType.VisualDTO;

        protected override bool ConvertToStrict { get; } = false;


        protected override System.Collections.Generic.IEnumerable<CodeTypeReference> GetBaseTypes()
        {
            foreach (var baseType in base.GetBaseTypes())
            {
                yield return baseType;
            }

            if (this.Configuration.GeneratePolicy.Used(this.DomainType, DTOGenerator.FileType.IdentityDTO))
            {
                yield return this.GetIdentityObjectContainerTypeReference();
            }
        }

        protected override System.Collections.Generic.IEnumerable<CodeTypeMember> GetMembers()
        {
            foreach (var baseMember in base.GetMembers())
            {
                yield return baseMember;
            }

            if (this.Configuration.GeneratePolicy.Used(this.DomainType, DTOGenerator.FileType.IdentityDTO))
            {
                yield return this.GetIdentityObjectContainerImplementation();
            }
        }
    }
}