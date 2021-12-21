using System.Runtime.Serialization;

namespace Framework.Core
{
    public interface INothing : IMaybe
    {

    }

    [DataContract(Name = "NothingOf{0}", Namespace = "Framework.Core")]
    public class Nothing<T> : Maybe<T>, INothing
    {
        public Nothing()
        {

        }


        public override string ToString()
        {
            return "";
        }
    }
}