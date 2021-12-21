using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.Persistent;

namespace Framework.CodeDom.TypeScript
{
    /// <summary>
    /// TypeScript language codedom renderer
    /// </summary>
    public class TypeScriptCodeDomRenderer : CodeDomProviderRenderer
    {
        public TypeScriptCodeDomRenderer(CodeDomProvider provider, CodeGeneratorOptions options = null)
            : base(provider, options)
        {
        }

        protected override CodeDomVisitor CreateVisitor()
        {
            return new TypeScriptCodeDomVisitor(
                this,
                CodeTypeReferenceComparer.Value,
                new Dictionary<Type, Type>
                {
                    { typeof(Maybe), TypeScriptTypeHelper.MaybeFactory },
                    { typeof(UpdateItemData), TypeScriptTypeHelper.UpdateItemDataFactory }
                },
                new Dictionary<string, string>
                {
                    { "Framework.Persistent", "FrameworkCore" },
                    { "Framework.Core", "FrameworkCore" }
                });
        }

        protected class TypeScriptCodeDomVisitor : ExpandExtendExpressionsVisitor
        {
            private readonly IEqualityComparer<CodeTypeReference> refComparer;

            private readonly IReadOnlyDictionary<Type, Type> mapTypeDict;

            private readonly IReadOnlyDictionary<string, string> mapNamespaceDict;

            public TypeScriptCodeDomVisitor(CodeDomProviderRenderer renderer, IEqualityComparer<CodeTypeReference> refComparer, IReadOnlyDictionary<Type, Type> mapTypeDict, IReadOnlyDictionary<string, string> mapNamespaceDict)
                : base(renderer)
            {
                this.refComparer = refComparer ?? throw new ArgumentNullException(nameof(refComparer));
                this.mapTypeDict = mapTypeDict ?? throw new ArgumentNullException(nameof(mapTypeDict));
                this.mapNamespaceDict = mapNamespaceDict ?? throw new ArgumentNullException(nameof(mapNamespaceDict));
            }


            public override CodeTypeReference VisitTypeReference(CodeTypeReference codeTypeReference)
            {
                var baseVisited = base.VisitTypeReference(codeTypeReference);

                if (baseVisited.BaseType.StartsWith(typeof(Period).ToTypeReference().BaseType))
                {
                    return baseVisited;
                }

                foreach (var overridePair in this.mapTypeDict)
                {
                    if (this.refComparer.Equals(baseVisited, overridePair.Key.ToTypeReference()))
                    {
                        baseVisited.BaseType = overridePair.Value.ToTypeReference().BaseType;

                        return baseVisited;
                    }
                }

                foreach (var overridePair in this.mapNamespaceDict)
                {
                    if (baseVisited.BaseType.StartsWith(overridePair.Key))
                    {
                        baseVisited.BaseType = $"{overridePair.Value}{baseVisited.BaseType.Skip(overridePair.Key, true)}";

                        return baseVisited;
                    }
                }

                return baseVisited;
            }

            protected override CodeTypeDeclaration NormalizeStaticClass(CodeTypeDeclaration decl) => decl;

            protected override CodeMemberMethod NormalizeExtensionMethod(CodeMemberMethod method) => method;

            protected override CodeExpression NormalizeExpression(CodeLambdaExpression lambdaExpression) => lambdaExpression;

            protected override CodeStatement NormalizeStatement(CodeMethodYieldBreakStatement statement) => statement;

            protected override CodeStatement NormalizeStatement(CodeMethodYieldReturnStatement statement) => statement;

            protected override CodeStatement NormalizeStatement(CodeForeachStatement statement) => statement;

            protected override CodeExpression NormalizeExpression(CodeNameofExpression nameofExpression) => nameofExpression;

            protected override CodeExpression NormalizeExpression(CodeBinaryOperatorCollectionExpression expression)
            {
                if (expression == null) throw new ArgumentNullException(nameof(expression));

                var result = expression.Expressions.ToArrayExceptNull(v => v).Select(item => this.Renderer.Render(item).SkipLast(Environment.NewLine)).Join($" {this.Renderer.BinaryOperators[expression.Operator]} ");

                return new CodeSnippetExpression(result);
            }

            protected override CodeExpression NormalizeExpression(CodeMaybePropertyReferenceExpression propertyReferenceExpression) => throw new NotImplementedException();

            public override CodeExpression VisitMethodInvokeExpression(CodeMethodInvokeExpression codeMethodInvokeExpression)
            {
                if (codeMethodInvokeExpression.Parameters.Count == 2 && codeMethodInvokeExpression.Method.MethodName == nameof(Framework.Core.EnumerableExtensions.ToList))
                {
                    switch (codeMethodInvokeExpression.Method.TargetObject)
                    {
                        case CodeTypeReferenceExpression typeRefExpr when this.refComparer.Equals(typeRefExpr.Type, typeof(Framework.Core.EnumerableExtensions).ToTypeReference()):
                            {
                                var visitedSource = this.VisitExpression(codeMethodInvokeExpression.Parameters[0]);
                                var visitedLambda = this.VisitExpression(codeMethodInvokeExpression.Parameters[1]);

                                return visitedSource.ToMethodInvokeExpression("map", visitedLambda);
                            }
                    }
                }

                return base.VisitMethodInvokeExpression(codeMethodInvokeExpression);
            }

            public override CodeParameterDeclarationExpression VisitParameterDeclarationExpression(CodeParameterDeclarationExpression codeParameterDeclarationExpression)
            {
                var visited = base.VisitParameterDeclarationExpression(codeParameterDeclarationExpression);

                if (codeParameterDeclarationExpression.IsOptional())
                {
                    visited.Name += "?";
                }

                return visited;
            }
        }

        public static readonly TypeScriptCodeDomRenderer Default = new TypeScriptCodeDomRenderer(
            new TypeScriptCodeDomProvider(new TypeScriptCodeGenerator("")),
            new CodeGeneratorOptions
            {
                BracingStyle = string.Empty,
                BlankLinesBetweenMembers = false
            });

        protected override string Render(CodeBinaryOperatorType @operator)
        {
            var temp = this.Render(new CodeBinaryOperatorExpression { Left = 0.ToPrimitiveExpression(), Right = 0.ToPrimitiveExpression(), Operator = @operator })
                           .SkipLast(Environment.NewLine)
                           .Skip("(" + 0, true)
                           .SkipLast(0 + ")", true)
                           .Trim();

            return temp;
        }
    }
}
