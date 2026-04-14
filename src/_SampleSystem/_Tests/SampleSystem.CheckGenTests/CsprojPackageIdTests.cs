using System.Xml.Linq;

namespace SampleSystem.CheckGenTests;

[TestClass]
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

    [DataTestMethod]
    [DynamicData(nameof(GetCsprojFiles), DynamicDataSourceType.Method)]
    public void PackageId_Should_Match_Convention(string file)
    {
        var projectName = Path.GetFileNameWithoutExtension(file);
        var expected = $"Luxoft.{projectName}";

        XDocument doc;
        try
        {
            doc = XDocument.Load(file);
        }
        catch (Exception ex)
        {
            Assert.Fail($"{file} — XML parse error: {ex.Message}");
            return;
        }

        var packageId = doc
            .Descendants()
            .FirstOrDefault(x => x.Name.LocalName == "PackageId")
            ?.Value
            ?.Trim();

        if (packageId == null)
            return;

        Assert.AreEqual(
            expected,
            packageId,
            $"{file}\nExpected: {expected}\nActual:   {packageId}");
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

    [DataTestMethod]
    [DynamicData(nameof(GetCsprojFilesForSlnx), DynamicDataSourceType.Method)]
    public void Csproj_Should_Be_Registered_In_Slnx(
        string root,
        string file,
        HashSet<string> slnxProjects)
    {
        var relative = ToRelativePath(root, file);

        Assert.IsTrue(
            slnxProjects.Contains(relative),
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
