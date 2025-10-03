using System.Xml.Linq;

using CommonFramework;

using Framework.Core;

using NHibernate.Cfg;

namespace Framework.DomainDriven.NHibernate;

public static class ConfigurationExtensions
{
    public static void AddDocument(this Configuration configuration, XDocument document)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));
        if (document == null) throw new ArgumentNullException(nameof(document));

        configuration.AddDocument(document.ToXmlDocument());
    }

    public static void AddDocuments(this Configuration configuration, IEnumerable<XDocument> documents)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));
        if (documents == null) throw new ArgumentNullException(nameof(documents));

        documents.Foreach(configuration.AddDocument);
    }
}
