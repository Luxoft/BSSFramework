using System;
using System.Linq;

using Framework.Core;
using Framework.Persistent;
using Framework.Configuration.Domain;
using Framework.DomainDriven.BLL;
using Framework.Validation;
using Framework.Exceptions;

namespace Framework.Configuration.BLL
{
    public partial class SubscriptionBLL
    {
        public bool HasActiveSubscriptions(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return this.Context.GetDomainType(type, false).Maybe(domainType => this.HasActiveSubscriptions(domainType));
        }

        public bool HasActiveSubscriptions(DomainType domainType)
        {
            if (domainType == null) throw new ArgumentNullException(nameof(domainType));

            return domainType.TargetSystem.SubscriptionEnabled && this.GetActiveSubscriptions(domainType).Any();
        }

        public IQueryable<Subscription> GetActiveSubscriptions(DomainType domainType, bool withCondition = true)
        {
            return this.GetUnsecureQueryable()
                       .Where(subscription => subscription.Active
                                           && subscription.DomainType == domainType
                                           && subscription.DomainType.TargetSystem.SubscriptionEnabled)
                       .Pipe(withCondition, q => q.Where(subscription => subscription.Condition != null));
        }

        public Subscription Create(SubscriptionCreateModel _)
        {
            return new Subscription ();
        }

        public override void Save(Subscription subscription)
        {
            if (subscription == null) throw new ArgumentNullException(nameof(subscription));

            this.ValidateBusunessRole(subscription);

            this.InitBusunessRoleNames(subscription);

            this.Validate(subscription);

            this.Recalculate(subscription);

            base.Save(subscription, false);
        }

        private void Validate(Subscription subscription)
        {
            this.Context.Validator.Validate(subscription);

            foreach (var securityItem in subscription.SecurityItems)
            {
                var entityType = this.Context.Authorization.GetEntityType(securityItem.AuthDomainTypeId);

                if (!entityType.Expandable && securityItem.ExpandType.IsHierarchical())
                {
                    throw new BusinessLogicException("Can't apply expandable mode {0} to {1}", securityItem.ExpandType, entityType.Name);
                }
            }
        }

        private void Recalculate(Subscription subscription)
        {
            if (subscription == null) throw new ArgumentNullException(nameof(subscription));

            this.InitBusunessRoleNames(subscription);
        }
    }
}