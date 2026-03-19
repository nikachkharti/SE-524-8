namespace MiniBank.Repository.Attributes
{
    public class CustomExactLengthAttribute : BaseValidationAttribute
    {
        private readonly int _length;

        public CustomExactLengthAttribute(int length)
        {
            _length = length;
        }

        public override bool IsValid(object value, out string errorMessage)
        {
            if (value is string str && str.Length != _length)
            {
                errorMessage = $"Length must be exactly {_length}";
                return false;
            }

            errorMessage = null;
            return true;
        }
    }
}
