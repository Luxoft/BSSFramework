using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Framework.Async;
using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.DTOGenerator.TypeScript.Facade;
using Framework.DomainDriven.Generation.Domain;
using Framework.ServiceModel.Async;
using Framework.Transfering;

using JetBrains.Annotations;

namespace Framework.DomainDriven.FacadeServiceProxyGenerator
{
    public partial class ServiceProxyFileFactory<TConfiguration> : CodeFileFactory<TConfiguration, FileType>
            where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        private readonly CodeMemberField serviceField;

        private readonly CodeExpression serviceFieldRefExpr;

        private readonly Lazy<CodeMemberProperty[]> lazyAsyncFuncProperties;

        public ServiceProxyFileFactory(TConfiguration configuration)
                : base(configuration, configuration.BaseContract)
        {
            this.serviceField = this.Configuration.GetCodeTypeReference(this.DomainType, FileType.ClientContact).ToMemberField("_service");

            this.serviceFieldRefExpr = new CodeThisReferenceExpression().ToFieldReference(this.serviceField);

            this.lazyAsyncFuncProperties = LazyHelper.Create(() => this.GetAsyncFuncProperties().ToArray());
        }

        public override FileType FileType { get; } = FileType.ServiceProxy;

        public override CodeTypeReference BaseReference => typeof(ServiceProxy<>).ToTypeReference(this.Configuration.GetCodeTypeReference(this.DomainType, FileType.SimpleClientImpl));

        protected override CodeTypeDeclaration GetCodeTypeDeclaration()
        {
            return new CodeTypeDeclaration(this.Name)
            {
                Attributes = MemberAttributes.Public,
                IsPartial = true,
                IsClass = true
            };
        }

        protected virtual bool GenerateContravariantMethod(MethodInfo sourceMethod)
        {
            return sourceMethod.GetParameters().Any(p => p.ParameterType.IsStrict());
        }

        protected override IEnumerable<CodeTypeReference> GetBaseTypes()
        {
            return base.GetBaseTypes().Concat(this.lazyAsyncFuncProperties.Value.Select(asyncProp => asyncProp.PrivateImplementationType).Where(t => t != null));
        }

        protected override IEnumerable<CodeConstructor> GetConstructors()
        {
            var factoryParameter = typeof(Func<>).ToTypeReference(this.Configuration.GetCodeTypeReference(this.DomainType, FileType.SimpleClientImpl))
                                                 .ToParameterDeclarationExpression("facadeFactory");

            yield return new CodeConstructor
            {
                Attributes = MemberAttributes.Public,
                Parameters = { factoryParameter },
                BaseConstructorArgs = { factoryParameter.ToVariableReferenceExpression() },
                Statements = { new CodeThisReferenceExpression().ToMethodInvokeExpression("GetService").ToPropertyReference("Channel").ToAssignStatement(this.serviceFieldRefExpr) }
            };
        }

        protected override IEnumerable<CodeTypeMember> GetMembers()
        {
            yield return this.serviceField;

            foreach (var baseMember in base.GetMembers())
            {
                yield return baseMember;
            }

            foreach (var asyncProperty in this.lazyAsyncFuncProperties.Value)
            {
                yield return asyncProperty;
            }
        }

        protected virtual IEnumerable<CodeMemberProperty> GetAsyncFuncProperties()
        {
            var asyncFuncProps = from sourceMethod in this.DomainType.ExtractContractMethods()

                                 where this.Configuration.Policy.Used(sourceMethod)

                                 orderby sourceMethod.Name

                                 select this.CreateAsyncFunc(sourceMethod);

            foreach (var asyncFuncInfo in asyncFuncProps)
            {
                yield return asyncFuncInfo.Property;

                var asyncServiceType = this.GetRoles(asyncFuncInfo.SourceMethod).CollectMaybe().FirstOrDefault();

                if (asyncServiceType != null)
                {
                    yield return this.CreateRoleExplicitAsyncFunc(asyncServiceType, asyncFuncInfo);
                }

                if (this.GenerateContravariantMethod(asyncFuncInfo.SourceMethod))
                {
                    var contravariantAsyncFuncInfo = this.CreateAsyncFuncR(asyncFuncInfo);

                    yield return contravariantAsyncFuncInfo.Property;

                    if (asyncServiceType != null)
                    {
                        yield return this.CreateRoleExplicitAsyncFunc(asyncServiceType, contravariantAsyncFuncInfo);
                    }
                }
            }
        }

