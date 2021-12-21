using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

using Framework.CodeDom;
using Framework.DomainDriven.BLL.Security;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Generation.Domain;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.Security;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLLCoreGenerator
{
    public class SecurityOperationFileFactory<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        public SecurityOperationFileFactory(TConfiguration configuration)
            : base(configuration, null)
        {

        }


        public override FileType FileType => FileType.SecurityOperation;


        protected override CodeTypeDeclaration GetCodeTypeDeclaration()
        {
            return new CodeTypeDeclaration
            {
                Attributes = MemberAttributes.Public,
                Name = this.Name
            }.MarkAsStatic();
        }

        protected override System.Collections.Generic.IEnumerable<CodeTypeMember> GetMembers()
        {
            foreach (var member in this.GetOperationMembers())
            {
                yield return member;
            }


            yield return this.GetGetByCodeMethod();


            var getCodeByModeMethod = this.GetGetCodeByModeMethod();

            yield return getCodeByModeMethod;

            var getCodeByModeGenericMethod = this.GetGetCodeByModeGenericMethod();

            yield return getCodeByModeGenericMethod;


            yield return this.GetGetByModeMethod(getCodeByModeMethod.Name);
        }

        private IEnumerable<CodeTypeMember> GetOperationMembers()
        {
            var disabledSecurityOperationType = typeof(DisabledSecurityOperation<>).MakeGenericType(this.Configuration.Environment.SecurityOperationCodeType);

            return from securityOperationCode in this.Configuration.Environment.GetSecurityOperationCodes()

                   let fieldInfo = securityOperationCode.ToFieldInfo()

                   let securityOperationAttribute = securityOperationCode.GetSecurityOperationAttribute()

                   where !securityOperationAttribute.IsClient

                   let isContextHierarchicalView = this.Configuration.DomainTypes
                                                                     .Where(domainType => domainType.IsHierarchical() && domainType.IsSecurityContext())
                                                                     .Select(domainType => domainType.GetViewDomainObjectCode())
                                                                     .Contains(securityOperationCode)

                   let expandType =

                    securityOperationAttribute.OverridedSecurityExpandType ? securityOperationAttribute.SecurityExpandType
                                               : isContextHierarchicalView ? HierarchicalExpandType.All
                                                                           : HierarchicalExpandType.Children

                   let securityOperationTypeRef = new CodeTypeReference(

                       securityOperationCode.IsDefaultEnumValue() ? typeof(DisabledSecurityOperation<>)
                           : securityOperationAttribute.IsContext ? typeof(ContextSecurityOperation<>)
                       : typeof(NonContextSecurityOperation<>))
                   {
                       TypeArguments = { this.Configuration.Environment.SecurityOperationCodeType }
                   }

                   let fieldMember = new CodeMemberField(securityOperationTypeRef, "_" + fieldInfo.Name.ToStartLowerCase())
                   {
                       Attributes = MemberAttributes.Private | MemberAttributes.Static | MemberAttributes.Final,
                       InitExpression =

                           securityOperationCode.IsDefaultEnumValue() ? disabledSecurityOperationType.ToTypeReference().ToObjectCreateExpression()
                         : securityOperationTypeRef.ToObjectCreateExpression(securityOperationAttribute.IsContext ? new[] { securityOperationCode.ToPrimitiveExpression(), expandType.ToPrimitiveExpression() }
                                                                                 : new[] { securityOperationCode.ToPrimitiveExpression() })
                   }


                   let propertyMember = new CodeMemberProperty
                   {
                       Name = fieldInfo.Name,
                       Attributes = MemberAttributes.Public | MemberAttributes.Static,
                       Type = securityOperationTypeRef,
                       GetStatements =
                       {
                           new CodeFieldReferenceExpression(null, fieldMember.Name).ToMethodReturnStatement()
                       }
                   }

                   from member in new CodeTypeMember[] { fieldMember, propertyMember }

                   select member;
        }

        private CodeMemberMethod GetGetCodeByModeGenericMethod()
        {
            var genericDomainObjectParameter = this.GetDomainObjectCodeTypeParameter();

            var modeParameter = typeof(BLLSecurityMode).ToTypeReference().ToParameterDeclarationExpression("mode");
            var modeParameterRefExpr = modeParameter.ToVariableReferenceExpression();

            var method = this.CurrentReference.ToTypeReferenceExpression().ToMethodInvokeExpression("GetCodeByMode", genericDomainObjectParameter.ToTypeOfExpression(), modeParameterRefExpr).ToMethodReturnStatement();

            return new CodeMemberMethod
            {
                Attributes = MemberAttributes.Static | MemberAttributes.Private,

                Name = "GetCodeByMode",

                ReturnType = this.Configuration.Environment.SecurityOperationCodeType.ToTypeReference(),

                Parameters = { modeParameter },

                Statements = { method },

                TypeParameters = { genericDomainObjectParameter }
            };
        }

        private CodeMemberMethod GetGetCodeByModeMethod()
        {
            var genericDomainObjectParameterDeclParameter = typeof(Type).ToTypeReference().ToParameterDeclarationExpression("domainType");
            var genericDomainObjectParameterVariable = genericDomainObjectParameterDeclParameter.ToVariableReferenceExpression();


            var modeParameter = typeof(BLLSecurityMode).ToTypeReference().ToParameterDeclarationExpression("mode");
            var modeParameterRefExpr = modeParameter.ToVariableReferenceExpression();

            var getByBLLSecurityModeMethodReturnStatements = from domainType in this.Configuration.DomainTypes

                                                             from isEdit in new[] { false, true }

                                                             let securityOperationCode = domainType.GetDomainObjectCode(isEdit)

                                                             where securityOperationCode != null

                                                             let mode = isEdit ? BLLSecurityMode.Edit : BLLSecurityMode.View

                                                             let condition = new CodeBooleanAndOperatorExpression(
                                                                new CodeValueEqualityOperatorExpression(modeParameterRefExpr, mode.ToPrimitiveExpression()),
                                                                new CodeValueEqualityOperatorExpression(domainType.ToTypeOfExpression(), genericDomainObjectParameterVariable))

                                                             let statement = securityOperationCode.ToPrimitiveExpression().ToMethodReturnStatement()

                                                             select Tuple.Create((CodeExpression)condition, (CodeStatement)statement);


            var lastSwitchStatement = this.Configuration.GetDisabledSecurityCodeExpression().ToMethodReturnStatement();

            var switchStatement = getByBLLSecurityModeMethodReturnStatements.ToSwitchExpressionStatement(lastSwitchStatement);

            return new CodeMemberMethod
                   {
                       Attributes = MemberAttributes.Static | MemberAttributes.Public,

                       Name = "GetCodeByMode",

                       ReturnType = this.Configuration.Environment.SecurityOperationCodeType.ToTypeReference(),

                       Statements = { switchStatement },

                       Parameters = { genericDomainObjectParameterDeclParameter, modeParameter }
                   };
        }

        private CodeMemberMethod GetGetByModeMethod(string getCodeByModeMethodName)
        {
            var genericDomainObjectParameter = this.GetDomainObjectCodeTypeParameter();
            var genericDomainObjectParameterTypeRef = genericDomainObjectParameter.ToTypeReference();

            var modeParameter = typeof(BLLSecurityMode).ToTypeReference().ToParameterDeclarationExpression("mode");
            var modeParameterRefExpr = modeParameter.ToVariableReferenceExpression();


            var codeVariableStatement = this.Configuration.Environment.SecurityOperationCodeType.ToTypeReference().ToVariableDeclarationStatement(
                "securityOperationCode", this.CurrentReference.ToTypeReferenceExpression().ToMethodReferenceExpression(getCodeByModeMethodName, genericDomainObjectParameterTypeRef).ToMethodInvokeExpression(modeParameterRefExpr));

            var codeVariableStatementRefExpr = codeVariableStatement.ToVariableReferenceExpression();

            var conditionStatement = new CodeConditionStatement
            {
                Condition = new CodeValueEqualityOperatorExpression(codeVariableStatementRefExpr, this.Configuration.GetDisabledSecurityCodeExpression()),
                TrueStatements =
                {
                    this.CurrentReference.ToTypeReferenceExpression().ToFieldReference(this.Configuration.GetDisabledSecurityCodeValue().ToString()).ToMethodReturnStatement()
                },

                FalseStatements =
                {
                    this.CurrentReference.ToTypeReferenceExpression().ToMethodInvokeExpression(this.Configuration.GetOperationByCodeMethodName, codeVariableStatementRefExpr).ToMethodReturnStatement()
                }
            };

            return new CodeMemberMethod
            {
                Attributes = MemberAttributes.Static | MemberAttributes.Public,

                Name = this.Configuration.GetOperationByModeMethodName,

                ReturnType = typeof(SecurityOperation<>).MakeGenericType(this.Configuration.Environment.SecurityOperationCodeType).ToTypeReference(),

                Parameters = { modeParameter },

                Statements = { codeVariableStatement, conditionStatement },

                TypeParameters = { genericDomainObjectParameter }
            };
        }

        private CodeMemberMethod GetGetByCodeMethod()
        {
            var codeParameter = this.Configuration.Environment.SecurityOperationCodeType.ToTypeReference().ToParameterDeclarationExpression("code");
            var codeParameterRefExpr = codeParameter.ToVariableReferenceExpression();

            var getByCodeMethodReturnStatements = from securityOperationCode in this.Configuration.Environment.GetSecurityOperationCodes()

                                                  let fieldInfo = securityOperationCode.ToFieldInfo()

                                                  let securityOperationAttribute = securityOperationCode.GetSecurityOperationAttribute()

                                                  where !securityOperationAttribute.IsClient

                                                  let condition = new CodeValueEqualityOperatorExpression(codeParameterRefExpr, securityOperationCode.ToPrimitiveExpression())

                                                  let statement = this.CurrentReference.ToTypeReferenceExpression().ToFieldReference(securityOperationCode.ToString()).ToMethodReturnStatement()

                                                  select Tuple.Create((CodeExpression)condition, (CodeStatement)statement);


            var lastSwitchStatement = codeParameter.ToThrowArgumentOutOfRangeExceptionStatement();

            var switchStatement = getByCodeMethodReturnStatements.ToSwitchExpressionStatement(lastSwitchStatement);

            return new CodeMemberMethod
            {
                Attributes = MemberAttributes.Static | MemberAttributes.Public,

                Name = this.Configuration.GetOperationByCodeMethodName,

                ReturnType = typeof(SecurityOperation<>).MakeGenericType(this.Configuration.Environment.SecurityOperationCodeType).ToTypeReference(),

                Parameters = { codeParameter },

                Statements = { switchStatement }
            };
        }
    }
}
