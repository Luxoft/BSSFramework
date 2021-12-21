using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Framework.Core;

using SExpressions = System.Linq.Expressions;

namespace Framework.QueryLanguage
{
    [DataContract]
    public class PropertyExpression : Expression
    {
        public PropertyExpression(Expression source, string propertyName)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

            this.Source = source;
            this.PropertyName = propertyName;
        }


        [DataMember]
        public Expression Source { get; private set; }

        [DataMember]
        public string PropertyName { get; private set; }


        public override string ToString()
        {
            return $"{this.Source}.{this.PropertyName}";
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ this.PropertyName.GetHashCode();
        }

        protected override bool InternalEquals(Expression other)
        {
            return (other as PropertyExpression).Maybe(otherPropertyExpression =>
                this.PropertyName == otherPropertyExpression.PropertyName
             && this.Source == otherPropertyExpression.Source);
        }
    }
}