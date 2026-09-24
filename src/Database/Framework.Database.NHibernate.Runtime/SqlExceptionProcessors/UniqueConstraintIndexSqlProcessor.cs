using System.Text.RegularExpressions;

namespace Framework.Database.NHibernate.SqlExceptionProcessors;

internal class UniqueConstraintIndexSqlProcessor(IDalValidationIdentitySource dalValidationIdentitySource) : UniqueViolationSqlProcessorBase(dalValidationIdentitySource)
{
    public override int ErrorNumber => 2601;

    protected override Regex MessageRegex { get; } =
        new(@"Cannot insert duplicate key row in object\s+'([a-zA-Z]+\.)?(.+)'\s+with unique index\s+'(.+)'");

    protected override (string IndexName, string Table) ExtractMatch(Match match) => (match.Groups[3].Value, match.Groups[2].Value);
}
