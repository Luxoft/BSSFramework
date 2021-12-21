using System;

namespace Framework.DomainDriven.DBGenerator
{
    public struct DomainTypeLinks
    {
        public static DomainTypeLinks Create(Type from, Type to)
        {
            return new DomainTypeLinks(from, to);
        }
        public static DomainTypeLinks Create<TFrom, TTo>()
        {
            return Create(typeof(TFrom), typeof(TTo));
        }
        public Type From { get; private set; }
        public Type To { get; private set; }

        public DomainTypeLinks(Type from, Type to) : this()
        {
            this.From = from;
            this.To = to;
        }

        public bool Equals(DomainTypeLinks other)
        {
            return this.From == other.From && this.To == other.To;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is DomainTypeLinks && this.Equals((DomainTypeLinks) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.From != null ? this.From.GetHashCode() : 0)*397) ^ (this.To != null ? this.To.GetHashCode() : 0);
            }
        }
    }
}