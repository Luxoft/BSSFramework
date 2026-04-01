using Framework.Database.Metadata;
using Framework.Database.NHibernate.DBGenerator.Contracts;
using Framework.Database.NHibernate.DBGenerator.ScriptGenerators.Support;

using Microsoft.SqlServer.Management.Smo;

using NSubstitute;

namespace Framework.Database.NHibernate.DBGenerator.Tests.Support;

public class DatabaseScriptGeneratorContextMockBuilder
{
    public DatabaseScriptGeneratorContextMockBuilder()
    {
        this.MainDaraBase = new Microsoft.SqlServer.Management.Smo.Database();
        this.MainServer = new Server();

        this.DatabaseScriptGeneratorContext = Substitute.For<IDatabaseScriptGeneratorContext>();

        this.SqlDatabaseFactory = Substitute.For<ISqlDatabaseFactory>();

        this.SqlDatabaseFactory.Server.Returns(this.MainServer);
        this.SqlDatabaseFactory.GetDatabase(Arg.Any<DatabaseName>()).Returns(this.MainDaraBase);
        this.SqlDatabaseFactory.GetOrCreateDatabase(Arg.Any<DatabaseName>()).Returns(this.MainDaraBase);

        this.DatabaseScriptGeneratorContext.SqlDatabaseFactory.Returns(this.SqlDatabaseFactory);
        this.DatabaseScriptGeneratorContext.AssemblyMetadata.Returns(new AssemblyMetadata(typeof(object)) { DomainTypes = Array.Empty<DomainTypeMetadata>() });
    }

    public IDatabaseScriptGeneratorContext DatabaseScriptGeneratorContext { get; }

    public ISqlDatabaseFactory SqlDatabaseFactory { get; }

    public Microsoft.SqlServer.Management.Smo.Database MainDaraBase { get; }

    public Server MainServer { get; }
}
