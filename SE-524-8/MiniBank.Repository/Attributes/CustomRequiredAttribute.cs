namespace MiniBank.Repository.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomRequiredAttribute : BaseValidationAttribute
    {
        public override bool IsValid(object value, out string errorMessage)
        {
            if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
            {
                errorMessage = "Field is required";
                return false;
            }

            errorMessage = null;
            return true;
        }
    }
}
