using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.DTOGenerator.TypeScript.Facade;
using Framework.DomainDriven.Generation.Domain;

using JetBrains.Annotations;

namespace Framework.DomainDriven.FacadeServiceProxyGenerator
{
    public abstract class GeneratorConfigurationBase<TEnvironment> : GeneratorConfiguration<TEnvironment, FileType>, IGeneratorConfigurationBase<TEnvironment>
            where TEnvironment : class, IGenerationEnvironmentBase
    {
        private readonly IDictionaryCache<Type, CodeTypeReference> resolvedParametersCache;

        protected GeneratorConfigurationBase(TEnvironment environment)
                : base(environment)
        {
            this.resolvedParametersCache = new DictionaryCache<Type, CodeTypeReference>(this.InternalResolveMethodParameterType).WithLock();
        }

        public abstract Type BaseContract { get; }

        public virtual string ContractConfigurationName => $"{this.Namespace.Split('.').Last()}.{this.BaseContract.Name}";

        public virtual string ContractNamespace => CustomAttributeProviderExtensions.GetCustomAttribute<ServiceContractAttribute>(this.BaseContract)?.Namespace;

        public virtual ITypeScriptMethodPolicy Policy { get; } = new DefaultTypeScriptMethodPolicy(true);

        public CodeTypeReference ResolveMethodParameterType([NotNull] Type type)
        {
            if (type == null) { throw new ArgumentNullException(nameof(type)); }

            return this.resolvedParametersCache[type];
        }


        public virtual CodeAttributeDeclaration GetServiceContractAttribute()
        {
            return typeof(ServiceContractAttribute).ToTypeReference().ToAttributeDeclaration(this.GetServiceContractAttributeArgument().ToArray());
        }

        protected virtual IEnumerable<CodeAttributeArgument> GetServiceContractAttributeArgument()
        {
            yield return new CodeAttributeArgument(nameof(ServiceContractAttribute.ConfigurationName), this.ContractConfigurationName.ToPrimitiveExpression());

            if (!this.ContractNamespace.IsNullOrWhiteSpace())
            {
                yield return new CodeAttributeArgument(nameof(ServiceContractAttribute.Namespace), this.ContractNamespace.ToPrimitiveExpression());
            }
        }

        public virtual CodeAttributeDeclaration GetOperationContractAttribute(MethodInfo method)
        {
            var serviceContractNamespace = method.DeclaringType.GetCustomAttribute<ServiceContractAttribute>()?.Namespace ?? $"http://tempuri.org";

            var actionName = $"{serviceContractNamespace}/{method.DeclaringType.Name}/{method.Name}";
            var replyActionName = $"{actionName}Response";

            return typeof(OperationContractAttribute).ToTypeReference().ToAttributeDeclaration(
                new CodeAttributeArgument(nameof(OperationContractAttribute.AsyncPattern), true.ToPrimitiveExpression()),
                new CodeAttributeArgument(nameof(OperationContractAttribute.Action), actionName.ToPrimitiveExpression()),
                new CodeAttributeArgument(nameof(OperationContractAttribute.ReplyAction), replyActionName.ToPrimitiveExpression()));
        }

        protected virtual bool IsServiceNamespaceType([NotNull] Type type)
        {
            if (type == null) { throw new ArgumentNullException(nameof(type)); }

            return false;
        }

        protected virtual CodeTypeReference InternalResolveMethodParameterType([NotNull] Type type)
        {
            if (type == null) { throw new ArgumentNullException(nameof(type)); }

            if (typeof(Stream).IsAssignableFrom(type))
            {
                return typeof(byte[]).ToTypeReference();
            }
            else if (this.IsServiceNamespaceType(type))
            {
                return new CodeTypeReference($"{this.Namespace}.{type.Name}");
            }
            else if (type.GetCollectionOrArrayElementType() is Type element && !element.IsPrimitive)
            {
                return this.Environment.ClientDTO.ClientEditCollectionType.ToTypeReference(this.ResolveMethodParameterType(element));
            }
            else
            {
                return this.Environment.ClientDTO.DefaultCodeTypeReferenceService.GetCodeTypeReferenceByType(type);
            }
        }


        protected virtual ICodeFileFactoryHeader<FileType> ClientContactFileFactoryHeader { get; } =

            new CodeFileFactoryHeader<FileType>(FileType.ClientContact, "", t => t.Name);

        protected virtual ICodeFileFactoryHeader<FileType> SimpleClientFileFactoryHeader { get; } =

            new CodeFileFactoryHeader<FileType>(FileType.SimpleClientImpl, "", t => t.Name.Skip("I", false) + "Client");

        protected virtual ICodeFileFactoryHeader<FileType> ServiceProxyFileFactoryHeader { get; } =

            new CodeFileFactoryHeader<FileType>(FileType.ServiceProxy, "", t => t.Name.Skip("I", false) + "ServiceProxy");

        protected override IEnumerable<ICodeFileFactoryHeader<FileType>> GetFileFactoryHeaders()
        {
            yield return this.ClientContactFileFactoryHeader;
            yield return this.SimpleClientFileFactoryHeader;
            yield return this.ServiceProxyFileFactoryHeader;
        }
    }
}
