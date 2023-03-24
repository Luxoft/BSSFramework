using System;
using System.CodeDom;
using System.Linq;
using System.Reflection;

using Framework.CodeDom;
using Framework.CodeDom.TypeScript;
using Framework.Core;
using Framework.DomainDriven.DTOGenerator.TypeScript.Configuration;
using Framework.DomainDriven.DTOGenerator.TypeScript.Extensions;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Base;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Base.ByProperty;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Base;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Observable;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Visual;
using Framework.DomainDriven.DTOGenerator.TypeScript.PropertyAssigner;
using Framework.DomainDriven.DTOGenerator.TypeScript.PropertyAssigner.Security;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Helpers;

/// <summary>
/// CodeDom helper extensions
/// </summary>
public static class CodeDomHelper
{
    public static CodeTypeMember GenerateFromMethodsJs<TConfiguration>(this BaseDTOFileFactory<TConfiguration> fileFactory, bool useAnyRefAsParameter = false)
            where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        if (fileFactory == null)
        {
            throw new ArgumentNullException(nameof(fileFactory));
        }

        var sourceTypeReference = useAnyRefAsParameter ? CodeExpressionHelper.GetAnonymousCodeTypeReference() : fileFactory.CurrentInterfaceReference;
        var sourceParameter = new CodeParameterDeclarationExpression(sourceTypeReference, Constants.SourceVariableName);
        var sourceParameterRef = sourceParameter.ToVariableReferenceExpression();

        var codeMemberMethod = new CodeMemberMethod
                               {
                                       Name = Constants.FromJsMethodName,
                                       Parameters = { sourceParameter },
                                       Attributes = MemberAttributes.Family,
                                       ReturnType = CodeExpressionHelper.GetVoidCodeTypeReference()
                               };

        var assignStatements = fileFactory.GetProperties().Select(property =>
                                                                          new MainPropertyAssigner<TConfiguration>(fileFactory)
                                                                                  .MaybeSecurityToSecurity(new MainCodeTypeReferenceService<TConfiguration>(fileFactory.Configuration), SecurityDirection.FromPlainJs)
                                                                                  .GetAssignStatementBySource(property, sourceParameterRef, new CodeThisReferenceExpression()));

