using System;

namespace Framework.SecuritySystem.Exceptions
{
    public class AccessDeniedException : Exception
    {
        public AccessDeniedException(string message)
            : base(message)
        {

        }

        public AccessDeniedException(Type type, TIdent id, string customMessage)
            : this(type.FromMaybe(() => new ArgumentNullException(nameof(type))).Name, id, customMessage)
        {

        }


        public static string GetDefaultMessage(string typeName, TIdent id, string instanceName = null)
        {
            return id.IsDefault() ? $"You have no permissions to create object \"{typeName}\""
                       : $"You have no permissions to access \"{typeName}\" with id = \"{id}\"";
        }
    }
}
