using System.CodeDom;

using CommonFramework;

using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.DTO.Extensions;
using Framework.BLL.Domain.Persistent;
using Framework.CodeDom.Extend;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.DomainMetadata;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration._Base;
using Framework.CodeGeneration.ServiceModelGenerator.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main.View._Base;
using Framework.Core;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main.View;

public class GetTreeByOperationMethodGenerator<TConfiguration> : ViewMethodGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public GetTreeByOperationMethodGenerator(TConfiguration configuration, Type domainType, ViewDTOType dtoType)
            : base(configuration, domainType, dtoType)
    {
        var elementTypeRef = typeof(HierarchicalNode<,>).ToTypeReference(
                                                                         this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, this.DTOType),
                                                                         this.Configuration.Environment.GetIdentityType().ToTypeReference());

        this.ReturnType = typeof(List<>).ToTypeReference(elementTypeRef);

        this.Identity = new MethodIdentity(MethodIdentityType.GetTreeByOperation, this.DTOType);
    }

    public override MethodIdentity Identity { get; }

    protected override string Name => this.CreateName(false, "Operation", "TreeBy");

    protected sealed override CodeTypeReference ReturnType { get; }

    protected override object GetBLLSecurityParameter(CodeExpression evaluateDataExpr) => this.GetSecurityRuleParameter().ToVariableReferenceExpression();

    protected override string GetComment() => $"Get hierarchical data of type {this.DomainType.GetPluralizedDomainName()} ({this.DTOType}) by operation";

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return this.GetSecurityRuleParameter();
    }

    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        var selectMethod = typeof(HierarchicalNodeExtensions).ToTypeReferenceExpression().ToMethodReferenceExpression(nameof(HierarchicalNodeExtensions.ChangeItem));

        var selectLambda = new CodeParameterDeclarationExpression { Name = this.DomainType.Name.ToStartLowerCase() }.Pipe(param => new CodeLambdaExpression
            {
                    Parameters = { param },
                    Statements = { param.ToVariableReferenceExpression().Pipe(source => this.ConvertToDTO(source, evaluateDataExpr.GetMappingService())) }
            });

        var treeDecl = new CodeVariableDeclarationStatement("var", "tree", bllRefExpr.ToMethodReferenceExpression("GetTree").ToMethodInvokeExpression(this.GetFetchRule()));

        yield return treeDecl;

        yield return treeDecl.ToVariableReferenceExpression()
                             .ToStaticMethodInvokeExpression(selectMethod, selectLambda)
                             .ToMethodReturnStatement();
    }
}
