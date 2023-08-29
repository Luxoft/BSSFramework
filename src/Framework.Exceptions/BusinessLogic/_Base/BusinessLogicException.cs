using System.Linq.Expressions;

using Framework.Core;

namespace Framework.Exceptions;

public class BusinessLogicException : ServiceFacadeException
{
    public static BusinessLogicException Create<T>(string format, Expression<Func<T, object>> expression)
    {
        return new BusinessLogicException(format, expression.ToPath());
    }

    public BusinessLogicException(Exception innerException, string format, params object[] args)
            : base(innerException, format, args)
    {

    }

    public BusinessLogicException(Exception innerException, string message)
            : base(innerException, message)
    {

    }

    public BusinessLogicException(string format, params object[] args)
            : base(format, args)
    {

    }

    public BusinessLogicException(string message)
            : base(message)
    {

    }

    protected BusinessLogicException()
    {

    }
}

public class BusinessLogicException<TDetail> : BusinessLogicException, IDetailException<TDetail>
{
    public BusinessLogicException(TDetail detail, Exception innerException, string message)
            : base(innerException, message)
    {
        this.Detail = detail;
    }

    public BusinessLogicException(TDetail detail, string format, params object[] args)
            : base(format, args)
    {
        this.Detail = detail;
    }

    public BusinessLogicException(TDetail detail, string message)
            : base(message)
    {
        this.Detail = detail;
    }


    public TDetail Detail { get; private set; }
}
