using System.CodeDom;

using Framework.CodeDom;
using Framework.Security;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLLCoreGenerator;

public class SecurityOperationResolverFileFactory<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public SecurityOperationResolverFileFactory(TConfiguration configuration)
            : base(configuration, null)
    {

    }


    public override FileType FileType => FileType.SecurityOperationResolver;

    public override CodeTypeReference BaseReference => typeof(ISecurityOperationResolver)
                                                       .ToTypeReference();

    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration
               {
                       Attributes = MemberAttributes.Public,
                       Name = this.Name
               };
    }

    protected override System.Collections.Generic.IEnumerable<CodeTypeMember> GetMembers()
    {
        var getCodeByModeMethod = this.GetGetCodeByModeMethod();

        yield return getCodeByModeMethod;

        yield return this.GetGetByModeMethod(getCodeByModeMethod.Name);
    }

    private CodeMemberMethod GetGetCodeByModeMethod()
    {
        var genericDomainObjectParameterDeclParameter = typeof(Type).ToTypeReference().ToParameterDeclarationExpression("domainType");
        var genericDomainObjectParameterVariable = genericDomainObjectParameterDeclParameter.ToVariableReferenceExpression();

        var modeParameter = typeof(BLLSecurityMode).ToTypeReference().ToParameterDeclarationExpression("mode");
        var modeParameterRefExpr = modeParameter.ToVariableReferenceExpression();

        var getByBLLSecurityModeMethodReturnStatements = from domainType in this.Configuration.DomainTypes

                                                         from isEdit in new[] { false, true }

                                                         let securityOperation = domainType.GetSecurityOperation(isEdit)

                                                         where securityOperation != null

                                                         let mode = isEdit ? BLLSecurityMode.Edit : BLLSecurityMode.View

                                                         let condition = new CodeBooleanAndOperatorExpression(
                                                            new CodeValueEqualityOperatorExpression(modeParameterRefExpr, mode.ToPrimitiveExpression()),
                                                            new CodeValueEqualityOperatorExpression(domainType.ToTypeOfExpression(), genericDomainObjectParameterVariable))

                                                         let statement = this.Configuration.GetSecurityCodeExpression(securityOperation).ToMethodReturnStatement()

                                                         select Tuple.Create((CodeExpression)condition, (CodeStatement)statement);


        var lastSwitchStatement = this.Configuration.GetDisabledSecurityCodeExpression().ToMethodReturnStatement();

        var switchStatement = getByBLLSecurityModeMethodReturnStatements.ToSwitchExpressionStatement(lastSwitchStatement);

        return new CodeMemberMethod
               {
                       Attributes = MemberAttributes.Public,

                       Name = "GetSecurityOperation",

                       ReturnType = typeof(SecurityOperation).ToTypeReference(),

                       Statements = { switchStatement },

                       Parameters = { genericDomainObjectParameterDeclParameter, modeParameter }
               };
    }

    private CodeMemberMethod GetGetByModeMethod(string getCodeByModeMethodName)
    {
        var genericDomainObjectParameter = this.GetDomainObjectCodeTypeParameter();
        var genericDomainObjectParameterTypeRefExpr = genericDomainObjectParameter.ToTypeOfExpression();

        var modeParameter = typeof(BLLSecurityMode).ToTypeReference().ToParameterDeclarationExpression("mode");
        var modeParameterRefExpr = modeParameter.ToVariableReferenceExpression();


        var conditionStatement = new CodeConditionStatement
                                 {
                                         Condition = new CodeValueEqualityOperatorExpression(modeParameterRefExpr, BLLSecurityMode.Disabled.ToDynamicPrimitiveExpression()),
                                         TrueStatements =
                                         {
                                             this.Configuration.GetDisabledSecurityCodeExpression().ToMethodReturnStatement()
                                         },

                                         FalseStatements =
                                         {
                                             new CodeThisReferenceExpression().ToMethodInvokeExpression(getCodeByModeMethodName, genericDomainObjectParameterTypeRefExpr, modeParameterRefExpr).ToMethodReturnStatement()
                                         }
                                 };

        return new CodeMemberMethod
               {
                       Attributes = MemberAttributes.Public,

                       Name = this.Configuration.GetOperationByModeMethodName,

                       ReturnType = typeof(SecurityOperation).ToTypeReference(),

                       Parameters = { modeParameter },

                       Statements = { conditionStatement },

                       TypeParameters = { genericDomainObjectParameter }
               };
    }
}
