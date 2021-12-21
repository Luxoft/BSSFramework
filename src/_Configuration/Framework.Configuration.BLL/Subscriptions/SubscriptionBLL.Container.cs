using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Configuration.Domain;
using Framework.DomainDriven.BLL;
using Framework.Validation;

using JetBrains.Annotations;

namespace Framework.Configuration.BLL
{
    public partial class SubscriptionBLL
    {
        public SubscriptionContainer GetSubscriptionContainer([NotNull] IEnumerable<Subscription> preSubscriptions)
        {
            if (preSubscriptions == null) throw new ArgumentNullException(nameof(preSubscriptions));

            var subscriptions = preSubscriptions.ToList();

            var lambdas = subscriptions.SelectMany(s => s.GetLambdas()).Distinct().ToList();

            var messageTemplates = subscriptions.Select(s => s.MessageTemplate).Distinct().ToList();

            var attachmentContainersRequest = from messageTemplate in messageTemplates

                                              let attachmentContainer = this.Context.Logics.AttachmentContainer.GetObjectBy(messageTemplate)

                                              where attachmentContainer != null

                                              select attachmentContainer;

            return new SubscriptionContainer
            {
                Subscriptions = subscriptions,

                Lambdas = lambdas,

                MessageTemplates = messageTemplates,

                AttachmentContainers = attachmentContainersRequest.ToList()
            };
        }

        public void Import([NotNull] SubscriptionContainer subscriptionContainer)
        {
            if (subscriptionContainer == null) throw new ArgumentNullException(nameof(subscriptionContainer));

            this.Context.Validator.Validate(subscriptionContainer);

            this.Context.Logics.MessageTemplate.Insert(subscriptionContainer.MessageTemplates);

            this.Context.Logics.AttachmentContainer.Insert(subscriptionContainer.AttachmentContainers);

            this.Context.Logics.SubscriptionLambda.Insert(subscriptionContainer.Lambdas);

            this.Insert(subscriptionContainer.Subscriptions);
        }

        public override void Insert([NotNull] Subscription subscription, Guid id)
        {
            if (subscription == null) throw new ArgumentNullException(nameof(subscription));

            this.InsertWithoutCascade(subscription, id);

            this.Context.Logics.Default.Create<SubBusinessRole>().Insert(subscription.SubBusinessRoles);
            this.Context.Logics.Default.Create<SubscriptionSecurityItem>().Insert(subscription.SecurityItems);

            base.Insert(subscription, id);
        }
    }
}
