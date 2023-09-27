using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;
using Framework.Persistent;
using Framework.Projection;
using Framework.Projection.Contract;
using Framework.Projection.Environment;
using Framework.Projection.Lambda;

namespace Framework.DomainDriven.Generation.Domain;

public abstract class GenerationEnvironment<TDomainObjectBase, TPersistentDomainObjectBase, TAuditPersistentDomainObjectBase, TIdent> : IGenerationEnvironment
        where TPersistentDomainObjectBase : TDomainObjectBase, IIdentityObject<TIdent>
        where TAuditPersistentDomainObjectBase : TPersistentDomainObjectBase, IAuditObject
{
    private readonly Assembly _modelAssembly;

    private readonly Lazy<ReadOnlyCollection<Assembly>> _domainObjectAssemblies;

    protected GenerationEnvironment(Expression<Func<TPersistentDomainObjectBase, TIdent>> identityPropertyExpr, Assembly modelAssembly = null)
    {
        if (identityPropertyExpr == null) throw new ArgumentNullException(nameof(identityPropertyExpr));

        this.IdentityProperty = (identityPropertyExpr.Body as MemberExpression)
                                .Maybe(expr => expr.Member as PropertyInfo)
                                .FromMaybe(() => new System.ArgumentException("invalid property expression"));

        this.DomainObjectBaseType = typeof(TDomainObjectBase);
        this.PersistentDomainObjectBaseType = typeof(TPersistentDomainObjectBase);
        this.AuditPersistentDomainObjectBaseType = typeof(TAuditPersistentDomainObjectBase);

        this._modelAssembly = modelAssembly;

        this._domainObjectAssemblies = LazyHelper.Create(() => this.GetDomainObjectAssemblies().Distinct().ToReadOnlyCollection());

        this.ProjectionEnvironments = LazyInterfaceImplementHelper.CreateProxy(() => this.GetProjectionEnvironments().ToReadOnlyCollectionI());
    }

    public PropertyInfo IdentityProperty { get; }

    public Type DomainObjectBaseType { get; }

    public Type PersistentDomainObjectBaseType { get; }

    public Type AuditPersistentDomainObjectBaseType { get; }

    public virtual string TargetSystemName => this.PersistentDomainObjectBaseType.GetTargetSystemName();

    protected virtual string ProjectionNamespace => $"{this.PersistentDomainObjectBaseType.GetNamespacePrefix()}.Domain.Projections";

    public abstract Type SecurityOperationType { get; }

    public abstract Type OperationContextType { get; }

    public IReadOnlyCollection<IProjectionEnvironment> ProjectionEnvironments { get; }

    public virtual IDomainTypeRootExtendedMetadata ExtendedMetadata { get; } = new DomainTypeRootExtendedMetadataBuilder();

    public ReadOnlyCollection<Assembly> DomainObjectAssemblies => this._domainObjectAssemblies.Value;

    protected virtual IEnumerable<Assembly> GetDomainObjectAssemblies()
    {
        yield return this.PersistentDomainObjectBaseType.Assembly;

        if (this._modelAssembly != null)
        {
            yield return this._modelAssembly;
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

    protected virtual IProjectionEnvironment GetProjectionEnvironment()
    {
        return null;
    }

    /// <summary>
    /// Создание окружения проекций через атрибуты
    /// </summary>
    /// <returns></returns>
    protected IProjectionEnvironment CreateDefaultProjectionContractEnvironment()
    {
        var baseName = this.PersistentDomainObjectBaseType.Assembly.GetName().Name;

        var assemblyName = $"{baseName}.Projections";

        var fullAssemblyNamePostfix = this.PersistentDomainObjectBaseType.Assembly.FullName.Skip(baseName, true);

        var fullAssemblyName = assemblyName + fullAssemblyNamePostfix;

        return ProjectionContractEnvironment.Create(this.ExtendedMetadata,
                                                    new TypeSource(this.GetDomainObjectAssemblies()),
                                                    assemblyName,
                                                    fullAssemblyName,
                                                    this.DomainObjectBaseType,
                                                    this.PersistentDomainObjectBaseType,
                                                    this.ProjectionNamespace);
    }

    /// <summary>
    /// Создание окружения проекций через лямбды
    /// </summary>
    /// <param name="projectionSource"></param>
    /// <returns></returns>
    protected IProjectionEnvironment CreateDefaultProjectionLambdaEnvironment(IProjectionSource projectionSource, CreateProjectionLambdaSetupParams createParams)
    {
        if (projectionSource == null) throw new ArgumentNullException(nameof(projectionSource));
        if (createParams == null) throw new ArgumentNullException(nameof(createParams));

        return ProjectionLambdaEnvironment.Create(this.ExtendedMetadata,
                                                  projectionSource,
                                                  createParams.AssemblyName,
                                                  createParams.FullAssemblyName,
                                                  this.DomainObjectBaseType,
                                                  this.PersistentDomainObjectBaseType,
                                                  this.ProjectionNamespace,
                                                  createParams.UseDependencySecurity);
    }

    /// <summary>
    /// Создание окружения проекций через лямбды
    /// </summary>
    /// <param name="projectionSource"></param>
    /// <returns></returns>
    protected IProjectionEnvironment CreateDefaultProjectionLambdaEnvironment(IProjectionSource projectionSource, Action<CreateProjectionLambdaSetupParams> setupAction = null)
    {
        if (projectionSource == null) { throw new ArgumentNullException(nameof(projectionSource)); }

        var createParams = this.GetCreateProjectionLambdaSetupParams().Self(@params => setupAction?.Invoke(@params));

        return this.CreateDefaultProjectionLambdaEnvironment(projectionSource, createParams);
    }

    protected CreateProjectionLambdaSetupParams GetCreateProjectionLambdaSetupParams(string projectionSubNamespace = "Projections", bool useDependencySecurity = true)
    {
        var baseName = this.PersistentDomainObjectBaseType.Assembly.GetName().Name;

        var assemblyName = $"{baseName}.{projectionSubNamespace}";

        var fullAssemblyNamePostfix = this.PersistentDomainObjectBaseType.Assembly.FullName.Skip(baseName, true);

        var fullAssemblyName = assemblyName + fullAssemblyNamePostfix;

        return new CreateProjectionLambdaSetupParams
               {
                       AssemblyName = assemblyName,
                       FullAssemblyName = fullAssemblyName,
                       UseDependencySecurity = useDependencySecurity
               };
    }

    protected IProjectionEnvironment CreateManualProjectionLambdaEnvironment(Assembly assembly)
    {
        if (assembly == null) throw new ArgumentNullException(nameof(assembly));

        return new ManualProjectionEnvironment(assembly, this.PersistentDomainObjectBaseType);
    }
}
