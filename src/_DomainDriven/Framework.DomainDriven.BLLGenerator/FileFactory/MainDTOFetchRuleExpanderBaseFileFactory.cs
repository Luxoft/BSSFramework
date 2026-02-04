using System.CodeDom;
using System.Reflection;


using Framework.CodeDom;
using Framework.Core;
using Framework.Projection;
using Framework.Transfering;

using GenericQueryable.Fetching;

namespace Framework.DomainDriven.BLLGenerator;

public class MainDTOFetchRuleExpanderBaseFileFactory<TConfiguration>(TConfiguration configuration) : FileFactory<TConfiguration>(configuration, null)
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public override FileType FileType => FileType.MainDTOFetchRuleExpanderBase;

    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration { Name = this.Name, TypeAttributes = TypeAttributes.Public | TypeAttributes.Abstract, IsPartial = true, };
    }

    protected override IEnumerable<string> GetImportedNamespaces()
    {
        yield return "GenericQueryable";
    }

    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        yield return typeof(DTOFetchRuleExpander<>).ToTypeReference(this.Configuration.Environment.PersistentDomainObjectBaseType);
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var member in base.GetMembers())
        {
            yield return member;
        }

        yield return this.GetGetContainerMethod();

        var request = from domainType in this.Configuration.DomainTypes

                      from method in new[] { this.GetGetDomainContainerMethod(domainType) }

                      select method;

        foreach (var method in request)
        {
            yield return method;
        }
    }

    private CodeMemberMethod GetGetContainerMethod()
    {
        var domainObjectParameter = new CodeTypeParameter("TDomainObject");
        var domainObjectTypeRef = domainObjectParameter.ToTypeReference();

        var dtoTypeParameter = new CodeParameterDeclarationExpression(typeof(ViewDTOType), "dtoType");
        var dtoTypeParameterExpr = dtoTypeParameter.ToVariableReferenceExpression();

        var statementsRequest = from domainType in this.Configuration.DomainTypes

                                let condition = new CodeValueEqualityOperatorExpression(domainObjectTypeRef.ToTypeOfExpression(), domainType.ToTypeOfExpression())

                                let statement = new CodeThisReferenceExpression().ToMethodInvokeExpression($"TryGet{domainType.Name}FetchRule", dtoTypeParameterExpr)
                                                                                 .ToCastExpression(typeof(object).ToTypeReference())
                                                                                 .ToCastExpression(typeof(PropertyFetchRule<>).ToTypeReference(domainObjectTypeRef))
                                                                                 .ToMethodReturnStatement()

                                select Tuple.Create((CodeExpression)condition, (CodeStatement)statement);

        return new CodeMemberMethod
               {
                   Attributes = MemberAttributes.Family | MemberAttributes.Override,
                   Name = "TryExpand",
                   ReturnType = typeof(PropertyFetchRule<>).ToTypeReference(domainObjectTypeRef),
                   TypeParameters = { domainObjectParameter },
                   Parameters = { dtoTypeParameter },
                   Statements = { statementsRequest.ToSwitchExpressionStatement(new CodePrimitiveExpression(null).ToMethodReturnStatement()) }
               };
    }


    private CodeMemberMethod GetGetDomainContainerMethod(Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        var dtoTypeParameter = new CodeParameterDeclarationExpression(typeof(ViewDTOType), "dtoType");
        var dtoTypeParameterExpr = dtoTypeParameter.ToVariableReferenceExpression();


        var statementsRequest = from dtoType in EnumHelper.GetValues<ViewDTOType>()

                                where domainType.IsProjection() || dtoType != ViewDTOType.ProjectionDTO

                                orderby dtoType

                                let condition = new CodeValueEqualityOperatorExpression(dtoTypeParameterExpr, dtoType.ToPrimitiveExpression())

                                let statement = this.GetDomainContainerExpression(domainType, dtoType).ToMethodReturnStatement()

                                select Tuple.Create((CodeExpression)condition, (CodeStatement)statement);

        return new CodeMemberMethod
               {
                   Attributes = MemberAttributes.Family,
                   Name = $"TryGet{domainType.Name}FetchRule",
                   ReturnType = typeof(PropertyFetchRule<>).ToTypeReference(domainType),
                   Parameters = { dtoTypeParameter },
                   Statements = { statementsRequest.ToSwitchExpressionStatement(new CodePrimitiveExpression(null).ToMethodReturnStatement()) }
        };
    }

    private CodeExpression GetDomainContainerExpression(Type domainType, ViewDTOType dtoType)
    {
        var paths = this.Configuration
                        .FetchPathFactory
                        .Create(domainType, dtoType)
                        .ToArray();

        if (paths.Any())
        {
            return paths.Select((path, index) => new { path, index })
                        .Aggregate(
                            (CodeExpression)typeof(FetchRule<>).ToTypeReferenceExpression(domainType.ToTypeReference()),
                            (state, pathInfo) => AddFetch(state, pathInfo.path, pathInfo.index))

                //.WithNewLineParameters()
                ;
        }
        else
        {
            return typeof(FetchRule<>).ToTypeReferenceExpression(domainType.ToTypeReference()).ToPropertyReference(nameof(FetchRule<>.Empty));
        }
    }

    private static CodeExpression AddFetch(CodeExpression startState, PropertyPath propertyPath, int pathIndex)
    {
        var lambdaParam = new CodeParameterDeclarationExpression { Name = "v" };

        var lambdaParamExpr = lambdaParam.ToVariableReferenceExpression();

        return propertyPath.Select((prop, index) => new { prop, index }).Aggregate(startState, (state, propInfo) =>
        {
            var methodName = propInfo.index == 0 ? pathIndex == 0 ? "Create" : "Fetch" : "ThenFetch";

            return state.ToMethodReferenceExpression(methodName)
                        .ToMethodInvokeExpression(
                            new CodeLambdaExpression
                            {
                                Parameters = { lambdaParam }, Statements = { lambdaParamExpr.ToPropertyReference(propInfo.prop).ToMethodReturnStatement() }
                            });
        });
    }
}
