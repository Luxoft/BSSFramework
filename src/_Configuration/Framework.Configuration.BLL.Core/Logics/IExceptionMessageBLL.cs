using System;
using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL;

public partial interface IExceptionMessageBLL
{
    void Save(Exception exception);
}
