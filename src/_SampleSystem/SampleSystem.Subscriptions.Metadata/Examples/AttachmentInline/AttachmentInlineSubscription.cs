using System;
using System.Linq;

using ASP;

using SampleSystem.Subscriptions.Metadata.Employee.Update;
using SampleSystem.Subscriptions.Metadata.Examples.Attachment;

namespace SampleSystem.Subscriptions.Metadata.Examples.AttachmentInline
{
    public sealed class AttachmentInlineSubscription
        : SubscriptionMetadataBase<Domain.Employee, _Examples_AttachmentInline_MessageTemplate_cshtml>
    {
        /// <summary>
        /// Sample with inline attachment
        /// </summary>
        public AttachmentInlineSubscription()
        {
            this.SenderName = "SampleSystem";
            this.SenderEmail = "InlineAttach@luxoft.com";
            this.ConditionLambda = new ConditionLambda();
            this.GenerationLambda = new GenerationLambda();
            this.CopyGenerationLambda = new CopyGenerationLambda();
            this.SecurityItemSourceLambdas = new[] { new SecurityItemSourceLambda() };
            this.RecepientsSelectorMode = Framework.Configuration.RecepientsSelectorMode.Union;
            this.SubBusinessRoleIds = Enumerable.Empty<Guid>();
            this.IncludeAttachments = true;
            this.AttachmentLambda = new AttachmentLambda();
        }
    }
}
