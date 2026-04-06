namespace Framework.Configuration.SubscriptionModeling.Tests.Unit.Metadata;

internal sealed class TestSubscription : SubscriptionWithCustomModelMetadata<object, object, object, RazorTemplate<object>>
{
    private LambdaMetadata<object, object, bool> conditionLambda = new ConditionLambda();
    private LambdaMetadata<object, object, IEnumerable<NotificationMessageGenerationInfo>> generationLambda = new GenerationLambda();
    private LambdaMetadata<object, object, IEnumerable<NotificationMessageGenerationInfo>> copyGenerationLambda = new CopyGenerationLambda();
    private IEnumerable<ISecurityItemSourceLambdaMetadata<object, object, ISecurityContext>> securityItemSourceLambdas = [new SecurityItemSourceSourceLambda()];

    public TestSubscription()
    {
        this.SenderName = "SampleSystem";
        this.SenderEmail = "SampleSystem@luxoft.com";
    }

    public override LambdaMetadata<object, object, bool> ConditionLambda => this.conditionLambda;

    public override LambdaMetadata<object, object, IEnumerable<NotificationMessageGenerationInfo>> GenerationLambda => this.generationLambda;

    public override LambdaMetadata<object, object, IEnumerable<NotificationMessageGenerationInfo>>
            CopyGenerationLambda => this.copyGenerationLambda;

    public override IEnumerable<ISecurityItemSourceLambdaMetadata<object, object, ISecurityContext>>
            SecurityItemSourceLambdas =>
        this.securityItemSourceLambdas;

    public override RecipientMergeType RecipientMergeType { get; protected set; } =
        RecipientMergeType.RolesExceptGeneration;

    public override IEnumerable<SecurityRole> SubBusinessRoles { get; protected set; } = [SecurityRole.Administrator];

    public override bool SendIndividualLetters { get; protected set; } = true;

    public override bool ExcludeCurrentUser { get; protected set; } = true;

    public override bool IncludeAttachments { get; protected set; } = true;

    public override bool AllowEmptyListOfRecipients { get; protected set; } = true;

    internal void SetConditionLambda(LambdaMetadata<object, object, bool> lambda) => this.conditionLambda = lambda;

    internal void SetGenerationLambda(
            LambdaMetadata<object, object, IEnumerable<NotificationMessageGenerationInfo>> lambda) =>
        this.generationLambda = lambda;

    internal void SetCopyGenerationLambda(
            LambdaMetadata<object, object, IEnumerable<NotificationMessageGenerationInfo>> lambda) =>
        this.copyGenerationLambda = lambda;

    internal void SetSecurityItemSourceLambdas(
            params ISecurityItemSourceLambdaMetadata<object, object, ISecurityContext>[] lamdas) =>
        this.securityItemSourceLambdas = lamdas;
}
