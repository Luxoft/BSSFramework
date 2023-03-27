using System.Text;
using System.Xml.Linq;

using JetBrains.Annotations;

namespace Framework.Core;

public class XDocumentFileRenderer : IFileRenderer<XDocument, string>
{
    private readonly Func<StringBuilder, TextWriter> _createWriter;


    public XDocumentFileRenderer([NotNull] Func<StringBuilder, TextWriter> createWriter)
    {
        this._createWriter = createWriter ?? throw new ArgumentNullException(nameof(createWriter));
    }

    public XDocumentFileRenderer(Encoding encoding)
            : this(sb => new EncodingStringWriter(sb, encoding))
    {
        if (encoding == null) throw new ArgumentNullException(nameof(encoding));
    }


    public string FileExtension => "xml";


    public string Render([NotNull] XDocument document)
    {
        if (document == null) throw new ArgumentNullException(nameof(document));

        var sb = new StringBuilder();
        using (var writer = this._createWriter(sb))
        {
            document.Save(writer);
            writer.Flush();
        }

        return sb.ToString();
    }


    public static readonly XDocumentFileRenderer Default = new XDocumentFileRenderer(Encoding.UTF8);
}
