namespace MiniBank.Repository.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomPositiveAttribute : BaseValidationAttribute
    {
        public override bool IsValid(object value, out string errorMessage)
        {
            if (value == null)
            {
                errorMessage = "Value is null";
                return false;
            }

            if (decimal.TryParse(value.ToString(), out var number))
            {
                if (number <= 0)
                {
                    errorMessage = "Value must be positive";
                    return false;
                }
            }

            errorMessage = null;
            return true;
        }
    }
}
