﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.Core.Services;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Configuration;
using Framework.DomainDriven.BLL.Security;
using Framework.SecuritySystem.Rules.Builders;
using Framework.DomainDriven.BLL.Tracking;
using Framework.HierarchicalExpand;
using Framework.Projection;
using Framework.QueryLanguage;
using Framework.Security;
using Framework.SecuritySystem;
using Framework.Validation;

using JetBrains.Annotations;

namespace Framework.Authorization.BLL
{
    public partial class AuthorizationBLLContext
    {
        private readonly IAuthorizationBLLFactoryContainer logics;

        private readonly Lazy<Principal> lazyCurrentPrincipal;

        private readonly Lazy<Settings> lazySettings;

        private readonly Func<string, IAuthorizationBLLContext> impersonateFunc;

        private readonly IDictionaryCache<string, EntityType> entityTypeByNameCache;

        private readonly IDictionaryCache<Guid, EntityType> entityTypeByIdCache;

        private readonly ISecurityProvider<Operation> operationSecurityProvider;

        private static readonly ITypeResolver<string> CurrentTargetSystemTypeResolver =
            TypeSource.FromSample<PersistentDomainObjectBase>().ToDefaultTypeResolver();

        public AuthorizationBLLContext(
            [NotNull] IServiceProvider serviceProvider,
            [NotNull] IDALFactory<PersistentDomainObjectBase, Guid> dalFactory,
            [NotNull] BLLOperationEventListenerContainer<DomainObjectBase> operationListeners,
            [NotNull] BLLSourceEventListenerContainer<PersistentDomainObjectBase> sourceListeners,
            [NotNull] IObjectStateService objectStateService,
            [NotNull] IAccessDeniedExceptionService<PersistentDomainObjectBase> accessDeniedExceptionService,
            [NotNull] IStandartExpressionBuilder standartExpressionBuilder,
            [NotNull] IValidator validator,
            [NotNull] IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
            [NotNull] IFetchService<PersistentDomainObjectBase, FetchBuildRule> fetchService,
            [NotNull] IDateTimeService dateTimeService,
            [NotNull] IUserAuthenticationService userAuthenticationService,
            [NotNull] ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid> securityExpressionBuilderFactory,
            [NotNull] IConfigurationBLLContext configuration,
            [NotNull] IAuthorizationSecurityService securityService,
            [NotNull] IAuthorizationBLLFactoryContainer logics,
            [NotNull] IAuthorizationExternalSource externalSource,
            [NotNull] IRunAsManager runAsManager,
            [NotNull] Func<string, IAuthorizationBLLContext> impersonateFunc,
            [NotNull] ITypeResolver<string> securityTypeResolver)
            : base(
                serviceProvider,
                dalFactory,
                operationListeners,
                sourceListeners,
                objectStateService,
                accessDeniedExceptionService,
                standartExpressionBuilder,
                validator,
                hierarchicalObjectExpanderFactory,
                fetchService,
                dateTimeService)
        {
            this.SecurityExpressionBuilderFactory = securityExpressionBuilderFactory ?? throw new ArgumentNullException(nameof(securityExpressionBuilderFactory));
            this.SecurityService = securityService ?? throw new ArgumentNullException(nameof(securityService));
            this.logics = logics ?? throw new ArgumentNullException(nameof(logics));
            this.ExternalSource = externalSource ?? throw new ArgumentNullException(nameof(externalSource));
            this.RunAsManager = runAsManager ?? throw new ArgumentNullException(nameof(runAsManager));

            this.impersonateFunc = impersonateFunc ?? throw new ArgumentNullException(nameof(impersonateFunc));
            this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            this.lazyCurrentPrincipal = LazyHelper.Create(() => this.Logics.Principal.GetCurrent());

            this.CurrentPrincipalName = userAuthenticationService.GetUserName();

            this.SecurityTypeResolver = securityTypeResolver.OverrideInput((EntityType entityType) => entityType.Name);

            this.lazySettings = LazyHelper.Create(() => this.Logics.Default.Create<Setting>().GetFullList().ToSettings());

            this.entityTypeByNameCache = new DictionaryCache<string, EntityType>(
                domainTypeName => this.Logics.EntityType.GetByName(domainTypeName, true),
                StringComparer.CurrentCultureIgnoreCase)
                .WithLock();

            this.entityTypeByIdCache = new DictionaryCache<Guid, EntityType>(
                domainTypeId => this.Logics.EntityType.GetById(domainTypeId, true))
                .WithLock();

            this.operationSecurityProvider = new OperationSecurityProvider(this);
        }

