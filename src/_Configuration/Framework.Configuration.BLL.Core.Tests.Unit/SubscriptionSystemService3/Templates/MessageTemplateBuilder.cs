using Framework.Configuration.Domain;
using Framework.Notification;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Templates;

internal class MessageTemplateBuilder
{
    private readonly List<MessageTemplateNotification> templates;

    private MessageTemplateBuilder(List<MessageTemplateNotification> templates)
    {
        this.templates = templates;
    }

    public static MessageTemplateBuilder With()
    {
        return new MessageTemplateBuilder(new List<MessageTemplateNotification>());
    }

    public TemplateBuilder Template(string templateCode)
    {
        var templateBuilder = new TemplateBuilder(this);
        templateBuilder.Template(templateCode);
        return templateBuilder;
    }

    public IEnumerable<MessageTemplateNotification> Build()
    {
        return this.templates;
    }

    private MessageTemplateBuilder End(MessageTemplateNotification template)
    {
        this.templates.Add(template);
        return this;
    }

    internal class TemplateBuilder
    {
        private static readonly string DefaultContextObject = "defaultContextObject";
        private readonly MessageTemplateBuilder host;

        private string templateCode;
        private string[] toRecipients = new string[0];
        private string[] ccRecipients = new string[0];
        private string[] replayTo = new string[0];
        private System.Net.Mail.Attachment[] attachments = new System.Net.Mail.Attachment[0];
        private bool includeAttachments;
        private string subscriptionCode;
        private bool sendIndividualLetters;
        private bool allowEmptyListOfRecipients;
        private object contextObject;
        private Type razorMessageTemplateType;

        public TemplateBuilder(MessageTemplateBuilder host)
        {
            this.host = host;
        }

        public TemplateBuilder Template(string templateCode)
        {
            this.templateCode = templateCode;
            return this;
        }

        public TemplateBuilder ToRecipients(params string[] recipients)
        {
            this.toRecipients = recipients;
            return this;
        }

        public TemplateBuilder CcRecipients(params string[] recipients)
        {
            this.ccRecipients = recipients;
            return this;
        }

        public TemplateBuilder ToReplayTo(params string[] replayTo)
        {
            this.replayTo = replayTo;
            return this;
        }


        public TemplateBuilder MailAttachements(params System.Net.Mail.Attachment[] mailAttachments)
        {
            this.attachments = mailAttachments;
            return this;
        }

        public TemplateBuilder IncludeAttachments()
        {
            this.includeAttachments = true;
            return this;
        }

        public TemplateBuilder SendIndividualLetters()
        {
            this.sendIndividualLetters = true;
            return this;
        }

        public TemplateBuilder AllowEmptyListOfRecipients()
        {
            this.allowEmptyListOfRecipients = true;
            return this;
        }

        public TemplateBuilder SubscriptionCode(string code)
        {
            this.subscriptionCode = code;
            return this;
        }

        public TemplateBuilder ContextObject(object contextObject)
        {
            this.contextObject = contextObject;
            return this;
        }

        public TemplateBuilder RazorMessageTemplateType(Type razorMessageTemplateType)
        {
            this.razorMessageTemplateType = razorMessageTemplateType;
            return this;
        }

        public MessageTemplateBuilder End()
        {
            var subscription = new Subscription();
            subscription.IncludeAttachments = this.includeAttachments;
            subscription.Code = this.subscriptionCode;
            subscription.SendIndividualLetters = this.sendIndividualLetters;

            var template = new MessageTemplateNotification(
                                                           this.templateCode,
                                                           this.contextObject ?? DefaultContextObject,
                                                           this.contextObject?.GetType() ?? DefaultContextObject.GetType(),
                                                           this.toRecipients,
                                                           this.ccRecipients,
                                                           this.replayTo,
                                                           this.attachments,
                                                           subscription,
                                                           this.allowEmptyListOfRecipients);

            template.RazorMessageTemplateType = this.razorMessageTemplateType;

            return this.host.End(template);
        }
    }
}