        if (fileFactory.NeedCallSuperMethod())
        {
            var baseCall = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("super"), Constants.FromJsMethodName, sourceParameterRef);
            codeMemberMethod.Statements.Add(baseCall);
        }

        codeMemberMethod.Statements.AddRange(assignStatements.ToArray());

        return codeMemberMethod;
    }

    public static CodeTypeMember GenerateFromObservableMethod<TConfiguration>(this BaseDTOFileFactory<TConfiguration> fileFactory, bool useAnyRefAsParameter = false)
            where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        if (fileFactory == null)
        {
            throw new ArgumentNullException(nameof(fileFactory));
        }

        var observableFactory = fileFactory.AsObservableFactory();

        var sourceParameter =
                new CodeParameterDeclarationExpression(useAnyRefAsParameter ? Constants.UnknownTypeName : observableFactory.CurrentReference.BaseType, Constants.SourceVariableName);
        var sourceParameterRef = sourceParameter.ToVariableReferenceExpression();

        var codeMemberMethod = new CodeMemberMethod
                               {
                                       Name = Constants.FromObservableMethodName,
                                       Parameters = { sourceParameter },
                                       Attributes = MemberAttributes.Family,
                                       ReturnType = CodeExpressionHelper.GetVoidCodeTypeReference()
                               };

        var assignStatements = fileFactory.GetProperties().Select(property =>

                                                                          new MainPropertyAssigner<TConfiguration>(fileFactory, true)
                                                                                  .MaybeSecurityToSecurity(new MainCodeTypeReferenceService<TConfiguration>(fileFactory.Configuration), SecurityDirection.FromObservable)
                                                                                  .GetAssignStatementBySource(property, sourceParameterRef, new CodeThisReferenceExpression()));

        if (NeedCallSuperMethod(fileFactory))
        {
            var baseCall = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("super"), Constants.FromObservableMethodName, sourceParameterRef);
            codeMemberMethod.Statements.Add(baseCall);
        }

        codeMemberMethod.Statements.AddRange(assignStatements.ToArray());

        return codeMemberMethod;
    }

    public static CodeTypeMember GenerateToStrictMethod<TConfiguration>(this BaseDTOFileFactory<TConfiguration> fileFactory)
            where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        if (fileFactory == null)
        {
            throw new ArgumentNullException(nameof(fileFactory));
        }

        var targetRef = fileFactory.Configuration.GetCodeTypeReference(fileFactory.DomainType, FileType.StrictDTO);

        var varialableStatement = new CodeVariableDeclarationStatement(fileFactory.DomainType, Constants.DefaultVariableName, targetRef.ToObjectCreateExpression());

        var initiStatement = new CodeVariableReferenceExpression { VariableName = Constants.DefaultVariableName }.ToMethodInvokeExpression(CreateFromMethod(fileFactory.FileType, false), new CodeThisReferenceExpression());

        return new CodeMemberMethod
               {
                       Attributes = MemberAttributes.Public,
                       Name = Constants.ToStrictMethodName,
                       ReturnType = targetRef,
                       Statements = { varialableStatement, initiStatement, new CodeVariableReferenceExpression { VariableName = Constants.DefaultVariableName }.ToMethodReturnStatement() }
               };
    }

    public static CodeTypeMember GenerateToUpdateMethod<TConfiguration>(this BaseDTOFileFactory<TConfiguration> fileFactory)
            where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        if (fileFactory == null)
        {
            throw new ArgumentNullException(nameof(fileFactory));
        }

        var targetRef = fileFactory.Configuration.GetCodeTypeReference(fileFactory.DomainType, FileType.UpdateDTO);

        var varialableStatement = new CodeVariableDeclarationStatement(fileFactory.DomainType, Constants.DefaultVariableName, targetRef.ToObjectCreateExpression());

        var initiStatement = new CodeVariableReferenceExpression { VariableName = Constants.DefaultVariableName }.ToMethodInvokeExpression(CreateFromMethod(fileFactory.FileType, false), new CodeThisReferenceExpression());

        return new CodeMemberMethod
               {
                       Attributes = MemberAttributes.Public,
                       Name = Constants.ToUpdateMethodName,
                       ReturnType = targetRef,
                       Statements = { varialableStatement, initiStatement, new CodeVariableReferenceExpression { VariableName = Constants.DefaultVariableName }.ToMethodReturnStatement() }
               };
    }

    public static CodeTypeMember GenerateToObservableMethod<TConfiguration>(this IBaseDTOFileFactory<TConfiguration> fileFactory)
            where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        if (fileFactory == null)
        {
            throw new ArgumentNullException(nameof(fileFactory));
        }

        var exp = fileFactory.FileType.AsObservableFileType();

        var targetRef = fileFactory.Configuration.GetCodeTypeReference(fileFactory.DomainType, exp);

        var varialableStatement = new CodeVariableDeclarationStatement(fileFactory.DomainType, Constants.DefaultVariableName, targetRef.ToObjectCreateExpression());

        var initiStatement = new CodeVariableReferenceExpression { VariableName = Constants.DefaultVariableName }.ToMethodInvokeExpression(Constants.FromJsMethodName, new CodeThisReferenceExpression());

        return new CodeMemberMethod
               {
                       Attributes = MemberAttributes.Public,
                       Name = Constants.ToObservableMethodName,
                       ReturnType = targetRef,
                       Statements = { varialableStatement, initiStatement, new CodeVariableReferenceExpression { VariableName = Constants.DefaultVariableName }.ToMethodReturnStatement() }
               };
    }

    public static CodeTypeMember GenerateFromStaticInitializeMethodJs(IClientFileFactory<ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>, DTOFileType> fileFactory, bool useAnyRefAsParameter = false)
    {
        if (fileFactory == null)
        {
            throw new ArgumentNullException(nameof(fileFactory));
        }

        var sourceTypeReference = useAnyRefAsParameter ? CodeExpressionHelper.GetAnonymousCodeTypeReference() : fileFactory.CurrentInterfaceReference ?? CodeExpressionHelper.GetAnonymousCodeTypeReference();

        var sourceParameter = new CodeParameterDeclarationExpression(sourceTypeReference, Constants.SourceVariableName);

        var targetRef = fileFactory.Configuration.GetCodeTypeReference(fileFactory.DomainType, fileFactory.FileType);

        var varialableStatement = new CodeVariableDeclarationStatement(fileFactory.DomainType, Constants.DefaultVariableName, targetRef.ToObjectCreateExpression());
        var initiStatement = new CodeVariableReferenceExpression { VariableName = Constants.DefaultVariableName }.ToMethodInvokeExpression(Constants.FromJsMethodName, sourceParameter.ToVariableReferenceExpression());
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
                       Name = Constants.FromJsMethodName,
                       ReturnType = targetRef,
                       Statements = { notNullStatement, varialableStatement, initiStatement, new CodeVariableReferenceExpression { VariableName = Constants.DefaultVariableName }.ToMethodReturnStatement() }
               };
    }

       
    public static CodeTypeMember GenerateFromStrictStaticInitializeMethodJs(IClientFileFactory<ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>, DTOFileType> fileFactory, Framework.DomainDriven.DTOGenerator.FileType fileType, Framework.DomainDriven.DTOGenerator.FileType targetfileType)
    {
        if (fileFactory == null)
        {
            throw new ArgumentNullException(nameof(fileFactory));
        }

        if (fileType == null)
        {
            throw new ArgumentNullException(nameof(fileType));
        }

        var codeTypeReference = fileFactory.Configuration.GetCodeTypeReference(fileFactory.DomainType, fileType);

        var sourceParameter = new CodeParameterDeclarationExpression(codeTypeReference, Constants.SourceVariableName);

        var targetRef = fileFactory.Configuration.GetCodeTypeReference(fileFactory.DomainType, targetfileType);

        var varialableStatement = new CodeVariableDeclarationStatement(fileFactory.DomainType, Constants.DefaultVariableName, targetRef.ToObjectCreateExpression());
        var initiStatement = new CodeVariableReferenceExpression { VariableName = Constants.DefaultVariableName }.ToMethodInvokeExpression(CreateFromMethod(fileType, false), sourceParameter.ToVariableReferenceExpression());
        return new CodeMemberMethod
               {
                       Attributes = MemberAttributes.Public | MemberAttributes.Static,
                       Parameters = { sourceParameter },
                       Name = CreateFromMethod(fileType),
                       ReturnType = targetRef,
                       Statements = { varialableStatement, initiStatement, new CodeVariableReferenceExpression { VariableName = Constants.DefaultVariableName }.ToMethodReturnStatement() }
               };
    }

    public static CodeTypeMember GenerateToStrictMethods<TConfiguration, TFileType>(this PropertyFileFactory<TConfiguration, TFileType> fileFactory, MainDTOFileType fileType)
            where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
            where TFileType : DTOFileType
    {
        if (fileFactory == null)
        {
            throw new ArgumentNullException(nameof(fileFactory));
        }

        var factory = GetFactory(fileFactory, fileType);

        var sourceParameter = new CodeParameterDeclarationExpression(factory.CurrentReference, Constants.SourceVariableName);
        var sourceParameterRef = sourceParameter.ToVariableReferenceExpression();

        var codeMemberMethod = new CodeMemberMethod
                               {
                                       Name = CreateFromMethod(fileType, false),
                                       Parameters = { sourceParameter },
                                       Attributes = MemberAttributes.Family,
                                       ReturnType = CodeExpressionHelper.GetVoidCodeTypeReference()
                               };

        var factoryProperties = factory.GetProperties().Select(x => x.Name);

        var assignStatements = fileFactory.GetProperties().Where(x => factoryProperties.Contains(x.Name))
                                          .Select(property =>
                                                          new PropertyAssigner.MainToStrictPropertyAssigner<TConfiguration>(fileFactory)
                                                                  .MaybeSecurityToSecurity(new MainCodeTypeReferenceService<TConfiguration>(fileFactory.Configuration), SecurityDirection.FromObservable)
                                                                  .GetAssignStatementBySource(property, sourceParameterRef, new CodeThisReferenceExpression()));
        var baseType = fileType.BaseType;

        var callBase = fileFactory.IsPersistent() || baseType != FileType.BaseAuditPersistentDTO;
        if (baseType != null && baseType != FileType.BaseAbstractDTO && callBase)
        {
            var methodName = CreateFromMethod(baseType, false);
            var baseCall = new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), methodName, sourceParameterRef);
            codeMemberMethod.Statements.Add(baseCall);
        }

        codeMemberMethod.Statements.AddRange(assignStatements.ToArray());

        return codeMemberMethod;
    }

    public static CodeTypeMember GenerateUpdateFromMethods<TConfiguration, TFileType>(this PropertyFileFactory<TConfiguration, TFileType> fileFactory, MainDTOFileType fileType)
            where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
            where TFileType : DTOFileType
    {
        if (fileFactory == null)
        {
            throw new ArgumentNullException(nameof(fileFactory));
        }

        var factory = GetFactory(fileFactory, fileType);

        var sourceParameter = new CodeParameterDeclarationExpression(factory.CurrentReference, Constants.SourceVariableName);
        var sourceParameterRef = sourceParameter.ToVariableReferenceExpression();

        var codeMemberMethod = new CodeMemberMethod
                               {
                                       Name = CreateFromMethod(fileType, false),
                                       Parameters = { sourceParameter },
                                       Attributes = MemberAttributes.Family,
                                       ReturnType = CodeExpressionHelper.GetVoidCodeTypeReference()
                               };
        var factoryProperties = factory.GetProperties().Select(x => x.Name);
        if (fileType == FileType.SimpleDTO)
        {
            factoryProperties = new[] { fileFactory.Configuration.Environment.IdentityProperty.Name }.Union(factoryProperties);
        }

        var assignStatements = fileFactory.GetProperties().Where(x => factoryProperties.Contains(x.Name))
                                          .Select(property =>
                                                          new MainToUpdatePropertyAssigner<TConfiguration>(fileFactory)
                                                                  .MaybeSecurityToSecurity(new MainCodeTypeReferenceService<TConfiguration>(fileFactory.Configuration), SecurityDirection.FromObservable)
                                                                  .GetAssignStatementBySource(property, sourceParameterRef, new CodeThisReferenceExpression()));

        if (fileType == FileType.FullDTO || fileType == FileType.RichDTO)
        {
            var methodName = CreateFromMethod(fileType.BaseType, false);
            var baseCall = new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), methodName, sourceParameterRef);
            codeMemberMethod.Statements.Add(baseCall);
        }

        codeMemberMethod.Statements.AddRange(assignStatements.ToArray());

        return codeMemberMethod;
    }

    public static CodeTypeMember GenerateToNativeJsonMethod<TConfiguration>(this PropertyFileFactory<TConfiguration, DTOFileType> fileFactory)
            where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        if (fileFactory == null)
        {
            throw new ArgumentNullException(nameof(fileFactory));
        }

        var sourceParameterRef = new CodeThisReferenceExpression();
        var variableDeclaration = new CodeVariableDeclarationStatement(Constants.UnknownTypeName, "result", new CodeSnippetExpression("{}"));

        var codeMemberMethod = new CodeMemberMethod
                               {
                                       Name = Constants.ToNativeJsonMethodName,
                                       Attributes = MemberAttributes.Family,
                                       ReturnType = CodeExpressionHelper.GetAnonymousCodeTypeReference(),
                                       Statements = { variableDeclaration }
                               };

        var assignStatements = fileFactory.GetProperties()
                                          .Select(property =>
                                                          new StrictToNativeJsonPropertyAssigner<TConfiguration>(fileFactory)
                                                                  .GetAssignStatementBySource(property, sourceParameterRef, variableDeclaration.ToVariableReferenceExpression()));

        codeMemberMethod.Statements.AddRange(assignStatements.ToArray());
        codeMemberMethod.Statements.Add(variableDeclaration.ToVariableReferenceExpression().ToMethodReturnStatement());
        return codeMemberMethod;
    }

    public static CodeTypeMember GenerateVisualFromMethodsJs<TConfiguration>(this BaseDTOFileFactory<TConfiguration> fileFactory)
            where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        if (fileFactory == null)
        {
            throw new ArgumentNullException(nameof(fileFactory));
        }

        var sourceParameter = new CodeParameterDeclarationExpression(CodeExpressionHelper.GetAnonymousCodeTypeReference(), Constants.SourceVariableName);
        var sourceParameterRef = sourceParameter.ToVariableReferenceExpression();

        var codeMemberMethod = new CodeMemberMethod
                               {
                                       Name = Constants.FromJsMethodName,
                                       Parameters = { sourceParameter },
                                       Attributes = MemberAttributes.Family,
                                       ReturnType = CodeExpressionHelper.GetVoidCodeTypeReference()
                               };

        var assignStatements = fileFactory.GetProperties().Select(property =>

                                                                          new MainPropertyAssigner<TConfiguration>(fileFactory)
                                                                                  .MaybeSecurityToSecurity(new MainCodeTypeReferenceService<TConfiguration>(fileFactory.Configuration), SecurityDirection.FromPlainJs)
                                                                                  .GetAssignStatementBySource(property, sourceParameterRef, new CodeThisReferenceExpression()));

        var baseCall = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("super"), Constants.FromJsMethodName, sourceParameterRef);
        codeMemberMethod.Statements.Add(baseCall);

        codeMemberMethod.Statements.AddRange(assignStatements.ToArray());

        return codeMemberMethod;
    }

    public static CodeTypeMember GenerateFromObservableBaseVisualMethod<TConfiguration>(this BaseDTOFileFactory<TConfiguration> fileFactory)
            where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        if (fileFactory == null)
        {
            throw new ArgumentNullException(nameof(fileFactory));
        }

        var obsrv = fileFactory.AsObservableFactory();

        var sourceParameter = new CodeParameterDeclarationExpression(obsrv.CurrentReference.BaseType, Constants.SourceVariableName);
        var sourceParameterRef = sourceParameter.ToVariableReferenceExpression();

        var codeMemberMethod = new CodeMemberMethod
                               {
                                       Name = Constants.FromObservableMethodName,
                                       Parameters = { sourceParameter },
                                       Attributes = MemberAttributes.Family,
                                       ReturnType = CodeExpressionHelper.GetVoidCodeTypeReference()
                               };

        var assignStatements = fileFactory.GetProperties().Select(property =>

                                                                          new MainPropertyAssigner<TConfiguration>(fileFactory, true)
                                                                                  .MaybeSecurityToSecurity(new MainCodeTypeReferenceService<TConfiguration>(fileFactory.Configuration), SecurityDirection.FromObservable)
                                                                                  .GetAssignStatementBySource(property, sourceParameterRef, new CodeThisReferenceExpression()));

        codeMemberMethod.Statements.AddRange(assignStatements.ToArray());

        return codeMemberMethod;
    }

    public static CodeTypeMember GenerateFromMethodsJs<TConfiguration>(this PropertyFileFactory<TConfiguration, DTOFileType> fileFactory, FileType baseFileType = null)
            where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        if (fileFactory == null)
        {
            throw new ArgumentNullException(nameof(fileFactory));
        }

        var sourceParameter = new CodeParameterDeclarationExpression(CodeExpressionHelper.GetAnonymousCodeTypeReference(), Constants.SourceVariableName);
        var sourceParameterRef = sourceParameter.ToVariableReferenceExpression();

        var codeMemberMethod = new CodeMemberMethod
                               {
                                       Name = Constants.FromJsMethodName,
                                       Parameters = { sourceParameter },
                                       Attributes = MemberAttributes.Family,
                                       ReturnType = CodeExpressionHelper.GetVoidCodeTypeReference()
                               };

        var assignStatements = fileFactory.GetProperties().Select(property =>
                                                                          new ClassPropertyAssigner<TConfiguration>(fileFactory)
                                                                                  .MaybeSecurityToSecurity(new MainCodeTypeReferenceService<TConfiguration>(fileFactory.Configuration), SecurityDirection.FromPlainJs)
                                                                                  .GetAssignStatementBySource(property, sourceParameterRef, new CodeThisReferenceExpression()));

        if (fileFactory.NeedCallSuperMethod())
        {
            var baseCall = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("super"), Constants.FromJsMethodName, sourceParameterRef);
            codeMemberMethod.Statements.Add(baseCall);
        }

        codeMemberMethod.Statements.AddRange(assignStatements.ToArray());

        return codeMemberMethod;
    }

    public static CodeTypeMember GenerateFromMethodsJs<TConfiguration>(this DefaultProjectionDTOFileFactory<TConfiguration> fileFactory, FileType baseFileType = null)
            where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        if (fileFactory == null)
        {
            throw new ArgumentNullException(nameof(fileFactory));
        }

        var sourceParameter = new CodeParameterDeclarationExpression(CodeExpressionHelper.GetAnonymousCodeTypeReference(), Constants.SourceVariableName);
        var sourceParameterRef = sourceParameter.ToVariableReferenceExpression();

        var codeMemberMethod = new CodeMemberMethod
                               {
                                       Name = Constants.FromJsMethodName,
                                       Parameters = { sourceParameter },
                                       Attributes = MemberAttributes.Family,
                                       ReturnType = CodeExpressionHelper.GetVoidCodeTypeReference()
                               };

        var assignStatements = fileFactory.GetProperties().Select(property =>
                                                                          new ProjectionPropertyAssigner<TConfiguration>(fileFactory)
                                                                                  .MaybeSecurityToSecurity(new MainCodeTypeReferenceService<TConfiguration>(fileFactory.Configuration), SecurityDirection.FromPlainJs)
                                                                                  .GetAssignStatementBySource(property, sourceParameterRef, new CodeThisReferenceExpression()));

        if (fileFactory.NeedCallSuperMethod())
        {
            var baseCall = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("super"), Constants.FromJsMethodName, sourceParameterRef);
            codeMemberMethod.Statements.Add(baseCall);
        }

        codeMemberMethod.Statements.AddRange(assignStatements.ToArray());

        return codeMemberMethod;
    }

    public static CodeTypeReference CheckForModuleReference<TConfiguration>(this CodeTypeReference reference, TConfiguration cfg)
            where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        var baseTypeName = reference.BaseType;
        var typeName = baseTypeName.GetLastKeyword();
        var nameSpace = baseTypeName.Replace($".{typeName}", string.Empty);
        RequireJsModule module = cfg.GetModules().FirstOrDefault(x => x.NameSpaces.Contains(nameSpace));

        return module != null ? new CodeTypeReference($"{module.ModuleName}.{typeName}") : reference;
    }

    // TODO: create specified CodeTypeReferenceService and use it
    public static CodeTypeReference SimplifyCodeTypeReference<TConfiguration>(this IClientFileFactory<TConfiguration, DTOFileType> fileFactory, PropertyInfo propertyType)
            where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        if (propertyType.PropertyType.IsCollectionOrArray())
        {
            var codeTypeReference = fileFactory.CodeTypeReferenceService.GetCodeTypeReference(propertyType, Constants.UseSecurity);
            var type = propertyType.PropertyType.GetCollectionOrArrayElementType();

            if (codeTypeReference.TypeArguments.Count > 0)
            {
                return codeTypeReference.NormalizeTypeReference(type).ConvertToArray(false);
            }

            return new CodeTypeReference(type.FullName).NormalizeTypeReference(type).ConvertToArray(false);
        }

        return fileFactory.CodeTypeReferenceService.GetCodeTypeReference(propertyType, Constants.UseSecurity);
    }

    private static BaseDTOFileFactory<TConfiguration> GetFactory<TConfiguration>(IClientFileFactory<TConfiguration, DTOFileType> fileFactory, FileType baseFileType = null)
            where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        var fileType = baseFileType ?? fileFactory.FileType;

        if (fileType == FileType.VisualDTO)
        {
            return new DefaultVisualDTOFileFactory<TConfiguration>(fileFactory.Configuration, fileFactory.DomainType);
        }

        if (fileType == FileType.SimpleDTO)
        {
            return new DefaultSimpleDTOFileFactory<TConfiguration>(fileFactory.Configuration, fileFactory.DomainType);
        }

        if (fileType == FileType.FullDTO)
        {
            return new DefaultFullDTOFileFactory<TConfiguration>(fileFactory.Configuration, fileFactory.DomainType);
        }

        if (fileType == FileType.RichDTO)
        {
            return new DefaultRichDTOFileFactory<TConfiguration>(fileFactory.Configuration, fileFactory.DomainType);
        }

        if (fileType == FileType.BasePersistentDTO)
        {
            return new DefaultBasePersistentDTOFileFactory<TConfiguration>(fileFactory.Configuration);
        }

        if (fileType == FileType.BaseAuditPersistentDTO)
        {
            return new DefaultBaseAuditPersistentDTOFileFactory<TConfiguration>(fileFactory.Configuration);
        }

        return null;
    }

    public static bool NeedCallSuperMethod<TConfiguration, TFileType>(this PropertyFileFactory<TConfiguration, TFileType> fileFactory)
            where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
            where TFileType : DTOFileType
    {
        var baseType = fileFactory.DomainType.BaseType;

        return !IsSystem(baseType);
    }

    private static bool IsSystem(Type type)
    {
        return type != null && type.Namespace?.StartsWith("System") == true;
    }

    private static string CreateFromMethod(FileType fileType, bool isInternal = false)
    {
        return isInternal ? "from" + fileType.Name.SkipLast(Constants.DTOName, true) + "Internal" : "from" + fileType.Name.SkipLast(Constants.DTOName, true);
    }
}
