
using System.Xml.Linq;

namespace SampleSystem.CheckGenTests;

[TestClass]
public class CsprojPackageIdTests
{
    public static IEnumerable<object[]> GetCsprojFiles()
    {
        var root = FindSolutionRoot();

        foreach (var file in Directory.GetFiles(root, "*.csproj", SearchOption.AllDirectories))
        {
            yield return new object[] { file };
        }
    }

    [TestMethod]
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
                        .Where(x => x.Name.LocalName == "PackageId")
                        .Select(x => x.Value.Trim())
                        .FirstOrDefault();

        if (packageId == null)
        {
            // SKIP логики в MSTest нет — считаем валидным
            return;
        }

        Assert.AreEqual(
            expected,
            packageId,
            $"{file}\nExpected: {expected}\nActual:   {packageId}");
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
