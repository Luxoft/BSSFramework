using System.CodeDom;

using CommonFramework;

using Framework.BLL;
using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.DTO.Extensions;
using Framework.BLL.Domain.Extensions;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration._Base;
using Framework.CodeGeneration.ServiceModelGenerator.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main.View._Base;
using Framework.Core;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main.View;

public class GetListByFilterModelMethodGenerator<TConfiguration> : ViewCollectionMethodGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    private readonly Type filterType;


    public GetListByFilterModelMethodGenerator(TConfiguration configuration, Type domainType, ViewDTOType dtoType, Type filterType)
            : base(configuration, domainType, dtoType)
    {
        if (filterType == null) throw new ArgumentNullException(nameof(filterType));

        this.filterType = filterType;

        this.Identity = new MethodIdentity(MethodIdentityType.GetListByFilter, this.filterType, this.DTOType);
    }


    public override MethodIdentity Identity { get; }

    protected override string Name => this.CreateName(true, this.filterType.Name.Skip(this.DomainType.Name).SkipLast("Model"));


    protected override string GetComment() => $"Get {this.DomainType.GetPluralizedDomainName()} ({this.DTOType}) by filter ({this.filterType})";

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.filterType, BaseFileType.StrictDTO)
                         .ToParameterDeclarationExpression("filter");
    }


    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        var typedFilterDecl = this.filterType.ToTypeReference().ToVariableDeclarationStatement("typedFilter",
            this.Parameter.ToVariableReferenceExpression().ToMethodInvokeExpression(this.Configuration.Environment.ServerDTO.ToDomainObjectMethodName, evaluateDataExpr.GetMappingService()));

        yield return typedFilterDecl;

        yield return bllRefExpr.ToMethodInvokeExpression(nameof(IBLLQueryBase<>.GetListBy), typedFilterDecl.ToVariableReferenceExpression(), this.GetFetchRule())
                               .Pipe(source => this.ConvertToDTOList(source, evaluateDataExpr.GetMappingService()))
                               .ToMethodReturnStatement();

    }

    protected override object? GetBLLSecurityParameter(CodeExpression evaluateDataExpr)
    {
        var modelSecurityAttribute = this.filterType.GetViewDomainObjectAttribute();

        if (null == modelSecurityAttribute)
        {
            return base.GetBLLSecurityParameter(evaluateDataExpr);
        }

        return modelSecurityAttribute.SecurityRule;
    }
}
