using System;

using Framework.Core;

namespace Framework.Configuration.Domain
{
    public static class ExceptionExtensions
    {
        public static ExceptionMessage ToMessage(this Exception source, bool isRoot = true)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new ExceptionMessage
            {
                IsRoot = isRoot,
                IsClient = false,
                InnerException = source.InnerException.Maybe(v => v.ToMessage(false)),
                MessageType = source.GetType().FullName,
                Message = source.Message,
                StackTrace = source.ToString()
            };
        }
    }
}
