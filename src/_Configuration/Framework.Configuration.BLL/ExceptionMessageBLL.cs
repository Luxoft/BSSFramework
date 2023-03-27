using Framework.Core;
using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL;

public partial class ExceptionMessageBLL
{
    public override void Save(ExceptionMessage exceptionMessage)
    {
        if (exceptionMessage == null) throw new ArgumentNullException(nameof(exceptionMessage));

        exceptionMessage.InnerException.Maybe(this.Save);

        base.Save(exceptionMessage);
    }

    public void Save(Exception exception)
    {
        if (exception == null) throw new ArgumentNullException(nameof(exception));

        this.Save(exception.ToMessage());
    }
}
