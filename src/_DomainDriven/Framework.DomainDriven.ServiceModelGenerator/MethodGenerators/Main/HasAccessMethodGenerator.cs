//using System.CodeDom;

//using Framework.CodeDom;
//using Framework.Core;
//using Framework.DomainDriven.BLL;
//using Framework.Transfering;

//namespace Framework.DomainDriven.ServiceModelGenerator;

//public class HasAccessMethodGenerator<TConfiguration> : MethodGenerator<TConfiguration, BLLViewRoleAttribute>
//        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
//{
//    public HasAccessMethodGenerator(TConfiguration configuration, Type domainType)
//            : base(configuration, domainType)
//    {
//    }


//    public override MethodIdentity Identity { get; } = MethodIdentityType.HasAccess;


//    protected override string Name => $"Has{this.DomainType.Name}Access";

//    protected override CodeTypeReference ReturnType { get; } = typeof(bool).ToTypeReference();

//    protected override bool IsEdit { get; } = false;

//    protected override bool RequiredSecurity { get; } = false;


//    protected override string GetComment()
//    {
//        return $"Check access for {this.DomainType.Name}";
//    }

//    private CodeParameterDeclarationExpression SecurityOperationCodeParameter => this.Configuration.Environment.SecurityOperationCodeType.ToTypeReference().ToParameterDeclarationExpression("securityOperationCode");

//    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
//    {
//        yield return this.Configuration.Environment.ServerDTO
//                         .GetCodeTypeReference(this.DomainType, DTOType.IdentityDTO)
//                         .ToParameterDeclarationExpression(this.DomainType.Name.ToStartLowerCase() + "Ident");

//        yield return this.SecurityOperationCodeParameter;
//    }

//    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
//    {
//        yield return this.GetCheckSecurityOperationCodeParameterStatement(1);

//        var domainObjectVarDecl = this.ToDomainObjectVarDeclById(bllRefExpr);

//        yield return domainObjectVarDecl;

//        yield return this.Configuration.Environment.BLLCore.GetGetSecurityProviderMethodReferenceExpression(evaluateDataExpr.GetContext(), this.DomainType)
//                         .ToMethodInvokeExpression(this.SecurityOperationCodeParameter.ToVariableReferenceExpression())
//                         .ToMethodInvokeExpression("HasAccess", domainObjectVarDecl.ToVariableReferenceExpression())
//                         .ToMethodReturnStatement();
//    }
//}
