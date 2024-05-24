namespace Automation.Utils.DatabaseUtils.Interfaces;

public interface IDatabaseItem
{
    public string DatabaseName { get; }
    public string DbDataDirectory { get; }
    public string CopyDataPath { get; }
    public string CopyLogPath { get; }
    public string SourceDataPath { get; }
    public string SourceLogPath { get; }
    public string DatabaseCollation { get; }
}
