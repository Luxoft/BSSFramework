using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

using Framework.CodeDom;
using Framework.CodeDom.TypeScript;
using Framework.Core;
using Framework.DomainDriven.DTOGenerator.TypeScript.Configuration;
using Framework.DomainDriven.DTOGenerator.TypeScript.Facade;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Facade
{
    /// <summary>
    /// Default facade file factory configuration
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public class DefaultFacadeFileFactory<TConfiguration> : CodeFileFactory<TConfiguration, TypeScriptFacadeFileType>
        where TConfiguration : class, ITypeScriptFacadeGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        private static readonly Regex TypeExpression = new Regex("(Identity|Visual|Simple|Full|Projection|Rich)DTO", RegexOptions.Compiled);

        private static readonly Regex IsGenericExpression = new Regex(@"`\d", RegexOptions.Compiled);

        public DefaultFacadeFileFactory(TConfiguration configuration)
            : base(configuration, null)
        {
        }

        public override TypeScriptFacadeFileType FileType => TypeScriptFacadeFileType.Facade;

        protected override IEnumerable<string> GetImportedNamespaces()
        {
            return this.Configuration.GetNamespaces();
        }

        protected override CodeTypeDeclaration GetCodeTypeDeclaration()
        {
            return new CodeTypeDeclaration();
        }

        protected override IEnumerable<CodeTypeMember> GetMembers()
        {
            string variable = "service";

            foreach (var methodInfo in this.Configuration.GetFacadeMethods())
            {
                var returnTypeIsPrimitive = this.ReturnTypeIsPrimitive(methodInfo);

                var parametersCount = methodInfo.GetParametersWithExpandAutoRequest().Count();

                var genericClass = this.Configuration.GetFacadeAsyncFuncName(returnTypeIsPrimitive, parametersCount);

                var typeDeclarationExpressionCollection = this.GetParameterCodeTypeDeclarationExpressionCollection(methodInfo);
                var methodInfoCodeTypeReference = this.GetCodeTypeReference(methodInfo);

                typeDeclarationExpressionCollection.Add(methodInfoCodeTypeReference);

                var baseDtoTypeName = this.GetParameterExpression(methodInfo.ReturnType).Type.BaseType;
                if (!returnTypeIsPrimitive)
                {
                    typeDeclarationExpressionCollection.Add(this.GetCodeObservableTypeReference(methodInfo));
                    typeDeclarationExpressionCollection.Add(new CodeTypeReference(baseDtoTypeName));
                    typeDeclarationExpressionCollection.Add(this.ToObservable(new CodeTypeReference(baseDtoTypeName)));
                }

                var promise = new CodeTypeReference(genericClass, typeDeclarationExpressionCollection.ToArray());

                CodeTypeReference[] references = returnTypeIsPrimitive
                    ? new[] { methodInfoCodeTypeReference }
                    : typeDeclarationExpressionCollection.Skip(Math.Max(0, typeDeclarationExpressionCollection.Count - 4)).ToArray();


                var facadeMethodInvokeExpression = new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(
                        new CodeVariableReferenceExpression(this.Configuration.GetFacadeFileFactoryName()),
                        this.Configuration.GetGenericFacadeMethodInvocation(returnTypeIsPrimitive),
                        references));

                var parametersStatement = new CodeVariableDeclarationStatement("const ", "baseParameters", this.GetMethodParametersCollection(methodInfo));

                var realParametersExpr = methodInfo.TryExtractAutoRequestParameter().Select(p =>
                {
                    var parametersCollection = new JsObjectCreateExpression();

                    parametersCollection.AddParameter(p.Name, parametersStatement.Name);

                    return (CodeExpression)parametersCollection;
                }).GetValueOrDefault(() => (CodeExpression)parametersStatement.ToVariableReferenceExpression());
                var realParametersStatement = new CodeVariableDeclarationStatement("const ", "realParameters", realParametersExpr);

                var varialableStatement = new CodeVariableDeclarationStatement("const ", variable, facadeMethodInvokeExpression);

                var typeCreateExpression = new JsObjectCreateExpression();
                typeCreateExpression.AddParameter("plain", this.GetTypeScriptType(baseDtoTypeName, false));
                typeCreateExpression.AddParameter("observable", this.ToObservable(this.GetTypeScriptType(baseDtoTypeName, true)));

                var fullServiceMethodName = methodInfo.ReflectedType.Name.SkipLastMaybe("Controller").Select(prefix => $"{prefix}/{methodInfo.Name}").GetValueOrDefault(methodInfo.Name);

                var codePrimitiveExpressions = new List<CodeExpression> { new CodePrimitiveExpression(fullServiceMethodName) };

                if (!returnTypeIsPrimitive)
                {
                    codePrimitiveExpressions.Add(typeCreateExpression);
                }

                codePrimitiveExpressions.Add(realParametersStatement.ToVariableReferenceExpression());

                var serviceMethodInvokationName = this.IsHierarchicalResultType(typeDeclarationExpressionCollection) ? "getHierarchicalData" : "getData";
                var initiStatement = new CodeVariableReferenceExpression { VariableName = variable }
                        .ToMethodInvokeExpression(serviceMethodInvokationName, codePrimitiveExpressions);

                var callbackExpression = new CodeLambdaExpression
                {
                    Parameters = this.GetParameterDeclarationExpressionCollection(methodInfo),
                    Statements = { parametersStatement, realParametersStatement, varialableStatement, initiStatement.ToMethodReturnStatement() }
                };

                var root = new CodeObjectCreateExpression(genericClass, callbackExpression);

                var methodName = methodInfo.Name.ToStartLowerCase();
                var functionName = "_" + methodName;

                var memberMethod = new CodeMemberMethod
                {
                    Name = functionName,
                    Attributes = MemberAttributes.AccessMask,
                    ReturnType = promise,
                    Statements = { root.ToMethodReturnStatement() }
                };

                yield return new CodeSnippetTypeMember($"export let {methodName}AsyncFunc = {functionName}();" + Environment.NewLine);

                yield return memberMethod;
            }
        }

        private JsObjectCreateExpression GetMethodParametersCollection(MethodInfo method)
        {
            var parametersCollection = new JsObjectCreateExpression();
            foreach (var parameter in method.GetParametersWithExpandAutoRequest().Select(parameter => new { parameter.Name, parameter.ParameterType }).ToList())
            {
                if (parameter.ParameterType.Name.EndsWith("StrictDTO"))
                {
                    parametersCollection.AddParameter(parameter.Name, parameter.Name + ".toNativeJson()");
                }
                else if (parameter.ParameterType.IsPeriod())
                {
                    parametersCollection.AddParameter(parameter.Name, Constants.PeriodToOdata(parameter.Name));
                }
                else if (parameter.ParameterType.IsDateTime())
                {
                    parametersCollection.AddParameter(parameter.Name, Constants.DateToOdata(parameter.Name));
                }
                else
                {
                    parametersCollection.AddParameter(parameter.Name, parameter.Name);
                }
            }

            return parametersCollection;
        }

        private List<CodeTypeReference> GetParameterCodeTypeDeclarationExpressionCollection(MethodInfo methodInfo)
        {
            return methodInfo.GetParametersWithExpandAutoRequest().Select(parameter => new CodeTypeReference(this.GetTypeDefenition(parameter.ParameterType))).ToList();
        }

        private CodeParameterDeclarationExpressionCollection GetParameterDeclarationExpressionCollection(MethodInfo methodInfo)
        {
            var result = new CodeParameterDeclarationExpressionCollection();
            foreach (var parameter in methodInfo.GetParametersWithExpandAutoRequest())
            {
                result.Add(this.GetParameterExpression(parameter.ParameterType, parameter.Name));
            }

            return result;
        }

        private CodeParameterDeclarationExpression GetParameterExpression(Type type, string parameterName = "type")
        {
            if (parameterName == "type" && type.IsGenericType)
            {
                return new CodeParameterDeclarationExpression(this.GetLastGenericType(type), parameterName);
            }

            return new CodeParameterDeclarationExpression(this.GetTypeDefenition(type), parameterName);
        }

        private string GetTypeDefenition(Type type)
        {
            if (type.IsNullable())
            {
                type = type.GetNullableElementType();
            }

            if (type.IsEnum)
            {
                return this.ResolveTypeName(type);
            }

            if (type.IsGenericType)
            {
                return this.GetGenericTypeName(type);
            }

            if (!type.IsClass)
            {
                return this.ResolveTypeName(type);
            }

            return !type.IsPrimitiveType() ? this.GetGenericTypeName(type) : this.ResolveTypeName(type);
        }

        private string ResolveTypeName(Type type)
        {
            RequireJsModule requireJsModule = this.Configuration.GetModules()
                .FirstOrDefault(x => x.NameSpaces.Contains(type.Namespace));
            if (requireJsModule != null)
            {
                if (string.IsNullOrEmpty(requireJsModule.ModuleName))
                {
                    return IsGenericExpression.Replace(this.GetTypeScriptType(type), string.Empty, 1);
                }

                return $"{requireJsModule.ModuleName}.{IsGenericExpression.Replace(type.Name, string.Empty, 1)}";
            }

            return IsGenericExpression.Replace(this.GetTypeScriptType(type), string.Empty, 1);
        }

        private string GetTypeScriptType(Type type)
        {
            var typeName = type.FullName;

            if (string.Compare(typeName, "Framework.Core.Period", StringComparison.Ordinal) == 0)
            {
                return "Core.Period";
            }

            var outputType = typeName.GetBasedOutputType();
            if (outputType != typeName)
            {
                return outputType;
            }

            return typeName.StartsWith("System.") ? this.MaybeToArray(typeName).Split('.').Last() : type.FullName;
        }

        private string GetTypeScriptType(string type, bool observable)
        {
            if (type == "string" || type == "boolean" || type == "number")
            {
                return observable ? "ObservableSimpleObject" : "SimpleObject";
            }

            if (type == "Date")
            {
                return observable ? "ObservableSimpleDate" : "SimpleDate";
            }

            return type;
        }

        private string GetGenericTypeName(Type returnType)
        {
            var sb = new StringBuilder();
            if (returnType.IsGenericType)
            {
                var typeDefinition = returnType.GetGenericTypeDefinition();
                sb.Append(this.ResolveTypeName(typeDefinition));
                sb.Append("<");
                for (int i = 0; i < returnType.GenericTypeArguments.Length; i++)
                {
                    var argument = returnType.GenericTypeArguments[i];
                    sb.AppendFormat(i == 0 ? "{0}" : ",{0}", argument.IsGenericType ? this.GetGenericTypeName(argument) : this.ResolveTypeName(argument));
                }

                sb.Append(">");
            }
            else
            {
                sb.AppendFormat("{0}", this.ResolveTypeName(returnType));
            }

            return sb.ToString();
        }

        private bool IsHierarchicalResultType(IList<CodeTypeReference> parameterTypeDeclarationCollection)
        {
            return parameterTypeDeclarationCollection.Any(x => x.BaseType.Contains("HierarchicalNode<"));
        }

        // Please note: in case aka IList<Tuple<T1,T2>> it will return type T1
        private string GetLastGenericType(Type returnType)
        {
            if (returnType.IsGenericType)
            {
                var argument = returnType.GenericTypeArguments[0];
                return this.GetLastGenericType(argument);
            }

            return this.ResolveTypeName(returnType);
        }


        private CodeTypeReference GetCodeTypeReference(MethodInfo methodInfo)
        {
            var returnType = methodInfo.ReturnType;
            if (returnType.IsGenericType)
            {
                return new CodeTypeReference(this.GetGenericTypeName(returnType));
            }

            return new CodeTypeReference(this.ResolveTypeName(returnType));
        }

        private CodeTypeReference GetCodeObservableTypeReference(MethodInfo methodInfo)
        {
            var returnType = methodInfo.ReturnType;


            if (!this.Configuration.UseObservable)
            {
                return this.GetCodeTypeReference(methodInfo);
            }

            return this.ToObservable(new CodeTypeReference(returnType.IsGenericType
                ? this.GetGenericTypeName(returnType)
                : this.ResolveTypeName(returnType)));
        }

        private CodeTypeReference ToObservable(CodeTypeReference reference)
        {
            if (!this.Configuration.UseObservable)
            {
                return reference;
            }

            return new CodeTypeReference(this.ToObservable(reference.BaseType));
        }

        private bool ReturnTypeIsPrimitive(MethodInfo methodInfo)
        {
            return methodInfo.ReturnType.IsPrimitiveType() || methodInfo.ReturnType == typeof(void);
        }

        private string ToObservable(string input)
        {
            if (!this.Configuration.UseObservable)
            {
                return input;
            }

            var asObservable = TypeExpression.Replace(input, (e) => e.Success ? "Observable" + e.Value : string.Empty);

            return asObservable;
        }

        private string MaybeToArray(string type)
        {
            if (type.StartsWith("System.Collections"))
            {
                return "Array";
            }

            return type;
        }
    }
}
