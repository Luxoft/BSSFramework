using Framework.Core;
using Framework.Core.TypeResolving;
using Framework.Core.TypeResolving.TypeSource;
using Framework.Database.Metadata;
using Framework.Projection.ExtendedMetadata;

namespace Framework.Projection.Contract;

public abstract class ProjectionContractEnvironment : ProjectionEnvironmentBase
{
    protected ProjectionContractEnvironment(IDomainTypeRootExtendedMetadata extendedMetadata, ITypeSource typeSource)
        : base(extendedMetadata) =>
        this.ContractTypeResolver = LazyInterfaceImplementHelper.CreateProxy(() => this.CreateContractTypeResolver(typeSource));

    public ITypeResolver<Type> ContractTypeResolver { get; private set; }

    private ITypeResolver<Type> CreateContractTypeResolver(ITypeSource typeSource)
    {
        var generateTypeResolver = new GenerateTypeResolver(this, typeSource);

        this.ContractTypeResolver = generateTypeResolver;

        return TypeResolverHelper.Create(generateTypeResolver.ProjectionContracts.ToDictionary(type => type, generateTypeResolver.TryResolve));
    }


    public static ProjectionContractEnvironment Create(
        IDomainTypeRootExtendedMetadata extendedMetadata,
        ITypeSource typeSource,
        string assemblyName,
        string assemblyFullName,
        Type domainObjectBaseType,
        Type persistentDomainObjectBaseType,
        string @namespace,
        bool useDependencySecurity = true)
    {
        return new DefaultProjectionContractEnvironment(
            extendedMetadata,
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
            IDomainTypeRootExtendedMetadata extendedMetadata,
            ITypeSource typeSource,
            string assemblyName,
            string assemblyFullName,
            Type domainObjectBaseType,
            Type persistentDomainObjectBaseType,
            string @namespace,
            bool useDependencySecurity)
            : base(extendedMetadata, typeSource)
        {
            if (assemblyName == null)
            {
                throw new ArgumentNullException(nameof(assemblyName));
            }

            if (assemblyFullName == null)
            {
                throw new ArgumentNullException(nameof(assemblyFullName));
            }

            if (@namespace == null)
            {
                throw new ArgumentNullException(nameof(@namespace));
            }

            if (string.IsNullOrWhiteSpace(@namespace))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(@namespace));


            this.Assembly = LazyInterfaceImplementHelper.CreateProxy<IAssemblyInfo>(
                () => new AssemblyInfo(assemblyName, assemblyFullName, this.ContractTypeResolver));
            this.Namespace = @namespace;
            this.DomainObjectBaseType = domainObjectBaseType ?? throw new ArgumentNullException(nameof(domainObjectBaseType));
            this.PersistentDomainObjectBaseType = persistentDomainObjectBaseType
                                                  ?? throw new ArgumentNullException(nameof(persistentDomainObjectBaseType));
            this.UseDependencySecurity = useDependencySecurity;
        }

        public override string Namespace { get; }

        public override IAssemblyInfo Assembly { get; }

        public override bool UseDependencySecurity { get; }

        public override Type DomainObjectBaseType { get; }

        public override Type PersistentDomainObjectBaseType { get; }
    }
}
