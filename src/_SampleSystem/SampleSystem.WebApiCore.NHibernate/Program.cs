using SampleSystem.ServiceEnvironment.DependencyInjection;

namespace SampleSystem.WebApiCore;

public static class Program
{
    private static Task Main(string[] args) => GenericProgram.Main(args, new SampleSystemNHibernateExtension());
}
