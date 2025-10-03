using System.CodeDom;
using System.CodeDom.Compiler;

using Framework.Core;

using CommonFramework;

namespace Framework.CodeDom;

public class CSharpCodeDomRenderer : CodeDomProviderRenderer
{
    public CSharpCodeDomRenderer(CodeDomProvider provider, CodeGeneratorOptions options = null)
            : base(provider, options)
    {
    }


    protected override string Render(CodeBinaryOperatorType @operator)
    {
        var temp = this.Render(new CodeBinaryOperatorExpression { Left = 0.ToPrimitiveExpression(), Right = 0.ToPrimitiveExpression(), Operator = @operator })
                       .SkipLast(Environment.NewLine)
                       .Skip("(" + 0, true)
                       .SkipLast(0 + ")", true)
                       .Trim();

        return temp;
    }


    protected override CodeDomVisitor CreateVisitor()
    {
        return new CSharpExpandExtendExpressionsVisitor(this);
    }


    private class CSharpExpandExtendExpressionsVisitor : ExpandExtendExpressionsVisitor
    {
        private static readonly CodeTypeReference VoidType = typeof(void).ToTypeReference();

        private bool skipOptimizeValueNotEquality;

        public CSharpExpandExtendExpressionsVisitor(CodeDomProviderRenderer renderer)
                : base(renderer)
        {
        }


        private string DeepOffset => new string(' ', this.Deep * 4);

        private string NextDeepOffset => new string(' ', this.NextDeep * 4);


        public override CodeStatement VisitConditionStatement(CodeConditionStatement codeConditionStatement)
        {
            if (codeConditionStatement.FalseStatements.Count == 1 && codeConditionStatement.FalseStatements[0] is CodeConditionStatement)
            {
                var innerStatement = codeConditionStatement.FalseStatements[0] as CodeConditionStatement;


                var head = this.Renderer.Render(new CodeConditionStatement { Condition = codeConditionStatement.Condition }.WithCopyUserDataFrom(codeConditionStatement).Self(newS => newS.TrueStatements.AddRange(codeConditionStatement.TrueStatements)));

                var tail = this.Renderer.Render(innerStatement);


                var result = $"{head}else {tail}".SkipLast(Environment.NewLine).Replace(Environment.NewLine, "\n").Split('\n').Select(line => this.DeepOffset + line).Join(Environment.NewLine);

                return new CodeSnippetStatement(result);
            }
            else
            {
                return base.VisitConditionStatement(codeConditionStatement);
            }
        }


        public override CodeExpression VisitMethodInvokeExpression(CodeMethodInvokeExpression codeMethodInvokeExpression)
        {
            var newLineParam = codeMethodInvokeExpression.UserData[ExtendRenderConst.NewLineParameters];

            if (codeMethodInvokeExpression.Parameters.Count > 1 && newLineParam is bool b && b)
            {
                var parameters = codeMethodInvokeExpression.Parameters.ToArrayExceptNull(p => p).Select(expr => $"{Environment.NewLine}{this.NextDeepOffset}{this.Renderer.Render(expr)}");

                return new CodeMethodInvokeExpression(codeMethodInvokeExpression.Method, new CodeSnippetExpression(parameters.Join(",")));
            }
            else
            {
                return base.VisitMethodInvokeExpression(codeMethodInvokeExpression);
            }
        }

        public override CodeStatementCollection VisitStatementCollection(CodeStatementCollection collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            var request = from CodeStatement statement in base.VisitStatementCollection(collection)

                          let conditionStatement = statement as CodeConditionStatement

                          let isComposite = conditionStatement.Maybe(c => c.FalseStatements.Count == 0 && c.Condition.IsPrimitiveValue(true))

                          from CodeStatement result in isComposite ? conditionStatement.TrueStatements : new CodeStatementCollection(new[] { statement })

                          select result;

            return new CodeStatementCollection(request.ToArray());
        }

        public override CodeExpression VisitBinaryOperatorExpression(CodeBinaryOperatorExpression binaryOperatorExpression)
        {
            if (binaryOperatorExpression == null) throw new ArgumentNullException(nameof(binaryOperatorExpression));

            if (binaryOperatorExpression.Operator == CodeBinaryOperatorType.ValueEquality && binaryOperatorExpression.Right.IsPrimitiveValue(false))
            {
                if (!this.skipOptimizeValueNotEquality
                    && binaryOperatorExpression.Left is CodeBinaryOperatorExpression innerBinary
                    && innerBinary.Operator == CodeBinaryOperatorType.ValueEquality)
                {
                    var leftRender = this.Renderer.Render(innerBinary.Left);

                    var rightRender = this.Renderer.Render(innerBinary.Right);

                    return new CodeSnippetExpression($"{leftRender} != {rightRender}");
                }
                else
                {
                    var result = $"!{this.Renderer.Render(binaryOperatorExpression.Left)}";

                    return new CodeSnippetExpression(result);
                }
            }
            else
            {
                return base.VisitBinaryOperatorExpression(binaryOperatorExpression);
            }
        }

        public override CodeMemberMethod VisitMemberMethod(CodeMemberMethod codeMemberMethod)
        {
            if (codeMemberMethod.Name == "operator !=")
            {
                var prevState = this.skipOptimizeValueNotEquality;

                this.skipOptimizeValueNotEquality = true;

                try
                {
                    return base.VisitMemberMethod(codeMemberMethod);
                }
                finally
                {
                    this.skipOptimizeValueNotEquality = prevState;
                }
            }
            else
            {
                return base.VisitMemberMethod(codeMemberMethod);
            }
        }

        protected override CodeExpression NormalizeExpression(CodeLambdaExpression lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));

