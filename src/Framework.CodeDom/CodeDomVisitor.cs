using System.CodeDom;

using CommonFramework;

using Framework.Core;

namespace Framework.CodeDom;

public abstract class CodeDomVisitor
{
    protected CodeDomVisitor()
    {
    }

    protected int Deep { get; private set; }

    protected int NextDeep => this.Deep + 1;

    public virtual CodeNamespaceImport VisitNamespaceImport(CodeNamespaceImport codeNamespaceImport)
    {
        if (codeNamespaceImport == null) throw new ArgumentNullException(nameof(codeNamespaceImport));

        return new CodeNamespaceImport(codeNamespaceImport.Namespace) { LinePragma = codeNamespaceImport.LinePragma };
    }

    public virtual CodeCompileUnit VisitCompileUnit(CodeCompileUnit codeCompileUnit)
    {
        if (codeCompileUnit == null) throw new ArgumentNullException(nameof(codeCompileUnit));

        var newCodeCompileUnit = new CodeCompileUnit();

        newCodeCompileUnit.AssemblyCustomAttributes.AddRange(codeCompileUnit.AssemblyCustomAttributes.ToArrayExceptNull(this.VisitAttributeDeclaration));
        newCodeCompileUnit.ReferencedAssemblies.AddRange(codeCompileUnit.ReferencedAssemblies.ToArrayExceptNull(v => v));
        newCodeCompileUnit.Namespaces.AddRange(codeCompileUnit.Namespaces.ToArrayExceptNull(this.VisitNamespace));

        newCodeCompileUnit.CopyUserDataFrom(codeCompileUnit);

        return newCodeCompileUnit;
    }

    public virtual CodeNamespace VisitNamespace(CodeNamespace codeNamespace)
    {
        if (codeNamespace == null) throw new ArgumentNullException(nameof(codeNamespace));

        var newCodeNamespace = new CodeNamespace(codeNamespace.Name);

        newCodeNamespace.Comments.AddRange(codeNamespace.Comments.ToArrayExceptNull(this.VisitCommentStatement));
        newCodeNamespace.Imports.AddRange(codeNamespace.Imports.Cast<CodeNamespaceImport>().ToArrayExceptNull(this.VisitNamespaceImport));

        this.DeepOperation(() => newCodeNamespace.Types.AddRange(codeNamespace.Types.OfType<CodeTypeDeclaration>().ToArrayExceptNull(this.VisitTypeDeclaration)));

        codeNamespace.CopyUserDataFrom(codeNamespace);

        return newCodeNamespace;
    }

    public virtual CodeCommentStatement VisitCommentStatement(CodeCommentStatement codeCommentStatement)
    {
        if (codeCommentStatement == null) throw new ArgumentNullException(nameof(codeCommentStatement));

        return codeCommentStatement;
    }

    public virtual CodeTypeDeclaration VisitTypeDeclaration(CodeTypeDeclaration codeTypeDeclaration)
    {
        if (codeTypeDeclaration == null) throw new ArgumentNullException(nameof(codeTypeDeclaration));

        var newTypeDeclaration = new CodeTypeDeclaration
                                 {
                                         IsClass = codeTypeDeclaration.IsClass,
                                         IsEnum = codeTypeDeclaration.IsEnum,
                                         IsInterface = codeTypeDeclaration.IsInterface,
                                         IsPartial = codeTypeDeclaration.IsPartial,
                                         IsStruct = codeTypeDeclaration.IsStruct,
                                         TypeAttributes = codeTypeDeclaration.TypeAttributes,
                                 };

        this.InitializeTypeMember(newTypeDeclaration, codeTypeDeclaration);

        newTypeDeclaration.BaseTypes.AddRange(codeTypeDeclaration.BaseTypes.ToArrayExceptNull(this.VisitTypeReference));
        newTypeDeclaration.TypeParameters.AddRange(codeTypeDeclaration.TypeParameters.ToArrayExceptNull(this.VisitTypeParameter));

        this.DeepOperation(() => newTypeDeclaration.Members.AddRange(codeTypeDeclaration.Members.Cast<CodeTypeMember>().ToArrayExceptNull(this.VisitTypeMember)));

        newTypeDeclaration.CopyUserDataFrom(codeTypeDeclaration);

        return newTypeDeclaration;
    }