        protected virtual GenerateAsyncFuncInfo CreateAsyncFunc([NotNull] MethodInfo sourceMethod)
        {
            if (sourceMethod == null) { throw new ArgumentNullException(nameof(sourceMethod)); }

            var voidResult = sourceMethod.ReturnType == typeof(void);

            var returnType = voidResult ? typeof(Ignore).ToTypeReference() : this.Configuration.ResolveMethodParameterType(sourceMethod.ReturnType);

            var realParameters = sourceMethod.GetParameters().ToArray(p => this.Configuration.ResolveMethodParameterType(p.ParameterType).ToParameterDeclarationExpression(p.Name));

            var parameters = sourceMethod.GetParameters().Any() ? realParameters : new[] { typeof(Ignore).ToTypeReference().ToParameterDeclarationExpression("_") };

            var generics = parameters.Select(p => p.Type).Concat(new[] { returnType }).ToArray();

            var asyncFunc2Rank = typeof(IAsyncProcessFunc<,>);

            var typedAsyncFuncType = generics.Length == 2 ? asyncFunc2Rank : asyncFunc2Rank.Assembly.GetType(asyncFunc2Rank.FullName.Replace("`2", $"`{generics.Length}"));

            var asyncFuncType = typedAsyncFuncType.ToTypeReference(generics);

            var factoryClass = parameters.Length == 1 ? typeof(AsyncProcessFunc) : typeof(RankAsyncProcessFunc);

            var callbackParameter = typeof(AsyncCallback).ToTypeReference().ToParameterDeclarationExpression("callback");
            var stateParameter = typeof(object).ToTypeReference().ToParameterDeclarationExpression("state");

            var beginLambda = new CodeLambdaExpression
                              {
                                      Parameters = new CodeParameterDeclarationExpressionCollection(parameters.Concat(new[] { callbackParameter, stateParameter }).ToArray()).WithoutTypes(),
                                      Statements = { this.serviceFieldRefExpr.ToMethodInvokeExpression(
                                                        $"Begin{sourceMethod.Name}",
                                                        realParameters.Concat(new[] { callbackParameter , stateParameter}).Select(p => p.ToVariableReferenceExpression())) }
                              };

            var asyncResultParameter = typeof(IAsyncResult).ToTypeReference().ToParameterDeclarationExpression("asyncResult");

            var endLambda = new CodeLambdaExpression
                            {
                                    Parameters = new CodeParameterDeclarationExpressionCollection(new[] { asyncResultParameter}).WithoutTypes(),
                                    Statements = { this.serviceFieldRefExpr.ToMethodInvokeExpression($"End{sourceMethod.Name}", asyncResultParameter.ToVariableReferenceExpression()) }
                            };

            var createAsyncFuncExpr = factoryClass.ToTypeReferenceExpression()
                                                  .ToMethodReferenceExpression("Create", voidResult ? generics.SkipLast(1).ToArray() : generics)
                                                  .ToMethodInvokeExpression(beginLambda, endLambda);

            var asyncVarStatement = new CodeVariableDeclarationStatement(asyncFuncType, "asyncFunc", createAsyncFuncExpr);

            var property = new CodeMemberProperty
                                   {
                                           Attributes = MemberAttributes.Public,
                                           Type = asyncFuncType,
                                           Name = $"{sourceMethod.Name}AsyncFunc",
                                           GetStatements =
                                           {
                                                   asyncVarStatement,

                                                   typeof(AsyncProcessFuncBaseExtensions)
                                                           .ToTypeReferenceExpression()
                                                           .ToMethodInvokeExpression("WithSync", asyncVarStatement.ToVariableReferenceExpression())
                                                           .ToMethodReturnStatement()
                                           }
                                   };

            return new GenerateAsyncFuncInfo { BaseTypedAsyncFuncType = typedAsyncFuncType, Generics = generics, Property = property, SourceMethod = sourceMethod };
        }

