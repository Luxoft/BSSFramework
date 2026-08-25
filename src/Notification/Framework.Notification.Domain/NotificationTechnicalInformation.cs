namespace Framework.Notification.Domain;

public readonly record struct NotificationTechnicalInformation(string MessageTemplateCode, string? ContextObjectType, Guid? ContextObjectId)
{
    public string Comment { get; init; } = "";
}
