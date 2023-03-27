namespace Framework.Notification;

public struct NotificationTechnicalInformation
{
    public readonly string MessageTemplateCode;
    public readonly string ContextObjectType;
    public readonly Guid? ContextObjectId;

    public NotificationTechnicalInformation(string messageTemplateCode, string contextObjectType, Guid? contextObjectId) : this()
    {
        this.MessageTemplateCode = messageTemplateCode;
        this.ContextObjectType = contextObjectType;
        this.ContextObjectId = contextObjectId;
    }
}
