using System.Text;
using System.Xml.Linq;

namespace Framework.Core.Rendering;

public class XDocumentFileRenderer(Func<StringBuilder, TextWriter> createWriter) : IFileRenderer<XDocument, string>
{
    private readonly Func<StringBuilder, TextWriter> createWriter = createWriter ?? throw new ArgumentNullException(nameof(createWriter));

    public XDocumentFileRenderer(Encoding encoding)
            : this(sb => new EncodingStringWriter(sb, encoding))
    {
    }

    public string FileExtension => "xml";


    public string Render(XDocument document)
    {
        if (document == null) throw new ArgumentNullException(nameof(document));

        var sb = new StringBuilder();
        using (var writer = this.createWriter(sb))
        {
            document.Save(writer);
            writer.Flush();
        }

        return sb.ToString();
    }


    public static readonly XDocumentFileRenderer Default = new(Encoding.UTF8);
}
