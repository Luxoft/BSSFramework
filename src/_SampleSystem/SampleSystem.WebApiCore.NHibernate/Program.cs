using SampleSystem.ServiceEnvironment.NHibernate;

namespace SampleSystem.WebApiCore;


public static class Program
{
    private static async Task Main(string[] args)
    {
        await GenericProgram.Main(args, new SampleSystemNHibernateExtension());
    }
}