        protected virtual GenerateAsyncFuncInfo CreateAsyncFuncR(GenerateAsyncFuncInfo baseAsyncFunc)
        {
            var sourceMethod = baseAsyncFunc.SourceMethod;

            var newArgs = sourceMethod.GetParameters().ToArray(p =>
                p.ParameterType.IsStrict()
              ? new
                {
                        Overrided = true,
                        Type = this.GetRichReference(p.ParameterType)

                }
              : new
                {
                        Overrided = false,
                        Type = this.Configuration.ResolveMethodParameterType(p.ParameterType)
                });


            var newGenerics = newArgs.Select(pair => pair.Type).Concat(new[] { baseAsyncFunc.Generics.Last() }).ToArray();

            var asyncFuncRType = baseAsyncFunc.BaseTypedAsyncFuncType.ToTypeReference(newGenerics);

            var factoryClass = newArgs.Length == 1 ? typeof(AsyncProcessFunc) : typeof(RankAsyncProcessFunc);

            var callbackParameter = new CodeParameterDeclarationExpression { Name = "callback" };

            var lambdaParameters = sourceMethod.GetParameters().Select(p => new CodeParameterDeclarationExpression { Name = p.Name }).Concat(new[] { callbackParameter }).ToArray();

            var callbackLambda = new CodeLambdaExpression
                                 {
                                    Parameters = new CodeParameterDeclarationExpressionCollection(lambdaParameters),
                                    Statements = { new CodeThisReferenceExpression().ToPropertyReference(baseAsyncFunc.Property).ToMethodInvokeExpression("Run",
                                                        lambdaParameters.SkipLast(1).ZipStrong(newArgs, (p, pair) =>
                                                            p.ToVariableReferenceExpression().Pipe(pair.Overrided, e => (CodeExpression)e.ToMethodInvokeExpression("ToStrict")))
                                                                        .Concat(new[] { callbackParameter.ToVariableReferenceExpression() })) }
                                 };

            var property = new CodeMemberProperty
                                  {
                                          Attributes = MemberAttributes.Public,
                                          Name = baseAsyncFunc.Property.Name + "R",
                                          Type = asyncFuncRType,
                                          GetStatements = { factoryClass.ToTypeReferenceExpression().ToMethodReferenceExpression("Create", newGenerics).ToMethodInvokeExpression(callbackLambda).ToMethodReturnStatement() }
                                  };

            return new GenerateAsyncFuncInfo { BaseTypedAsyncFuncType = baseAsyncFunc.BaseTypedAsyncFuncType, Generics = newGenerics, Property = property, SourceMethod = sourceMethod, IsContravariant = true };
        }

        protected virtual CodeMemberProperty CreateRoleExplicitAsyncFunc([NotNull] Type baseAsyncServiceType, GenerateAsyncFuncInfo asyncFuncInfo)
        {
            if (baseAsyncServiceType == null) { throw new ArgumentNullException(nameof(baseAsyncServiceType)); }

            var currentAsyncServiceType = asyncFuncInfo.IsContravariant
                                          ? baseAsyncServiceType.GetGenericTypeDefinition().ToTypeReference(baseAsyncServiceType.GetGenericArguments().ToArray(t =>
                                            t.IsStrict() ? this.GetRichReference(t) : t.ToTypeReference()))
                                          : baseAsyncServiceType.ToTypeReference();

            return new CodeMemberProperty
                   {
                           Name = baseAsyncServiceType.GetProperties().Single().Name,
                           PrivateImplementationType = currentAsyncServiceType,
                           Type = asyncFuncInfo.Property.Type,
                           GetStatements =
                           {
                                   new CodeThisReferenceExpression().ToPropertyReference(asyncFuncInfo.Property).ToMethodReturnStatement()
                           }
                   };
        }

        private CodeTypeReference GetRichReference(Type dtoType)
        {
            return dtoType.GetCustomAttribute<DTOFileTypeAttribute>(true)
                          .DomainType
                          .Pipe(domainType => this.Configuration.Environment.ClientDTO.GetCodeTypeReference(domainType, DTOType.RichDTO));
        }
    }
}
