using CommonFramework;

using Framework.Core;

namespace Framework.Configuration.Domain;

public static class ExceptionMessageExtensions
{
    public static Exception ToException(this ExceptionMessage exceptionMessage)
    {
        if (exceptionMessage == null) throw new ArgumentNullException(nameof(exceptionMessage));

        return new RestoredException(exceptionMessage);
    }


    private class RestoredException : Exception
    {
        private readonly ExceptionMessage message;

        public RestoredException(ExceptionMessage message)
                : base("", message.FromMaybe(() => new ArgumentNullException(nameof(message))).InnerException.Maybe(v => v.ToException()))
        {
            this.message = message;
        }

        public override string Message => this.message.Message;

        public override string StackTrace => this.message.StackTrace;
    }
}
