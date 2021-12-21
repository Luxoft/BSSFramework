using System;
using System.Runtime.Serialization;

namespace Framework.Core
{
    [DataContract]
    public struct Ignore : IEquatable<Ignore>
    {
        public override bool Equals(object obj)
        {
            return obj is Ignore;
        }

        public bool Equals(Ignore other)
        {
            return true;
        }

        public override int GetHashCode()
        {
            return 0;
        }


        public static readonly Ignore Value;
    }
}