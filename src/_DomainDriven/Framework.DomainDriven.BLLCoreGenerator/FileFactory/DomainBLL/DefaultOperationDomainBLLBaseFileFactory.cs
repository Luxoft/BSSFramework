using System.CodeDom;
using Framework.CodeDom;
using Framework.DomainDriven.BLL;

namespace Framework.DomainDriven.BLLCoreGenerator
{
    public class DefaultOperationDomainBLLBaseFileFactory<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        public DefaultOperationDomainBLLBaseFileFactory(TConfiguration configuration)
            : base(configuration, null)
        {

        }


        public override FileType FileType => FileType.DefaultOperationDomainBLLBase;


        protected override CodeTypeDeclaration GetCodeTypeDeclaration()
        {
            var genericDomainObjectParameter = this.GetDomainObjectCodeTypeParameter();
            var genericDomainObjectParameterTypeRef = genericDomainObjectParameter.ToTypeReference();

            var contextParameter = this.GetContextParameter();
            var contextParameterRefExpr = contextParameter.ToVariableReferenceExpression();


            return new CodeTypeDeclaration
            {
                TypeParameters =
                {
                    genericDomainObjectParameter
                },

                Name = this.Name,

                IsClass = true,

                IsPartial = true,

                BaseTypes =
                {
                    new CodeTypeReference
                    {
                        BaseType = this.Configuration.GetCodeTypeReference(null, FileType.DomainBLLBase).BaseType,
                        TypeArguments = { genericDomainObjectParameterTypeRef, typeof(BLLBaseOperation) }
                    }
                },
                Members =
                {
                    new CodeConstructor
                    {
                        Attributes = MemberAttributes.Public,
                        Parameters = { contextParameter },
                        BaseConstructorArgs = { contextParameterRefExpr }
                    }
                }
            };
        }
    }
}