using System;
using System.Reflection;

namespace Framework.ReferencesManager
{
    /// <summary>
    /// Ленивый класс, инкапсулирующий линк на тип
    /// </summary>
    public class Reference
    {
        private readonly Type _masterObjectType;
        private readonly string _propertyName;

        public Reference(Type masterObjectType, string propertyName)
        {
            this._masterObjectType = masterObjectType;
            this._propertyName = propertyName;
        }

        public Type MasterObjectType
        {
            get { return this._masterObjectType; }
        }

        public string PropertyName
        {
            get { return this._propertyName; }
        }
    }
}