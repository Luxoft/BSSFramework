using System;
using System.IO;
using System.Net.Mail;
using System.Text;

using Framework.Core;
using Framework.Notification.DTO;
using Framework.Notification.New;
using Framework.Configuration.BLL.Notification;
using Framework.Configuration.Domain;
using Framework.Validation;
using Framework.Exceptions;

namespace Framework.Configuration.BLL
{
    public partial class SubscriptionBLL
    {
        public string Test(TestSubscriptionModel testModel)
        {
            if (testModel == null) throw new ArgumentNullException(nameof(testModel));

            this.Context.Validator.Validate(testModel);

            var subscription = testModel.Subscription;

            if (!subscription.TargetSystem.IsRevision) { throw new BusinessLogicException("Target system of test subscription must be revision"); }



            var targetSystemService = this.Context.GetPersistentTargetSystemService(testModel.Subscription.TargetSystem);

            var log = new StringBuilder();

            using (var writer = new StringWriter(log))
            {
                var messageSender = MessageSender<Message>.Create(writer)
                                                          .OverrideInput((NotificationEventDTO notification) => notification.ToMessage())
                                                          .ToMessageTemplateSender(this.Context, new MailAddress("log_sender@luxoft.com", "log_sender"));

                var subscriptionSystem = targetSystemService.GetSubscriptionService(messageSender);

                var result = subscriptionSystem.Process(subscription, testModel.Revision, testModel.DomainObjectId);

                var resultText = result.Match(s  => $"{subscription.ToFormattedString()} evaluated with success result",
                                              ex => $"{subscription.ToFormattedString()} evaluated with fault result: {ex.Message}");

                writer.WriteLine("-----------------------------------");
                writer.WriteLine(resultText);

                return log.ToString();
            }
        }
    }
}
