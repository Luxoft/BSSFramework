using System.Text;

namespace Framework.Core;

public class EncodingStringWriter(StringBuilder sb, Encoding encoding) : StringWriter(sb)
{
    public override Encoding Encoding => encoding;
}
