#pragma warning disable SA1600 // ElementsMustBeDocumented. Internal type does not require inline documentation by convention.
using System.Collections.Generic;
using System.Linq;
using Framework.Authorization.BLL;
using Framework.Configuration.BLL.SubscriptionSystemService3.Lambdas;
using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL.Security;
using Framework.Persistent;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Recipients
{
    internal sealed class ByRolesRecipientsResolverDynamic<TBLLContext> : ByRolesRecipientsResolverBase<TBLLContext>
        where TBLLContext : class
    {
        public ByRolesRecipientsResolverDynamic(
            ConfigurationContextFacade configurationContextFacade,
            LambdaProcessorFactory<TBLLContext> lambdaProcessorFactory)
            : base(configurationContextFacade, lambdaProcessorFactory)
        {
        }

        internal override RecipientCollection Resolve<T>(Subscription subscription, DomainObjectVersions<T> versions)
        {
            if (subscription.SourceMode != SubscriptionSourceMode.Dynamic)
            {
                return new RecipientCollection();
            }

            var businessRolesIds = this.GetBusinessRolesIds(subscription);
            var fids = this.GetFilterItemIdentities(subscription, versions).ToList();
            var filterGroups = this.GetNotificationFilterGroups(fids, subscription.DynamicSourceExpandType.Value);

            var principals = this.ConfigurationContextFacade.GetNotificationPrincipals(businessRolesIds, filterGroups);
            var employees = this.ConfigurationContextFacade.ConvertPrincipals(principals);
            var recipients = employees.Select(this.CreateRecipient);

            return new RecipientCollection(recipients);
        }

        private IEnumerable<FilterItemIdentity> GetFilterItemIdentities<T>(
            Subscription subscription,
            DomainObjectVersions<T> versions)
            where T : class
        {
            var processor = this.LambdaProcessorFactory.Create<DynamicSourceLambdaProcessor<TBLLContext>>();
            var result = processor.Invoke(subscription, versions);

            return result;
        }

        private IEnumerable<NotificationFilterGroup> GetNotificationFilterGroups(
            IEnumerable<FilterItemIdentity> fids,
            NotificationExpandType expandType)
        {
            var result =
                from item in fids.GroupBy(fid => fid.EntityName)
                let entityType = this.ConfigurationContextFacade.GetEntityType(item.Key.ToLowerInvariant())
                let securityType = this.ConfigurationContextFacade.GetSecurityType(entityType)
                let ids = item.Select(i => i.Id)
                let et = entityType.Expandable ? expandType : expandType.WithoutHierarchical()
                select new NotificationFilterGroup(securityType, ids, et);

            return result;
        }
    }
}
#pragma warning restore SA1600 // ElementsMustBeDocumented