        public IConfigurationBLLContext Configuration { get; private set; }

        public ITypeResolver<string> TypeResolver => CurrentTargetSystemTypeResolver;

        public ITypeResolver<EntityType> SecurityTypeResolver { get; private set; }

        public IRunAsManager RunAsManager { get; private set; }

        public string CurrentPrincipalName { get; private set; }

        public IAuthorizationSecurityService SecurityService { get; private set; }

        public Settings Settings => this.lazySettings.Value;

        public override IAuthorizationBLLFactoryContainer Logics => this.logics;

        public IAuthorizationExternalSource ExternalSource { get; private set; }

        public Principal CurrentPrincipal => this.lazyCurrentPrincipal.Value;

        public ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid> SecurityExpressionBuilderFactory { get; private set; }

        public IAuthorizationBLLContext Impersonate(string principalName)
        {
            return this.impersonateFunc(principalName);
        }

        public EntityType GetEntityType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return this.GetEntityType(type.Name);
        }

        public EntityType GetEntityType(string domainTypeName)
        {
            if (domainTypeName == null) throw new ArgumentNullException(nameof(domainTypeName));

            return this.entityTypeByNameCache[domainTypeName];
        }

        public EntityType GetEntityType(Guid domainTypeId)
        {
            if (domainTypeId.IsDefault()) throw new ArgumentOutOfRangeException(nameof(domainTypeId));

            return this.entityTypeByIdCache[domainTypeId];
        }

        private bool HasAccess(AvailablePermissionFilter filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            return this.Logics.Permission.GetSecureQueryable().Where(filter.ToFilterExpression()).Any();
        }

