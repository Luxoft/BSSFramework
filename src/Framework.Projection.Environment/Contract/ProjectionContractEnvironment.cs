using System;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.Metadata;

using JetBrains.Annotations;

namespace Framework.Projection.Contract;

public abstract class ProjectionContractEnvironment : ProjectionEnvironmentBase
{
    protected ProjectionContractEnvironment([NotNull] ITypeSource typeSource)
    {
        if (typeSource == null) throw new ArgumentNullException(nameof(typeSource));

        this.ContractTypeResolver = LazyInterfaceImplementHelper.CreateProxy(() => this.CreateContractTypeResolver(typeSource));
    }

    public ITypeResolver<Type> ContractTypeResolver { get; private set; }

    private ITypeResolver<Type> CreateContractTypeResolver(ITypeSource typeSource)
    {
        var generateTypeResolver = new GenerateTypeResolver(this, typeSource);

        this.ContractTypeResolver = generateTypeResolver;

        return TypeResolverHelper.Create(generateTypeResolver.projectionContracts.ToDictionary(type => type, generateTypeResolver.Resolve));
    }


    public static ProjectionContractEnvironment Create(
            [NotNull] ITypeSource typeSource,
            [NotNull] string assemblyName,
            [NotNull] string assemblyFullName,
            [NotNull] Type domainObjectBaseType,
            [NotNull] Type persistentDomainObjectBaseType,
            [NotNull] string @namespace,
            bool useDependencySecurity = true)
    {
        return new DefaultProjectionContractEnvironment(
                                                        typeSource,
                                                        assemblyName,
                                                        assemblyFullName,
                                                        domainObjectBaseType,
                                                        persistentDomainObjectBaseType,
                                                        @namespace,
                                                        useDependencySecurity);
    }

    private class DefaultProjectionContractEnvironment : ProjectionContractEnvironment
    {
        public DefaultProjectionContractEnvironment(
                [NotNull] ITypeSource typeSource,
                [NotNull] string assemblyName,
                [NotNull] string assemblyFullName,
                [NotNull] Type domainObjectBaseType,
                [NotNull] Type persistentDomainObjectBaseType,
                [NotNull] string @namespace,
                bool useDependencySecurity)
                : base(typeSource)
        {
            if (assemblyName == null) { throw new ArgumentNullException(nameof(assemblyName)); }
            if (assemblyFullName == null) { throw new ArgumentNullException(nameof(assemblyFullName)); }
            if (@namespace == null) { throw new ArgumentNullException(nameof(@namespace)); }

            if (string.IsNullOrWhiteSpace(@namespace)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(@namespace));


            this.Assembly = LazyInterfaceImplementHelper.CreateProxy<IAssemblyInfo>(() => new AssemblyInfo(assemblyName, assemblyFullName, this.ContractTypeResolver));
            this.Namespace = @namespace;
            this.DomainObjectBaseType = domainObjectBaseType ?? throw new ArgumentNullException(nameof(domainObjectBaseType));
            this.PersistentDomainObjectBaseType = persistentDomainObjectBaseType ?? throw new ArgumentNullException(nameof(persistentDomainObjectBaseType));
            this.UseDependencySecurity = useDependencySecurity;
        }

        public override string Namespace { get; }

        public override IAssemblyInfo Assembly { get; }

        public override bool UseDependencySecurity { get; }

        public override Type DomainObjectBaseType { get; }

        public override Type PersistentDomainObjectBaseType { get; }
    }
}
