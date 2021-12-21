using System;
using System.CodeDom;
using System.Collections.Generic;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.DTOGenerator;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator
{
    public class GetObjectWithRevisionMethodGenerator<TConfiguration> : ViewMethodGenerator<TConfiguration>
        where TConfiguration : class, IAuditGeneratorConfigurationBase<IAuditGenerationEnvironmentBase>
    {
        public GetObjectWithRevisionMethodGenerator(TConfiguration configuration, Type domainType, ViewDTOType dtoType)
            : base(configuration, domainType, dtoType)
        {
            this.Identity = new MethodIdentity(MethodIdentityType.GetRevision, this.DTOType);
        }


        public override MethodIdentity Identity { get; }

        protected override string Name => this.CreateName(false, "Revision", "With");

        protected override CodeTypeReference ReturnType => this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, this.DTOType);

        private CodeParameterDeclarationExpression RevisionParameter => new CodeParameterDeclarationExpression(typeof (long), "revision");

        protected override string GetComment()
        {
            return $"Get {this.DomainType.Name} ({this.DTOType}) by revision";
        }

        protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
        {
            yield return this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, Transfering.DTOType.IdentityDTO)
                             .ToParameterDeclarationExpression(this.DomainType.Name.ToStartLowerCase() + "Identity");

            yield return this.RevisionParameter;
        }


        protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
        {
            var domainObjectDecl = this.ToDomainObjectVarDecl(bllRefExpr.ToMethodInvokeExpression("GetObjectByRevision",

                this.Parameter.ToVariableReferenceExpression().ToPropertyReference(this.Configuration.Environment.IdentityProperty), this.RevisionParameter.ToVariableReferenceExpression()));

            yield return domainObjectDecl;

            yield return domainObjectDecl.ToVariableReferenceExpression()
                                         .Pipe(source => this.ConvertToDTO(source, evaluateDataExpr.GetMappingService()))
                                         .ToMethodReturnStatement();
        }
    }
}
