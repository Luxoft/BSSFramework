using CommandLine;

namespace SampleSystem.DbGenerate;

public class DbGenerationOptions
{
    [Option('s', Required = false, HelpText = "Target generation server")]
    public string Server { get; set; }

    [Option('d', Required = false, HelpText = "Target generation DataBase name")]
    public string DataBase { get; set; }

    [Option('o', Required = false, HelpText = "Output script path. If set, output script file is produced")]
    public string OutputPath { get; set; }
}
