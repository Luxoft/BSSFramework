using System.CodeDom;
using System.Reflection;

using CommonFramework;

using Framework.CodeDom;
using Framework.Core;
using Framework.Projection;
using Framework.Transfering;

namespace Framework.DomainDriven.BLLCoreGenerator;

public class MainFetchServiceBaseFileFactory<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public MainFetchServiceBaseFileFactory(TConfiguration configuration)
            : base(configuration, null)
    {
    }

    public override FileType FileType => FileType.MainFetchServiceBase;

    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration
               {
                       Name = this.Name,
                       TypeAttributes = TypeAttributes.Public | TypeAttributes.Abstract,
                       IsPartial = true,
               };
    }

    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        yield return typeof(MainFetchServiceBase<>).ToTypeReference(this.Configuration.Environment.PersistentDomainObjectBaseType);
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

        var ruleParameter = new CodeParameterDeclarationExpression(typeof(ViewDTOType), "rule");
        var ruleVar = ruleParameter.ToVariableReferenceExpression();

        var statementsRequest = from domainType in this.Configuration.DomainTypes

                                let condition = new CodeValueEqualityOperatorExpression(domainObjectTypeRef.ToTypeOfExpression(), domainType.ToTypeOfExpression())

                                let statement = new CodeThisReferenceExpression().ToMethodInvokeExpression($"Get{domainType.Name}Container", ruleVar)
                                                                                 .ToCastExpression(typeof(FetchRule<>).ToTypeReference(domainObjectTypeRef))
                                                                                 .ToMethodReturnStatement()

                                select Tuple.Create((CodeExpression)condition, (CodeStatement)statement);

        return new CodeMemberMethod
               {
                       Attributes = MemberAttributes.Family | MemberAttributes.Override,
                       Name = "GetContainer",
                       ReturnType = typeof(FetchRule<>).ToTypeReference(domainObjectTypeRef),
                       TypeParameters = { domainObjectParameter },
                       Parameters = { ruleParameter },
                       Statements =
                       {
                               statementsRequest.ToSwitchExpressionStatement(new CodeThrowArgumentOutOfRangeExceptionStatement(domainObjectParameter))
                       }
               };
    }


    private CodeMemberMethod GetGetDomainContainerMethod(Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        var ruleParameter = new CodeParameterDeclarationExpression(typeof(ViewDTOType), "rule");
        var ruleVar = ruleParameter.ToVariableReferenceExpression();


        var statementsRequest = from dtoType in EnumHelper.GetValues<ViewDTOType>()

                                where domainType.IsProjection() || dtoType != ViewDTOType.ProjectionDTO

                                orderby dtoType

                                let condition = new CodeValueEqualityOperatorExpression(ruleVar, dtoType.ToPrimitiveExpression())

                                let fetchBuildRule = new FetchBuildRule.DTOFetchBuildRule(dtoType)

                                let statement = this.GetDomainContainerExpression(domainType, fetchBuildRule).ToMethodReturnStatement()

                                select Tuple.Create((CodeExpression)condition, (CodeStatement)statement);

        return new CodeMemberMethod
               {
                       Attributes = MemberAttributes.Family,
                       Name = $"Get{domainType.Name}Container",
                       ReturnType = typeof(FetchRule<>).ToTypeReference(domainType),
                       Parameters = { ruleParameter },

                       Statements = { statementsRequest.ToSwitchExpressionStatement(new CodeThrowArgumentOutOfRangeExceptionStatement(ruleParameter)) }
               };
    }

    private CodeExpression GetDomainContainerExpression(Type domainType, FetchBuildRule.DTOFetchBuildRule fetchBuildRule)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (fetchBuildRule == null) throw new ArgumentNullException(nameof(fetchBuildRule));

        var paths = this.Configuration.FetchPathFactory.Create(domainType, fetchBuildRule).ToArray();

        if (paths.Any())
        {
            var ruleParam = new CodeParameterDeclarationExpression { Name = "fetchRootRule" };
            var ruleExpr = ruleParam.ToVariableReferenceExpression();

            return typeof(FetchContainer).ToTypeReferenceExpression()
                                         .ToMethodReferenceExpression("Create", domainType.ToTypeReference())
                                         .ToMethodInvokeExpression(paths.ToArray(path =>

                                                                                         new CodeLambdaExpression
                                                                                         {
                                                                                                 Parameters = { ruleParam },

                                                                                                 Statements =
                                                                                                 {
                                                                                                         path.Aggregate((CodeExpression)ruleExpr, (state, property) =>

                                                                                                                 state.ToMethodInvokeExpression(property.PropertyType.IsCollection() ? "SelectMany" : "SelectNested",

                                                                                                                     new CodeParameterDeclarationExpression { Name = property.ReflectedType.Name.ToStartLowerCase() }.Pipe(domainObjectParam =>

                                                                                                                             new CodeLambdaExpression
                                                                                                                             {
                                                                                                                                     Parameters = { domainObjectParam },

                                                                                                                                     Statements = { domainObjectParam.ToVariableReferenceExpression().ToPropertyReference(property) }

                                                                                                                             })))
                                                                                                 }
                                                                                         }))
                                         .WithNewLineParameters();
        }
        else
        {
            return typeof(FetchContainer<>).ToTypeReferenceExpression(domainType.ToTypeReference()).ToPropertyReference("Empty");
        }
    }
}