            var parameters = this.VisitParameterDeclarationExpressionCollection(lambdaExpression.Parameters)
                                 .ToArrayExceptNull(v => v)
                                 .Join(", ", expr => expr.Type.BaseType == VoidType.BaseType ? expr.Name : this.Renderer.Render(expr));

            var bodyBlocks = this.RenderLambdaExpressionBody(this.VisitStatementCollection(lambdaExpression.Statements)
                                                                 .ToArrayExceptNull(v => v));

            var body = bodyBlocks.Length == 1 ? bodyBlocks[0] : "{" + bodyBlocks.Select(str => Environment.NewLine + this.DeepOffset + str).Concat() + "}";

            var formattedParameters = lambdaExpression.Parameters.Count == 1 ? parameters
                                              : "(" + parameters + ")";

            return new CodeSnippetExpression(formattedParameters + " => " + body);
        }

        protected override CodeStatement NormalizeStatement(CodeMethodYieldBreakStatement statement)
        {
            if (statement == null) throw new ArgumentNullException(nameof(statement));

            return new CodeSnippetStatement($"{this.DeepOffset}yield break;");
        }

        protected override CodeStatement NormalizeStatement(CodeMethodYieldReturnStatement statement)
        {
            if (statement == null) throw new ArgumentNullException(nameof(statement));

            var renderedExpression = this.Renderer.Render(this.VisitExpression(statement.Expression));

            return new CodeSnippetStatement($"{this.DeepOffset}yield return {renderedExpression};");
        }

        protected override CodeStatement NormalizeStatement(CodeForeachStatement statement)
        {
            var renderedSource = this.Renderer.Render(this.VisitExpression(statement.Source));

            var renderedStatements = statement.Statements.OfType<CodeStatement>().ToArray(st => $"{this.NextDeepOffset}{this.WithoutDeepOperation(() => this.Renderer.Render(this.VisitStatement(st)))}");

            var renderedIterator = statement.Iterator.FromMaybe(() => "Iterator not initialized")
                                            .Pipe(iterator => this.Renderer.Render(iterator.Type.BaseType == VoidType.BaseType ? new CodeParameterDeclarationExpression("var", iterator.Name) : iterator));

            var body = new[] { $"{this.DeepOffset}{{" }.Concat(renderedStatements).Concat(new[] { $"{this.DeepOffset}}}" }).Concat(str => Environment.NewLine + str.SkipLast(Environment.NewLine, false));

            return new CodeSnippetStatement($"{this.DeepOffset}foreach({renderedIterator} in {renderedSource}){body}");
        }

        protected override CodeExpression NormalizeExpression(CodeBinaryOperatorCollectionExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            var result = expression.Expressions.ToArrayExceptNull(v => v).Select(item => this.Renderer.Render(item).SkipLast(Environment.NewLine)).Join($" {this.Renderer.BinaryOperators[expression.Operator]} ");

            return new CodeSnippetExpression(result);
        }

        protected override CodeExpression NormalizeExpression(CodeMaybePropertyReferenceExpression propertyReferenceExpression)
        {
            if (propertyReferenceExpression == null) throw new ArgumentNullException(nameof(propertyReferenceExpression));

            return new CodeSnippetExpression($"{this.Renderer.Render(propertyReferenceExpression.TargetObject)}?.{propertyReferenceExpression.PropertyName}");
        }

        protected override CodeExpression NormalizeExpression(CodeNameofExpression nameofExpression)
        {
            if (nameofExpression == null) throw new ArgumentNullException(nameof(nameofExpression));

            return new CodeSnippetExpression($"nameof({nameofExpression.Value})");
        }

        private string[] RenderLambdaExpressionBody(CodeStatement[] codeStatements)
        {
            if (codeStatements == null) throw new ArgumentNullException(nameof(codeStatements));

            if (codeStatements.Length == 1)
            {
                if (codeStatements[0] is CodeMethodReturnStatement)
                {
                    var returnExpr = (codeStatements[0] as CodeMethodReturnStatement).Expression;

                    if (returnExpr != null)
                    {
                        return new[] { this.Renderer.Render(returnExpr) };
                    }
                }
                else if (codeStatements[0] is CodeExpressionStatement)
                {
                    var statExpr = (codeStatements[0] as CodeExpressionStatement).Expression;

                    return new[] { this.Renderer.Render(statExpr) };
                }
            }

            return codeStatements.ToArray(this.Renderer.Render);
        }

        protected override CodeTypeDeclaration NormalizeStaticClass(CodeTypeDeclaration decl)
        {
            if (decl == null) throw new ArgumentNullException(nameof(decl));

            decl.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, Environment.NewLine + "\tstatic"));

            decl.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, string.Empty));

            return decl.UnmarkAsStatic();
        }

        protected override CodeMemberMethod NormalizeExtensionMethod(CodeMemberMethod method)
        {
            var parameter = method.Parameters[0];

            var renderedParameter = this.Renderer.Render(parameter);

            var type = renderedParameter.SkipLast($" {parameter.Name}", true);

            parameter.Type = new CodeTypeReference($"this {type}");

            return method.UnmarkAsExtension();
        }

        protected override CodeTypeReference NormalizeNullableCodeTypeReference(CodeTypeReference codeTypeReference)
        {
            var varName = "$temp";

            var expr = new CodeVariableDeclarationStatement(codeTypeReference.TypeArguments[0], varName);

            var renderedExpr = this.Renderer.Render(expr);

            var pureArg = renderedExpr.SkipLast($" {varName};{Environment.NewLine}", true);

            return new CodeTypeReference($"{pureArg}?");
        }
    }
}
