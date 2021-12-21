using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.DAL.Revisions;
using Framework.Exceptions;
using Framework.Notification;
using Framework.Persistent;

using JetBrains.Annotations;

using Serilog;

namespace Framework.Configuration.BLL.DBSubscription
{
    public class SubscriptionSystemService<TBLLContext> : BLLContextContainer<IConfigurationBLLContext>, ISubscriptionSystemService
        where TBLLContext : class
    {
        protected readonly ITargetSystemService<TBLLContext> TargetSystemService;

        protected readonly IMessageSender<MessageTemplateNotification> SubscriptionSender;

        private readonly ILogger logger;


        protected SubscriptionSystemService(IConfigurationBLLContext context, ITargetSystemService<TBLLContext> targetSystemService, ILogger logger, IMessageSender<MessageTemplateNotification> subscriptionSender)
            : base(context)
        {
            this.TargetSystemService = targetSystemService ?? throw new ArgumentNullException(nameof(targetSystemService));

            this.logger = logger;
            this.SubscriptionSender = subscriptionSender;
        }

        public SubscriptionSystemService(IConfigurationBLLContext context, ITargetSystemService<TBLLContext> targetSystemService)
            : this(context, targetSystemService, Log.ForContext("Module", "Subscription"), context.SubscriptionSender)
        {
        }

        public TBLLContext TargetSystemContext
        {
            get { return this.TargetSystemService.TargetSystemContext; }
        }

        public IList<ITryResult<Subscription>> ProcessChangedObjectUntyped(object prev, object next, Type type)
        {
            var methodDelegate = (Func<object, object, IList<ITryResult<Subscription>>>)(this.ProcessChangedObject<object>);

            return (IList<ITryResult<Subscription>>)methodDelegate.CreateGenericMethod(type).Invoke(this, new[] { prev, next });
        }

        public SubscriptionRecipientInfo GetRecipientsUntyped(Type type, object prev, object next, string subscriptionCode)
        {
            Func<object, object, string, SubscriptionRecipientInfo> methodDelegate = this.GetRecipients;

            var untypedResult = methodDelegate.CreateGenericMethod(type).Invoke(this, new[] { prev, next, subscriptionCode });

            return (SubscriptionRecipientInfo)untypedResult;
        }

        protected SubscriptionRecipientInfo GetRecipients<TDomainObject>(TDomainObject prev, TDomainObject next, string subscriptionCode)
            where TDomainObject : class
        {
            this.logger.Information("GetSubscriptionRecipients Type name: {Name}", typeof(TDomainObject).Name);

            var domainType = this.Context.GetDomainType(typeof(TDomainObject), true);

            this.logger.Information("GetSubscriptionRecipients TypeInfo:{Id}", domainType.Id);

            var activeSubscriptions = this.Context.Logics.Subscription.GetActiveSubscriptions(domainType, false)
                                                                      .Where(z => z.Code == subscriptionCode)
                                                                      .ToList();

            if (!activeSubscriptions.Any())
            {
                this.logger.Information("GetSubscriptionRecipients no ActiveSubscriptions");

                return null;
            }

            this.logger.Information("GetSubscriptionRecipients  activeSubscriptions count: {Count}", activeSubscriptions.Count);

            foreach (var subscription in activeSubscriptions)
            {
                var result = this.GetResultRecepients(prev, next, subscription);

                var recipients = this.TryExcludeCurrentUser(prev, next, subscription, result);

                return new SubscriptionRecipientInfo
                {
                    Recipients = recipients.Select(x => x.Email).Distinct().ToList(),
                    Subscription = subscription
                };
            }

            return null;
        }

        private IList<IEmployee> GetResultRecepients<TDomainObject>(TDomainObject prev, TDomainObject next, Subscription subscription)
            where TDomainObject : class
        {
            if (subscription == null) throw new ArgumentNullException(nameof(subscription));

            var recepientsByRoles = this.GetReceiversByRoles(prev, next, subscription);

            var recipientsByGeneration = subscription.Generation == null
                                       ? new IEmployee[0]
                                       : this.GetGenerationInfos(prev, next, subscription).SelectMany(q => q.Recipients).ToArray();

            return recepientsByRoles.GetMergeResult(recipientsByGeneration, subscription.RecepientsMode).ToList();
        }

        private IList<IEmployee> ApplyRecepientSelector(Subscription subscription, IEnumerable<IEmployee> recepientsByRoles, NotificationMessageGenerationInfo generationInfo)
        {
            if (subscription == null) throw new ArgumentNullException(nameof(subscription));
            if (recepientsByRoles == null) throw new ArgumentNullException(nameof(recepientsByRoles));
            if (generationInfo == null) throw new ArgumentNullException(nameof(generationInfo));

            var recipientsByGeneration = generationInfo.Recipients.ToList();

            return recepientsByRoles.GetMergeResult(recipientsByGeneration, subscription.RecepientsMode).ToList();
        }


        protected IList<ITryResult<Subscription>> ProcessChangedObject<TDomainObject>(TDomainObject prev, TDomainObject next)
            where TDomainObject : class
        {
            var domainType = this.Context.GetDomainType(typeof(TDomainObject), true);

            var request = from subscription in this.Context.Logics.Subscription.GetActiveSubscriptions(domainType).ToList()

                          select this.TryProcessSubscription(prev, next, subscription);

            return request.ToList();
        }


        protected ITryResult<Subscription> TryProcessSubscription<TDomainObject>(TDomainObject prev, TDomainObject next, [NotNull] Subscription subscription)
            where TDomainObject : class
        {
            if (subscription == null)
            {
                throw new ArgumentNullException(nameof(subscription));
            }

            var condition = subscription.Condition;

            return TryResult.Catch(() =>
            {
                if (condition == null)
                {
                    throw new BusinessLogicException("Condition of {0} not initialized", subscription);
                }

                if (condition.IsProcessed(prev, next))
                {
                    var result = TryResult.Catch(() => condition.WithContext
                                                 ? this.Context.ExpressionParsers.GetBySubscriptionCondition<TBLLContext, TDomainObject>().GetDelegate(subscription).Pipe(del => del(this.TargetSystemContext, prev, next))
                                                 : this.Context.ExpressionParsers.GetBySubscriptionCondition<TDomainObject>().GetDelegate(subscription).Pipe(del => del(prev, next)))

                                      .GetValue(ex => new BusinessLogicException(ex, $"Condition of subscription \"{subscription.Code}\" is evaluated with error: {ex.Message}"));

                    if (result)
                    {
                        this.ProcessSubscription(prev, next, subscription);

                        return subscription;
                    }

                    this.logger.Information("Subscription {Subscription} not processed by condition {Condition}", subscription, condition);

                    return null;
                }

                this.logger.Information(
                                        "{condition} of {subscription} not processed by ProcessType ({ProcessType}): Prev ({Prev}), Next ({Next})",
                                        condition,
                                        subscription,
                                        condition.ProcessType,
                                        prev.Maybe(v => v.ToFormattedString(typeof(TDomainObject).Name)) ?? "NULL",
                                        next.Maybe(v => v.ToFormattedString(typeof(TDomainObject).Name)) ?? "NULL");

                return null;
            });
        }

        protected void ProcessSubscription<TDomainObject>(TDomainObject prev, TDomainObject next, [NotNull] Subscription subscription)
            where TDomainObject : class
        {
            if (subscription == null)
            {
                throw new ArgumentNullException(nameof(subscription));
            }

            var notificationTemplates = this.GetNotificationTemplates(prev, next, subscription).ToList();

            var sendTemplatesRequest = from notificationTemplate in notificationTemplates

                                       where subscription.AllowEmptyListOfRecipients || notificationTemplate.Recipients.Any()

                                       from sendTemplate in subscription.SendIndividualLetters ? notificationTemplate.SplitByRecipient() : new[] { notificationTemplate }

                                       select sendTemplate;

            var sendTemplates = sendTemplatesRequest.ToList();

            foreach (var sendTemplate in sendTemplates)
            {
                this.SubscriptionSender.Send(

                    new MessageTemplateNotification(
                        subscription.MessageTemplate.Code,
                        sendTemplate,
                        typeof(TDomainObject),
                        sendTemplate.Recipients.Where(z => null != z)
                            .Select(x => x.Email)
                            .Where(q => !string.IsNullOrWhiteSpace(q)),
                        Enumerable.Empty<System.Net.Mail.Attachment>(),
                        subscription,
                        subscription.AllowEmptyListOfRecipients),
                    TransactionMessageMode.Auto);
            }
        }

        private IEnumerable<ObjectsVersion> GetNotificationTemplates<TDomainObject>(TDomainObject prev, TDomainObject next, [NotNull] Subscription subscription)
            where TDomainObject : class
        {
            if (subscription == null) throw new ArgumentNullException(nameof(subscription));

            var employeesByRoles = this.GetReceiversByRoles(prev, next, subscription);


            if (subscription.Generation == null)
            {
                var withExcudeCurrentUser = this.TryExcludeCurrentUser(prev, next, subscription, employeesByRoles);

                yield return ObjectsVersion.Create(prev, next, withExcudeCurrentUser);
            }
            else
            {
                var notificationMessageGenerationInfos = this.GetGenerationInfos(prev, next, subscription).EmptyIfNull().ToList();

                foreach (var generationInfo in notificationMessageGenerationInfos)
                {
                    var receivers = this.ApplyRecepientSelector(subscription, employeesByRoles, generationInfo);

                    var withExcudeCurrentUser = this.TryExcludeCurrentUser(prev, next, subscription, receivers);

                    if ((generationInfo.PreviousRoot == null || generationInfo.PreviousRoot is TDomainObject)
                     && (generationInfo.CurrentRoot == null || generationInfo.CurrentRoot is TDomainObject))
                    {
                        yield return ObjectsVersion.Create(generationInfo.PreviousRoot as TDomainObject, generationInfo.CurrentRoot as TDomainObject, withExcudeCurrentUser);
                    }
                    else
                    {
                        yield return ObjectsVersion.CreateDynamic(generationInfo.PreviousRoot, generationInfo.CurrentRoot, withExcudeCurrentUser);
                    }
                }
            }
        }

        private IEnumerable<NotificationMessageGenerationInfo> GetGenerationInfos<TDomainObject>(TDomainObject prev, TDomainObject next, [NotNull] Subscription subscription)
            where TDomainObject : class
        {
            if (subscription == null) throw new ArgumentNullException(nameof(subscription));
            if (subscription.Generation == null) throw new ArgumentException("generation lambda not initialized", nameof(subscription));

            if (!subscription.Generation.IsProcessed(prev, next))
            {
                return new NotificationMessageGenerationInfo[0];
            }

            try
            {
                return subscription.Generation.WithContext
                     ? this.Context.ExpressionParsers.GetBySubscriptionGeneration<TBLLContext, TDomainObject>().GetDelegate(subscription).Pipe(del => del(this.TargetSystemContext, prev, next))
                     : this.Context.ExpressionParsers.GetBySubscriptionGeneration<TDomainObject>().GetDelegate(subscription).Pipe(del => del(prev, next));
            }
            catch (Exception ex)
            {
                var errorMessage = $"Executing error in GetGenerationInfos '{subscription.Code}','{ex.Message}' ";

                this.logger.Error(ex, errorMessage);

                throw new Exception(errorMessage, ex);
            }
        }

        private IList<IEmployee> TryExcludeCurrentUser<TDomainObject>(TDomainObject prev, TDomainObject next, Subscription subscription, IList<IEmployee> employees)
        {
            if (subscription.ExcludeCurrentUser)
            {
                this.logger.Information("ExcludeCurrentUser subscription:{Code}", subscription.Code);

                var prevCurrentUserEmail = (prev as ICurrentUserEmailContainer).Maybe(container => container.CurrentUserEmail).TrimNull();

                var currentCurrentUserEmail = (next as ICurrentUserEmailContainer).Maybe(container => container.CurrentUserEmail).TrimNull();

                this.logger.Information("ExcludeCurrentUser currentCurrentUserEmail: {currentCurrentUserEmail}", currentCurrentUserEmail);
                this.logger.Information("ExcludeCurrentUser prevCurrentUserEmail: {prevCurrentUserEmail}", prevCurrentUserEmail);

                this.logger.Information("ExcludeCurrentUser Original Employee Email List: {Emails}", employees.Join(", ", x => x.Email));

                var result = employees.Where(x => !string.IsNullOrWhiteSpace(x.Email))
                                      .Where(x => !string.Equals(x.Email, currentCurrentUserEmail, StringComparison.OrdinalIgnoreCase))
                                      .Where(x => !string.Equals(x.Email, prevCurrentUserEmail, StringComparison.OrdinalIgnoreCase))
                                      .Distinct()
                                      .ToList();

                this.logger.Information("ExcludeCurrentUser Result Employee Email List: {Emails}", result.Join(", ", x => x.Email));

                return result;
            }
            else
            {
                return employees;
            }
        }

        private List<IEmployee> GetReceiversByRoles<TDomainObject>(TDomainObject prev, TDomainObject next, [NotNull] Subscription subscription)
            where TDomainObject : class
        {
            if (subscription == null) throw new ArgumentNullException(nameof(subscription));

            this.logger.Information("GetReceiversByRoles start");

            var principals = this.GetPrincipals(subscription, prev, next);

            var employees = this.PrincipalsToEmployees(principals);

            employees.Foreach(z => this.logger.Information("employe login: {Login}; email: {Email}; ", z.Login, z.Email));
            this.logger.Information("GetReceiversByRoles end");

            return employees;
        }

        private List<IEmployee> PrincipalsToEmployees(IEnumerable<Principal> principals)
        {
            if (principals == null) throw new ArgumentNullException(nameof(principals));

            var principalNames = principals.ToList(principal => principal.Name);

            principalNames.Foreach(z => this.logger.Information("principalNames: {principalNames}", z));

            var employees =
                this.Context.GetEmployeeSource()
                    .GetUnsecureQueryable()
                    .Where(employee => principalNames.Contains(employee.Login))
                    .ToList();

            return employees;
        }

        private NotificationFilterGroup GetNotificationFilterGroup<TDomainObject, TSecurityObject>(TDomainObject prev, TDomainObject next, SubscriptionSecurityItem securityItem)
            where TDomainObject : class
            where TSecurityObject : IIdentityObject<Guid>
        {
            if (securityItem == null) throw new ArgumentNullException(nameof(securityItem));
            if (securityItem.Source == null) throw new ArgumentException("source lambda not initialized", nameof(securityItem));

            if (!securityItem.Source.IsProcessed(prev, next))
            {
                return new NotificationFilterGroup<TSecurityObject>(new TSecurityObject[0], securityItem.ExpandType);
            }

            var securityItems = securityItem.Source.WithContext
                              ? this.Context.ExpressionParsers.GetBySubscriptionSecurityItemSource<TBLLContext, TDomainObject, TSecurityObject>().GetDelegate(securityItem).Pipe(del => del(this.TargetSystemContext, prev, next))
                              : this.Context.ExpressionParsers.GetBySubscriptionSecurityItemSource<TDomainObject, TSecurityObject>().GetDelegate(securityItem).Pipe(del => del(prev, next));

            return new NotificationFilterGroup<TSecurityObject>(securityItems, securityItem.ExpandType);
        }

        private IEnumerable<Principal> GetPrincipals<TDomainObject>([NotNull] Subscription subscription, TDomainObject prev, TDomainObject next)
            where TDomainObject : class
        {
            if (subscription == null) throw new ArgumentNullException(nameof(subscription));

            this.logger.Information("GetPrincipals subscription: {Code}", subscription.Code);

            var businessRoleIdents = subscription.SubBusinessRoles.ToArray(z => z.BusinessRoleId);

            var permissionBLL = this.Context.Authorization.Logics.Permission;

            var securityTypeResolver = this.Context.Authorization.SecurityTypeResolver;

            var dict = subscription.SecurityItems.ToDictionary(item => this.Context.Authorization.Logics.EntityType.GetById(item.AuthDomainTypeId, IdCheckMode.SkipEmpty).Maybe(entityType => securityTypeResolver.Resolve(entityType, true)));

            switch (subscription.SourceMode)
            {
                case SubscriptionSourceMode.Dynamic:
                    {
                        var filterItems = subscription.DynamicSource.WithContext
                            ? this.Context.ExpressionParsers.GetBySubscriptionDynamicSourceLegacy<TBLLContext, TDomainObject>().GetDelegate(subscription).Pipe(del => del(this.TargetSystemContext, prev, next))
                            : this.Context.ExpressionParsers.GetBySubscriptionDynamicSourceLegacy<TDomainObject>().GetDelegate(subscription).Pipe(del => del(prev, next));

                        var groups = from filterItem in filterItems

                                     group filterItem by filterItem.EntityName.ToLower() into g

                                     let entityType = this.Context.Authorization.GetEntityType(g.Key)

                                     let expandType = subscription.DynamicSourceExpandType.Value

                                     select new NotificationFilterGroup(securityTypeResolver.Resolve(entityType, true), g.Select(fi => fi.Id), entityType.Expandable ? expandType : expandType.WithoutHierarchical());

                        return permissionBLL.GetNotificationPrincipalsByRoles(businessRoleIdents, groups);
                    }

                case SubscriptionSourceMode.Typed:
                    {
                        var method = new Func<TDomainObject, TDomainObject, SubscriptionSecurityItem, NotificationFilterGroup>(this.GetNotificationFilterGroup<TDomainObject, IIdentityObject<Guid>>).Method.GetGenericMethodDefinition();

                        var groups = from pair in dict

                                     let authGroup = method.MakeGenericMethod(typeof(TDomainObject), pair.Key).Invoke<NotificationFilterGroup>(this, new object[] { prev, next, pair.Value })

                                     select authGroup;

                        return permissionBLL.GetNotificationPrincipalsByRoles(businessRoleIdents, groups);
                    }

                case SubscriptionSourceMode.NonContext:
                    return this.Context.Authorization.Logics.Permission.GetNotificationPrincipalsByRoles(businessRoleIdents);

                default:
                    throw new ArgumentException("Invalid source mode", nameof(subscription));
            }
        }
    }
}
