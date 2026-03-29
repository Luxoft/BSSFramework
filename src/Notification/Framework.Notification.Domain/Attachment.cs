namespace Framework.Notification.Domain;

public record Attachment(byte[] Data, string Filename)
{
    public string? ContentId { get; init; }

    public bool IsInline { get; init; }

    public override string ToString() => $"Filename = {this.Filename} (Length = {this.Data.Length})";
}
