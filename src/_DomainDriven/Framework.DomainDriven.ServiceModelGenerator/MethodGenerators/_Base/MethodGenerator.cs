using System.CodeDom;
using System.Collections.ObjectModel;
using System.ServiceModel;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.Generation.Domain;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.ServiceModelGenerator;

public abstract class MethodGenerator<TConfiguration, TBLLRoleAttribute> : GeneratorConfigurationContainer<TConfiguration>, IServiceMethodGenerator
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
        where TBLLRoleAttribute : BLLServiceRoleAttribute
{
    private readonly Lazy<ReadOnlyCollection<CodeParameterDeclarationExpression>> lazyParameters;

    protected MethodGenerator(TConfiguration configuration, Type domainType)
            : base(configuration)
    {
        this.DomainType = domainType ?? throw new ArgumentNullException(nameof(domainType));
        this.lazyParameters = LazyHelper.Create(() => this.GetParameters().ToReadOnlyCollection());
    }

    public Type DomainType { get; }

    public abstract MethodIdentity Identity { get; }

    internal string InternalName => this.Name + "Internal";

    protected abstract string Name { get; }

    protected abstract CodeTypeReference ReturnType { get; }

    protected abstract bool IsEdit { get; }

    protected virtual TBLLRoleAttribute Attribute => this.DomainType.GetCustomAttribute<TBLLRoleAttribute>().FromMaybe(() => $"Attr {typeof(TBLLRoleAttribute)} not found for type {this.DomainType?.FullName}");// ?? this.GetDefaultAttribute();

    protected virtual DBSessionMode DefaultSessionMode => this.IsEdit ? DBSessionMode.Write : DBSessionMode.Read;

    protected virtual DBSessionMode SessionMode => this.DefaultSessionMode;

    protected virtual bool RequiredSecurity { get; } = true;

    protected virtual bool UseBLL { get; } = true;

    protected ReadOnlyCollection<CodeParameterDeclarationExpression> Parameters => this.lazyParameters.Value;

    protected CodeParameterDeclarationExpression Parameter => this.Parameters.First();

    protected abstract string GetComment();

    public virtual CodeMemberMethod GetContractMethod() =>
            new CodeMemberMethod
                    {
                            CustomAttributes = { typeof(OperationContractAttribute).ToTypeReference().ToAttributeDeclaration() },
                            Name = this.Name,
                            ReturnType = this.ReturnType
                    }.WithParameters(this.Parameters)
                     .WithComment(this.GetComment());

    public IEnumerable<CodeMemberMethod> GetFacadeMethods()
    {
        var evaluateDataParameterExpr = new CodeParameterDeclarationExpression(this.Configuration.EvaluateDataTypeReference, "evaluateData");
        var bllParameterExpr = new CodeParameterDeclarationExpression(this.Configuration.Environment.BLLCore.GetCodeTypeReference(this.DomainType, BLLCoreGenerator.FileType.BLLInterface), "bll");

        return this.GetFacadeMethods(evaluateDataParameterExpr, bllParameterExpr);
    }

    protected virtual IEnumerable<CodeMemberMethod> GetFacadeMethods(CodeParameterDeclarationExpression evaluateDataParameterExpr, CodeParameterDeclarationExpression bllParameterExpr)
    {
        if (this.Attribute == null || !this.Attribute.CustomImplementation)
        {
            yield return this.GetFacadeMethod(evaluateDataParameterExpr);
            yield return this.GetFacadeMethodInternal(evaluateDataParameterExpr, bllParameterExpr);
        }
    }

    protected CodeVariableDeclarationStatement ToDomainObjectVarDecl(CodeExpression initExpression)
    {
        if (initExpression == null) throw new ArgumentNullException(nameof(initExpression));

        return this.DomainType.ToTypeReference().ToVariableDeclarationStatement("domainObject", initExpression);
    }

    protected CodeVariableDeclarationStatement ToDomainObjectVarDeclById(CodeExpression bllRefExpr, CodeParameterDeclarationExpression parameter)
    {
        if (bllRefExpr == null) throw new ArgumentNullException(nameof(bllRefExpr));
        if (parameter == null) throw new ArgumentNullException(nameof(parameter));

        return this.ToDomainObjectVarDecl(this.Configuration.GetByIdExpr(bllRefExpr, parameter.ToVariableReferenceExpression()));
    }

    protected CodeVariableDeclarationStatement ToDomainObjectVarDeclById(CodeExpression bllRefExpr)
    {
        if (bllRefExpr == null) throw new ArgumentNullException(nameof(bllRefExpr));

        return this.ToDomainObjectVarDeclById(bllRefExpr, this.Parameter);
    }

    protected CodeMethodReferenceExpression GetConvertToDTOMethod(DTOGenerator.FileType dtoType) =>
            this.Configuration.Environment.ServerDTO.GetConvertToDTOMethod(this.DomainType, dtoType);

    protected CodeMethodReferenceExpression GetConvertToDTOListMethod(DTOGenerator.FileType dtoType) =>
            this.Configuration.Environment.ServerDTO.GetConvertToDTOListMethod(this.DomainType, dtoType);

    protected abstract IEnumerable<CodeParameterDeclarationExpression> GetParameters();

    protected abstract IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr);

    protected virtual object GetBLLSecurityParameter(CodeExpression evaluateDataExpr)
    {
        if (this.RequiredSecurity)
        {
            return this.IsEdit ? BLLSecurityMode.Edit : BLLSecurityMode.View;
        }
        else
        {
            return null;
        }
    }

    protected CodeVariableDeclarationStatement GetCreateDefaultBLLVariableDeclaration(CodeExpression evaluateDataExpr, string varName, Type objectType, params CodeExpression[] parameters)
    {
        var bllRef = new CodeTypeReference("var");

        var bllCreateExpr =
                evaluateDataExpr.GetContext()
                                .ToPropertyReference((IBLLFactoryContainerContext<object> context) => context.Logics)
                                .ToPropertyReference("Default")
                                .ToMethodInvokeExpression($"Create<{objectType}>");

        return bllRef.ToVariableDeclarationStatement(varName, bllCreateExpr);
    }

    private CodeMemberMethod GetFacadeMethod(CodeParameterDeclarationExpression evaluateDataParameterExpr)
    {
        var evaluateStatement = new CodeThisReferenceExpression().ToMethodInvokeExpression("Evaluate",
            this.SessionMode.ToPrimitiveExpression(),
            new CodeLambdaExpression
            {
                    Parameters = { new CodeParameterDeclarationExpression { Name = evaluateDataParameterExpr.Name } },
                    Statements =
                    {
                            new CodeThisReferenceExpression().ToMethodInvokeExpression(this.InternalName, this.Parameters.Concat(new[] { evaluateDataParameterExpr }).Select(decl => decl.ToVariableReferenceExpression()))
                                                             .ToResultStatement(this.ReturnType)
                    }
            });

        return new CodeMemberMethod
               {
                       Attributes = MemberAttributes.Public,
                       Name = this.Name,
                       ReturnType = this.ReturnType,
                       Statements = { evaluateStatement.ToResultStatement(this.ReturnType) }
               }.WithParameters(this.Parameters)
                .WithComment(this.GetComment());
    }

    private CodeMemberMethod GetFacadeMethodInternal(CodeParameterDeclarationExpression evaluateDataParameterExpr, CodeParameterDeclarationExpression bllParameterExpr)
    {
        if (evaluateDataParameterExpr == null) throw new ArgumentNullException(nameof(evaluateDataParameterExpr));

        var evaluateDataExpr = evaluateDataParameterExpr.ToVariableReferenceExpression();

        var bllDecl = this.UseBLL ? this.GetBLLVariableDeclaration(evaluateDataExpr, bllParameterExpr, this.IsEdit) : null;

        return new CodeMemberMethod
               {
                       Attributes = MemberAttributes.Family,
                       Name = this.InternalName,
                       ReturnType = this.ReturnType,
               }.WithParameters(this.Parameters.Concat(new[] { evaluateDataParameterExpr }))
                .WithStatements(this.UseBLL, () => new[] { bllDecl })
                .WithStatements(this.GetFacadeMethodInternalStatements(evaluateDataExpr, bllDecl.Maybe(v => v.ToVariableReferenceExpression())));
    }

    protected CodeVariableDeclarationStatement GetBLLVariableDeclaration(CodeExpression evaluateDataExpr, CodeParameterDeclarationExpression bllParameterExpr, bool isEdit)
    {
        if (evaluateDataExpr == null) throw new ArgumentNullException(nameof(evaluateDataExpr));

        var bllCreateExpr = this.Configuration.Environment.BLLCore.Logics.GetCreateSecurityBLLExpr(
         evaluateDataExpr.GetContext().ToPropertyReference((IBLLFactoryContainerContext<object> context) => context.Logics),
         this.DomainType,
         this.GetBLLSecurityParameter(evaluateDataExpr));

        return bllParameterExpr.Type.ToVariableDeclarationStatement(bllParameterExpr.Name, bllCreateExpr);
    }

    protected CodeVariableDeclarationStatement GetDefaultBLLVariableDeclaration(CodeExpression evaluateDataExpr, string varName, Type objectType, params CodeExpression[] parameters)
    {
        var bllRef = this.Configuration.Environment.BLLCore.GetCodeTypeReference(objectType, BLLCoreGenerator.FileType.BLLInterface);

        var bllCreateExpr = evaluateDataExpr.GetContext().ToPropertyReference((IBLLFactoryContainerContext<object> context) => context.Logics).ToPropertyReference(objectType.Name);

        return bllRef.ToVariableDeclarationStatement(varName, bllCreateExpr);
    }

    protected CodeExpression GetConvertToSecurityOperationCodeParameterExpression(CodeExpression evaluateDataExpr, int parameterIndex)
    {
        return evaluateDataExpr.GetContext()
                               .ToPropertyReference((IAuthorizationBLLContextContainer<object> context) => context.Authorization)
                               .ToPropertyReference("SecurityOperationParser")
                               .ToMethodInvokeExpression(
                                   "Parse",
                                   this.Parameters[parameterIndex]
                                       .Pipe(v => v.Type.BaseType == nameof(String)
                                                      ? (CodeExpression)v.ToVariableReferenceExpression()
                                                      : v.ToVariableReferenceExpression().ToMethodInvokeExpression("ToString")));
    }
}
