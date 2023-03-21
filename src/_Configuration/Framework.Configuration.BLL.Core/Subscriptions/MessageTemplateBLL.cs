using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.Configuration.SubscriptionModeling;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.DAL.Revisions;
using Framework.Notification;

using JetBrains.Annotations;

using MAttachment = System.Net.Mail.Attachment;

namespace Framework.Configuration.BLL;

public class MessageTemplateBLL : BLLContextContainer<IConfigurationBLLContext>
{
    public MessageTemplateBLL(IConfigurationBLLContext context)
            : base(context)
    {
    }

    public MailMessage CreateMailMessage(
            MessageTemplate messageTemplate,
            bool includeAttachments,
            object rootObject,
            MailAddress sender,
            IEnumerable<string> targetEmails,
            IEnumerable<string> carbonCopyEmails,
            IEnumerable<string> replyTo)
    {
        return this.CreateMailMessage(
                                      includeAttachments,
                                      messageTemplate,
                                      rootObject,
                                      new Dictionary<string, object>(),
                                      sender,
                                      targetEmails.ToList(z => new TargetEmail(z)),
                                      carbonCopyEmails.ToList(z => new TargetEmail(z, TargetEmailType.Copy)),
                                      replyTo.ToList(z => new TargetEmail(z, TargetEmailType.ReplyTo)),
                                      new List<MAttachment>(0));
    }

    /// <inheritdoc />
    public MailMessage CreateMailMessage(
            MessageTemplateNotification messageTemplateNotification,
            MessageTemplate messageTemplate,
            bool includeAttachments,
            object rootObject,
            MailAddress sender,
            IEnumerable<string> targetEmails,
            IEnumerable<string> carbonCopyEmails,
            IEnumerable<string> replyTo,
            IEnumerable<MAttachment> attachments)
    {
        return this.BeginCreateMailMessage(
                                           includeAttachments,
                                           messageTemplateNotification,
                                           messageTemplate,
                                           rootObject,
                                           new Dictionary<string, object>(),
                                           sender,
                                           targetEmails.ToList(z => new TargetEmail(z)),
                                           carbonCopyEmails.ToList(z => new TargetEmail(z, TargetEmailType.Copy)),
                                           replyTo.ToList(z => new TargetEmail(z, TargetEmailType.ReplyTo)),
                                           attachments);
    }

    /// <summary>
    /// Creates e-mail message
    /// </summary>
    /// <param name="includeAttachments">Whether to include attachement into message</param>
    /// <param name="messageTemplate">Message template code</param>
    /// <param name="rootObject">Context object</param>
    /// <param name="variables">Variables</param>
    /// <param name="sender">E-mail sender</param>
    /// <param name="recipients">Recipients list</param>
    /// <param name="copyRecipients">Copy recipients (CC) list</param>
    /// <param name="replyTo">The reply to.</param>
    /// <param name="attachments">A list of attachments</param>
    /// <returns>MailMessage.</returns>
    public MailMessage CreateMailMessage(
            bool includeAttachments,
            MessageTemplate messageTemplate,
            object rootObject,
            Dictionary<string, object> variables,
            MailAddress sender,
            IList<TargetEmail> recipients,
            IList<TargetEmail> copyRecipients,
            IList<TargetEmail> replyTo,
            IEnumerable<MAttachment> attachments)
    {
        var result = this.BeginCreateMailMessage(
                                                 includeAttachments,
                                                 null,
                                                 messageTemplate,
                                                 rootObject,
                                                 variables,
                                                 sender,
                                                 recipients,
                                                 copyRecipients,
                                                 replyTo,
                                                 attachments);

        return result;
    }

    /// <summary>
    /// Creates e-mail message
    /// </summary>
    /// <param name="subject">E-mail subject</param>
    /// <param name="body">E-mail body text</param>
    /// <param name="sender">E-mail sender</param>
    /// <param name="recipients">Recipients list</param>
    /// <param name="copyRecipients">Carbon copy recipients (CC) list</param>
    /// <param name="attachments">A list of attachments</param>
    /// <returns>E-mail message according to input params</returns>
    public MailMessage CreateMailMessage(
            string subject,
            string body,
            MailAddress sender,
            IList<TargetEmail> recipients,
            IList<TargetEmail> copyRecipients,
            IList<TargetEmail> replyTo,
            IEnumerable<MAttachment> attachments)
    {
        var result = new MailMessage
                     {
                             Subject = subject,
                             Body = body,
                             From = sender
                     };

        result.IsBodyHtml = result.Body.Contains("<html");
        this.InsertTargetAdresses(recipients, z => result.To.Add(z), TargetEmailType.To);
        this.InsertTargetAdresses(copyRecipients, z => result.CC.Add(z), TargetEmailType.Copy);
        this.InsertTargetAdresses(replyTo, z => result.ReplyToList.Add(z), TargetEmailType.ReplyTo);

        if (null == attachments)
        {
            return result;
        }

        foreach (var currentAttachment in attachments)
        {
            AddAttachment(result, currentAttachment);
        }

        return result;
    }

    private static void AddAttachment(MailMessage message, MAttachment attachment)
    {
        message.Attachments.Add(attachment);

        if (message.IsBodyHtml)
        {
            message.Body = ReplaceSrcByName(message.Body, attachment.Name, attachment.ContentId);
        }
    }

