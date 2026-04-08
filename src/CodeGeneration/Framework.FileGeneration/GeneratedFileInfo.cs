using System.Text;

using Framework.FileGeneration.Checkout;

namespace Framework.FileGeneration;

public record GeneratedFileInfo(string RelativePath, string Content)
{
    public string? PrevContent { get; init; }

    public string? AbsolutePath { get; init; }

    public State FileState { get; init; } = State.Unknown;

    public GeneratedFileInfo WithSave(string path, ICheckOutService? checkOutService = null, Encoding? encoding = null)
    {
        var usingEncoding = encoding ?? Encoding.UTF8;

        var absolutePath = Path.Combine(path, this.RelativePath);

        var directoryPath = Path.GetDirectoryName(absolutePath)!;

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        State state;
        string? prevContent;

        if (File.Exists(absolutePath))
        {
            prevContent = File.ReadAllText(absolutePath, usingEncoding);

            if (prevContent.Replace("\r\n", "\n") != this.Content.Replace("\r\n", "\n"))
            {
                state = State.Modified;
                this.InternalSave(absolutePath, checkOutService, usingEncoding);
            }
            else
            {
                state = State.NotModified;
            }
        }
        else
        {
            prevContent = null;
            state = State.New;

            this.InternalSave(absolutePath, checkOutService, usingEncoding);
        }

        return this with { AbsolutePath = absolutePath, FileState = state, PrevContent = prevContent };
    }

    private void InternalSave(string absolutePath, ICheckOutService? checkOutService, Encoding encoding)
    {
        checkOutService?.CheckOutFile(absolutePath);

        File.WriteAllText(absolutePath, this.Content, encoding);
    }

    public enum State : byte
    {
        Unknown = 0,
        NotModified,
        New,
        Modified
    }
}
