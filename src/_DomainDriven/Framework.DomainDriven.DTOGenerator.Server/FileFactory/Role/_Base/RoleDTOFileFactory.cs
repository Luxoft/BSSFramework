using System;
using System.CodeDom;
using System.Reflection;

namespace Framework.DomainDriven.DTOGenerator.Server
{
    public abstract class RoleDTOFileFactory<TConfiguration> : DTOFileFactory<TConfiguration, DTOFileType>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
    {
        protected RoleDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
        }


        protected abstract string DataContractNamespace { get; }


        protected override IPropertyAssigner MapDomainObjectToMappingObjectPropertyAssigner => new DomainObjectToDTOPropertyAssigner<TConfiguration>(this);


        protected override CodeTypeDeclaration GetCodeTypeDeclaration()
        {
            return new CodeTypeDeclaration
            {
                Name = this.Name,
                IsClass = true,
                IsPartial = true,
                TypeAttributes = TypeAttributes.Public
            };
        }

        protected override System.Collections.Generic.IEnumerable<CodeAttributeDeclaration> GetCustomAttributes()
        {
            yield return this.Configuration.GetDTOFileAttribute(this.DomainType, this.FileType);
            yield return this.GetDataContractCodeAttributeDeclaration(this.DataContractNamespace);
        }

        protected override System.Collections.Generic.IEnumerable<CodeTypeMember> GetMembers()
        {
            foreach (var baseMember in base.GetMembers())
            {
                yield return baseMember;
            }

            yield return this.GenerateDefaultConstructor();

            yield return this.GenerateFromDomainObjectConstructor(this.MapDomainObjectToMappingObjectPropertyAssigner);
        }
    }
}
