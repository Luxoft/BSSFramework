using System;

using Framework.Configuration.BLL;
using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DTO;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.SecuritySystem;

namespace Framework.Configuration.WebApi
{
    public partial class ConfigSLJsonController
    {
        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(TestSubscription))]
        public string TestSubscription(TestSubscriptionModelStrictDTO testSubscription)
        {
            if (testSubscription == null) throw new ArgumentNullException(nameof(testSubscription));

            return this.Evaluate(DBSessionMode.Write, evaluateData =>
            {
                evaluateData.Context.Authorization.CheckAccess(ConfigurationSecurityOperation.SubscriptionTest);

                return evaluateData.Context.Logics.Subscription.Test(testSubscription.ToDomainObject(evaluateData.MappingService));
            });
        }

        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(ExportSubscriptions))]
        public SubscriptionContainerRichDTO ExportSubscriptions(SubscriptionIdentityDTO[] idents)
        {
            if (idents == null) throw new ArgumentNullException(nameof(idents));

            return this.Evaluate(DBSessionMode.Read, evaluateData =>
            {
                var bll = evaluateData.Context.Logics.SubscriptionFactory.Create(BLLSecurityMode.View);

                var subscriptions = bll.GetObjectsByIdents(idents);

                var subscriptionContainer = bll.GetSubscriptionContainer(subscriptions);

                return subscriptionContainer.ToRichDTO(evaluateData.MappingService);
            });
        }

        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(ImportSubscriptions))]
        public void ImportSubscriptions(ImportSubscriptionsRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            this.EvaluateC(DBSessionMode.Write, context =>
            {
                var bll = context.Logics.SubscriptionFactory.Create(BLLSecurityMode.Edit);

                var subscriptionContainer = new SubscriptionContainer();

                new SubscriptionContainerDTOMappingService(context, request.IgnoreCollision).Map(request.Container, subscriptionContainer);

                bll.Import(subscriptionContainer);
            });
        }
    }
}
