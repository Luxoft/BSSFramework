using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Framework.Core;

using SExpressions = System.Linq.Expressions;

namespace Framework.QueryLanguage
{
    [DataContract]
    public abstract class ConstantExpression : Expression
    {
        internal ConstantExpression()
        {

        }


        public abstract object UntypedValue { get; }

        //public abstract Type ValueType { get; }


        public override string ToString()
        {
            return this.UntypedValue.ToString();
        }
    }

    [DataContract]
    public class NullConstantExpression : ConstantExpression
    {
        internal NullConstantExpression()
        {

        }

        public override object UntypedValue
        {
            get { return null; }
        }

        public override string ToString()
        {
            return "null";
        }

        protected override bool InternalEquals(Expression other)
        {
            return other is NullConstantExpression;
        }


        public static readonly NullConstantExpression Value = new NullConstantExpression();
    }


    [DataContract]
    public abstract class ConstantExpression<TValue> : ConstantExpression
    {
        internal ConstantExpression()
        {

        }


        internal ConstantExpression(TValue value)
        {
            this.Value = value;
        }


        [DataMember]
        public TValue Value { get; private set; }

        public override object UntypedValue
        {
            get { return this.Value; }
        }


        public override int GetHashCode()
        {
            return this.Value == null ? 0 : this.Value.GetHashCode();
        }

        protected override bool InternalEquals(Expression other)
        {
            return (other as ConstantExpression<TValue>).Maybe(otherConstantExpression =>
                EqualityComparer<TValue>.Default.Equals(this.Value, otherConstantExpression.Value));
        }
    }
}