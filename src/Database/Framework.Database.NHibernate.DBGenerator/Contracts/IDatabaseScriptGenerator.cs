namespace Framework.Database.NHibernate.DBGenerator.Contracts;

public interface IDatabaseScriptGenerator
{
    IDatabaseScriptResult GenerateScript(IDatabaseScriptGeneratorContext context);
}
