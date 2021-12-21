using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Configuration.Domain
{
    public class SubscriptionRootFilterModel : DomainObjectMultiFilterModel<Subscription>
    {
        public TargetSystem TargetSystem { get; set; }

        public DomainType DomainType { get; set; }

        public SubscriptionLambda Lambda { get; set; }

        public MessageTemplate MessageTemplate { get; set; }


        protected override IEnumerable<System.Linq.Expressions.Expression<Func<Subscription, bool>>> ToFilterExpressionItems()
        {
            var lambda = this.Lambda;

            if (lambda != null)
            {
                yield return subscription => subscription.Condition == lambda
                                          || subscription.DynamicSource == lambda
                                          || subscription.Generation == lambda
                                          || subscription.SecurityItems.Any(si => si.Source == lambda);
            }


            var domainType = this.DomainType;

            if (domainType != null)
            {
                yield return subscription => subscription.DomainType == domainType;
            }


            var targetSystem = this.TargetSystem;

            if (targetSystem != null)
            {
                yield return subscription => subscription.TargetSystem == targetSystem;
            }

            var messageTemplate = this.MessageTemplate;

            if (messageTemplate != null)
            {
                yield return subscription => subscription.MessageTemplate == messageTemplate;
            }
        }
    }
}