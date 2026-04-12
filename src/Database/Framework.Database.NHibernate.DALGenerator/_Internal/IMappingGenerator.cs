using System.Xml.Linq;

using Framework.Core.TypeResolving;

namespace Framework.Database.NHibernate.DALGenerator._Internal;

public interface IMappingGenerator
{
    IAssemblyInfo Assembly { get; }

    XDocument Generate();
}
