using System.Text;

namespace Framework.Core;

[Obsolete("v10 This method will be protected in future")]
public class EncodingStringWriter : StringWriter
{
    private readonly Encoding _encoding;


    public EncodingStringWriter(StringBuilder sb, Encoding encoding)
            : base(sb)
    {
        this._encoding = encoding;
    }


    public override Encoding Encoding
    {
        get { return this._encoding; }
    }
}