    private static string ReplaceSrcByName(string body, string name, string contentId)
    {
        var pattern = $"src\\s*=\\s*\"{name}\"";

        return Regex.Replace(body, pattern, $"src=\"cid:{contentId}\"", RegexOptions.IgnoreCase);
    }

    private static string ReplaceSemiToComma(string str)
    {
        var builder = new StringBuilder(str);

        return builder.Replace(';', ',').ToString();
    }

    private void InsertTargetAdresses(
            IEnumerable<TargetEmail> targetAddressList,
            Action<string> insertToAddressesAction,
            TargetEmailType targetEmailType)
    {
        foreach (var address in targetAddressList
                                .Where(z => z.TargetEmailType == targetEmailType)
                                .Where(z => !string.IsNullOrWhiteSpace(z.Email))
                                .Select(z => z.Email)
                                .Distinct())
        {
            insertToAddressesAction(ReplaceSemiToComma(address));
        }
    }

    private MailMessage BeginCreateMailMessage(
            bool includeAttachments,
            MessageTemplateNotification messageTemplateNotification,
            MessageTemplate messageTemplate,
            object rootObject,
            Dictionary<string, object> variables,
            MailAddress sender,
            IList<TargetEmail> recipients,
            IList<TargetEmail> copyRecipients,
            IList<TargetEmail> replyTo,
            IEnumerable<MAttachment> attachments)
    {
        var subjectAndBody = this.GetMailMessageSubjectAndBody(
                                                               messageTemplateNotification,
                                                               messageTemplate,
                                                               rootObject,
                                                               variables);

        var subject = subjectAndBody.Item1;
        var body = subjectAndBody.Item2;

        var resultAttachments = new List<MAttachment>();

        if (includeAttachments)
        {
            if (attachments != null)
            {
                resultAttachments.AddRange(attachments);
            }

            IAttachmentContainer attachmentContainer = null;

            if (rootObject is IAttachmentContainer)
            {
                attachmentContainer = rootObject as IAttachmentContainer;
            }
            else if (rootObject is ObjectsVersion)
            {
                var next = ((ObjectsVersion)rootObject).Current;
                var container = next as IAttachmentContainer;
                if (container != null)
                {
                    attachmentContainer = container;
                }
            }

            if (attachmentContainer?.Attachments != null)
            {
                resultAttachments.AddRange(
                                           attachmentContainer.Attachments.Select(
                                                                                  dto => new MAttachment(new MemoryStream(dto.Content), dto.Name)));
            }
        }

        var result = this.CreateMailMessage(subject, body, sender, recipients, copyRecipients, replyTo, resultAttachments);

        return result;
    }

    private Tuple<string, string> GetMailMessageSubjectAndBody(
            MessageTemplateNotification messageTemplateNotification,
            MessageTemplate messageTemplate,
            object rootObject,
            Dictionary<string, object> variables)
    {
        if (messageTemplateNotification?.RazorMessageTemplateType == null)
        {
            throw null;

            //var evaluator = this.Context.TemplateEvaluatorFactory.Create<string>(messageTemplate);
            //var subject = evaluator.Evaluate(messageTemplate.Subject, rootObject, variables).SubStringUnsafe(500);
            //var body = evaluator.Evaluate(messageTemplate.Message, rootObject, variables);
            //return new Tuple<string, string>(subject, body);
        }

        var method = this.GetType()
                         .GetMethod(nameof(this.GetRazorTemplate), BindingFlags.Instance | BindingFlags.NonPublic)
                         .MakeGenericMethod(messageTemplateNotification.SourceContextObjectType, messageTemplateNotification.ContextObjectType);

        var razorTemplate = (IRazorTemplate)method.Invoke(this, new object[] { messageTemplateNotification });
        var sb = new StringBuilder();
        razorTemplate.SetWriter(new StringWriter(sb));
        razorTemplate.Execute();
        var result = new Tuple<string, string>(razorTemplate.Subject, sb.ToString());

        return result;
    }

    [UsedImplicitly]
    private IRazorTemplate GetRazorTemplate<TSourceDomainObjectType, TModelObjectType>(MessageTemplateNotification messageTemplate)
            where TModelObjectType : class
    {
        if (messageTemplate == null)
        {
            throw new ArgumentNullException(nameof(messageTemplate));
        }

        if (!typeof(RazorTemplate<TModelObjectType>).IsAssignableFrom(messageTemplate.RazorMessageTemplateType))
        {
            throw new InvalidOperationException($"Wrong type of RazorTemplate instance. Required type '{typeof(RazorTemplate<TModelObjectType>)}' but actual type {messageTemplate.RazorMessageTemplateType}");
        }

        var versions = messageTemplate.ContextObject as DomainObjectVersions<TModelObjectType>;

        if (versions == null)
        {
            throw new InvalidOperationException($"Wrong type of DomainObjectVersions instance. Required type '{typeof(DomainObjectVersions<TModelObjectType>)}' but actual type {messageTemplate.ContextObject.GetType()}");
        }

        var viewTemplate = (RazorTemplate<TModelObjectType>)Activator.CreateInstance(messageTemplate.RazorMessageTemplateType);
        viewTemplate.Previous = versions.Previous;
        viewTemplate.Current = versions.Current;
        viewTemplate.Context = this.Context.GetTargetSystemService(typeof(TModelObjectType), false)?.TargetSystemContext
                               ?? this.Context.GetTargetSystemService(typeof(TSourceDomainObjectType), true).TargetSystemContext;

        return viewTemplate;
    }
}
