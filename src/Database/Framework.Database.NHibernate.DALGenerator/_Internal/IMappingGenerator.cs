using System.Xml.Linq;

namespace Framework.Database.NHibernate.DALGenerator._Internal;

public interface IMappingGenerator
{
    IAssemblyInfo Assembly { get; }

    XDocument Generate();
}
