namespace Framework.Validation
{
    public class IntValueValidator : IPropertyValidator<object, int>
    {
        private readonly int _min;
        private readonly int _max;

        public IntValueValidator(int min, int max)
        {
            this._min = min;
            this._max = max;
        }


        public ValidationResult GetValidationResult(IPropertyValidationContext<object, int> context)
        {
            return ValidationResult.FromCondition(
                this._min <= context.Value && context.Value <= this._max,
                () => $"The value of {context.GetPropertyName()} property of {context.GetSourceTypeName()} should between {this.GetDesignRange()}");
        }


        private string GetDesignRange()
        {
            if (this._max == int.MaxValue)
            {
                return this._min.ToString();
            }

            if (this._min == int.MinValue)
            {
                return this._max.ToString();
            }

            return $"{this._min}-{this._max}";
        }
    }
}