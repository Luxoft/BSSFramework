using System;

using Framework.DomainDriven.DAL.Revisions;

using JetBrains.Annotations;

namespace Framework.DomainDriven.BLL
{
    public class ObjectModification : IEquatable<ObjectModification>
    {
        private readonly ModificationType _modificationType;
        private readonly object _object;
        private readonly Type _objectType;


        public ObjectModification(object @object, [NotNull] Type objectType, ModificationType modificationType)
        {
            if (@object == null) throw new ArgumentNullException(nameof(@object));
            if (objectType == null) throw new ArgumentNullException(nameof(objectType));


            this._object = @object;
            this._modificationType = modificationType;
            this._objectType = objectType;
        }


        public ModificationType ModificationType
        {
            get { return this._modificationType; }
        }

        public object Object
        {
            get { return this._object; }
        }

        public Type ObjectType
        {
            get { return this._objectType; }
        }


        public override int GetHashCode()
        {
            return this._object.GetHashCode() ^ this._modificationType.GetHashCode();
        }

        public bool Equals(ObjectModification other)
        {
            return other != null
                   && object.Equals(other.Object, this._object)
                   && object.Equals(other.ModificationType, this._modificationType);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ObjectModification);
        }
    }
}
