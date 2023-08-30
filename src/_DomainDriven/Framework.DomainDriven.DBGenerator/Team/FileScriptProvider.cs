using System.Text.RegularExpressions;

using Framework.DomainDriven.DBGenerator.Contracts;

namespace Framework.DomainDriven.DBGenerator.Team;

public class FileScriptReader : IMigrationScriptReader
{
    private readonly string _directoryPath;

    public FileScriptReader(string directoryPath)
    {
        if (directoryPath == null)
        {
            throw new ArgumentNullException(nameof(directoryPath));
        }

        this._directoryPath = directoryPath;
    }

    public IEnumerable<MigrationDbScript> Read()
    {
        return this.Read(this._directoryPath);
    }

    private IEnumerable<MigrationDbScript> Read(string folderPath)
    {
        var directories = Directory.GetDirectories(folderPath);
        var orderedDirectories = directories.OrderBy(s => s).ToList();

        IEnumerable<MigrationDbScript> results = new MigrationDbScript[0];

        if (orderedDirectories.Any())
        {
            results = results.Concat(orderedDirectories.SelectMany(this.Read));
        }

        results = results.Concat(Directory.GetFiles(folderPath, "*.sql", SearchOption.TopDirectoryOnly)
                                          .OrderBy(s => s)
                                          .Select(this.ReadScript));

        return results;
    }

    private MigrationDbScript ReadScript(string filePath)
    {
        var allLines = File.ReadAllLines(filePath).ToList();

        var fileName = Path.GetFileNameWithoutExtension(filePath);

        var commandLines = allLines.TakeWhile(z => z.StartsWith("--")).ToList();

        allLines.Insert(0, Environment.NewLine);
        allLines.Insert(0, Environment.NewLine);
        allLines.Insert(0, $"-- START FILE {filePath}");
        allLines.Add($"-- END FILE {filePath}");
        allLines.Add(Environment.NewLine);

        var body = string.Join(Environment.NewLine, allLines);

        var runalways = this.GetValue(commandLines, "runalways", bool.Parse);

        var fileversion = this.GetValue(commandLines, "fileversion", s => s);

        var scheme = this.GetValue(commandLines, "scheme", s => s);

        var applyMode = this.GetValue(commandLines, "applyMode", s => (ApplyMigrationDbScriptMode)Enum.Parse(typeof(ApplyMigrationDbScriptMode), s));

        return new MigrationDbScript(fileName, runalways, applyMode, scheme, fileversion, body);
    }

    private T GetValue<T>(IEnumerable<string> commands, string param, Func<string, T> convert)
    {
        if (commands == null)
        {
            throw new ArgumentNullException(nameof(commands));
        }

        var regex = new Regex(@"--" + param + @"\s*=\s*(.*)", RegexOptions.IgnoreCase);

        var command = commands.SingleOrDefault(regex.IsMatch);
        try
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                throw new InvalidOperationException($"Parameter '{param}' not found!");
            }

            return convert(regex.Match(command).Groups[1].Value);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Incorrect command: {command}", ex);
        }
    }
}
