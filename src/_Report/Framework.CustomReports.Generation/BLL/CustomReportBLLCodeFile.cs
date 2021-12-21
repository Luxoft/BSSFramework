using System;
using System.CodeDom;
using System.Collections.Generic;

using Framework.CustomReports.BLL;
using Framework.DomainDriven.Generation.Domain;

using JetBrains.Annotations;

namespace Framework.CustomReports.Generation.BLL
{
    public class CustomReportBLLCodeFile<TConfiguration> : CodeFileFactory<TConfiguration, FileType>
        where TConfiguration : class, ICustomReportBLLGeneratorConfiguration<ICustomReportGenerationEnvironmentBase>
    {
        private readonly Type parameterType;

        public CustomReportBLLCodeFile(TConfiguration configuration, Type domainType, [NotNull] Type parameterType)
             : base(configuration, domainType)
        {
            if (parameterType == null)
            {
                throw new ArgumentNullException(nameof(parameterType));
            }

            this.parameterType = parameterType;
        }

        public override FileType FileType { get; } = FileType.CustomReportBLL;

        protected override CodeTypeDeclaration GetCodeTypeDeclaration()
        {
            return new CodeTypeDeclaration
            {
                Name = this.Name,
                Attributes = MemberAttributes.Public,
                IsPartial = true
            };
        }

        protected override IEnumerable<CodeTypeReference> GetBaseTypes()
        {
            var environment = this.Configuration.Environment;

            yield return new CodeTypeReference(
                typeof(CustomReportBLLBase<,,,,>).FullName,
                environment.BLLCore.BLLContextInterfaceTypeReference,
                new CodeTypeReference(environment.SecurityOperationCodeType),
                new CodeTypeReference(environment.PersistentDomainObjectBaseType),
                new CodeTypeReference(this.DomainType),
                new CodeTypeReference(this.parameterType));
        }

        protected override IEnumerable<CodeConstructor> GetConstructors()
        {
            var contextParameter = new CodeParameterDeclarationExpression
            {
                Type = this.Configuration.Environment.BLLCore.BLLContextInterfaceTypeReference,
                Name = "context"
            };

            yield return new CodeConstructor
            {
                Attributes = MemberAttributes.Public,
                Parameters =
                {
                    contextParameter,
                },
                BaseConstructorArgs =
                {
                    new CodeVariableReferenceExpression(contextParameter.Name)
                }
            };
        }
    }
}
