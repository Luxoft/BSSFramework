namespace Framework.Notification.Domain;

public record Attachment(byte[] Data, string Filename)
{
    public string? ContentId { get; init; }

    public bool IsInline { get; init; }

    public override string ToString() => $"Filename = {this.Filename} (Length = {this.Data.Length})";

    public System.Net.Mail.Attachment ToMailAttachment() =>
        new(new MemoryStream(this.Data), this.Filename) { ContentId = this.ContentId, ContentDisposition = { Inline = this.IsInline } };
}
