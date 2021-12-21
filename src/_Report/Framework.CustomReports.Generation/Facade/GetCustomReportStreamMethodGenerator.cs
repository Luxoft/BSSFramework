using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.Linq;
using System.Linq.Expressions;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.ServiceModelGenerator;
using Framework.CustomReports.Domain;

using FileType = Framework.CustomReports.Generation.BLL.FileType;

namespace Framework.CustomReports.Generation.Facade
{
    public class GetCustomReportStreamMethodGenerator<TConfiguration> : GeneratorConfigurationContainer<TConfiguration>, IServiceMethodGenerator, IDomainTypeContainer
        where TConfiguration : class, ICustomReportServiceGeneratorConfiguration<ICustomReportServiceGenerationEnvironmentBase>
    {
        public GetCustomReportStreamMethodGenerator(TConfiguration configuration, Type customReportType, Type parameterType) : base(configuration)
        {
            this.DomainType = customReportType;
            this.CustomReportParameterType = parameterType;
        }

        public virtual string MethodName => $"Get{this.DomainType.Name}";

        public virtual MethodIdentity Identity => CustomReportMethodIdentityType.GetCustomReport;

        public virtual CodeMemberMethod GetContractMethod()
        {
            return new CodeMemberMethod
            {
                Name = this.MethodName,
                ReturnType = this.ResultType
            }.WithParameters(new[] { this.FacadeMethodParameterDeclaration })
             .WithComment(this.GetComment());
        }

        protected virtual string GetComment()
        {
            return $"Get {this.DomainType.Name} custom report";
        }

        protected virtual CodeParameterDeclarationExpression FacadeMethodParameterDeclaration
        {
            get
            {
                var configuration = this.Configuration.Environment.ServerDTO;

                var dtoTypeRef = configuration.GetCodeTypeReference(this.CustomReportParameterType, Framework.DomainDriven.DTOGenerator.FileType.StrictDTO);

                return dtoTypeRef.ToParameterDeclarationExpression("modelDTO");
            }
        }

        public virtual IEnumerable<CodeMemberMethod> GetFacadeMethods()
        {
            yield return new CodeMemberMethod
            {
                Name = this.MethodName,
                ReturnType = this.ResultType,
                Attributes = MemberAttributes.Public,
            }
            .WithParameters(new[] { this.FacadeMethodParameterDeclaration })
            .WithComment(this.GetComment())
            .WithStatements(this.GetFacadeMethodStatements());
        }

        private IEnumerable<CodeStatement> GetFacadeMethodStatements()
        {
            var evaluateDataParameterExpr = new CodeParameterDeclarationExpression(this.Configuration.EvaluateDataTypeReference, "evaluateData");

            var evaluateMethod = new CodeThisReferenceExpression().ToMethodInvokeExpression(
                "Evaluate",
                DBSessionMode.Read.ToPrimitiveExpression(),
                new CodeLambdaExpression
                {
                    Parameters = { new CodeParameterDeclarationExpression { Name = evaluateDataParameterExpr.Name } },
                    Statements = new CodeStatementCollection(this.GetEvaluateStatements(evaluateDataParameterExpr).ToArray())
                }).ToResultStatement(this.ResultType);

            yield return evaluateMethod;
        }

        private IEnumerable<CodeStatement> GetEvaluateStatements(CodeParameterDeclarationExpression evaluateDataParameterExpr)
        {
            var customReportParameter = new CodeParameterDeclarationExpression(this.CustomReportParameterType, "customReportParameter");

            var customReportParameterAssign = new CodeAssignStatement(customReportParameter, new CodeObjectCreateExpression(this.CustomReportParameterType));

            yield return customReportParameterAssign;

            var evaluateDataRef = evaluateDataParameterExpr.ToVariableReferenceExpression();

            yield return this.FacadeMethodParameterDeclaration
                             .ToVariableReferenceExpression()
                             .ToMethodInvokeExpression(this.Configuration.Environment.ServerDTO.MapToDomainObjectMethodName, evaluateDataRef.GetMappingService(), customReportParameter.ToVariableReferenceExpression())
                             .ToExpressionStatement();

            var reportBLLType = this.Configuration.Environment.ReportBLL.GetCodeTypeReference(this.DomainType, FileType.CustomReportBLL);

            var customReport = reportBLLType.ToParameterDeclarationExpression("customReport");

            var customReportAssign = reportBLLType.ToObjectCreateExpression(evaluateDataRef.GetContext())
                                                  .ToAssignStatement(customReport);

            yield return customReportAssign;

            var customReportStreamResult = new CodeParameterDeclarationExpression(typeof(IReportStream).ToTypeReference(), "reportStreamResult");

            //TODO: MethodName from Expressin<Func....
            var getReportStreamMethod = customReport.ToVariableReferenceExpression().ToMethodInvokeExpression("GetReportStream", customReportParameter.ToVariableReferenceExpression());

            var customReportStreamResultAssign = new CodeAssignStatement(customReportStreamResult, getReportStreamMethod);

            yield return customReportStreamResultAssign;
            yield return new CodeThisReferenceExpression().ToMethodInvokeExpression("GetReportResult", customReportStreamResult.ToVariableReferenceExpression()).ToMethodReturnStatement();
        }

        public CodeTypeReference ResultType => typeof(Microsoft.AspNetCore.Mvc.FileStreamResult).ToTypeReference();

        public Type CustomReportParameterType { get; }

        public Type DomainType { get; }
    }
}
