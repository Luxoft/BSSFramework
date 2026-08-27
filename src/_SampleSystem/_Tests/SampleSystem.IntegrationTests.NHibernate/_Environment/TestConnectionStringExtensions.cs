using Anch.Testing.Database.ConnectionStringManagement;

using Framework.AutomationCore.Extensions;
using Framework.Database.Domain;

namespace SampleSystem.IntegrationTests._Environment;

public static class TestConnectionStringExtensions
{
    public static DbUserCredential? TryGetDbUserCredential(this TestConnectionString connectionString) =>
        !string.IsNullOrWhiteSpace(connectionString.UserId)
        || !string.IsNullOrWhiteSpace(connectionString.Password)
            ? new DbUserCredential(connectionString.UserId, connectionString.Password)
            : null;
}
