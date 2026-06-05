using Anch.Testing.Database.ConnectionStringManagement;

using Framework.AutomationCore.Extensions;
using Framework.Database.NHibernate.DBGenerator;

namespace SampleSystem.IntegrationTests._Environment;

public static class TestConnectionStringExtensions
{
    public static DbUserCredential? TryGetDbUserCredential(this TestConnectionString connectionString) =>
        !string.IsNullOrWhiteSpace(connectionString.UserId)
        || !string.IsNullOrWhiteSpace(connectionString.Password)
            ? DbUserCredential.Create(
                connectionString.UserId,
                connectionString.Password)
            : null;
}

