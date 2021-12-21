namespace Framework.Validation
{
    public class Int64ValueValidator : IPropertyValidator<object, long>
    {
        private readonly long _min;
        private readonly long _max;

        public Int64ValueValidator(long min, long max)
        {
            this._min = min;
            this._max = max;
        }


        public ValidationResult GetValidationResult(IPropertyValidationContext<object, long> context)
        {
            return ValidationResult.FromCondition(
                this._min <= context.Value && context.Value <= this._max,
                () => $"The value of {context.GetPropertyName()} property of {context.GetSourceTypeName()} should between {this.GetDesignRange()}");
        }


        private string GetDesignRange()
        {
            if (this._max == long.MaxValue)
            {
                return this._min.ToString();
            }

            if (this._min == long.MinValue)
            {
                return this._max.ToString();
            }

            return $"{this._min}-{this._max}";
        }
    }
}