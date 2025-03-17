using System.Reflection;
using System.Runtime.Serialization;

using Framework.Core;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class TypeSerializationTests
{
    [TestMethod]
    public void GetTypesWithFieldSerialization_TypesNotExists()
    {
        // Act
        var wrongTypes = AppDomain.CurrentDomain.GetAssemblies()
                                  .SelectMany(a => a.GetTypes())
                                  .Where(t => t.GetFields().Any(f => f.HasAttribute<DataMemberAttribute>()))
                                  .ToArray();

        // Assert
        wrongTypes.Should().HaveCount(0);
    }

    [TestMethod]
    public void GetDataContractTypesWithMissedPropertyDataMemberDeclaration_TypesNotExists()
    {
        // Act
        var wrongTypes = AppDomain.CurrentDomain.GetAssemblies()
                                  .SelectMany(a => a.GetTypes())
                                  .Where(
                                      t => t.Namespace != null
                                         && (t.Namespace.StartsWith(nameof(SampleSystem)) || t.Namespace.StartsWith(nameof(Framework))))
                                  .Where(
                                      t => t.HasAttribute<DataContractAttribute>()
                                           && t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                               .Where(
                                                   prop => !prop.HasAttribute<DataMemberAttribute>()
                                                           && !prop.HasAttribute<IgnoreDataMemberAttribute>())
                                               .Select(v => v)
                                               .Any())
                                  .ToArray();

        // Assert
        wrongTypes.Should().HaveCount(0);
    }
}
