using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.Core.Services;
using Framework.SecuritySystem.Rules.Builders;
using Framework.DomainDriven.BLL.Tracking;
using Framework.DomainDriven.UnitTest.Mock;
using Framework.HierarchicalExpand;
using Framework.OData;
using Framework.QueryableSource;
using Framework.QueryLanguage;
using Framework.SecuritySystem;
using Framework.Validation;

using NSubstitute;

namespace Framework.DomainDriven.BLL.Security.Test.SecurityHierarchy.Domain
{
    using Framework.Core.Serialization;

    public class TestBLLContext : ITestBLLContext, ISecurityBLLContext<IAuthorizationBLLContext<Guid>, PersistentDomainObjectBase, DomainObjectBase, Guid>, IAccessDeniedExceptionServiceContainer<PersistentDomainObjectBase>
    {
        private readonly HierarchyDomainDAL hierarchyDomainDal;

        private readonly Lazy<SyncDenormolizedValuesService<ITestBLLContext, PersistentDomainObjectBase, HierarchyObject, HierarchyObjectAncestorLink, HierarchyObjectToAncestorOrChildLink, Guid, NamedLockObject, NamedLockOperation>> _syncHierarchyService;

        private readonly HierarchyDomainAncestorLogic _hierarchyDomainAncestorLogic;

        private readonly MockDAL<HierarchyObjectAncestorLink, Guid> _domainAncestorLinkDal;
        private readonly MockDAL<NamedLockObject, Guid> _namedLockDAL;

        private readonly IBLLFactoryContainer<IDefaultBLLFactory<PersistentDomainObjectBase, Guid>> _defaultFactoryContainer;

        public TestBLLContext(List<HierarchyObject> hierarchicalObjects)
        {
            this.hierarchyDomainDal = new HierarchyDomainDAL(hierarchicalObjects);

            this._domainAncestorLinkDal = new MockDAL<HierarchyObjectAncestorLink, Guid>(hierarchicalObjects.ToAncestorLinks<HierarchyObject, HierarchyObjectAncestorLink, HierarchyObjectToAncestorOrChildLink>().ToList());

            this._namedLockDAL = new MockDAL<NamedLockObject, Guid>(new NamedLockObject() { LockOperation = NamedLockOperation.HierarchyObjectAncestorLinkLock });

            this._defaultFactoryContainer = Substitute.For<IBLLFactoryContainer<IDefaultBLLFactory<PersistentDomainObjectBase, Guid>>>();

            this._syncHierarchyService = new Lazy<SyncDenormolizedValuesService<ITestBLLContext, PersistentDomainObjectBase, HierarchyObject, HierarchyObjectAncestorLink, HierarchyObjectToAncestorOrChildLink, Guid, NamedLockObject, NamedLockOperation>>(() =>
                new SyncDenormolizedValuesService<ITestBLLContext, PersistentDomainObjectBase, HierarchyObject, HierarchyObjectAncestorLink, HierarchyObjectToAncestorOrChildLink, Guid, NamedLockObject, NamedLockOperation>(
                 NSubstitute.s
                 this.hierarchyDomainDal,
                 this._domainAncestorLinkDal
                 this._namedLockDAL));

            var defaultFactory = Substitute.For<IDefaultBLLFactory<PersistentDomainObjectBase, Guid>>();

            this._defaultFactoryContainer.Default.Returns(defaultFactory);

            var namedLockLogic = new NamedLockLogic(this); // MAGIC

            defaultFactory.Create<NamedLockObject>().Returns(namedLockLogic);
            defaultFactory.Create<HierarchyObjectAncestorLink>().Returns(this._hierarchyDomainAncestorLogic);

        }

        public IParser<string, SelectOperation> SelectOperationParser { get; private set; }

        public HierarchyDomainAncestorLogic HierarchyDomainAncestorLogic => this._hierarchyDomainAncestorLogic;

        public IOperationEventSenderContainer<PersistentDomainObjectBase> OperationSenders => new OperationEventSenderContainer<DomainObjectBase>(new List<IOperationEventListener<DomainObjectBase>>());

        public IBLLFactoryContainer<IDefaultBLLFactory<PersistentDomainObjectBase, Guid>> Logics => this._defaultFactoryContainer;

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

        public IQueryableSource<PersistentDomainObjectBase> GetQueryableSource() => new BLLQueryableSource<TestBLLContext, PersistentDomainObjectBase, DomainObjectBase, Guid>(this);

        public IValidator Validator => ValidatorBase.Success;

        public SyncDenormolizedValuesService<ITestBLLContext, PersistentDomainObjectBase, HierarchyObject, HierarchyObjectAncestorLink, HierarchyObjectToAncestorOrChildLink, Guid, NamedLockObject, NamedLockOperation> SyncHierarchyService => this._syncHierarchyService.Value;

        public bool AllowVirtualPropertyInOdata(Type domainType) => false;

        public void Flush()
        {
            this._domainAncestorLinkDal.Flush();
            this._namedLockDAL.Flush();
            this.hierarchyDomainDal.Flush();
        }

        public ITrackingService<PersistentDomainObjectBase> TrackingService { get; private set; }

        public IHierarchicalObjectExpanderFactory<Guid> HierarchicalObjectExpanderFactory => new HierarchicalObjectExpanderFactory<PersistentDomainObjectBase, Guid>(this.GetQueryableSource(), new ProjectionHierarchicalRealTypeResolver());

        IAccessDeniedExceptionService IAccessDeniedExceptionServiceContainer.AccessDeniedExceptionService => this.AccessDeniedExceptionService;

        public IServiceProvider ServiceProvider => throw new NotImplementedException();
    }
}
