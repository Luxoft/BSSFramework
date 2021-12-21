using System;
using System.CodeDom;
using System.Collections.Generic;

using Framework.Core;
using Framework.CodeDom;
using Framework.Projection;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator
{
    public class GetSingleByIdentityMethodGenerator<TConfiguration> : ViewMethodGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        public GetSingleByIdentityMethodGenerator(TConfiguration configuration, Type domainType, ViewDTOType dtoType)
            : base(configuration, domainType, dtoType)
        {
            this.Identity = new MethodIdentity(MethodIdentityType.GetSingleByIdentity, dtoType);
        }


        public override MethodIdentity Identity { get; }


        protected override string Name => this.CreateName(false, null);

        protected override CodeTypeReference ReturnType => this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, this.DTOType);


        protected override string GetComment()
        {
            return $"Get {this.DomainType.Name} ({this.DTOType}) by identity";
        }

        protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
        {
            yield return this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType.GetProjectionSourceTypeOrSelf(), Transfering.DTOType.IdentityDTO)
                             .ToParameterDeclarationExpression(this.DomainType.Name.ToStartLowerCase() + "Identity");
        }


        protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
        {
            var domainObjectDecl = this.ToDomainObjectVarDeclById(evaluateDataExpr, bllRefExpr, true);

            yield return domainObjectDecl;

            yield return domainObjectDecl.ToVariableReferenceExpression()
                                         .Pipe(source => this.ConvertToDTO(source, evaluateDataExpr.GetMappingService()))
                                         .ToMethodReturnStatement();
        }
    }
}
