namespace Automation.Extensions;

public static class StreamExtensions
{
    public static void Save(this Stream stream, string folder, string fileName = null, string extension = "xlsx")
    {
        fileName = fileName != null ? $"{fileName}.{extension}" : $"report_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.{extension}";
        var filePath = Path.Combine(folder, fileName);

        using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        stream.CopyTo(fileStream);
        stream.Close();
        fileStream.Close();
    }
}
