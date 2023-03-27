using CommandLine;

namespace SampleSystem.DbGenerate;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Start");
        Parser.Default.ParseArguments<DbGenerationOptions>(args).WithParsed(GenerateDatabase);
    }

    private static void GenerateDatabase(DbGenerationOptions options)
    {
        var generators = new DbGeneratorTest();
        generators.GenerateDatabase(options);
    }
}
