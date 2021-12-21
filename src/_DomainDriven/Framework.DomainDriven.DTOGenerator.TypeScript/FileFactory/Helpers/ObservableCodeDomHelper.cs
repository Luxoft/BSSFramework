using System;
using System.CodeDom;
using System.Linq;

using Framework.CodeDom;
using Framework.CodeDom.TypeScript;
using Framework.DomainDriven.DTOGenerator.TypeScript.CodeTypeReferenceService;
using Framework.DomainDriven.DTOGenerator.TypeScript.Extensions;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Base.ByProperty;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Base;
using Framework.DomainDriven.DTOGenerator.TypeScript.PropertyAssigner;
using Framework.DomainDriven.DTOGenerator.TypeScript.PropertyAssigner.Security;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Helpers
{
    /// <summary>
    /// Observable codedom helper extensions
    /// </summary>
    public static class ObservableCodeDomHelper
    {
        public static CodeTypeMember GenerateObservableProjectionFromPlainJs<TConfiguration, TFileType>(this PropertyFileFactory<TConfiguration, TFileType> fileFactory, bool useTypedReference = true)
           where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
           where TFileType : DTOFileType
        {
            if (fileFactory == null)
            {
                throw new ArgumentNullException(nameof(fileFactory));
            }

            CodeVariableReferenceExpression sourceParameterRef;
            CodeParameterDeclarationExpression sourceParameter;
            if (useTypedReference)
            {
                var typeReference = fileFactory.AsPlainFactory().CurrentInterfaceReference;
                sourceParameter = new CodeParameterDeclarationExpression(typeReference, Constants.SourceVariableName);
                sourceParameterRef = sourceParameter.ToVariableReferenceExpression();
            }
            else
            {
                sourceParameter = new CodeParameterDeclarationExpression(CodeExpressionHelper.GetAnonymousCodeTypeReference(), Constants.SourceVariableName);
                sourceParameterRef = sourceParameter.ToVariableReferenceExpression();
            }

            var codeMemberMethod = new CodeMemberMethod
            {
                Name = CreateFromMethodName(),
                Parameters = { sourceParameter },
                Attributes = MemberAttributes.Family,
                ReturnType = CodeExpressionHelper.GetVoidCodeTypeReference()
            };

            var assignStatements = fileFactory.GetProperties().Select(property =>

                new ObservableProjectionPropertyAssigner<TConfiguration>(fileFactory)
                    .MaybeSecurityToSecurity(new ProjectionObservableCodeTypeReferenceService<TConfiguration>(fileFactory.Configuration), SecurityDirection.FromPlainJs)
                    .GetAssignStatementBySource(property, sourceParameterRef, new CodeThisReferenceExpression()));

            if (fileFactory.NeedCallSuperMethod())
            {
                var methodName = CreateFromMethodName();
                var baseCall = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("super"), methodName, sourceParameterRef);
                codeMemberMethod.Statements.Add(baseCall);
            }

            codeMemberMethod.Statements.AddRange(assignStatements.ToArray());

            return codeMemberMethod;
        }

        public static CodeTypeMember GenerateObservableFromPlainJs<TConfiguration, TFileType>(this PropertyFileFactory<TConfiguration, TFileType> fileFactory, bool useTypedReference = true)
            where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
            where TFileType : DTOFileType
        {
            if (fileFactory == null)
            {
                throw new ArgumentNullException(nameof(fileFactory));
            }

            CodeVariableReferenceExpression sourceParameterRef;
            CodeParameterDeclarationExpression sourceParameter;
            if (useTypedReference)
            {
                var typeReference = fileFactory.AsPlainFactory().CurrentInterfaceReference;
                sourceParameter = new CodeParameterDeclarationExpression(typeReference, Constants.SourceVariableName);
                sourceParameterRef = sourceParameter.ToVariableReferenceExpression();
            }
            else
            {
                sourceParameter = new CodeParameterDeclarationExpression(CodeExpressionHelper.GetAnonymousCodeTypeReference(), Constants.SourceVariableName);
                sourceParameterRef = sourceParameter.ToVariableReferenceExpression();
            }

            var codeMemberMethod = new CodeMemberMethod
            {
                Name = CreateFromMethodName(),
                Parameters = { sourceParameter },
                Attributes = MemberAttributes.Family,
                ReturnType = CodeExpressionHelper.GetVoidCodeTypeReference()
            };

            var assignStatements = fileFactory.GetProperties().Select(property =>

                new VisualObservablePropertyAssigner<TConfiguration>(fileFactory)
                    .MaybeSecurityToSecurity(new ObservableCodeTypeReferenceService<TConfiguration>(fileFactory.Configuration), SecurityDirection.FromPlainJs)
                    .GetAssignStatementBySource(property, sourceParameterRef, new CodeThisReferenceExpression()));

            if (fileFactory.NeedCallSuperMethod())
            {
                var methodName = CreateFromMethodName();
                var baseCall = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("super"), methodName, sourceParameterRef);
                codeMemberMethod.Statements.Add(baseCall);
            }

            codeMemberMethod.Statements.AddRange(assignStatements.ToArray());

            return codeMemberMethod;
        }

        public static CodeTypeMember GenerateStaticFromPlainJsMethod<TConfiguration, TFileType>(this PropertyFileFactory<TConfiguration, TFileType> fileFactory)
            where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
            where TFileType : MainDTOFileType
        {
            if (fileFactory == null)
            {
                throw new ArgumentNullException(nameof(fileFactory));
            }

            var sourceTypeReference = fileFactory.AsPlainFactory().CurrentInterfaceReference;

            var sourceParameter = new CodeParameterDeclarationExpression(sourceTypeReference, Constants.SourceVariableName);

            var targetRef = fileFactory.Configuration.GetCodeTypeReference(fileFactory.DomainType, fileFactory.FileType);

            var varialableStatement = new CodeVariableDeclarationStatement(fileFactory.DomainType, Constants.DefaultVariableName, targetRef.ToObjectCreateExpression());
            var initiStatement = new CodeVariableReferenceExpression { VariableName = Constants.DefaultVariableName }
                .ToMethodInvokeExpression(CreateFromMethodName(), sourceParameter.ToVariableReferenceExpression());

            var notNullStatement = new ToIsNullOrUndefinedConditionStatement(sourceParameter.ToVariableReferenceExpression())
            {
                TrueStatements =
                    {
                        new CodePrimitiveExpression(null).ToMethodReturnStatement()
                    }
            };

            return new CodeMemberMethod
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Static,
                Parameters = { sourceParameter },
                Name = CreateFromMethodName(),
                ReturnType = targetRef,
                Statements = { notNullStatement, varialableStatement, initiStatement, new CodeVariableReferenceExpression { VariableName = Constants.DefaultVariableName }.ToMethodReturnStatement() }
            };
        }

        public static CodeTypeMember GenerateToJsonMethod<TConfiguration>(this BaseDTOFileFactory<TConfiguration> fileFactory)
            where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
        {
            if (fileFactory == null)
            {
                throw new ArgumentNullException(nameof(fileFactory));
            }

            var exp = fileFactory.FileType.AsPlainFileType();

            var targetRef = fileFactory.Configuration.GetCodeTypeReference(fileFactory.DomainType, exp);

            var varialableStatement = new CodeVariableDeclarationStatement(fileFactory.DomainType, Constants.DefaultVariableName, targetRef.ToObjectCreateExpression());

            var initiStatement = new CodeVariableReferenceExpression { VariableName = Constants.DefaultVariableName }.ToMethodInvokeExpression(CreateFromMethodName(Constants.FromObservableMethodName), new CodeThisReferenceExpression());

            return new CodeMemberMethod
            {
                Attributes = MemberAttributes.Public,
                Name = Constants.ToJsMethodName,
                ReturnType = targetRef,
                Statements = { varialableStatement, initiStatement, new CodeVariableReferenceExpression { VariableName = Constants.DefaultVariableName }.ToMethodReturnStatement() }
            };
        }

        public static CodeTypeMember GenerateObservableToStrictMethod<TConfiguration>(this BaseDTOFileFactory<TConfiguration> fileFactory)
            where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
        {
            if (fileFactory == null)
            {
                throw new ArgumentNullException(nameof(fileFactory));
            }

            var fileType = fileFactory.FileType.AsPlainFileType();

            var typeReference = fileFactory.Configuration.GetCodeTypeReference(fileFactory.DomainType, fileType);

            var targetRef = fileFactory.Configuration.GetCodeTypeReference(fileFactory.DomainType, FileType.StrictDTO);

            var variableStatement = new CodeVariableDeclarationStatement(fileFactory.DomainType, Constants.DefaultVariableName, typeReference.ToObjectCreateExpression());

            var initiStatement = new CodeVariableReferenceExpression { VariableName = Constants.DefaultVariableName }.ToMethodInvokeExpression(CreateFromMethodName(Constants.FromObservableMethodName), new CodeThisReferenceExpression());

            return new CodeMemberMethod
            {
                Attributes = MemberAttributes.Public,
                Name = Constants.ToStrictMethodName,
                ReturnType = targetRef,
                Statements = { variableStatement, initiStatement, new CodeVariableReferenceExpression { VariableName = Constants.DefaultVariableName }.ToMethodInvokeExpression(Constants.ToStrictMethodName).ToMethodReturnStatement() }
            };
        }

        public static CodeTypeMember GenerateVisualObservableFromPlainJs<TConfiguration, TFileType>(this PropertyFileFactory<TConfiguration, TFileType> fileFactory)
            where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
            where TFileType : MainDTOFileType
        {
            if (fileFactory == null)
            {
                throw new ArgumentNullException(nameof(fileFactory));
            }

            var typeReference = CodeExpressionHelper.GetAnonymousCodeTypeReference();
            var sourceParameter = new CodeParameterDeclarationExpression(typeReference, Constants.SourceVariableName);
            var sourceParameterRef = sourceParameter.ToVariableReferenceExpression();

            var codeMemberMethod = new CodeMemberMethod
                                   {
                                       Name = CreateFromMethodName(),
                                       Parameters = { sourceParameter },
                                       Attributes = MemberAttributes.Family,
                                       ReturnType = CodeExpressionHelper.GetVoidCodeTypeReference()
            };

            var assignStatements = fileFactory.GetProperties().Select(property => new VisualObservablePropertyAssigner<TConfiguration>(fileFactory).MaybeSecurityToSecurity(new ObservableCodeTypeReferenceService<TConfiguration>(fileFactory.Configuration), SecurityDirection.FromPlainJs).GetAssignStatementBySource(property, sourceParameterRef, new CodeThisReferenceExpression()));

            if (fileFactory.FileType == ObservableFileType.ObservableVisualDTO)
            {
                var methodName = CreateFromMethodName();
                var baseCall = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("super"), methodName, sourceParameterRef);
                codeMemberMethod.Statements.Add(baseCall);
            }

            codeMemberMethod.Statements.AddRange(assignStatements.ToArray());

            return codeMemberMethod;
        }

        private static string CreateFromMethodName(string prefix = Constants.FromJsMethodName)
        {
            return prefix;
        }
    }
}
