using System.Reflection;

using Framework.Core;
using Framework.Core.TypeResolving;
using Framework.ExtendedMetadata;
using Framework.Projection._ImplType;

namespace Framework.Projection.Contract;

public abstract class ProjectionContractEnvironment : ProjectionEnvironmentBase
{
    protected ProjectionContractEnvironment(IMetadataProxyProvider metadataProxyProvider, ITypeSource typeSource)
        : base(metadataProxyProvider) =>
        this.ContractTypeResolver = LazyInterfaceImplementHelper.CreateProxy(() => this.CreateContractTypeResolver(typeSource));

    public ITypeResolver<Type> ContractTypeResolver { get; private set; }

    private ITypeResolver<Type> CreateContractTypeResolver(ITypeSource typeSource)
    {
        var generateTypeResolver = new GenerateTypeResolver(this, typeSource);

        this.ContractTypeResolver = generateTypeResolver;
        return TypeResolverHelper.Create(generateTypeResolver.ProjectionContracts.ToDictionary(type => type, this.ContractTypeResolver.Resolve));
    }

    public static ProjectionContractEnvironment Create(
        IMetadataProxyProvider metadataProxyProvider,
        ITypeSource typeSource,
        string assemblyName,
        string assemblyFullName,
        Type domainObjectBaseType,
        Type persistentDomainObjectBaseType,
        string @namespace,
        bool useDependencySecurity = true) =>
        new DefaultProjectionContractEnvironment(
            metadataProxyProvider,
            typeSource,
            assemblyName,
            assemblyFullName,
            domainObjectBaseType,
            persistentDomainObjectBaseType,
            @namespace,
            useDependencySecurity);

    private class DefaultProjectionContractEnvironment(
        IMetadataProxyProvider metadataProxyProvider,
        ITypeSource typeSource,
        string assemblyFullName,
        string assemblyName,
        Type domainObjectBaseType,
        Type persistentDomainObjectBaseType,
        string @namespace,
        bool useDependencySecurity) : ProjectionContractEnvironment(metadataProxyProvider, typeSource)
    {
        public override string Namespace { get; } =
            string.IsNullOrWhiteSpace(@namespace) ? throw new ArgumentException("Value cannot be null or whitespace.", nameof(@namespace)) : @namespace;

        public override Assembly Assembly => field ??= new GeneratedAssembly(assemblyFullName, assemblyName, this.ContractTypeResolver);

        public override bool UseDependencySecurity { get; } = useDependencySecurity;

        public override Type DomainObjectBaseType { get; } = domainObjectBaseType;

        public override Type PersistentDomainObjectBaseType { get; } = persistentDomainObjectBaseType;
    }
}
