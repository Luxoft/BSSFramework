using Framework.DomainDriven;
using Framework.DomainDriven.DBGenerator;
using Framework.DomainDriven.DBGenerator.Contracts;
using Framework.DomainDriven.Metadata;

using Microsoft.SqlServer.Management.Smo;

using NSubstitute;

namespace DBGenerator.Tests.Unit
{
    public class DatabaseScriptGeneratorContextMockBuilder
    {
        public DatabaseScriptGeneratorContextMockBuilder()
        {
            this.MainDaraBase = new Database();
            this.MainServer = new Server();

            this.DatabaseScriptGeneratorContext = Substitute.For<IDatabaseScriptGeneratorContext>();

            this.SqlDatabaseFactory = Substitute.For<ISqlDatabaseFactory>();

            this.SqlDatabaseFactory.Server.Returns(this.MainServer);
            this.SqlDatabaseFactory.GetDatabase(Arg.Any<DatabaseName>()).Returns(this.MainDaraBase);
            this.SqlDatabaseFactory.GetOrCreateDatabase(Arg.Any<DatabaseName>()).Returns(this.MainDaraBase);

            this.DatabaseScriptGeneratorContext.SqlDatabaseFactory.Returns(this.SqlDatabaseFactory);
            this.DatabaseScriptGeneratorContext.AssemblyMetadata.Returns(new AssemblyMetadata(typeof(object))
                                                                                     {
                                                                                         DomainTypes = new DomainTypeMetadata[0]
                                                                                     });
        }

        public IDatabaseScriptGeneratorContext DatabaseScriptGeneratorContext { get; private set; }

        public ISqlDatabaseFactory SqlDatabaseFactory { get; private set; }

        public Database MainDaraBase { get; private set; }

        public Server MainServer { get; private set; }
    }
}
