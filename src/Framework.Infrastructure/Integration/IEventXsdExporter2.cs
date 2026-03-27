namespace Framework.DomainDriven.WebApiNetCore.Integration;

public interface IEventXsdExporter2
{
    public Stream Export(string xsdNamespace, string localName, IReadOnlyCollection<Type> types);

    public Stream Export<TBaseEventDto>()
            where TBaseEventDto : class;
}
