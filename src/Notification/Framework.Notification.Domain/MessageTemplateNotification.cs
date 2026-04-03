using System.Collections.Immutable;

namespace Framework.Notification.Domain;

public record MessageTemplateNotification
{
    public required ImmutableArray<string> CopyReceivers { get; init; } = [];

    public required ImmutableArray<string> ReplyTo { get; init; } = [];

    public required string MessageTemplateCode { get; init; }

    public required Type ContextObjectType { get; init; }

    public required Type SourceContextObjectType { get; init; }

    public required object ContextObject { get; init; }

    public required ImmutableArray<string> Receivers { get; init; }

    public required ImmutableArray<System.Net.Mail.Attachment> Attachments { get; init; }

    public required ISubscription? Subscription { get; init; }

    public required bool SendWithEmptyListOfRecipients { get; init; }

    public required Type? RazorMessageTemplateType { get; init; }

    public override string ToString() => this.MessageTemplateCode;
}
