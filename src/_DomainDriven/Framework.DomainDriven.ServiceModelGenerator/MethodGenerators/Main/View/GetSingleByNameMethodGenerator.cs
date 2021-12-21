using System;
using System.CodeDom;
using System.Collections.Generic;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.DTOGenerator;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator
{
    public class GetSingleByNameMethodGenerator<TConfiguration> : ViewMethodGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        public GetSingleByNameMethodGenerator(TConfiguration configuration, Type domainType, ViewDTOType dtoType)
            : base(configuration, domainType, dtoType)
        {
            this.Identity = new MethodIdentity(MethodIdentityType.GetSingleByName, dtoType);
        }


        public override MethodIdentity Identity { get; }


        protected override string Name => this.CreateName(false, "Name");

        protected override CodeTypeReference ReturnType => this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, this.DTOType);


        protected override string GetComment()
        {
            return $"Get {this.DomainType.Name} ({this.DTOType}) by name";
        }

        protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
        {
            yield return typeof(string).ToTypeReference().ToParameterDeclarationExpression(this.DomainType.Name.ToStartLowerCase() + "Name");
        }


        protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
        {
            var domainObjectDecl = this.ToDomainObjectVarDeclByName(evaluateDataExpr, bllRefExpr);

            yield return domainObjectDecl;

            yield return domainObjectDecl.ToVariableReferenceExpression()
                                         .Pipe(source => this.ConvertToDTO(source, evaluateDataExpr.GetMappingService()))
                                         .ToMethodReturnStatement();
        }
    }
}