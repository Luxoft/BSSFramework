using System.Xml.Linq;

using Anch.Core;

using Framework.Core;

using NHibernate.Cfg;

namespace Framework.Database.NHibernate;

public static class ConfigurationExtensions
{
    public static void AddDocument(this Configuration configuration, XDocument document)
    {
        if (configuration is null) throw new ArgumentNullException(nameof(configuration));
        if (document is null) throw new ArgumentNullException(nameof(document));

        configuration.AddDocument(document.ToXmlDocument());
    }

    public static void AddDocuments(this Configuration configuration, IEnumerable<XDocument> documents)
    {
        if (configuration is null) throw new ArgumentNullException(nameof(configuration));
        if (documents is null) throw new ArgumentNullException(nameof(documents));

        documents.Foreach(configuration.AddDocument);
    }
}

