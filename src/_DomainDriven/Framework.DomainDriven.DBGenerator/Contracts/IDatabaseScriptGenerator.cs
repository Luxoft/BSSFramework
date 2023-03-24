namespace Framework.DomainDriven.DBGenerator.Contracts;

public interface IDatabaseScriptGenerator
{
    IDatabaseScriptResult GenerateScript(IDatabaseScriptGeneratorContext context);
}
