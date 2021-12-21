using System.Collections.Generic;

namespace Framework.DomainDriven.Generation
{
    public interface IFileGenerator<out TFileFactory>
    {
        IEnumerable<TFileFactory> GetFileGenerators();
    }

    public interface IFileGenerator<out TFileFactory, out TRenderer> : IFileGenerator<TFileFactory>
    {
        TRenderer Renderer { get; }
    }
}