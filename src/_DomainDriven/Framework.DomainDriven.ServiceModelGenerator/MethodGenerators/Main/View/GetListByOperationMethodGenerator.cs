using System;
using System.CodeDom;
using System.Collections.Generic;

using Framework.CodeDom;
using Framework.Core;
using Framework.Projection;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator
{
    public class GetListByOperationMethodGenerator<TConfiguration> : ViewCollectionMethodGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        public GetListByOperationMethodGenerator(TConfiguration configuration, Type domainType, ViewDTOType dtoType)
            : base(configuration, domainType, dtoType)
        {
            this.Identity = new MethodIdentity(MethodIdentityType.GetListByOperation, this.DTOType);
        }


        public override MethodIdentity Identity { get; }


        protected override string Name => this.CreateName(true, "Operation");


        protected override string GetComment()
        {
            return $"Get {this.DomainType.GetPluralizedDomainName()} ({this.DTOType}) by operation";
        }

        protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
        {
            yield return new CodeParameterDeclarationExpression
            {
                Name = "securityOperationCode",
                Type = this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType.GetProjectionSourceTypeOrSelf(), Framework.DomainDriven.DTOGenerator.FileType.DomainObjectSecurityOperationCode)
            };
        }

        protected override object GetBLLSecurityParameter()
        {
            return this.GetConvertSecurityOperationCodeParameterExpression(0);
        }

        protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
        {
            yield return bllRefExpr.ToMethodInvokeExpression("GetFullList", this.GetFetchsExpression(evaluateDataExpr))
                                   .Pipe(source => this.ConvertToDTOList(source, evaluateDataExpr.GetMappingService()))
                                   .ToMethodReturnStatement();
        }
    }
}
