using System.Text.RegularExpressions;

namespace Framework.Database.NHibernate.SqlExceptionProcessors;

internal class UniqueIndexSqlProcessor(IDalValidationIdentitySource dalValidationIdentitySource) : UniqueViolationSqlProcessorBase(dalValidationIdentitySource)
{
    public override int ErrorNumber => 2627;

    protected override Regex MessageRegex { get; } =
        new(@"Violation of UNIQUE KEY constraint\s+'(.+)'.\s+Cannot insert duplicate key in object\s+'([a-zA-Z]+\.)?(.+)'");

    protected override (string IndexName, string Table) ExtractMatch(Match match) => (match.Groups[1].Value, match.Groups[3].Value);
}
