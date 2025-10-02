using CommonFramework;

using Framework.DomainDriven.DBGenerator.Contracts;
using Framework.DomainDriven.DBGenerator.Team;
using Framework.DomainDriven.Metadata;
using Framework.Persistent;
using Microsoft.SqlServer.Management.Common;

namespace Framework.DomainDriven.DBGenerator;

public abstract class PostDatabaseScriptGeneratorBase : IDatabaseScriptGenerator
{
    public IDatabaseScriptResult GenerateScript(IDatabaseScriptGeneratorContext context)
    {
        var dictionary = new Dictionary<ApplyMigrationDbScriptMode, Lazy<IEnumerable<string>>>();
        dictionary.Add(ApplyMigrationDbScriptMode.PostChangeIndexies, new Lazy<IEnumerable<string>>(() => this.Execute(context)));
        return DatabaseScriptResultFactory.Create(dictionary);
    }

    private IEnumerable<string> Execute(IDatabaseScriptGeneratorContext context)
    {
        var server = context.SqlDatabaseFactory.Server;
        server.ConnectionContext.CapturedSql.Clear();
        server.ConnectionContext.SqlExecutionModes = SqlExecutionModes.ExecuteAndCaptureSql;

        this.Apply(context);

        return server.ConnectionContext.CapturedSql.GetScriptsForBatchExecuting();
    }

    protected static ReferenceTypeFieldMetadata GetMasterRef(Type from, Type to, Dictionary<Type, DomainTypeMetadata> dictionary)
    {
        var masterRefCandidates = dictionary[from].ReferenceFields.Where(z => z.ToType == to).ToList();

        var masterRef = masterRefCandidates.First();

        if (masterRefCandidates.Count > 1)
        {
            masterRef = masterRefCandidates
                        .Where(z => z.IsMasterReference)
                        .Single(() =>
                                        new Exception(
                                                      $"Type:{@from.Name} has more then one property with master type:{to.Name}. Mark one property:{typeof(IsMasterAttribute).Name} attribute"),
                                () => new Exception($"Type:{@from.Name} has more then one property with IsMaster attribute master type:{to.Name}"));
        }

        return masterRef;

    }


    protected static ReferenceTypeFieldMetadata GetMasterRefMetadata(DomainTypeMetadata detailMetadata,
                                                                     DomainTypeMetadata masterMetadata)
    {
        var masterRefCandidates = detailMetadata.GetExpandedUpFields().OfType<ReferenceTypeFieldMetadata>()
                                                .Where(z => masterMetadata.DomainType.IsAssignableFrom(z.ToType)).ToList();

        var masterRef = masterRefCandidates.First();

        if (masterRefCandidates.Count > 1)
        {
            masterRef = masterRefCandidates.Single(z => z.Attributes.Any(q => q is IsMasterAttribute),
                                                   () =>
                                                           new Exception(
                                                                         $"Type:{detailMetadata.DomainType.Name} has more then one property with master type:{masterMetadata.DomainType.Name}. Mark one property:{typeof(IsMasterAttribute).Name} attribute"),
                                                   () =>
                                                           new Exception(
                                                                         $"Type:{detailMetadata.DomainType.Name} has more then one property with IsMaster attribute master type:{masterMetadata.DomainType.Name}"));
        }

        return masterRef;
    }


    protected abstract void Apply(IDatabaseScriptGeneratorContext context);
}
