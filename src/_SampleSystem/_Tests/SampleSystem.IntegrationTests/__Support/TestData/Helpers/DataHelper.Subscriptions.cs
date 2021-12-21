using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Authorization.Domain;
using Framework.Configuration.BLL;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.DAL.Revisions;

namespace SampleSystem.IntegrationTests.__Support.TestData.Helpers
{
    public partial class DataHelper
    {
        public Subscription SaveSubscription(
            string code,
            bool active,
            DomainType domainType,
            MessageTemplate messageTemplate,
            SubscriptionLambda conditionLambda,
            SubscriptionLambda generationLambda,
            SubscriptionLambda copyGenerationLambda = null)
        {
            var result = this.EvaluateWrite(context =>
                                            {
                                                var subscription = new Subscription();
                                                subscription.Code = code;
                                                subscription.Active = active;
                                                subscription.DomainType = domainType;
                                                subscription.MessageTemplate = messageTemplate;
                                                subscription.Condition = conditionLambda;
                                                subscription.Generation = generationLambda;
                                                subscription.CopyGeneration = copyGenerationLambda;

                                                var bll = context.Configuration.Logics.SubscriptionFactory.Create();

                                                if (!bll.GetUnsecureQueryable().Any(s => s.Code == code))
                                                {
                                                    bll.SaveOrInsert(subscription);
                                                }

                                                return subscription;
                                            });

            return result;
        }

        public MessageTemplate SaveMessageTemplate(string messageTemplateCode, string messageBody = "message body")
        {
            var result = this.EvaluateWrite(context =>
            {
                var messageTemplate = new MessageTemplate
                {
                    Code = messageTemplateCode,
                    Message = messageBody
                };

                var bll = context.Configuration.Logics.MessageTemplateFactory.Create();

                var existing = bll.GetObjectBy(s => s.Code == messageTemplateCode);

                if (existing != null)
                {
                    existing.Code = messageTemplateCode;
                    existing.Message = messageBody;
                }

                bll.SaveOrInsert(existing ?? messageTemplate);

                return existing ?? messageTemplate;
            });

            return result;
        }

        public SubscriptionLambda SaveSubscriptionLambda(
            string name,
            SubscriptionLambdaType subscriptionLambdaType,
            DomainType domainType,
            string value,
            bool withContext,
            Guid? authDomainTypeId = null)
        {
            var result = this.EvaluateWrite(context =>
            {
                var lambda = new SubscriptionLambda();
                lambda.Name = name;
                lambda.Type = subscriptionLambdaType;
                lambda.DomainType = domainType;
                lambda.Value = value;
                lambda.WithContext = withContext;
                lambda.AuthDomainTypeId = authDomainTypeId ?? Guid.Empty;

                var bll = context.Configuration.Logics.SubscriptionLambdaFactory.Create();
                var existing = bll.GetObjectBy(l => l.Name == name);

                if (existing != null)
                {
                    existing.Name = name;
                    existing.Type = subscriptionLambdaType;
                    existing.DomainType = domainType;
                    existing.Value = value;
                    existing.WithContext = withContext;
                }

                bll.SaveOrInsert(existing ?? lambda);

                return existing ?? lambda;
            });

            return result;
        }

        public AttachmentContainer SaveAttachmentContainer(Guid objectId, DomainType objectType)
        {
            return this.EvaluateWrite(context =>
            {
                var attachmentContainer = new AttachmentContainer
                {
                    DomainType = objectType,
                    ObjectId = objectId
                };

                var bll = context.Configuration.Logics.AttachmentContainerFactory.Create();
                var existing = bll.GetObjectBy(x => x.ObjectId == objectId && x.DomainType == objectType);

                bll.SaveOrInsert(existing ?? attachmentContainer);

                return existing ?? attachmentContainer;
            });
        }

        public Attachment SaveAttachment(
            AttachmentContainer attachmentContainer,
            string name,
            byte[] content)
        {
            return this.EvaluateWrite(context =>
            {
                var attachment = new Attachment(attachmentContainer)
                {
                    Name = name,
                    Content = content
                };

                var bll = context.Configuration.Logics.AttachmentFactory.Create();
                var existing = bll.GetObjectBy(x => x.Name == name);

                if (existing != null)
                {
                    existing.Content = content;
                }

                bll.SaveOrInsert(existing ?? attachment);

                return existing ?? attachment;
            });
        }

        public DomainType GetDomainType(Type domainObjectType)
        {
            return this.EvaluateRead(context =>
                                      {
                                          var bll = context.Configuration.Logics.DomainTypeFactory.Create();
                                          var result = bll.GetByType(domainObjectType);
                                          return result;
                                      });
        }

        public EntityType GetEntityType(Type domainObjectType)
        {
            return this.EvaluateRead(context =>
                                     {
                                         var bll = context.Authorization.Logics.EntityType;
                                         var result = bll.GetObjectBy(x => x.Name == domainObjectType.Name);
                                         return result;
                                     });
        }

        public SubscriptionRecipientInfo GetRecipientsUntyped(
            Type domainObjectType,
            object prev,
            object next,
            string subscriptionCode)
        {
            return this.EvaluateRead(context =>
            {
                var bll = context.Configuration.Logics.SubscriptionFactory.Create();

                var result = bll.GetRecipientsUntyped(
                    domainObjectType,
                    prev,
                    next,
                    subscriptionCode);

                return result;
            });
        }

        public IList<ITryResult<Subscription>> ProcessChangedObjectUntyped(
            Type domainObjectType,
            object prev,
            object next)
        {
            return this.EvaluateWrite(context =>
            {
                var bll = context.Configuration.Logics.SubscriptionFactory.Create();

                var result = bll.ProcessChangedObjectUntyped(
                    prev,
                    next,
                    domainObjectType);

                return result;
            });
        }

        public IList<ITryResult<Subscription>> ProcessChangedObjectInfo(ObjectModificationInfo<Guid> changedObjectInfo)
        {
            return this.EvaluateWrite(context =>
            {
                var bll = context.Configuration.Logics.SubscriptionFactory.Create();
                var result = bll.Process(changedObjectInfo);
                return result;
            });
        }

        public SubscriptionSecurityItem SaveSecurityItem(Subscription subscription, DomainType domainType, Type authDomainType, string lambda)
        {
            var entityType = this.GetEntityType(authDomainType);
            var source = this.SaveSubscriptionLambda("security", SubscriptionLambdaType.AuthSource, domainType, lambda, false, entityType.Id);
            return this.EvaluateWrite(context =>
            {
                var securityItem = new SubscriptionSecurityItem(subscription)
                {
                    AuthDomainTypeId = entityType.Id,
                    Source = source
                };

                context.Configuration.Logics.SubscriptionFactory.Create().Save(subscription);

                return securityItem;
            });
        }
    }
}
