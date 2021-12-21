using System;
using System.Collections.Generic;
using Framework.Core;

namespace Framework.Configuration.Domain
{
    public class SubscriptionLambdaRootFilterModel : DomainObjectMultiFilterModel<SubscriptionLambda>
    {
        public TargetSystem TargetSystem { get; set; }

        public DomainType DomainType { get; set; }

        public SubscriptionLambdaType? Type { get; set; }

        public Guid AuthDomainTypeId { get; set; }


        protected override IEnumerable<System.Linq.Expressions.Expression<Func<SubscriptionLambda, bool>>> ToFilterExpressionItems()
        {
            var type = this.Type;

            if (type != null)
            {
                yield return lambda => lambda.Type == type;
            }


            var domainType = this.DomainType;

            if (domainType != null)
            {
                yield return lambda => lambda.DomainType == domainType;
            }


            var targetSystem = this.TargetSystem;

            if (targetSystem != null)
            {
                yield return lambda => lambda.TargetSystem == targetSystem;
            }

            var authDomainTypeId = this.AuthDomainTypeId;

            if (!authDomainTypeId.IsDefault())
            {
                yield return lambda => lambda.AuthDomainTypeId == authDomainTypeId;
            }
        }
    }
}
