using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

using CommonFramework;
using CommonFramework.DependencyInjection;

using Framework.Application.Domain;
using Framework.BLL.Domain.Persistent.Extensions;
using Framework.Core;
using Framework.Core.TypeResolving.TypeSource;
using Framework.Database;
using Framework.ExtendedMetadata;
using Framework.FileGeneration.Extensions;
using Framework.Projection;
using Framework.Projection.Contract;
using Framework.ExtendedMetadata;
using Framework.Projection.Lambda;
using Framework.Projection.Lambda.ProjectionSource._Base;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.FileGeneration.Configuration;

public abstract class FileGenerationEnvironment<TDomainObjectBase, TPersistentDomainObjectBase, TAuditPersistentDomainObjectBase, TIdent> : IFileGenerationEnvironment
    where TPersistentDomainObjectBase : TDomainObjectBase, IIdentityObject<TIdent>
    where TAuditPersistentDomainObjectBase : TPersistentDomainObjectBase, IAuditObject
{
    private readonly Assembly? modelAssembly;

    private readonly Lazy<ReadOnlyCollection<Assembly>> domainObjectAssemblies;

    private readonly Lazy<IServiceProvider> lazyServiceProvider;

    protected FileGenerationEnvironment(Expression<Func<TPersistentDomainObjectBase, TIdent>> identityPropertyExpr, Assembly? modelAssembly = null)
    {
        if (identityPropertyExpr == null) throw new ArgumentNullException(nameof(identityPropertyExpr));

        this.IdentityProperty = (identityPropertyExpr.Body as MemberExpression)
                                .Maybe(expr => expr.Member as PropertyInfo)
                                .FromMaybe(() => new ArgumentException("invalid property expression"));

        this.DomainObjectBaseType = typeof(TDomainObjectBase);
        this.PersistentDomainObjectBaseType = typeof(TPersistentDomainObjectBase);
        this.AuditPersistentDomainObjectBaseType = typeof(TAuditPersistentDomainObjectBase);

        this.modelAssembly = modelAssembly;

        this.domainObjectAssemblies = LazyHelper.Create(() => this.GetDomainObjectAssemblies().Distinct().ToReadOnlyCollection());

        this.lazyServiceProvider = LazyHelper.Create(this.BuildServiceProvider);

        this.ProjectionEnvironments = LazyInterfaceImplementHelper.CreateProxy(() => this.GetProjectionEnvironments().ToReadOnlyCollectionI());
    }

    public IServiceProvider ServiceProvider => this.lazyServiceProvider.Value;

    public PropertyInfo IdentityProperty { get; }

    public Type DomainObjectBaseType { get; }

    public Type PersistentDomainObjectBaseType { get; }

    public Type AuditPersistentDomainObjectBaseType { get; }

    public virtual string TargetSystemName => this.PersistentDomainObjectBaseType.ExtractSystemName();

    public IReadOnlyCollection<IProjectionEnvironment> ProjectionEnvironments { get; }

    public abstract IMetadataProxyProvider MetadataProxyProvider { get; }

    public ReadOnlyCollection<Assembly> DomainObjectAssemblies => this.domainObjectAssemblies.Value;

    protected virtual string ProjectionNamespace => $"{this.PersistentDomainObjectBaseType.GetNamespacePrefix()}.Domain.Projections";

    protected virtual IServiceProvider BuildServiceProvider() =>
        new ServiceCollection()
            .AddServiceProxyFactory()
            .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

    protected virtual IEnumerable<Assembly> GetDomainObjectAssemblies()
    {
        yield return this.PersistentDomainObjectBaseType.Assembly;

        if (this.modelAssembly != null)
        {
            yield return this.modelAssembly;
        }
    }

    /// <summary>
    /// Получение окружения для работы с проекциями
    /// </summary>
    /// <returns></returns>

    protected virtual IEnumerable<IProjectionEnvironment> GetProjectionEnvironments()
    {
        var baseProjectionEnvironment = this.GetProjectionEnvironment();

        if (baseProjectionEnvironment != null)
        {
            yield return baseProjectionEnvironment;
        }
    }

    protected virtual IProjectionEnvironment? GetProjectionEnvironment() => null;

    /// <summary>
    /// Создание окружения проекций через атрибуты
    /// </summary>
    /// <returns></returns>
    protected IProjectionEnvironment CreateDefaultProjectionContractEnvironment()
    {
        var baseName = this.PersistentDomainObjectBaseType.Assembly.GetName().Name!;

        var assemblyName = $"{baseName}.Projections";

        var fullAssemblyNamePostfix = this.PersistentDomainObjectBaseType.Assembly.FullName!.Skip(baseName, true);

        var fullAssemblyName = assemblyName + fullAssemblyNamePostfix;

        return ProjectionContractEnvironment.Create(
            this.MetadataProxyProvider,
            new TypeSource(this.GetDomainObjectAssemblies()),
            assemblyName,
            fullAssemblyName,
            this.DomainObjectBaseType,
            this.PersistentDomainObjectBaseType,
            this.ProjectionNamespace);
    }

    protected IProjectionEnvironment CreateDefaultProjectionLambdaEnvironment(IProjectionSource projectionSource, CreateProjectionLambdaSetupParams createParams)
    {
        if (projectionSource == null) throw new ArgumentNullException(nameof(projectionSource));
        if (createParams == null) throw new ArgumentNullException(nameof(createParams));

        return ProjectionLambdaEnvironment.Create(
            this.ExtendedMetadata,
            projectionSource,
            createParams.AssemblyName,
            createParams.FullAssemblyName,
            this.DomainObjectBaseType,
            this.PersistentDomainObjectBaseType,
            this.ProjectionNamespace,
            createParams.UseDependencySecurity);
    }

    protected IProjectionEnvironment CreateDefaultProjectionLambdaEnvironment(IProjectionSource projectionSource, Action<CreateProjectionLambdaSetupParams>? setupAction = null)
    {
        var createParams = this.GetCreateProjectionLambdaSetupParams().Self(@params => setupAction?.Invoke(@params));

        return this.CreateDefaultProjectionLambdaEnvironment(projectionSource, createParams);
    }

    protected CreateProjectionLambdaSetupParams GetCreateProjectionLambdaSetupParams(string projectionSubNamespace = "Projections", bool useDependencySecurity = true)
    {
        var baseName = this.PersistentDomainObjectBaseType.Assembly.GetName().Name!;

        var assemblyName = $"{baseName}.{projectionSubNamespace}";

        var fullAssemblyNamePostfix = this.PersistentDomainObjectBaseType.Assembly.FullName!.Skip(baseName, true);

        var fullAssemblyName = assemblyName + fullAssemblyNamePostfix;

        return new CreateProjectionLambdaSetupParams { AssemblyName = assemblyName, FullAssemblyName = fullAssemblyName, UseDependencySecurity = useDependencySecurity };
    }

    protected IProjectionEnvironment CreateManualProjectionLambdaEnvironment(Assembly assembly)
    {
        if (assembly == null) throw new ArgumentNullException(nameof(assembly));

        return new ManualProjectionEnvironment(assembly, this.PersistentDomainObjectBaseType, this.ExtendedMetadata);
    }
}
