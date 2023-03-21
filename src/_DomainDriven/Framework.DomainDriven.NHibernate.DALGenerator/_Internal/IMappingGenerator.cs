using System.Reflection;
using System.Xml.Linq;

using Framework.Core;

namespace Framework.DomainDriven.NHibernate.DALGenerator;

public interface IMappingGenerator
{
    IAssemblyInfo Assembly { get; }

    XDocument Generate();
}
