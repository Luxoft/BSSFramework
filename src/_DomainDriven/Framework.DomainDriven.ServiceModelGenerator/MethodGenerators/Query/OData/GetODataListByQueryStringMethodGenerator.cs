using System;
using System.CodeDom;
using System.Collections.Generic;

using Framework.CodeDom;
using Framework.Core.Serialization;
using Framework.DomainDriven.BLL;
using Framework.OData;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator
{
    public class GetODataListByQueryStringMethodGenerator<TConfiguration> : GetByODataQueryMethodGeneratorBase<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        public GetODataListByQueryStringMethodGenerator(TConfiguration configuration, Type domainType, ViewDTOType dtoType)
            : base(configuration, domainType, dtoType)
        {
            this.Identity = new MethodIdentity(MethodIdentityType.GetODataListByQueryString, this.DTOType);
        }


        public override MethodIdentity Identity { get; }

        protected override string Name => this.CreateName(true, "ODataQueryString");


        protected override string GetComment()
        {
            return $"Get {this.DomainType.GetPluralizedDomainName()} ({this.DTOType}) by odata string";
        }

        protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
        {
            yield return typeof(string).ToTypeReference().ToParameterDeclarationExpression("odataQueryString");
        }


        protected override CodeExpression GetSelectOperationExpression(CodeExpression evaluateDataExpr)
        {
            return evaluateDataExpr.GetContext().ToPropertyReference((IODataBLLContext c) => c.SelectOperationParser)
                                      .ToMethodInvokeExpression((IParser<string, SelectOperation> parser) => parser.Parse(null), this.Parameter.ToVariableReferenceExpression());
        }
    }
}