using System;
using System.Collections.Generic;

using Framework.Notification;
using Framework.SecuritySystem;

namespace Framework.Configuration.SubscriptionModeling.Tests.Unit.Metadata;

internal sealed class TestSubscription : SubscriptionWithCustomModelMetadata<object, object, object, RazorTemplate<object>>
{
    private string senderName = "SampleSystem";
    private string senderEmail = "SampleSystem@luxoft.com";
    private LambdaMetadata<object, object, bool> conditionLambda = new ConditionLambda();
    private LambdaMetadata<object, object, IEnumerable<NotificationMessageGenerationInfo>> generationLambda = new GenerationLambda();
    private LambdaMetadata<object, object, IEnumerable<NotificationMessageGenerationInfo>> copyGenerationLambda = new CopyGenerationLambda();
    private IEnumerable<ISecurityItemSourceLambdaMetadata<object, object, ISecurityContext>> securityItemSourceLambdas = new[] { new SecurityItemSourceSourceLambda() };

    public override string SenderName => this.senderName;

    public override string SenderEmail => this.senderEmail;

    public override LambdaMetadata<object, object, bool> ConditionLambda => this.conditionLambda;

    public override LambdaMetadata<object, object, IEnumerable<NotificationMessageGenerationInfo>> GenerationLambda => this.generationLambda;

    public override LambdaMetadata<object, object, IEnumerable<NotificationMessageGenerationInfo>>
            CopyGenerationLambda => this.copyGenerationLambda;

    public override IEnumerable<ISecurityItemSourceLambdaMetadata<object, object, ISecurityContext>>
            SecurityItemSourceLambdas
    {
        get { return this.securityItemSourceLambdas; }
    }

    public override RecepientsSelectorMode RecepientsSelectorMode { get; protected set; } =
        RecepientsSelectorMode.RolesExceptGeneration;

    public override IEnumerable<Guid> SubBusinessRoleIds { get; protected set; } = new[] {BusinessRole.Administrator};

    public override bool SendIndividualLetters { get; protected set; } = true;

    public override bool ExcludeCurrentUser { get; protected set; } = true;

    public override bool IncludeAttachments { get; protected set; } = true;

    public override bool AllowEmptyListOfRecipients { get; protected set; } = true;

    internal void SetSenderName(string senderName)
    {
        this.senderName = senderName;
    }

    internal void SetSenderEmail(string senderEmail)
    {
        this.senderEmail = senderEmail;
    }

    internal void SetConditionLambda(LambdaMetadata<object, object, bool> lambda)
    {
        this.conditionLambda = lambda;
    }

    internal void SetGenerationLambda(
            LambdaMetadata<object, object, IEnumerable<NotificationMessageGenerationInfo>> lambda)
    {
        this.generationLambda = lambda;
    }

    internal void SetCopyGenerationLambda(
            LambdaMetadata<object, object, IEnumerable<NotificationMessageGenerationInfo>> lambda)
    {
        this.copyGenerationLambda = lambda;
    }

    internal void SetSecurityItemSourceLambdas(
            params ISecurityItemSourceLambdaMetadata<object, object, ISecurityContext>[] lamdas)
    {
        this.securityItemSourceLambdas = lamdas;
    }
}
