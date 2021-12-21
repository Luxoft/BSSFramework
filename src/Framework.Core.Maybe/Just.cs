using System;
using System.Runtime.Serialization;

namespace Framework.Core
{
    public interface IJust : IMaybe
    {
        object Value { get; }
    }

    public interface IJust<out T> : IJust
    {
        new T Value { get; }
    }

[DataContract(Name = "JustOf{0}", Namespace = "Framework.Core")]
    public class Just<T> : Maybe<T>, IJust<T>
    {
        internal Just()
        {

        }


        public Just(T value)
        {
            this.Value = value;
        }


        public override string ToString()
        {
            return this.Value == null ? null : this.Value.ToString();
        }

        [DataMember]
        public T Value
        {
            get;
            private set;
        }

        object IJust.Value => this.Value;
    }
}