    public virtual CodeTypeMember VisitTypeMember(CodeTypeMember codeTypeMember)
    {
        if (codeTypeMember == null) throw new ArgumentNullException(nameof(codeTypeMember));

        switch (codeTypeMember)
        {
            case CodeMemberField field:
                return this.VisitMemberField(field);

            case CodeMemberProperty property:
                return this.VisitMemberProperty(property);

            case CodeMemberMethod method:
                return this.VisitMemberMethod(method);

            case CodeTypeDeclaration declaration:
                return this.VisitTypeDeclaration(declaration);

            default:
                return codeTypeMember;
        }
    }

    protected virtual CodeStatementCollection VisitMemberMethodStatements(CodeStatementCollection collection)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));

        return this.VisitStatementCollection(collection);
    }

    private void InitializeTypeMember<T>(T newInstance, T baseInstance)
            where T : CodeTypeMember
    {
        if (newInstance == null) throw new ArgumentNullException(nameof(newInstance));
        if (baseInstance == null) throw new ArgumentNullException(nameof(baseInstance));

        newInstance.Name = baseInstance.Name;
        newInstance.Attributes = baseInstance.Attributes;
        newInstance.LinePragma = baseInstance.LinePragma;
        newInstance.Comments.AddRange(baseInstance.Comments.ToArrayExceptNull(this.VisitCommentStatement));
        newInstance.CustomAttributes.AddRange(baseInstance.CustomAttributes.ToArrayExceptNull(this.VisitAttributeDeclaration));

        newInstance.StartDirectives.AddRange(baseInstance.StartDirectives);
        newInstance.EndDirectives.AddRange(baseInstance.EndDirectives);

        newInstance.CopyUserDataFrom(baseInstance);
    }

    private void InitializeMemberMethod<T>(T newInstance, T baseInstance)
            where T : CodeMemberMethod
    {
        if (newInstance == null) throw new ArgumentNullException(nameof(newInstance));
        if (baseInstance == null) throw new ArgumentNullException(nameof(baseInstance));

        this.InitializeTypeMember(newInstance, baseInstance);

        newInstance.ReturnType = baseInstance.ReturnType.Maybe(v => this.VisitTypeReference(v));

        this.DeepOperation(() => newInstance.Statements.AddRange(this.VisitMemberMethodStatements(baseInstance.Statements)));

        newInstance.Parameters.AddRange(this.VisitParameterDeclarationExpressionCollection(baseInstance.Parameters));
        newInstance.PrivateImplementationType = baseInstance.PrivateImplementationType.Maybe(v => this.VisitTypeReference(v));
        newInstance.ImplementationTypes.AddRange(baseInstance.ImplementationTypes.ToArrayExceptNull(this.VisitTypeReference));
        newInstance.ReturnTypeCustomAttributes.AddRange(baseInstance.ReturnTypeCustomAttributes.ToArrayExceptNull(this.VisitAttributeDeclaration));
        newInstance.TypeParameters.AddRange(baseInstance.TypeParameters.ToArrayExceptNull(this.VisitTypeParameter));

        newInstance.CopyUserDataFrom(baseInstance);
    }

    public virtual CodeConstructor VisitConstructor(CodeConstructor codeConstructor)
    {
        if (codeConstructor == null) throw new ArgumentNullException(nameof(codeConstructor));

        var newConstructor = new CodeConstructor();

        newConstructor.BaseConstructorArgs.AddRange(codeConstructor.BaseConstructorArgs.ToArrayExceptNull(this.VisitExpression));
        newConstructor.ChainedConstructorArgs.AddRange(codeConstructor.ChainedConstructorArgs.ToArrayExceptNull(this.VisitExpression));

        this.InitializeMemberMethod(newConstructor, codeConstructor);

        return newConstructor;
    }

    public virtual CodeMemberProperty VisitMemberProperty(CodeMemberProperty codeMemberProperty)
    {
        if (codeMemberProperty == null) throw new ArgumentNullException(nameof(codeMemberProperty));

        var newMemberProperty = new CodeMemberProperty
                                {
                                        HasGet = codeMemberProperty.HasGet,
                                        HasSet = codeMemberProperty.HasSet,
                                        PrivateImplementationType = codeMemberProperty.PrivateImplementationType.Maybe(v => this.VisitTypeReference(v)),
                                        Type = this.VisitTypeReference(codeMemberProperty.Type)
                                };

        this.InitializeTypeMember(newMemberProperty, codeMemberProperty);

        newMemberProperty.ImplementationTypes.AddRange(codeMemberProperty.ImplementationTypes.ToArrayExceptNull(this.VisitTypeReference));

        this.DeepOperation(() =>
                           {
                               newMemberProperty.GetStatements.AddRange(this.VisitStatementCollection(codeMemberProperty.GetStatements));
                               newMemberProperty.SetStatements.AddRange(this.VisitStatementCollection(codeMemberProperty.SetStatements));
                           });

        newMemberProperty.Parameters.AddRange(this.VisitParameterDeclarationExpressionCollection(codeMemberProperty.Parameters));

        return newMemberProperty;
    }

    public virtual CodeMemberMethod VisitMemberMethod(CodeMemberMethod codeMemberMethod)
    {
        if (codeMemberMethod == null) throw new ArgumentNullException(nameof(codeMemberMethod));

        if (codeMemberMethod is CodeConstructor)
        {
            return this.VisitConstructor(codeMemberMethod as CodeConstructor);
        }
        else
        {
            return new CodeMemberMethod().Self(v => this.InitializeMemberMethod(v, codeMemberMethod));
        }
    }

    public virtual CodeMemberField VisitMemberField(CodeMemberField codeMemberField)
    {
        if (codeMemberField == null) throw new ArgumentNullException(nameof(codeMemberField));

        return new CodeMemberField
               {
                       Type = this.VisitTypeReference(codeMemberField.Type),
                       InitExpression = codeMemberField.InitExpression.Maybe(this.VisitExpression)
               }.Self(v => this.InitializeTypeMember(v, codeMemberField));
    }

    public virtual CodeTypeParameter VisitTypeParameter(CodeTypeParameter codeTypeParameter)
    {
        if (codeTypeParameter == null) throw new ArgumentNullException(nameof(codeTypeParameter));

        var newTypeParameter = new CodeTypeParameter(codeTypeParameter.Name) { HasConstructorConstraint = codeTypeParameter.HasConstructorConstraint };

        newTypeParameter.CustomAttributes.AddRange(codeTypeParameter.CustomAttributes.ToArrayExceptNull(this.VisitAttributeDeclaration));
        newTypeParameter.Constraints.AddRange(codeTypeParameter.Constraints.ToArrayExceptNull(this.VisitTypeReference));

        return newTypeParameter;
    }

    public virtual CodeAttributeDeclaration VisitAttributeDeclaration(CodeAttributeDeclaration codeAttributeDeclaration)
    {
        var newAttributeDeclaration = new CodeAttributeDeclaration (codeAttributeDeclaration.AttributeType)
                                      {
                                              Name = codeAttributeDeclaration.Name
                                      };

        newAttributeDeclaration.Arguments.AddRange(codeAttributeDeclaration.Arguments.Cast<CodeAttributeArgument>().ToArrayExceptNull(this.VisitAttributeArgument));

        return newAttributeDeclaration;
    }

    public virtual CodeAttributeArgument VisitAttributeArgument (CodeAttributeArgument codeAttributeArgument)
    {
        if (codeAttributeArgument == null) throw new ArgumentNullException(nameof(codeAttributeArgument));

        return new CodeAttributeArgument(codeAttributeArgument.Name, codeAttributeArgument.Value.Maybe(v => this.VisitExpression(v)));
    }

    public virtual CodeParameterDeclarationExpressionCollection VisitParameterDeclarationExpressionCollection (CodeParameterDeclarationExpressionCollection collection)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));

        return new CodeParameterDeclarationExpressionCollection(collection.ToArrayExceptNull(this.VisitParameterDeclarationExpression));
    }

    public virtual CodeStatementCollection VisitStatementCollection(CodeStatementCollection collection)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));

        return new CodeStatementCollection(collection.ToArrayExceptNull(this.VisitStatement));
    }

    public virtual CodeParameterDeclarationExpression VisitParameterDeclarationExpression(CodeParameterDeclarationExpression codeParameterDeclarationExpression)
    {
        if (codeParameterDeclarationExpression == null) throw new ArgumentNullException(nameof(codeParameterDeclarationExpression));

        return new CodeParameterDeclarationExpression(this.VisitTypeReference(codeParameterDeclarationExpression.Type), codeParameterDeclarationExpression.Name)
               {
                       Direction = codeParameterDeclarationExpression.Direction,
                       CustomAttributes = new CodeAttributeDeclarationCollection(codeParameterDeclarationExpression.CustomAttributes.ToArrayExceptNull(this.VisitAttributeDeclaration))
               }.WithCopyUserDataFrom(codeParameterDeclarationExpression);
    }

    public virtual CodeExpression VisitExpression (CodeExpression codeExpression)
    {
        if (codeExpression == null) throw new ArgumentNullException(nameof(codeExpression));

        if (codeExpression is CodeCastExpression)
        {
            return this.VisitCastExpression(codeExpression as CodeCastExpression);
        }
        if (codeExpression is CodeObjectCreateExpression)
        {
            return this.VisitObjectCreateExpression(codeExpression as CodeObjectCreateExpression);
        }
        else if (codeExpression is CodeMethodInvokeExpression)
        {
            return this.VisitMethodInvokeExpression(codeExpression as CodeMethodInvokeExpression);
        }
        else if (codeExpression is CodeParameterDeclarationExpression)
        {
            return this.VisitParameterDeclarationExpression(codeExpression as CodeParameterDeclarationExpression);
        }
        else if (codeExpression is CodeBinaryOperatorExpression)
        {
            return this.VisitBinaryOperatorExpression(codeExpression as CodeBinaryOperatorExpression);
        }
        else if (codeExpression is CodeThisReferenceExpression)
        {
            return this.VisitThisReferenceExpression(codeExpression as CodeThisReferenceExpression);
        }
        else if (codeExpression is CodeTypeReferenceExpression)
        {
            return this.VisitTypeReferenceExpression(codeExpression as CodeTypeReferenceExpression);
        }
        else if(codeExpression is CodeLambdaExpression)
        {
            return this.VisitLambdaExpression(codeExpression as CodeLambdaExpression);
        }
        else
        {
            return codeExpression;
        }
    }

    public virtual CodeExpression VisitLambdaExpression(CodeLambdaExpression codeExpression)
    {
        return new CodeLambdaExpression
               {
                       Parameters =
                               this.VisitParameterDeclarationExpressionCollection(codeExpression.Parameters),
                       Statements = this.VisitStatementCollection(codeExpression.Statements),
               }.WithCopyUserDataFrom(codeExpression);
    }

    public virtual CodeExpression VisitThisReferenceExpression(CodeThisReferenceExpression codeExpression)
    {
        if (codeExpression == null) throw new ArgumentNullException(nameof(codeExpression));

        return codeExpression;
    }

    public virtual CodeExpression VisitTypeReferenceExpression(CodeTypeReferenceExpression codeExpression)
    {
        if (codeExpression == null) throw new ArgumentNullException(nameof(codeExpression));

        return new CodeTypeReferenceExpression
               {
                       Type = this.VisitTypeReference(codeExpression.Type)
               }.WithCopyUserDataFrom(codeExpression);
    }

    protected virtual CodeMethodReferenceExpression VisitMethodReferenceExpression(CodeMethodReferenceExpression codeMethodReferenceExpression)
    {
        if (codeMethodReferenceExpression == null) throw new ArgumentNullException(nameof(codeMethodReferenceExpression));

        return new CodeMethodReferenceExpression(
                                                 codeMethodReferenceExpression.TargetObject.Maybe(v => this.VisitExpression(v)),
                                                 codeMethodReferenceExpression.MethodName,
                                                 codeMethodReferenceExpression.TypeArguments.ToArrayExceptNull(this.VisitTypeReference));
    }

    protected virtual CodeExpressionCollection VisitExpressionCollection(CodeExpressionCollection collection)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));

        return new CodeExpressionCollection(collection.ToArrayExceptNull(this.VisitExpression));
    }

    public virtual CodeExpression VisitMethodInvokeExpression(CodeMethodInvokeExpression codeMethodInvokeExpression)
    {
        if (codeMethodInvokeExpression == null) throw new ArgumentNullException(nameof(codeMethodInvokeExpression));

        return new CodeMethodInvokeExpression(
                                              this.VisitMethodReferenceExpression(codeMethodInvokeExpression.Method),
                                              this.VisitExpressionCollection(codeMethodInvokeExpression.Parameters).ToArrayExceptNull(v => v))
                .WithCopyUserDataFrom(codeMethodInvokeExpression);
    }

    public virtual CodeExpression VisitCastExpression(CodeCastExpression codeCastExpression)
    {
        if (codeCastExpression == null) throw new ArgumentNullException(nameof(codeCastExpression));

        return new CodeCastExpression(this.VisitTypeReference(codeCastExpression.TargetType), this.VisitExpression(codeCastExpression.Expression));
    }

    public virtual CodeExpression VisitObjectCreateExpression(CodeObjectCreateExpression objectCreateExpression)
    {
        if (objectCreateExpression == null) throw new ArgumentNullException(nameof(objectCreateExpression));

        return new CodeObjectCreateExpression(this.VisitTypeReference(objectCreateExpression.CreateType), this.VisitExpressionCollection(objectCreateExpression.Parameters).ToArrayExceptNull(v => v));
    }

    public virtual CodeExpression VisitBinaryOperatorExpression(CodeBinaryOperatorExpression binaryOperatorExpression)
    {
        if (binaryOperatorExpression == null) throw new ArgumentNullException(nameof(binaryOperatorExpression));

        return new CodeBinaryOperatorExpression(this.VisitExpression(binaryOperatorExpression.Left), binaryOperatorExpression.Operator, this.VisitExpression(binaryOperatorExpression.Right));
    }

    public virtual CodeStatement VisitStatement(CodeStatement codeStatement)
    {
        if (codeStatement == null) throw new ArgumentNullException(nameof(codeStatement));

        if (codeStatement is CodeExpressionStatement)
        {
            return this.VisitExpressionStatement(codeStatement as CodeExpressionStatement);
        }
        else if (codeStatement is CodeVariableDeclarationStatement)
        {
            return this.VisitVariableDeclarationStatement(codeStatement as CodeVariableDeclarationStatement);
        }
        else if (codeStatement is CodeConditionStatement)
        {
            return this.VisitConditionStatement(codeStatement as CodeConditionStatement);
        }
        else if (codeStatement is CodeAssignStatement)
        {
            return this.VisitAssignStatement(codeStatement as CodeAssignStatement);
        }
        else if (codeStatement is CodeMethodReturnStatement)
        {
            return this.VisitMethodReturnStatement(codeStatement as CodeMethodReturnStatement);
        }
        else
        {
            return codeStatement;
        }
    }

    public virtual CodeStatement VisitConditionStatement(CodeConditionStatement codeConditionStatement)
    {
        if (codeConditionStatement == null) throw new ArgumentNullException(nameof(codeConditionStatement));

        return this.DeepOperation(() => new CodeConditionStatement(
                                                                   this.VisitExpression(codeConditionStatement.Condition),
                                                                   this.VisitStatementCollection(codeConditionStatement.TrueStatements).ToArrayExceptNull(v => v),
                                                                   this.VisitStatementCollection(codeConditionStatement.FalseStatements).ToArrayExceptNull(v => v)));
    }

    public virtual CodeStatement VisitExpressionStatement(CodeExpressionStatement codeExpressionStatement)
    {
        if (codeExpressionStatement == null) throw new ArgumentNullException(nameof(codeExpressionStatement));

        return this.DeepOperation(() => new CodeExpressionStatement(this.VisitExpression(codeExpressionStatement.Expression)));
    }

    public virtual CodeStatement VisitMethodReturnStatement(CodeMethodReturnStatement codeExpressionStatement)
    {
        if (codeExpressionStatement == null) throw new ArgumentNullException(nameof(codeExpressionStatement));

        return new CodeMethodReturnStatement(this.VisitExpression(codeExpressionStatement.Expression));
    }

    public virtual CodeStatement VisitAssignStatement(CodeAssignStatement codeExpressionStatement)
    {
        if (codeExpressionStatement == null) throw new ArgumentNullException(nameof(codeExpressionStatement));

        return new CodeAssignStatement(this.VisitExpression(codeExpressionStatement.Left), this.VisitExpression(codeExpressionStatement.Right));
    }

    public virtual CodeStatement VisitVariableDeclarationStatement(CodeVariableDeclarationStatement codeVariableDeclarationStatement)
    {
        if (codeVariableDeclarationStatement == null) throw new ArgumentNullException(nameof(codeVariableDeclarationStatement));

        return new CodeVariableDeclarationStatement(this.VisitTypeReference(codeVariableDeclarationStatement.Type), codeVariableDeclarationStatement.Name, codeVariableDeclarationStatement.InitExpression.Maybe(v => this.VisitExpression(v)))
               {
                       LinePragma = codeVariableDeclarationStatement.LinePragma
               };
    }

    public virtual CodeTypeReference VisitTypeReference(CodeTypeReference codeTypeReference)
    {
        if (codeTypeReference == null) throw new ArgumentNullException(nameof(codeTypeReference));

        var newTypeReference = new CodeTypeReference
                               {
                                       ArrayElementType = codeTypeReference.ArrayElementType.Maybe(this.VisitTypeReference),
                                       ArrayRank = codeTypeReference.ArrayRank,
                                       BaseType = codeTypeReference.BaseType,
                                       Options = codeTypeReference.Options
                               };

        newTypeReference.TypeArguments.AddRange(codeTypeReference.TypeArguments.ToArrayExceptNull(this.VisitTypeReference));

        return newTypeReference.WithCopyUserDataFrom(codeTypeReference);
    }

    protected T DeepOperation<T>(Func<T> func, int deep = 1)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));
        if (deep < 1) throw new ArgumentOutOfRangeException(nameof(deep));

        this.Deep += deep;

        try
        {
            return func();
        }
        finally
        {
            this.Deep -= deep;
        }
    }

    protected void DeepOperation(Action action, int deep = 1)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));
        if (deep < 1) throw new ArgumentOutOfRangeException(nameof(deep));

        this.DeepOperation(() =>
                           {
                               action();
                               return default(object);
                           }, deep);
    }

    protected T WithoutDeepOperation<T>(Func<T> func)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        var prevDeep = this.Deep;

        this.Deep = 0;

        try
        {
            return func();
        }
        finally
        {
            this.Deep = prevDeep;
        }
    }

    /// <summary>
    /// Fixed point for clone
    /// </summary>
    public static readonly CodeDomVisitor Identity = new CloneVisitor();

    private class CloneVisitor : CodeDomVisitor
    {
    }
}
