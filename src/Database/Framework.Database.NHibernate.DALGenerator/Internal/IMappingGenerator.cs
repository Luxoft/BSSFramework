using System.Reflection;
using System.Xml.Linq;

namespace Framework.Database.NHibernate.DALGenerator.Internal;

public interface IMappingGenerator
{
    Assembly Assembly { get; }

    XDocument Generate();
}
