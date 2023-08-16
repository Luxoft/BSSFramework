using Framework.Core.Services;
using Framework.SecuritySystem.Rules.Builders;
using Framework.DomainDriven.BLL.Tracking;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.UnitTest.Mock;
using Framework.HierarchicalExpand;
using Framework.OData;
using Framework.QueryableSource;
using Framework.QueryLanguage;
using Framework.SecuritySystem;
using Framework.Validation;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

namespace Framework.DomainDriven.BLL.Security.Test.SecurityHierarchy.Domain;

using Framework.Core.Serialization;

public class TestBllContext : ITestBLLContext, ISecurityBLLContext<IAuthorizationBLLContext<Guid>, PersistentDomainObjectBase, DomainObjectBase, Guid>, IAccessDeniedExceptionServiceContainer<PersistentDomainObjectBase>
{
    private readonly Lazy<SyncDenormolizedValuesService<PersistentDomainObjectBase, HierarchyObject, HierarchyObjectAncestorLink, HierarchyObjectToAncestorOrChildLink, Guid, NamedLockObject, NamedLockOperation>> syncHierarchyService;

    private readonly IBLLFactoryContainer<IDefaultBLLFactory<PersistentDomainObjectBase, Guid>> defaultFactoryContainer;

    public TestBllContext(List<HierarchyObject> hierarchicalObjects)
    {
        this.HierarchyDomainDal = new HierarchyDomainDAL(hierarchicalObjects);

        this.DomainAncestorLinkDal = new MockDAL<HierarchyObjectAncestorLink, Guid>(hierarchicalObjects.ToAncestorLinks<HierarchyObject, HierarchyObjectAncestorLink, HierarchyObjectToAncestorOrChildLink>().ToList());

        this.NamedLockDal = new MockDAL<NamedLockObject, Guid>(new NamedLockObject() { LockOperation = NamedLockOperation.HierarchyObjectAncestorLinkLock });

        this.defaultFactoryContainer = Substitute.For<IBLLFactoryContainer<IDefaultBLLFactory<PersistentDomainObjectBase, Guid>>>();

        this.syncHierarchyService =
                new Lazy<SyncDenormolizedValuesService<PersistentDomainObjectBase, HierarchyObject, HierarchyObjectAncestorLink, HierarchyObjectToAncestorOrChildLink, Guid, NamedLockObject, NamedLockOperation>>(() =>
                        new SyncDenormolizedValuesService<PersistentDomainObjectBase, HierarchyObject, HierarchyObjectAncestorLink, HierarchyObjectToAncestorOrChildLink, Guid, NamedLockObject, NamedLockOperation>(
                         this.HierarchyDomainDal,
                         this.DomainAncestorLinkDal,
                         this.NamedLockDal));

        var defaultFactory = Substitute.For<IDefaultBLLFactory<PersistentDomainObjectBase, Guid>>();

        this.defaultFactoryContainer.Default.Returns(defaultFactory);

        this.ServiceProvider = new ServiceCollection()
                               .AddSingleton<IDAL<HierarchyObject, Guid>>(this.HierarchyDomainDal)
                               .AddSingleton<IDAL<HierarchyObjectAncestorLink, Guid>>(this.DomainAncestorLinkDal)
                               .AddSingleton<IDAL<NamedLockObject, Guid>>(this.NamedLockDal)
                               .BuildServiceProvider();

        var namedLockLogic = new NamedLockLogic(this); // MAGIC

        defaultFactory.Create<NamedLockObject>().Returns(namedLockLogic);
        defaultFactory.Create<HierarchyObjectAncestorLink>().Returns(this.HierarchyDomainAncestorLogic);
    }

    public IParser<string, SelectOperation> SelectOperationParser { get; }

    public HierarchyDomainDAL HierarchyDomainDal { get; }

    public HierarchyDomainAncestorLogic HierarchyDomainAncestorLogic { get; }

    public MockDAL<HierarchyObjectAncestorLink, Guid> DomainAncestorLinkDal { get; }

    public MockDAL<NamedLockObject, Guid> NamedLockDal { get; }

    public IOperationEventSenderContainer<PersistentDomainObjectBase> OperationSenders => new OperationEventSenderContainer<DomainObjectBase>(new List<IOperationEventListener<DomainObjectBase>>());

    public IBLLFactoryContainer<IDefaultBLLFactory<PersistentDomainObjectBase, Guid>> Logics => this.defaultFactoryContainer;

    public IAuthorizationBLLContext<Guid> Authorization { get; private set; }

    public ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid> SecurityExpressionBuilderFactory
    {
        get;
    }

    public IAccessDeniedExceptionService<PersistentDomainObjectBase> AccessDeniedExceptionService => new AccessDeniedExceptionService<PersistentDomainObjectBase, Guid>();

    public IStandartExpressionBuilder StandartExpressionBuilder => new StandartExpressionBuilder();

    public bool AllowedExpandTreeParents<TDomainObject>()
            where TDomainObject : PersistentDomainObjectBase =>
            throw new NotImplementedException();

    public IUserAuthenticationService UserAuthenticationService => Framework.Core.Services.UserAuthenticationService.CreateFor("testUser");

    public IQueryableSource<PersistentDomainObjectBase> GetQueryableSource() => new BLLQueryableSource<TestBllContext, PersistentDomainObjectBase, DomainObjectBase, Guid>(this);

    public IValidator Validator => ValidatorBase.Success;

    public SyncDenormolizedValuesService<PersistentDomainObjectBase, HierarchyObject, HierarchyObjectAncestorLink, HierarchyObjectToAncestorOrChildLink, Guid, NamedLockObject, NamedLockOperation> SyncHierarchyService => this.syncHierarchyService.Value;

    public bool AllowVirtualPropertyInOdata(Type domainType) => false;

    public void Flush()
    {
        this.DomainAncestorLinkDal.Flush();
        this.NamedLockDal.Flush();
        this.HierarchyDomainDal.Flush();
    }

    public ITrackingService<PersistentDomainObjectBase> TrackingService { get; private set; }

    public IHierarchicalObjectExpanderFactory<Guid> HierarchicalObjectExpanderFactory => new HierarchicalObjectExpanderFactory<PersistentDomainObjectBase, Guid>(this.GetQueryableSource(), new ProjectionHierarchicalRealTypeResolver());

    IAccessDeniedExceptionService IAccessDeniedExceptionServiceContainer.AccessDeniedExceptionService => this.AccessDeniedExceptionService;

    public IServiceProvider ServiceProvider { get; }
}
