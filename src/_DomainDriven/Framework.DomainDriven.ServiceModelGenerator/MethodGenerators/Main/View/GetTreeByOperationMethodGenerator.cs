using System.CodeDom;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.Generation.Domain;
using Framework.Persistent;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator;

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

    protected override object GetBLLSecurityParameter(CodeExpression evaluateDataExpr)
    {
        return this.GetSecurityRuleParameter().ToVariableReferenceExpression();
    }

    protected override string GetComment()
    {
        return $"Get hierarchical data of type {this.DomainType.GetPluralizedDomainName()} ({this.DTOType}) by operation";
    }

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

        var treeDecl = new CodeVariableDeclarationStatement("var", "tree", bllRefExpr.ToMethodReferenceExpression("GetTree").ToMethodInvokeExpression(this.GetFetchsExpression(evaluateDataExpr)));

        yield return treeDecl;

        yield return treeDecl.ToVariableReferenceExpression()
                             .ToStaticMethodInvokeExpression(selectMethod, selectLambda)
                             .ToMethodReturnStatement();
    }
}
