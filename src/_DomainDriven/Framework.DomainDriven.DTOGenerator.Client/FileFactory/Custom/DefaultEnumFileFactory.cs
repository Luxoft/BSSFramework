using System;
using System.CodeDom;
using System.Linq;
using System.Runtime.Serialization;

using Framework.CodeDom;
using Framework.Core;

namespace Framework.DomainDriven.DTOGenerator.Client
{
    public class DefaultEnumFileFactory<TConfiguration> : FileFactory<IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>, RoleFileType>
        where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>
    {
        private readonly Type _underlyingType;


        public DefaultEnumFileFactory(TConfiguration configuration, Type domainType) : base(configuration, domainType)
        {
            if (!domainType.IsEnum)
            {
                throw new Exception($"DomainType {domainType.FullName} is not enum");
            }


            this._underlyingType = Enum.GetUnderlyingType(this.DomainType);
        }


        public override RoleFileType FileType { get; } = ClientFileType.Enum;

        public override CodeTypeReference BaseReference => this._underlyingType.ToTypeReference();


        protected override CodeTypeDeclaration GetCodeTypeDeclaration()
        {
            return new CodeTypeDeclaration(this.Name)
            {
                IsEnum = true,
                Attributes = MemberAttributes.Public
            };
        }

        protected override System.Collections.Generic.IEnumerable<CodeTypeMember> GetMembers()
        {
            foreach (var field in this.DomainType.GetFields().Where(field => field.IsLiteral))
            {
                yield return new CodeMemberField
                {
                    Name = field.Name,
                    InitExpression = new CodePrimitiveExpression(Convert.ChangeType(field.GetValue(null), this._underlyingType)),
                    CustomAttributes =
                    {
                        new CodeAttributeDeclaration (new CodeTypeReference(typeof(EnumMemberAttribute)))
                    },
                };
            }
        }

        protected override System.Collections.Generic.IEnumerable<CodeAttributeDeclaration> GetCustomAttributes()
        {
            yield return this.GetDataContractCodeAttributeDeclaration("http://schemas.datacontract.org/2004/07/" + this.DomainType.Namespace);

            if (this.DomainType.HasAttribute<FlagsAttribute>())
            {
                yield return typeof(FlagsAttribute).ToTypeReference().ToAttributeDeclaration();
            }
        }
    }
}