        public bool HasAccess(Operation operation)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));

            return this.HasAccess(new AvailablePermissionOperationFilter(this.DateTimeService, this.RunAsManager.PrincipalName, operation));
        }

        public bool HasAccess<TSecurityOperationCode>([NotNull] NonContextSecurityOperation<TSecurityOperationCode> securityOperation, bool withRunAs)
            where TSecurityOperationCode : struct, Enum
        {
            if (securityOperation == null) throw new ArgumentNullException(nameof(securityOperation));
            if (securityOperation.Code.IsDefault()) { throw new ArgumentOutOfRangeException(); }

            var principalName = withRunAs ? this.RunAsManager.PrincipalName : this.CurrentPrincipalName;

            var filter = new AvailablePermissionOperationFilter<TSecurityOperationCode>(
                this.DateTimeService,
                principalName,
                securityOperation.Code);

            return this.HasAccess(filter);
        }

        public bool HasAccess<TSecurityOperationCode>([NotNull] NonContextSecurityOperation<TSecurityOperationCode> securityOperation)
            where TSecurityOperationCode : struct, Enum
        {
            return this.HasAccess(securityOperation, true);
        }

        public string ResolveSecurityTypeName(Type type)
        {
            return type.GetProjectionSourceTypeOrSelf().Name;
        }

        public List<Dictionary<Type, IEnumerable<Guid>>> GetPermissions<TSecurityOperationCode>(
            ContextSecurityOperation<TSecurityOperationCode> securityOperation,
            IEnumerable<Type> securityTypes)
            where TSecurityOperationCode : struct, Enum
        {
            if (securityOperation == null) throw new ArgumentNullException(nameof(securityOperation));
            if (securityTypes == null) throw new ArgumentNullException(nameof(securityTypes));

            var filter = new AvailablePermissionOperationFilter<TSecurityOperationCode>(
                this.DateTimeService, this.RunAsManager.PrincipalName, securityOperation.Code);

            var permissions = this.Logics.Permission.GetObjectsBy(
                filter, z => z.SelectMany(q => q.FilterItems).SelectNested(q => q.Entity).Select(q => q.EntityType));

            var securityTypesCache = securityTypes.ToReadOnlyCollection();

            return permissions.Select(permission => permission.ToDictionary(securityTypesCache))
                .Optimize()
                .ToList(permission => this.TryExpandPermission(permission, securityOperation.SecurityExpandType));
        }

        public IQueryable<IPermission<Guid>> GetPermissionQuery<TSecurityOperationCode>(ContextSecurityOperation<TSecurityOperationCode> securityOperation)
                where TSecurityOperationCode : struct, Enum
        {
            var filter = new AvailablePermissionOperationFilter<TSecurityOperationCode>(
               this.DateTimeService, this.RunAsManager.PrincipalName, securityOperation.Code);

            return this.Logics.Permission.GetUnsecureQueryable().Where(filter.ToFilterExpression());
        }

        private IEnumerable<string> GetAccessors(Expression<Func<Principal, bool>> principalFilter, AvailablePermissionFilter permissionFilter)
        {
            if (principalFilter == null) throw new ArgumentNullException(nameof(principalFilter));
            if (permissionFilter == null) throw new ArgumentNullException(nameof(permissionFilter));

            var extraPrincipalFilter =
                principalFilter.UpdateBody(
                    new OverridePropertyInfoVisitor<Principal, IEnumerable<Permission>>(
                        principal => principal.Permissions, permissionFilter.ToFilterExpression().ToCollectionFilter()));

            var principals = this.Logics.Principal.GetObjectsBy(extraPrincipalFilter);

            return principals.Select(principal => principal.Name);
        }

        public IEnumerable<string> GetAccessors(Operation operation, Expression<Func<Principal, bool>> principalFilter)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));
            if (principalFilter == null) throw new ArgumentNullException(nameof(principalFilter));

            return this.GetAccessors(principalFilter, new AvailablePermissionOperationFilter(this.DateTimeService, null, operation));
        }

        public IEnumerable<string> GetAccessors<TSecurityOperationCode>(
            TSecurityOperationCode securityOperationCode, Expression<Func<IPrincipal<Guid>, bool>> principalFilter)
            where TSecurityOperationCode : struct, Enum
        {
            if (principalFilter == null) throw new ArgumentNullException(nameof(principalFilter));

            var mapVisitor = new OverrideParameterTypeVisitor(
                new Dictionary<Type, Type>
                {
                    { typeof(IPrincipal<Guid>), typeof(Principal) },
                    { typeof(IPermission<Guid>), typeof(Permission) },
                    { typeof(IPermissionFilterItem<Guid>), typeof(PermissionFilterItem) },
                    { typeof(IPermissionFilterEntity<Guid>), typeof(PermissionFilterEntity) },
                    { typeof(IEntityType), typeof(EntityType) }
                });

            return this.GetAccessors(
                (Expression<Func<Principal, bool>>)mapVisitor.Visit(principalFilter),
                new AvailablePermissionOperationFilter<TSecurityOperationCode>(this.DateTimeService, null, securityOperationCode));
        }

        private Dictionary<Type, IEnumerable<Guid>> TryExpandPermission(Dictionary<Type, List<Guid>> permission, HierarchicalExpandType expandType)
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));

            return permission.ToDictionary(
                pair => pair.Key,
                pair => this.HierarchicalObjectExpanderFactory.Create(pair.Key).Expand(pair.Value, expandType));
        }

        public ISecurityProvider<TDomainObject> GetPrincipalSecurityProvider<TDomainObject>(
            Expression<Func<TDomainObject, Principal>> principalSecurityPath)
            where TDomainObject : PersistentDomainObjectBase
        {
            if (principalSecurityPath == null) throw new ArgumentNullException(nameof(principalSecurityPath));

            return new PrincipalSecurityProvider<TDomainObject>(this, principalSecurityPath);
        }

        public ISecurityProvider<TDomainObject> GetBusinessRoleSecurityProvider<TDomainObject>(
            Expression<Func<TDomainObject, BusinessRole>> businessRoleSecurityPath)
            where TDomainObject : PersistentDomainObjectBase
        {
            if (businessRoleSecurityPath == null) throw new ArgumentNullException(nameof(businessRoleSecurityPath));

            return new BusinessRoleSecurityProvider<TDomainObject>(this, businessRoleSecurityPath);
        }

        public ISecurityProvider<Operation> GetOperationSecurityProvider()
        {
            return this.operationSecurityProvider;
        }

        /// <inheritdoc />
        public string GetFormattedPermission(Permission permission, string separator = " | ")
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));

            return this.GetPermissionVisualParts(permission).Join(separator);
        }

        private IEnumerable<string> GetPermissionVisualParts([NotNull] Permission permission)
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));

            yield return $"Role: {permission.Role}";

            yield return $"Period: {permission.Period}";

            foreach (var entityTypeGroup in permission.FilterItems.GroupBy(fi => fi.Entity.EntityType, fi => fi.Entity.EntityId))
            {
                var securityEntities = this.ExternalSource.GetTyped(entityTypeGroup.Key).GetSecurityEntitiesByIdents(entityTypeGroup);

                yield return $"{entityTypeGroup.Key.Name.ToPluralize()}: {securityEntities.Join(", ")}";
            }
        }

        IAuthorizationBLLContext IAuthorizationBLLContextContainer<IAuthorizationBLLContext>.Authorization => this;
    }
}
