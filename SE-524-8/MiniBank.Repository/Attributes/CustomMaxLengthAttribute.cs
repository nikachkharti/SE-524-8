namespace MiniBank.Repository.Attributes
{
    public class CustomMaxLengthAttribute : BaseValidationAttribute
    {
        private readonly int _max;

        public CustomMaxLengthAttribute(int max)
        {
            _max = max;
        }

        public override bool IsValid(object value, out string errorMessage)
        {
            if (value is string str && str.Length > _max)
            {
                errorMessage = $"Max length is {_max}";
                return false;
            }

            errorMessage = null;
            return true;
        }
    }
}
