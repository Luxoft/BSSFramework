using System.Text;

namespace Framework.Core;

[Obsolete("v10 This method will be protected in future")]
public class EncodingStringWriter(StringBuilder sb, Encoding encoding) : StringWriter(sb)
{
    public override Encoding Encoding => encoding;
}
