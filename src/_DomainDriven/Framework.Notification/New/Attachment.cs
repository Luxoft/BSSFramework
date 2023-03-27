namespace Framework.Notification.New;

public class Attachment
{
    public Attachment(byte[] data, string filename)
    {
        this.Data = data ?? throw new ArgumentNullException(nameof(data));
        this.Filename = filename ?? throw new ArgumentNullException(nameof(filename));
    }

    public byte[] Data { get; }

    public string Filename { get; }

    public string ContentId { get; set; }

    public bool IsInline { get; set; }

    public override string ToString()
    {
        return $"Filename = {this.Filename} (Length = {this.Data.Length})";
    }
}
