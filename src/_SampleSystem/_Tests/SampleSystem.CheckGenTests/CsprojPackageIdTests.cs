using System.Xml.Linq;

using Xunit;

namespace SampleSystem.CheckGenTests;

public class CsprojValidationTests
{
    // =====================================================
    // 1. PackageId convention
    // =====================================================

    public static IEnumerable<object[]> GetCsprojFiles()
    {
        var root = FindSolutionRoot();

        foreach (var file in Directory.GetFiles(root, "*.csproj", SearchOption.AllDirectories))
        {
            yield return new object[] { file };
        }
    }

    [Theory]
    [MemberData(nameof(GetCsprojFiles))]
    public void PackageId_Should_Match_Convention(string file)
    {
        // Arrange
        var projectName = Path.GetFileNameWithoutExtension(file);
        var expected = $"Luxoft.{projectName}";

        XDocument doc;
        try
        {
            // Act
            doc = XDocument.Load(file);
        }
        catch (Exception ex)
        {
            throw new Xunit.Sdk.XunitException($"{file} — XML parse error: {ex.Message}");
            return;
        }

        var packageId = doc
            .Descendants()
            .FirstOrDefault(x => x.Name.LocalName == "PackageId")
            ?.Value
            ?.Trim();

        if (packageId == null)
            return;

        // Assert
        Xunit.Assert.Equal(
            expected,
            packageId);
    }

    // =====================================================
    // 2. SLNX coverage
    // =====================================================

    public static IEnumerable<object[]> GetCsprojFilesForSlnx()
    {
        var root = FindSolutionRoot();

        var slnxPath = Directory.GetFiles(root, "*.slnx", SearchOption.AllDirectories)
            .FirstOrDefault();

        if (slnxPath == null)
            throw new InvalidOperationException("No .slnx file found");

        var slnxProjects = LoadSlnxProjects(slnxPath);

        foreach (var file in Directory.GetFiles(root, "*.csproj", SearchOption.AllDirectories))
        {
            yield return new object[] { root, file, slnxProjects };
        }
    }

    [Theory]
    [MemberData(nameof(GetCsprojFilesForSlnx))]
    public void Csproj_Should_Be_Registered_In_Slnx(
        string root,
        string file,
        HashSet<string> slnxProjects)
    {
        // Arrange
        var relative = ToRelativePath(root, file);

        // Act
        var containsProject = slnxProjects.Contains(relative);

        // Assert
        Xunit.Assert.True(
            containsProject,
            $"""
            Project not found in .slnx

            Expected:
              {relative}

            Full path:
              {file}
            """);
    }

    // =====================================================
    // Helpers
    // =====================================================

    private static HashSet<string> LoadSlnxProjects(string slnxPath)
    {
        var doc = XDocument.Load(slnxPath);

        return doc
            .Descendants()
            .Where(x => x.Name.LocalName == "Project")
            .Select(x => x.Attribute("Path")?.Value)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(NormalizePath)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    private static string ToRelativePath(string root, string file)
    {
        return NormalizePath(Path.GetRelativePath(root, file));
    }

    private static string NormalizePath(string path)
    {
        return path.Replace('\\', '/').TrimStart('/');
    }

    private static string FindSolutionRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);

        while (dir != null)
        {
            if (dir.GetFiles("*.slnx").Any())
                return dir.FullName;

            dir = dir.Parent;
        }

        throw new InvalidOperationException("Solution root not found");
    }
}
